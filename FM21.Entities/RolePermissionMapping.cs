using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FM21.Entities
{
    [Table("RolePermissionMapping")]
    public class RolePermissionMapping
    {
        [Key]
        public int RolePermissionID { get; set; }
        public int RoleID { get; set; }
        public int PermissionID { get; set; }
        public byte PermissionType { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public virtual PermissionMaster Permission { get; set; }
        public virtual RoleMaster Role { get; set; }
    }
}