using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FM21.Entities
{
    [Table("FormulaRossCode")]
    public class FormulaRossCode
    {
        [Key]
        public int FormulaRossCodeID { get; set; }
        public int SiteID { get; set; }
        public int IncrementNumber { get; set; }
        public string CustomerName { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public virtual SiteMaster Site { get; set; }
    }
}
