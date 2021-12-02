using FM21.Core;
using FM21.Core.Model;
using FM21.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FM21.Service
{
    public interface IProjectMasterService
    {
        Task<GeneralResponse<ICollection<ProjectMasterModel>>> GetAll();
        Task<PagedEntityResponse<ProjectMasterModel>> GetPageWiseData(SearchFilter searchFilter);
        Task<GeneralResponse<ProjectMaster>> Get(int id);
        Task<GeneralResponse<bool>> Create(ProjectMasterModel entity);
        Task<GeneralResponse<bool>> Update(ProjectMasterModel entity);
        Task<GeneralResponse<bool>> Delete(int id);
        Task<GeneralResponse<int>> GetNextProjectCode();
    }
}