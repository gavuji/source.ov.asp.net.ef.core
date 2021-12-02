using FM21.Core;
using FM21.Core.Model;
using FM21.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FM21.Service
{
    public interface IRoleMasterService
    {
        Task<GeneralResponse<ICollection<RoleMaster>>> GetAll();
        Task<PagedEntityResponse<RoleMaster>> GetPageWiseData(SearchFilter searchFilter);
        Task<GeneralResponse<RoleMaster>> Get(int id);
        Task<GeneralResponse<bool>> Create(RoleMasterModel roleMasterModel);
        Task<GeneralResponse<bool>> Update(RoleMasterModel roleMasterModel);
        Task<GeneralResponse<bool>> Delete(int id);
    }
}