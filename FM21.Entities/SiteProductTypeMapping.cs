using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FM21.Entities
{
    [Table("SiteMaster")]
    public class SiteProductTypeMapping
    {
        public SiteProductTypeMapping()
        {
            InstructionMaster = new HashSet<InstructionMaster>();
            FormulaMaster = new HashSet<FormulaMaster>();
            PowderUnitServingSiteMapping = new HashSet<PowderUnitServingSiteMapping>();
        }

        [Key]
        public int SiteProductMapID { get; set; }
        public int SiteID { get; set; }
        public int ProductTypeID { get; set; }

        public virtual ProductTypeMaster ProductType { get; set; }
        public virtual SiteMaster Site { get; set; }
        public virtual ICollection<InstructionMaster> InstructionMaster { get; set; }
        public virtual ICollection<FormulaMaster> FormulaMaster { get; set; }
        public virtual ICollection<PowderUnitServingSiteMapping> PowderUnitServingSiteMapping { get; set; }
    }
}