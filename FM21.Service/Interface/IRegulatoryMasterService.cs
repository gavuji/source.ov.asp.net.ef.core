using FM21.Core;
using FM21.Core.Model;
using FM21.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FM21.Service
{
    public interface IRegulatoryMasterService
    {
        Task<GeneralResponse<ICollection<RegulatoryMaster>>> GetAll();
        Task<PagedEntityResponse<RegulatoryModel>> GetPageWiseData(string filter, int pageIndex, int pageSize, string sortColumn, string sortDirection);
        Task<GeneralResponse<RegulatoryMaster>> Get(int id);
        Task<GeneralResponse<bool>> Create(RegulatoryModel entity);
        Task<GeneralResponse<bool>> Update(RegulatoryModel entity);
        Task<GeneralResponse<bool>> Delete(int id); 
        Task<GeneralResponse<ICollection<UnitOfMeasurementMaster>>> GetUnitOfMeasurement();
    }
}