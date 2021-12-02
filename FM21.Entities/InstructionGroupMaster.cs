using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FM21.Entities
{
    [Table("InstructionGroupMaster")]
    public class InstructionGroupMaster
    {
        public InstructionGroupMaster()
        {
            InstructionMaster = new HashSet<InstructionMaster>();
        }

        [Key]
        public int InstructionGroupID { get; set; }
        public string InstructionGroupName { get; set; }
        public bool IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public virtual ICollection<InstructionMaster> InstructionMaster { get; set; }
    }
}