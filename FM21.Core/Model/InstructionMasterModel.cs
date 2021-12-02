using AutoMapper.Configuration.Annotations;

namespace FM21.Core.Model
{
    public class InstructionMasterModel
    {
        public int InstructionMasterID { get; set; }
        public int SiteProductMapID { get; set; }
        [Ignore]
        public string SiteProductMap { get; set; }
        public int InstructionCategoryID { get; set; }
        [Ignore]
        public string InstructionCategory { get; set; }
        public int InstructionGroupID { get; set; }
        [Ignore]
        public string InstructionGroup { get; set; }
        public string DescriptionEn { get; set; }
        public string DescriptionFr { get; set; }
        public string DescriptionEs { get; set; }
        [Ignore]
        public int GroupDisplayOrder { get; set; }
        [Ignore]
        public int GroupItemDisplayOrder { get; set; }
    }
}