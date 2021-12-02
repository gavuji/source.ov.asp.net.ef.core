using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FM21.Entities
{
    [Table("FormulaTypeMaster")]
    public class FormulaTypeMaster
    {
        public FormulaTypeMaster()
        {
            FormulaMaster = new HashSet<FormulaMaster>();
            FormulaTypeProductMapping = new HashSet<FormulaTypeProductMapping>();
            FormulaChangeCode = new HashSet<FormulaChangeCode>();
        }

        [Key]
        public int FormulaTypeID { get; set; }
        public string FormulaTypeCode { get; set; }
        public string FormulaDescription { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual ICollection<FormulaMaster> FormulaMaster { get; set; }
        public virtual ICollection<FormulaTypeProductMapping> FormulaTypeProductMapping { get; set; }
        public virtual ICollection<FormulaChangeCode> FormulaChangeCode { get; set; }
    }
}