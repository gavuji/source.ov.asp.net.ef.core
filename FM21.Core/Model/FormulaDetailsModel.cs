using AutoMapper.Configuration.Annotations;

namespace FM21.Core.Model
{
    public class FormulaDetailsModel
    {
        public int FormulaDetailMapID { get; set; }
        public int FormulaID { get; set; }  
        public int? ReferenceID { get; set; }
        public int? ReferenceType { get; set; }
        public decimal? SubgroupPercent { get; set; }
        public decimal? Amount { get; set; }
        public decimal? TotalPercent { get; set; }
        public decimal? OvgPercent { get; set; }
        public decimal? Target { get; set; }
        public int? RowNumber { get; set; }
        public string Code { get; set; }
        public string Unit { get; set; }
        public int? CreatedBy { get; set; }
        [Ignore]
        public int RowID { get; set; }
        [Ignore]
        public int ParentRowID { get; set; }
        [Ignore]
        public string HierarchyRowID { get; set; }
        [Ignore]
        public int Level { get; set; }
        [Ignore]
        public string PartCode { get; set; }
        [Ignore]
        public string Description { get; set; }
        [Ignore]
        public bool IsAlertInfoExist { get; set; }
    }
}