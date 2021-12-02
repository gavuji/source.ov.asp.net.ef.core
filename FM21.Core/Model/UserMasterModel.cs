using System.Collections.Generic;

namespace FM21.Core.Model
{
    public class UserMasterModel
    {
        public int UserID { get; set; }
        public string DomainFullName { get; set; }
        public string DisplayName { get; set; }
        public string[] AssignedRoles { get; set; }
        public int[] AssignedRoleList { get; set; }
        public string AssignedRoleNames { get; set; }
        public List<int> AssignedPermissionList { get; set; }
        public bool IsDeleted { get; set; }
    }
}