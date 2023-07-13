 namespace Terrasoft.Configuration.SummaryTableVisa
{
	using System.Runtime.Serialization;
	using System;
    using System.Collections.Generic;

    [DataContract(Name = "CostVisa")]
    public class CostVisa
    {
        [DataMember(Name = "ShareFilialItems")]
        public List<ShareFilialItem> ShareFilialItems { get; set; }
        [DataMember(Name = "TableVisaId")]
        public Guid TableVisaId { get; set; }
        [DataMember(Name = "SummaryData")]
        public SummaryData SummaryData { get; set; }
        
        [DataMember(Name = "Error")]
        public string Error { get; set; }

        [DataMember(Name = "CostItemsResult")]
        public List<CostItem> ListCostItem { get; set; }

        [DataMember(Name = "CostItemColumn")]
        public List<CostItemColumn> ListColumn { get; set; }

        [DataMember(Name = "MetaData")]
        public CostVisaMetaData MetaData { get; set; }
    }
    [DataContract(Name = "SummaryData")]
    public class ShareFilialItem
    {
        [DataMember(Name = "FilialId")]
        public Guid FilialId { get; set; }
        [DataMember(Name = "ShareFilialSum")]
        public decimal ShareFilialSum { get; set; }
    }
    [DataContract(Name = "SummaryData")]
    public class SummaryData
    {
        [DataMember(Name = "ReserveSum")]
        public decimal ReserveSum { get; set; }

        [DataMember(Name = "TotalSumPlanSummary")]
        public decimal TotalSumPlanSummary { get; set; }

        [DataMember(Name = "TotalSumPlanYearBranchSummary")]
        public decimal TotalSumPlanYearBranchSummary { get; set; }

        [DataMember(Name = "TotalSumBudgetPlan")]
        public decimal TotalSumBudgetPlan { get; set; }

        [DataMember(Name = "TotalSumPlanYearBranch")]
        public decimal TotalSumBudgetPlanBranch { get; set; }
    }

    [DataContract(Name = "CostItemsResult")]
    public class CostItem
    {
        [DataMember(Name = "FilialId")]
        public Guid FilialId { get; set; }
        [DataMember(Name = "DeltaStarterPlan")]
        public decimal DeltaStarterPlan { get; set; }
        [DataMember(Name = "DeltaChanged")]
        public decimal DeltaChanged { get; set; }
        [DataMember(Name = "IsAproveBrendManager")]
        public bool IsAproveBrendManager { get; set; }

        [DataMember(Name = "IsAproveOwnerFilial")]
        public bool IsAproveOwnerFilial { get; set; }

        [DataMember(Name = "YearName")]
        public string YearName { get; set; }

        [DataMember(Name = "DetailBudgetId")]
        public Guid DetailBudgetId { get; set; }

        [DataMember(Name = "VisaId")]
        public Guid VisaId { get; set; }

        [DataMember(Name = "VisaBudgetFilialId")]
        public Guid VisaBudgetFilialId { get; set; }

        [DataMember(Name = "FilialName")]
        public string FilialName { get; set; }

        [DataMember(Name = "GroupItemName")]
        public string GroupItemName { get; set; }

        [DataMember(Name = "CostItemName")]
        public string CostItemName { get; set; }

        [DataMember(Name = "OwnerName")]
        public string OwnerName { get; set; }

        [DataMember(Name = "BrandName")]
        public string BrandName { get; set; }

        [DataMember(Name = "FirstQuarterPlanSum")]
        public decimal FirstQuarterPlanSum { get; set; }

        [DataMember(Name = "FirstQuarterNewSum")]
        public decimal FirstQuarterNewSum { get; set; }

        [DataMember(Name = "FirstQuarterPlanSumBranch")]
        public decimal FirstQuarterPlanSumBranch { get; set; }

        [DataMember(Name = "SecondQuarterPlanSum")]
        public decimal SecondQuarterPlanSum { get; set; }

        [DataMember(Name = "SecondQuarterNewSum")]
        public decimal SecondQuarterNewSum { get; set; }

        [DataMember(Name = "SecondQuarterPlanSumBranch")]
        public decimal SecondQuarterPlanSumBranch { get; set; }

        [DataMember(Name = "ThirdQuarterPlanSum")]
        public decimal ThirdQuarterPlanSum { get; set; }

        [DataMember(Name = "ThirdQuarterNewSum")]
        public decimal ThirdQuarterNewSum { get; set; }

        [DataMember(Name = "ThirdQuarterPlanSumBranch")]
        public decimal ThirdQuarterPlanSumBranch { get; set; }

        [DataMember(Name = "FourthQuarterPlanSum")]
        public decimal FourthQuarterPlanSum { get; set; }

        [DataMember(Name = "FourthQuarterNewSum")]
        public decimal FourthQuarterNewSum { get; set; }

        [DataMember(Name = "FourthQuarterPlanSumBranch")]
        public decimal FourthQuarterPlanSumBranch { get; set; }

        [DataMember(Name = "TotalSumPlan")]
        public decimal TotalSumPlan { get; set; }

        [DataMember(Name = "TotalSumPlanYearBranch")]
        public decimal TotalSumPlanYearBranch { get; set; }
        [DataMember(Name = "TotalYearNewSum")]
        public decimal TotalYearNewSum { get; set; }

    }

    public class CostItemColumn
    {
        [DataMember(Name = "Path")]
        public string Path { get; set; }

        [DataMember(Name = "Name")]
        public string Name { get; set; }

        [DataMember(Name = "ItemCostKey")]
        public string ItemCostKey { get; set; }
    }

    public class CostVisaMetaData
    {
        [DataMember(Name = "FilialId")]
        public Guid FilialId { get; set; }
        [DataMember(Name = "YearName")]
        public string YearName { get; set; }

        [DataMember(Name = "YearId")]
        public Guid YearId { get; set; }

        [DataMember(Name = "BrandName")]
        public string BrandName { get; set; }

        [DataMember(Name = "BrandId")]
        public Guid BrandId { get; set; }

        [DataMember(Name = "CurrentUserId")]
        public Guid CurrentUserId { get; set; }
    }
    public class PairVisaFilial
    {
        public Guid VisaId { get; set; }
        public Guid VisaBudgetFilialId { get; set; }
    }
    public class CostVisaSaveData
    {
        [DataMember(Name = "FilialName")]
        public string FilialName { get; set; }
        [DataMember(Name = "IsAproveBrendManager")]
        public bool IsAproveBrendManager { get; set; }

        [DataMember(Name = "IsAproveOwnerFilial")]
        public bool IsAproveOwnerFilial { get; set; }

        [DataMember(Name = "DetailBudgetId")]
        public Guid DetailBudgetId { get; set; }

        [DataMember(Name = "VisaId")]
        public Guid VisaId { get; set; }

        [DataMember(Name = "VisaBudgetFilialId")]
        public Guid VisaBudgetFilialId { get; set; }

        [DataMember(Name = "TotalSumPlan")]
        public decimal TotalSumPlan { get; set; }

        [DataMember(Name = "FirstQuarterNewSum")]
        public decimal FirstQuarterNewSum { get; set; }

        [DataMember(Name = "SecondQuarterNewSum")]
        public decimal SecondQuarterNewSum { get; set; }

        [DataMember(Name = "ThirdQuarterNewSum")]
        public decimal ThirdQuarterNewSum { get; set; }

        [DataMember(Name = "FourthQuarterNewSum")]
        public decimal FourthQuarterNewSum { get; set; }
    }
 }