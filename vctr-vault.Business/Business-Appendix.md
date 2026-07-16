## Material Domain

| Term | Business Definition |
|---|---|
| **Category** | Classification grouping for organizing materials (search, reporting, reorder logic). |
| **Consumption Rate** | The rate at which a material is used/depleted over time — feeds reorder and forecasting logic. |
| **Current Stock** | The live, on-hand quantity of a material — mutated only via `UpdateStock`, never set directly (encapsulated). |
| **Estimated Days Left** | Projected number of days until a material runs out, derived from `CurrentStock` and `ConsumptionRate`. |
| **Inventory Value (per Material)** | `UnitPrice × CurrentStock` for a single material — the per-item building block of total inventory valuation. |
| **Lead Time** *(Material-level)* | Expected time between placing an order for this material and receiving it. |
| **Material** | Any physical item tracked in inventory — raw goods, components, or finished products. |
| **MOQ (Minimum Order Quantity)** | The smallest quantity of a material that can be ordered in a single purchase, regardless of actual need. |
| **Optimal Order Quantity** | A recommended purchase quantity balancing `MOQ`, `CurrentStock`, and `ConsumptionRate` — distinct from `MOQ` itself, which is only a floor. |
| **Reorder Level** | The stock threshold that should trigger replenishment; currently derived as `SafetyBuffer` at construction time (a simplification from the documented formula: Max Consumption × Max Delivery Time). |
| **Safety Buffer** | A cushion quantity (currently `CurrentStock / 2`) held above expected need to absorb demand/supply variability. |
| **Shortage Qty** | The gap between current stock and the reorder threshold — how much is missing, not just that it is low. |
| **Unit** | The unit of measure a material is tracked/counted in (e.g., "kg", "box", "each"). |
| **Unit Price** | The cost per unit of a material. |

## Supplier Domain

| Term | Business Definition |
|---|---|
| **Avg / Min / Max Unit Cost** | The average, lowest, and highest price paid to a specific supplier for a specific material across analyzed receipts — used to compare sourcing options. |
| **Compliance Certifications** | Standards the supplier is certified against (e.g., "ISO 9001", "FDA"). |
| **Contact Person** | The named individual point of contact at the supplier. |
| **Currency Code** *(Supplier-level)* | Currency the supplier transacts in — stored as a raw string; diverges from the `CurrencyCode` enum used on stock transactions (an inconsistency to resolve). |
| **Is Active** | Whether the supplier is currently eligible for new orders. |
| **Lead Time Days** *(Supplier-level)* | Supplier-specific fulfillment lead time — distinct from `Material.LeadTime`; the two may diverge and may need reconciliation logic later. |
| **Min/Max Order Qty** | The supplier's floor and ceiling on order size per purchase. |
| **Payment Terms** | Agreed payment conditions (e.g., "Net 30", "2/10 Net 60"). |
| **Quality Notes** | Free-text record of quality/compliance issues or observations. |
| **Quality Rating** | A score (e.g., 1–5) reflecting historical supplier quality performance. |
| **Receipts Analysed (count)** | The sample size (number of past receipts) backing a supplier cost comparison — signals how statistically reliable the average is. |
| **Shipping Method** | The mode of transport/delivery the supplier uses (e.g., freight, courier). |
| **Supplier** | External vendor from whom materials are purchased and received. |
| **Tax ID** | Supplier's tax registration identifier, for compliance/invoicing. |

## Warehouse Domain

| Term | Business Definition |
|---|---|
| **Capacity** | Maximum storage capacity, in whatever unit the business defines (sq ft, pallet slots, etc.); nullable to represent unknown/unlimited. |
| **Code** | Short human-readable identifier (e.g., "WH-NYC") distinct from the internal `Id`. |
| **Incoming Transactions** | All stock transactions where this warehouse is the **destination** — stock arriving. |
| **Is Active** | Whether the warehouse currently accepts stock movement. |
| **Outgoing Transactions** | All stock transactions where this warehouse is the **source** — stock leaving. |
| **Warehouse** | A physical or virtual storage location holding inventory. |
| **Warehouse Type** | Functional classification (e.g., "Main", "Satellite", "Distribution", "Returns"). |

## Stock Transaction Domain

### Core Transaction Terms

| Term | Business Definition |
|---|---|
| **Available Stock** | `CurrentStock − SafetyBuffer` — the portion of stock actually eligible to be issued, as opposed to raw on-hand quantity. |
| **Created By** | The user/actor who initiated the transaction — audit trail field. |
| **Currency Code** *(Transaction-level)* | Enum-typed currency of the transaction's cost values. |
| **Quantity** | The amount moved; enforced positive at construction (invariant). |
| **Reason** | Free-text or categorical explanation for the transaction (especially important for Adjustments). |
| **Reference Number** | Identifier linking the transaction to an external business document (PO, sales order, transfer order). |
| **State (Transaction Phase)** | Lifecycle status of the transaction — `TransState.Pending` / `Completed`. |
| **Stock Transaction** | Abstract record of any inventory movement — the general ledger entry of the system. Concrete cost calculation is deferred to subtypes via `abstract CalculateTotalCost()`. |
| **Total Cost (Calculated)** | `CalculateTotalCost()` — abstract on the base, overridden as `UnitCost × Quantity` in all current subtypes. |
| **Transaction Type** | Bitwise-flag classification (`TransType`) used to validate a transaction's direction (In/Out/Neutral) against its concrete subtype. |
| **Transfer as Dual Transaction** | Business rule: a `TransferTransaction` is executed internally as a paired Issue (from source) + Receipt (to destination), committed atomically — not a single ledger row conceptually, even though it is one entity. |
| **Unit Cost** | Per-unit cost recorded at time of transaction (may differ from `Material.UnitPrice`). |
| **Unit of Measure** | The unit the transaction quantity is expressed in — should reconcile with `Material.Unit`. |
| **Weighted Average Cost / Unit Price** | A material's unit cost recalculated as a rolling average across receipts, reflecting blended purchase history rather than the last price paid alone. |

### Receipt Transaction (IN)

| Term | Business Definition |
|---|---|
| **Receipt** | Stock movement *into* a warehouse, typically from a supplier. |
| **Destination Warehouse (FK)** | The warehouse receiving the stock — required. |
| **Supplier (FK)** | The supplier the received material came from — required. |

### Issue Transaction (OUT)

| Term | Business Definition |
|---|---|
| **Issue** | Stock movement *out of* a warehouse, typically to a customer or for consumption. |
| **Source Warehouse (FK)** | The warehouse stock is leaving. |
| **Customer (FK)** | The external party receiving the issued material. |

### Transfer Transaction (Neutral)

| Term | Business Definition |
|---|---|
| **Transfer** | Stock movement between two warehouses with no net change to system-wide inventory. |
| **Source / Destination Warehouse (FK)** | Both required — the origin and receiving warehouses of the transfer. |

### Adjustment Transaction

| Term | Business Definition |
|---|---|
| **Adjustment** | A manual correction to stock levels not tied to a supplier, customer, or warehouse-to-warehouse movement — e.g., cycle count correction or damaged-goods write-off. |
| **Auto-Completion** | Adjustments are created with `State = TransState.Completed` immediately — they bypass the normal Pending → Processing lifecycle. |
| **Reason (required)** | Unlike other subtypes, `Reason` defaults to `"Manual adjustment"` if not supplied — effectively a required-with-fallback field, signalling that adjustments should always be explainable. |

## Financial & Reporting Domain

| Term | Business Definition |
|---|---|
| **COGS Report** | A per-material breakdown of cost of goods sold over a date range — quantity issued × weighted average cost, aggregated per material. |
| **Currency Exposure** | Total spend grouped by currency over a period — used to surface foreign-exchange risk when receipts span multiple currencies (e.g., USD, EGP, EUR) across suppliers. |
| **Period Start / Period End** | The reporting date range boundary common to financial DTOs — defines the window a report's figures are scoped to. |
| **Supplier Cost Comparison** | A per-material, per-supplier cost breakdown (avg/min/max) enabling sourcing decisions — the direct data feed for `SuggestReceipts()`. |
| **Transaction Count** *(reporting)* | The number of transactions rolled up into a given report metric (e.g., how many transactions make up a currency exposure figure). |