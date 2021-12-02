using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FM21.Entities
{
    [Table("SiteInstructionCategoryMapping")]
    public class SiteInstructionCategoryMapping
    {
        [Key]
        public int SiteInstructionCategoryMapID { get; set; }
        public int SiteID { get; set; }
        public int InstructionCategoryID { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }

        public virtual InstructionCategoryMaster InstructionCategory { get; set; }
        public virtual SiteMaster Site { get; set; }
    }
}