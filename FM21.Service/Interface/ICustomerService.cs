using FM21.Core;
using FM21.Core.Model;
using FM21.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FM21.Service
{
    public interface ICustomerService
    {
        
        Task<GeneralResponse<ICollection<Customer>>> GetAll();
        Task<PagedEntityResponse<Customer>> GetPageWiseData(string filter, int pageIndex, int pageSize, string sortColumn, string sortDirection);
        Task<GeneralResponse<Customer>> Get(int id);
        Task<GeneralResponse<bool>> Create(CustomerModel entity);
        Task<GeneralResponse<bool>> Update(CustomerModel entity);
        Task<GeneralResponse<bool>> Delete(int id);
    }
}