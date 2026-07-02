# DTOs vs Services 
In Business Layer (vctr-vault.Business), Services and DTOs serve completely opposite but complementary purposes.

# Services:
- contain Behavior (the "verbs" or "actions" of the system).
- Has methods (e.g., ProcessReceiptAsync).
- Depends on other abstractions (e.g., IMaterialRepository, IUnitOfWork) injected via the       constructor.
- Does NOT expose internal domain state—it holds no properties representing a Material or Supplier; it only holds dependencies.
- Maps between DTOs and Entities internally.
# DTOs:
- contain Data (the "nouns" or "shapes" of the system).
- A DTO is a dumb, flat, serializable container for data. It contains zero business logic, zero methods (except for minimal constructors or ToString() overrides), and zero dependencies.
- DTOs exist strictly to transport data across the architectural boundary
- Has properties (getters/setters) only.
- Has no behavior—no UpdateStock(), no Validate(), no calculations.
- Flattened/Shaped—may combine fields from Material and Supplier into one object to reduce round-trips.
-------------------------------------------------------
Console/API (UI)
     │
     │ (Sends DTO) ProcessReceiptRequestDto
     ▼
IInventoryService (Business Layer)
     │ 
     │ 1. Converts DTO -> Domain Entity (Material)
     │ 2. Executes logic (UpdateStock, Validate)
     │ 3. Calls IMaterialRepository (Data Layer)
     │ 4. Converts Domain Entity -> DTO
     │
     │ (Returns DTO) ReceiptResultDto
     ▼
Console/API (UI)