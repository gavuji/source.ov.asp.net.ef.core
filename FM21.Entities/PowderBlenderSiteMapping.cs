using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FM21.Entities
{
    [Table("PowderBlenderSiteMapping")]
    public class PowderBlenderSiteMapping
    {
        [Key]
        public int PowderBlenderSiteMapID { get; set; }
        public int PowderBlenderID { get; set; }
        public int SiteID { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual PowderBlenderMaster PowderBlender { get; set; }
        public virtual SiteMaster Site { get; set; }
    }
}