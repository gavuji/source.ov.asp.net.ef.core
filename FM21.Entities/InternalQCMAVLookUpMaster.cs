using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FM21.Entities
{
    [Table("InternalQCMAVLookUpMaster")]
    public class InternalQCMAVLookUpMaster
    {
        [Key]
        public int InternalQCMAVLookUpMasterID { get; set; }
        public decimal TotalBarWeight { get; set; }
        public decimal Subtract { get; set; }
        public bool IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}