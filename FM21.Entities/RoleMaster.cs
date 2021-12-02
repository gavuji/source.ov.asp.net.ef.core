using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FM21.Entities
{
    [Table("RoleMaster")]
    public class RoleMaster
    {
        public RoleMaster()
        {
            RolePermissionMapping = new HashSet<RolePermissionMapping>();
            UserRole = new HashSet<UserRole>();
        }

        [Key]
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual ICollection<RolePermissionMapping> RolePermissionMapping { get; set; }
        public virtual ICollection<UserRole> UserRole { get; set; }
    }
}