using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FM21.Entities
{
    [Table("SiteProductionLineMapping")]
    public class SiteProductionLineMapping
    {
        public SiteProductionLineMapping()
        {
            ProductionLineMixerMapping = new HashSet<ProductionLineMixerMapping>();
            FormulaMaster = new HashSet<FormulaMaster>();
        }

        [Key]
        public int SiteProductionLineMapID { get; set; }
        public int SiteID { get; set; }
        public int ProductionLineID { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual ProductionLineMaster ProductionLine { get; set; }
        public virtual SiteMaster Site { get; set; }
        public virtual ICollection<ProductionLineMixerMapping> ProductionLineMixerMapping { get; set; }
        public virtual ICollection<FormulaMaster> FormulaMaster { get; set; }
    }
}