using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FM21.Entities
{
    [Table("FormulaCriteriaMapping")]
    public class FormulaCriteriaMapping
    {
        [Key]
        public int FormulaCriteriaMapID { get; set; }
        public int FormulaID { get; set; }
        public int CriteriaID { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual CriteriaMaster CriteriaMaster { get; set; }
        public virtual FormulaMaster FormulaMaster { get; set; }
    }
}