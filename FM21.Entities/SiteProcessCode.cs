using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FM21.Entities
{
    [Table("SiteProcessCode")]
    public class SiteProcessCode
    {
        [Key]
        public int SiteProcessCodeID { get; set; }
        public int SiteID { get; set; }
        public string ProcessCode { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int DisplayOrder { get; set; }
        public virtual SiteMaster Site { get; set; }
    }
}
