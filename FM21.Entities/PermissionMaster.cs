using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FM21.Entities
{
    [Table("PermissionMaster")]
    public class PermissionMaster
    {
        public PermissionMaster()
        {
            RolePermissionMapping = new HashSet<RolePermissionMapping>();
        }

        [Key]
        public int PermissionID { get; set; }
        public string PermissionFor { get; set; }
        public bool?   IsDeleted { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public virtual ICollection<RolePermissionMapping> RolePermissionMapping { get; set; }
    }
}