# Stock transaction types

---

### 1. The Primary Differentiator: Receipt (IN) vs. Issue (OUT)
The table in your class comment establishes the foundational binary. This dictates the **polarity of the `Quantity`** and the **financial ledger effect**, but the rest of the attributes branch differently from here:

- **Receipt (IN)** (`Quantity` > 0): The system **credits** the inventory. The `DestinationWarehouseId` is mandatory (stock arrives somewhere). The `UnitCost` represents the **acquisition cost** (what we paid the supplier). 
- **Issue (OUT)** (`Quantity` < 0): The system **debits** the inventory. The `SourceWarehouseId` is mandatory (stock leaves somewhere). The `UnitCost` is usually **retrieved dynamically** from the system’s valuation logic (e.g., Weighted Average or FIFO) at the moment of picking, rather than being manually typed.

---

### 2. Differentiating by `TransactionType` (The Business Context)

The `TransactionType` enum is the highest-level discriminator. Here is how the supporting attributes change across each type:

- **Purchase (IN)**: 
  - *Parties*: `SupplierId` is mandatory. `CustomerId` is null.
  - *Reference*: Ties to the external Purchase Order (PO).
  - *Costing*: `UnitCost` is locked at the invoiced supplier price.
  - *Location*: Only `DestinationWarehouseId` is populated.

- **Sale / Consumption (OUT)**: 
  - *Parties*: `CustomerId` is mandatory (if it's a direct sale) OR `CustomerId` is null if it is an internal production floor consumption.
  - *Reference*: Ties to a Sales Order or Work Order.
  - *Costing*: `UnitCost` is **not** input by the user; it is calculated by the system based on current inventory valuation to debit the Cost of Goods Sold (COGS).
  - *Location*: Only `SourceWarehouseId` is populated.

- **Customer Return (IN)**: 
  - Although it adds stock, the `TransactionType` is "Return". 
  - *Parties*: `CustomerId` is mandatory (the one sending it back).
  - *Reason*: Must be populated (e.g., "Defective", "Wrong item").
  - *Costing*: `UnitCost` might use the *original* sales cost or a restocking fee-adjusted cost, rather than the standard supplier purchase cost.

- **Supplier Return (OUT)**: 
  - Stock leaves the warehouse to go back to the vendor.
  - *Parties*: `SupplierId` is mandatory.
  - *Reference*: Ties to a Debit Note or Return Authorization.
  - *Location*: Only `SourceWarehouseId`.

- **Transfer (Neutral Net Inventory, but Dual Movement)**: 
  - *Location Differentiator*: **Both** `SourceWarehouseId` and `DestinationWarehouseId` are populated simultaneously (they cannot be null).
  - *Quantity*: While the database stores it as negative for the source and positive for the destination, logically, the absolute quantity is the same. 
  - *Costing*: The `UnitCost` is carried over from the source location to the destination without changing the total valuation.
  - *Parties*: `SupplierId` and `CustomerId` are both null.

- **Adjustment (Physical Count Correction)**: 
  - Can be IN (found surplus) or OUT (found shortage).
  - *Reason*: This attribute is **critical** here. It must specify the nature (e.g., "Cycle Count Variance", "Damaged beyond repair").
  - *Costing*: If it is a negative adjustment, the `UnitCost` must pull the current average cost to write off the value. If positive, it often uses the current market or standard cost.
  - *Parties*: Both Supplier and Customer are null.

- **Scrap (OUT)**: 
  - A specific subtype of Issue.
  - *Reason*: Mandatory (e.g., "Rust", "Expired shelf-life").
  - *Destination*: Notice that `DestinationWarehouseId` is not used for sales; here, it could technically be repurposed to point to a "Scrap Bin" location, or you rely entirely on `Reason` to mark it as disposed.

---

### 3. Differentiating by Cost Application (Financial Weight)

The behavior of `UnitCost` and the computed `TotalCost` changes entirely depending on whether the transaction is a **Valuation Setter** (IN) or a **Valuation Consumer** (OUT):

- **For Incoming Transactions (Purchase, Return)**: `UnitCost` is **explicitly provided** by the user or external document. It actively drives the new Weighted Average Cost of the material. `TotalCost` is a simple multiplication of `Quantity` × `UnitCost`.
- **For Outgoing Transactions (Sale, Scrap)**: `UnitCost` is **implicitly fetched** from the database's valuation engine at the moment of transaction creation. The user does not type it. The `TotalCost` is calculated by the system to subtract that exact amount from the inventory asset account.

---

### 4. Differentiating by Warehouse Scope (Single vs. Multi-site)

- **Local Transaction**: Only `SourceWarehouseId` OR only `DestinationWarehouseId` is filled. This affects the inventory balance of just one location.
- **Inter-Warehouse Transfer**: **Both** fields are filled. The system must validate that `SourceWarehouseId` ≠ `DestinationWarehouseId`. The validation logic must ensure that the negative movement and positive movement are booked as a single atomic pair to keep the General Ledger in balance across sites.

---

### 5. Differentiating by Audit Trail and Reason

- **Planned Transactions (Purchase/Sale)**: `ReferenceNumber` is your primary searchable key (linking to external ERP documents). `Reason` is usually generic or optional.
- **Unplanned / Corrective Transactions (Adjustment/Scrap)**: `Reason` becomes a **mandatory compliance field**. In a construction warehouse, writing off a batch of cement requires a mandatory reason (e.g., "Moisture damage") to satisfy audit controls. `ReferenceNumber` here might point to an internal authorization form rather than an external PO.

---

### Summary of the Decision Matrix

To differentiate any incoming transaction request, the system must evaluate these attributes in this exact priority:

1. **Sign of `Quantity`** → Determines if we validate `Source` or `Destination`.
2. **`TransactionType`** → Determines which foreign key (`SupplierId` vs `CustomerId`) is mandatory and which is null.
3. **Presence of both Warehouse IDs** → Triggers "Transfer" logic (cost remains neutral; both locations update).
4. **`Reason` + `TransactionType` combo** → If `TransactionType` is Adjustment or Scrap, the system enforces strict validation on `Reason` to prevent inventory leakage without justification.
5. **`UnitCost` nullability** → If `TransactionType` is OUT and `UnitCost` is null, the system must reject the transaction or trigger an automatic valuation lookup before finalizing. If IN, it must reject if `UnitCost` is null (unless it is a free sample, which requires a zero-cost flag).