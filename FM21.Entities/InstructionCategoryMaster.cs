using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FM21.Entities
{
    [Table("InstructionCategoryMaster")]
    public class InstructionCategoryMaster
    {
        public InstructionCategoryMaster()
        {
            InstructionMaster = new HashSet<InstructionMaster>();
            SiteInstructionCategoryMapping = new HashSet<SiteInstructionCategoryMapping>();
        }

        [Key]
        public int InstructionCategoryID { get; set; }
        public string InstructionCategory { get; set; }
        public bool IsActive { get; set; }

        public virtual ICollection<InstructionMaster> InstructionMaster { get; set; }
        public virtual ICollection<SiteInstructionCategoryMapping> SiteInstructionCategoryMapping { get; set; }
    }
}