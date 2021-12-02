using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FM21.Entities
{
    [Table("FormulaStatusMaster")]
    public class FormulaStatusMaster
    {
        [Key]
        public int FormulaStatusID { get; set; }
        public string FormulaStatusCode { get; set; }
        public string FormulaStatusCodeDescription { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}