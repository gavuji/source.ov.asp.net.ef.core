using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FM21.Entities
{
    [Table("FormulaDetailMapping")]
    public class FormulaDetailMapping
    {
        [Key]
        public int FormulaDetailMapID { get; set; }
        public int FormulaID { get; set; }
        public int? ReferenceID { get; set; }
        public int? ReferenceType { get; set; }
        public string InstructionDescription { get; set; }
        public decimal? SubgroupPercent { get; set; }
        public decimal? Amount { get; set; }
        public decimal? TotalPercent { get; set; }
        public decimal? OvgPercent { get; set; }
        public decimal? Target { get; set; }
        public int RowNumber { get; set; }
        public string Code { get; set; }
        public string Unit { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual FormulaMaster Formula { get; set; }
    }
}