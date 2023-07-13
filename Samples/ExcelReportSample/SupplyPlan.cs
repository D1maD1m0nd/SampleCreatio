namespace Terrasoft.Configuration.HaleonReport.Model
{
    using System;
    using System.Runtime.Serialization;
    using System.Collections.Generic;


    [DataContract]
    public class SupplyPlan
    {
        [DataMember(Name = "DbResult")]
        public int DbResult { get; set; }
        [DataMember(Name = "IsSuccess")]
        public bool IsSuccess { get; set; }
        [DataMember(Name = "ErrorMessage")]
        public string ErrorMessage { get; set; }
        [DataMember(Name = "StackTaraceMessage")]
        public string StackTaraceMessage { get; set; }
        [DataMember(Name = "RowCount")]
        public int RowCount { get; set; }
        [DataMember(Name = "SupplyPlanItems")]
        public List<SupplyPlanItem> SupplyPlanItems { get; set; }
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
        [DataMember(Name = "VersionDate")]
        //Версия плана поставок
        public DateTime VersionDate { get; set; }

        public override string ToString()
        {
            return $"{nameof(GmmCode)}: {GmmCode}, {nameof(ReceiptQuantity)}: {ReceiptQuantity}, {nameof(DateShipmentFactory)}: {DateShipmentFactory}, {nameof(DateShipmentCustomPost)}: {DateShipmentCustomPost}, {nameof(VersionDate)}: {VersionDate}";
        }
    }

    public class ProductEntity
    {
        public Guid ProductId { get; set; }
        public string Site { get; set; }
        public string Brand { get; set; }
        public decimal UnitPallet { get; set; }
        public decimal UnitTrucks { get; set; }
        public decimal PricePerPallet { get; set; }
    }
}