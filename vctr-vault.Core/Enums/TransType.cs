namespace vctr_vault.Core.Enums;
[Flags]
public enum TransType
{
//In
Purchase=1,
Return =2,
//Out
Consumption=4,
Scrap=8,
SupplierReturn=10,
//Neutral
Transfere=12,
Adjustment=16, //In or Out
In= Purchase|Return|Adjustment,
Out= Consumption|Scrap|Adjustment,
Neutral=Transfere //Other neutral cases could be added
}