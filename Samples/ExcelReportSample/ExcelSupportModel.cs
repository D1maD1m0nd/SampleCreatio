using System.Runtime.Serialization;

namespace Terrasoft.Configuration.SummaryTableVisa;

public class ExcelColumn
{
    public ExcelColumn(string name, int index)
    {
        Name = name;
        Index = index;
    }

    public string Name { get; set; }
    public int Index { get; set; }
}

[DataContract]
public class SupplyPlanItem
{
    [DataMember(Name = "GmmCode")]
    //Код продукта
    public string GmmCode { get; set; }
    [DataMember(Name = "ReceiptQuantity")]
    //Плановое кол-во ед. продукта
    public decimal ReceiptQuantity { get; set; }
    [DataMember(Name = "DateShipmentFactory")]
    //Дата отгрузки с завода
    public DateTime DateShipmentFactory { get; set; }
    [DataMember(Name = "DateShipmentCustomPost")]
    //Дата прибытия на таможенный пост
    public DateTime DateShipmentCustomPost { get; set; } 
    [DataMember(Name = "Version")]
    //Версия плана поставок
    public DateTime VersionDate { get; set; }
}

[DataContract]
public class InfoEntity
{
    [DataMember(Name = "TableMetadata")]
    public Dictionary<string,string> TableMetadata { get; set; }
    [DataMember(Name = "SupplyPlanMapping")]
    public Dictionary<string,string> SupplyPlanMapping { get; set; }
    [DataMember(Name = "MappingValues")]
    public string MappingValues { get; set; }
}