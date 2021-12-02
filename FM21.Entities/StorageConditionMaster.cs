using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FM21.Entities
{
    [Table("StorageConditionMaster")]
    public class StorageConditionMaster
    {
        [Key]
        public int StorageConditionID { get; set; }
        public string StorageDescription { get; set; }
        public string StorageType { get; set; }
        public int? StorageGroupNumber { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}