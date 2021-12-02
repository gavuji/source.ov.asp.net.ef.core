using FM21.Core;
using FM21.Core.Model;
using FM21.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FM21.Service
{
    public interface IInstructionMasterService
    {
        Task<GeneralResponse<ICollection<InstructionMasterModel>>> GetAll();
        Task<PagedEntityResponse<InstructionMasterModel>> GetPageWiseData(SearchFilter searchFilter);
        Task<PagedEntityResponse<InstructionMasterModel>> GetSearchListWithFilter(SearchFilter searchFilter, int siteProductMapID, int instructionCategoryID);
        Task<GeneralResponse<InstructionMaster>> Get(int id);
        Task<GeneralResponse<ICollection<InstructionMasterModel>>> GetBySiteProductCategoryAndGroup(int siteProductMapID, int categoryID, int groupID);
        Task<GeneralResponse<bool>> Create(InstructionMasterModel instructionModel);
        Task<GeneralResponse<bool>> Update(InstructionMasterModel instructionModel);
        Task<GeneralResponse<bool>> Delete(int id);
        Task<GeneralResponse<bool>> UpdateInstructionGroupOrder(int siteProductMapID, int categoryID, ICollection<InstructionGroupMasterModel> lstInstructionGroup);
        Task<GeneralResponse<bool>> UpdateInstructionOrder(ICollection<InstructionMasterModel> lstInstruction);
    }
}