using FM21.Core;
using FM21.Core.Model;
using FM21.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FM21.Service.Interface
{
    public interface ISupplierMasterService
    {
        Task<GeneralResponse<ICollection<SupplierMaster>>> GetAll();
        Task<PagedEntityResponse<SupplierMaster>> GetPageWiseData(string filter, int pageIndex, int pageSize, string sortColumn, string sortDirection);
        Task<GeneralResponse<SupplierMaster>> Get(int id);
        Task<GeneralResponse<bool>> Create(SupplierMasterModel supplierMasterModel);
        Task<GeneralResponse<bool>> Update(SupplierMasterModel supplierMasterModel);
        Task<GeneralResponse<bool>> Delete(int id);
    }
}