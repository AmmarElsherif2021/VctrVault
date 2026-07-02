using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vctr_vault.Core.Entities;
using vctr_vault.Core.Enums;
using vctr_vault.Core.Interfaces;

namespace vctr_vault.Business.Services
{
    /// <summary>
    /// Concrete implementation of Material business logic.
    /// Lives in the Application layer, depends only on Core abstractions.
    /// </summary>
    public class MaterialService : IMaterialService
    {
        private readonly IMaterialRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public MaterialService(IMaterialRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        // =============================================
        // 1. BUSINESS WRITE OPERATIONS
        // =============================================

        public async Task<Material> CreateMaterialAsync(Material material)
        {
            // 1. Business Rule: Unique Name Validation
            var existing = await _repository.SearchByNameAsync(material.Name);
            if (existing.Any())
                throw new Exception($"A material with the name '{material.Name}' already exists.");

            // 2. Business Rule: Price must be positive
            if (material.UnitPrice <= 0)
                throw new Exception("UnitPrice must be greater than zero.");

            // 3. Business Rule: Auto-calculate derived fields (Domain Logic)
            material.SafetyBuffer = material.CurrentStock / 2;
            material.ReorderLevel = material.SafetyBuffer; // Simplification; or use ConsumptionRate * LeadTime

            // 4. Delegate raw persistence to the Repository
            await _repository.AddAsync(material);
            await _unitOfWork.SaveChangesAsync(); // Atomic commit

            return material;
        }

        public async Task<Material> UpdateMaterialAsync(Material material)
        {
            // 1. Fetch the existing entity to compare state
            var existing = await _repository.GetByIdAsync(material.Id);
            if (existing == null)
                throw new Exception($"Material with ID '{material.Id}' not found.");

            // 2. Business Rule: Recalculate ReorderLevel if dependent fields changed
            if (existing.ConsumptionRate != material.ConsumptionRate || existing.LeadTime != material.LeadTime)
            {
                // Formula: Reorder Level = (Maximum Consumption) × (Maximum Delivery Time)
                material.ReorderLevel = material.ConsumptionRate * material.LeadTime;
            }

            // 3. Business Rule: Recalculate SafetyBuffer if CurrentStock changed externally
            if (Math.Abs(existing.CurrentStock - material.CurrentStock) > 0.001)
            {
                material.SafetyBuffer = material.CurrentStock / 2;
            }

            // 4. Update timestamp (handled by BaseEntity.UpdateTimestamp in Repository/Interceptor)
            // 5. Delegate persistence
            await _repository.UpdateAsync(material);
            await _unitOfWork.SaveChangesAsync();

            return material;
        }

        public async Task<bool> DeleteMaterialAsync(string materialId)
        {
            var material = await _repository.GetByIdAsync(materialId);
            if (material == null)
                throw new Exception($"Material with ID '{materialId}' not found.");

            // Business Rule: Cannot delete inventory that still has stock
            if (material.CurrentStock > 0)
                throw new Exception(
                    $"Cannot delete material '{material.Name}' because CurrentStock is {material.CurrentStock}. " +
                    "Adjust stock to zero before deletion.");

            await _repository.DeleteByIdAsync(materialId);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<Material> AdjustStockAsync(string materialId, double adjustmentQty, string reason)
        {
            // 1. Fetch material
            var material = await _repository.GetByIdAsync(materialId);
            if (material == null)
                throw new Exception($"Material with ID '{materialId}' not found.");

            // 2. Execute domain logic on the entity (throws if negative)
            material.UpdateStock(adjustmentQty);

            // 3. Log the transaction (Business Orchestration)
            var transactionType = adjustmentQty >= 0 ? TransType.In : TransType.Out;
            var absQty = Math.Abs((decimal)adjustmentQty);

            var transaction = new AdjustmentTransaction(
                transactionType,
                material.Id,
                absQty,
                material.Unit,
                reason ?? $"Stock adjustment via MaterialService"
            )
            {
                UnitCost = (decimal?)material.UnitPrice,
                CreatedBy = "System" // Or pass from context
            };

            // 4. Add transaction via UnitOfWork
            await _unitOfWork.Transactions.AddAsync(transaction);

            // 5. Update the material's UpdatedAt timestamp
            material.UpdatedAt = DateTime.UtcNow;

            // 6. Persist both changes atomically
            await _unitOfWork.SaveChangesAsync();

            return material;
        }

        // =============================================
        // 2. ENRICHED DOMAIN QUERIES
        // =============================================

        public async Task<Material> GetMaterialOrThrowAsync(string materialId)
        {
            var material = await _repository.GetByIdAsync(materialId);
            return material ?? throw new Exception($"Material with ID '{materialId}' not found.");
        }

        public async Task<IEnumerable<(Material Material, double ShortageQty, int EstimatedDaysLeft)>> 
            GetReorderRecommendationsWithContextAsync()
        {
            // 1. Get raw data from Repository
            var materials = await _repository.GetMaterialsNeedingReorderAsync();
            var result = new List<(Material, double, int)>();

            foreach (var material in materials)
            {
                // 2. Enrich with business calculations
                var shortageQty = material.ReorderLevel - material.CurrentStock;
                var estimatedDaysLeft = material.ConsumptionRate > 0 
                    ? (int)(material.CurrentStock / material.ConsumptionRate) 
                    : int.MaxValue;

                result.Add((material, shortageQty, estimatedDaysLeft));
            }

            return result;
        }

        public async Task<decimal> CalculateInventoryValueAsync(string materialId)
        {
            var material = await GetMaterialOrThrowAsync(materialId);
            // Business Math: UnitPrice * CurrentStock
            return (decimal)(material.UnitPrice * material.CurrentStock);
        }

        // =============================================
        // 3. VALIDATION & FEASIBILITY
        // =============================================

        public async Task<bool> IsStockAvailableAsync(string materialId, double requestedQty)
        {
            var material = await _repository.GetByIdAsync(materialId);
            if (material == null) return false;

            return material.CurrentStock >= requestedQty;
        }

        public async Task<int> SuggestOptimalOrderQuantityAsync(string materialId)
        {
            var material = await GetMaterialOrThrowAsync(materialId);

            // Business Rule: Calculate how much to order to reach SafetyBuffer + (Consumption * LeadTime)
            var targetStock = material.SafetyBuffer + (material.ConsumptionRate * material.LeadTime);
            var requiredQty = targetStock - material.CurrentStock;

            if (requiredQty <= 0) return 0;

            // Ensure it meets the MOQ (Minimum Order Quantity)
            var orderQty = (int)Math.Ceiling(requiredQty);
            return orderQty < material.MOQ ? material.MOQ : orderQty;
        }
    }
}