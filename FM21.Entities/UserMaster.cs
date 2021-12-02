using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FM21.Entities
{
    [Table("UserMaster")]
    public class UserMaster
    {
        public UserMaster()
        {
            UserRole = new HashSet<UserRole>();
        }

        [Key]
        public int UserID { get; set; }
        public string DomainFullName { get; set; }
        public string DisplayName { get; set; }
        public bool IsDeleted { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual ICollection<UserRole> UserRole { get; set; }
    }
}