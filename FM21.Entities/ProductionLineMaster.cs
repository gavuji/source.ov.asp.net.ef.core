using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FM21.Entities
{
    [Table("ProductionLineMaster")]
    public class ProductionLineMaster
    {
        public ProductionLineMaster()
        {
            SiteProductionLineMapping = new HashSet<SiteProductionLineMapping>();
            FormulaProductionLineMapping = new HashSet<FormulaProductionLineMapping>();
        }

        [Key]
        public int ProductionLineID { get; set; }
        public string LineCode { get; set; }
        public string LineDescription { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual ICollection<SiteProductionLineMapping> SiteProductionLineMapping { get; set; }
        public virtual ICollection<FormulaProductionLineMapping> FormulaProductionLineMapping { get; set; }
    }
}