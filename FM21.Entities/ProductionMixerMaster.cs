using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FM21.Entities
{
    [Table("ProductionMixerMaster")]
    public class ProductionMixerMaster
    {
        public ProductionMixerMaster()
        {
            ProductionLineMixerMapping = new HashSet<ProductionLineMixerMapping>();
            FormulaProductionLineMapping = new HashSet<FormulaProductionLineMapping>();
        }

        [Key]
        public int ProductionMixerID { get; set; }
        public string MixerDescription { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual ICollection<ProductionLineMixerMapping> ProductionLineMixerMapping { get; set; }
        public virtual ICollection<FormulaProductionLineMapping> FormulaProductionLineMapping { get; set; }
    }
}