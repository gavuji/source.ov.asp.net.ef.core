using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FM21.Entities
{
    [Table("PowderBlenderMaster")]
    public class PowderBlenderMaster
    {
        public PowderBlenderMaster()
        {
            FormulaMaster = new HashSet<FormulaMaster>();
            PowderBlenderSiteMapping = new HashSet<PowderBlenderSiteMapping>();
        }

        [Key]
        public int PowderBlenderID { get; set; }
        public string BlenderDescription { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int DisplayOrder { get; set; }

        public virtual ICollection<FormulaMaster> FormulaMaster { get; set; }
        public virtual ICollection<PowderBlenderSiteMapping> PowderBlenderSiteMapping { get; set; }
    }
}