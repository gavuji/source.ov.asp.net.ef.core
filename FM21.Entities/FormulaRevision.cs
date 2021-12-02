using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FM21.Entities
{
    [Table("FormulaRevision")]
    public class FormulaRevision
    {
        [Key]
        public int FormulaRevisionID { get; set; }
        public int FormulaID { get; set; }
        public string ProcessCode { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int RevisionNumber { get; set; }
        public virtual FormulaMaster FormulaMaster { get; set; }
    }
}
