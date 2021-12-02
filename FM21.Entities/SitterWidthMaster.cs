using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FM21.Entities
{
    [Table("SitterWidthMaster")]
    public class SitterWidthMaster
    {
        [Key]
        public int SitterWidthID { get; set; }
        public string SitterWidth { get; set; }
        public int SiteID { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public virtual SiteMaster Site { get; set; }
    }
}