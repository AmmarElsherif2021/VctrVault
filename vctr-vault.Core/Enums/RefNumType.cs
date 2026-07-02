namespace vctr_vault.Core.Enums;
[Flags]
public enum RefNumType
{
    PO, // in case of issuing a transaction -- created by consumer
    InvoiceNum, // in case of purchasing a transaction -- created by warehouse
}