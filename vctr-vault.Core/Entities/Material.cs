// vctr-vault.Data/Entities/Material.cs
namespace vctr_vault.Core.Entities
{
    public class Material:BaseEntity
    {
        public string Name { get; set; } = string.Empty; 
        public string Category { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public double UnitPrice {get; set;} 
        public double ReorderLevel { get; set; } // associated with - MaxDeliveryTime - MaxConsumptionTime 
        public double CurrentStock { get; private set; }
        public int ConsumptionRate {get; set;}
        public int LeadTime {get; set;}
        public int MOQ {get; set;} //minimum ordered quantity
        
        public double SafetyBuffer {get; set;}
    
        //Paramaterized constructor
        public Material(string Name,string Category,double UnitPrice, string Unit, double CurrentStock)
        {
            Id = Guid.NewGuid().ToString();
            this.Name=Name;
            this.Category=Category;
            this.Unit=Unit;
            this.UnitPrice=UnitPrice;
            this.CurrentStock=CurrentStock;
            SafetyBuffer= CurrentStock/2;
            ReorderLevel= SafetyBuffer;
        
        
        }
        public Material()
        {
            Id= Guid.NewGuid().ToString();
        }
        public void UpdateStock(double qty){
            double result= CurrentStock + qty;
                if(result<0){
                    throw new Exception($"Insufficient stock. Current: {CurrentStock}, Attempted subtract: {Math.Abs(qty)}");
                    }
                else{
                    CurrentStock=result;  
                }
            }
    }

    /*
    Reorder Level or Reorder Point (ROP): The minimum stock threshold that triggers a replenishment order. 
    🔢 Formula
    Reorder Level=(Maximum Consumption)×(Maximum Delivery Time)
    Used when consumption and delivery times vary significantly.
    ✅ Why It Matters
    Avoid Stockouts: Prevent lost sales and customer dissatisfaction.
    Prevent Overstocking: Reduce storage costs and tied-up capital.
    Improve Forecasting: Forces analysis of demand, lead times, and seasonal trends.
    Customer Satisfaction: Ensures timely fulfillment of orders.
    */
}