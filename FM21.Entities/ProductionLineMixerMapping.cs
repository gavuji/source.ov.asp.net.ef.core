using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FM21.Entities
{
    [Table("ProductionLineMixerMapping")]
    public class ProductionLineMixerMapping
    {
        public ProductionLineMixerMapping()
        {
            FormulaProductionLineMapping = new HashSet<FormulaProductionLineMapping>();
        }

        [Key]
        public int ProductionLineMixerMapID { get; set; }
        public int ProductionMixerID { get; set; }
        public int SiteProductionLineID { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual ProductionMixerMaster ProductionMixerMaster { get; set; }
        public virtual SiteProductionLineMapping SiteProductionLine { get; set; }
        public virtual ICollection<FormulaProductionLineMapping> FormulaProductionLineMapping { get; set; }
    }
}