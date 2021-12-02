using FM21.Core;
using FM21.Core.Model;
using FM21.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FM21.Service.Interface
{
    public interface IAllergenMasterService
    {
        Task<GeneralResponse<ICollection<AllergenMaster>>> GetAll();
        Task<GeneralResponse<bool>> Create(AllergenMasterModel allergenMasterModel);
        Task<GeneralResponse<bool>> Update(AllergenMasterModel allergenMasterModel);
        Task<GeneralResponse<bool>> Delete(int id);
        Task<GeneralResponse<AllergenMaster>> Get(int id);
        Task<PagedEntityResponse<AllergenMaster>> GetPageWiseData(string filter, int pageIndex, int pageSize, string sortColumn, string sortDirection);
    }
}