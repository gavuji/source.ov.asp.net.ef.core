using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FM21.Entities
{
    [Table("InstructionMaster")]
    public class InstructionMaster
    {
        [Key]
        public int InstructionMasterID { get; set; }
        public int SiteProductMapID { get; set; }
        public int InstructionCategoryID { get; set; }
        public int InstructionGroupID { get; set; }
        public string DescriptionEn { get; set; }
        public string DescriptionFr { get; set; }
        public string DescriptionEs { get; set; }
        public int GroupDisplayOrder { get; set; }
        public int GroupItemDisplayOrder { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual SiteProductTypeMapping SiteProductMap { get; set; }
        public virtual InstructionCategoryMaster InstructionCategory { get; set; }
        public virtual InstructionGroupMaster InstructionGroup { get; set; }
    }
}