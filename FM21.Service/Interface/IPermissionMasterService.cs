using FM21.Core;
using FM21.Core.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FM21.Service
{
    public interface IPermissionMasterService
    {
        Task<GeneralResponse<ICollection<RolePermissionMatrix>>> GetRolePermissionMatrix();
        Task<GeneralResponse<bool>> UpdateRoleMatrix(List<RolePermissionAccess> roleAccessList);
    }
}