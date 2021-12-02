using System.Collections.Generic;

namespace FM21.Core.Model
{
    public class RolePermissionMatrix
    {
        public int permissionId { get; set; }
        public string name { get; set; }

        public List<RolePermissionAccess> roleAccessList { get; set; } = new List<RolePermissionAccess>();
    }
    public class RolePermissionAccess
    {
        public int rolePermissionID { get; set; }
        public int roleID { get; set; }
        public int permissionID { get; set; }
        public string roleName { get; set; }
        public bool isAccess { get; set; }
    }
}