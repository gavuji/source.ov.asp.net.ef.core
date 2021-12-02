using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FM21.Entities
{
    [Table("FormulaRegulatoryCategoryMaster")]
    public class FormulaRegulatoryCategoryMaster
    {
        public FormulaRegulatoryCategoryMaster()
        {
            FormulaMaster = new HashSet<FormulaMaster>();
        }

        [Key]
        public int FormulaRegulatoryCateoryID { get; set; }
        public string FormulaRegulatoryCategoryDescription { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual ICollection<FormulaMaster> FormulaMaster { get; set; }
    }
}