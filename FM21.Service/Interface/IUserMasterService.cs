using FM21.Core;
using FM21.Core.Model;
using System.Threading.Tasks;

namespace FM21.Service
{
    public interface IUserMasterService
    {
        Task<GeneralResponse<UserMasterModel>> GetCurrentUser(string currentUserName);
        Task<PagedEntityResponse<UserMasterModel>> GetAllADUsersAlongWithRoles(SearchFilter searchFilter);
        Task<GeneralResponse<bool>> UpdateUserRolePermission(UserMasterModel userMasterModel);
    }
}