using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FM21.Entities
{
    [Table("FormulaTypeProductMapping")]
    public class FormulaTypeProductMapping
    {
        [Key]
        public int FormulaTypeProductMapID { get; set; }
        public int FormulaTypeID { get; set; }
        public int ProductID { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual FormulaTypeMaster FormulaType { get; set; }
        public virtual ProductTypeMaster Product { get; set; }
    }
}