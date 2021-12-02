using FM21.Core;
using FM21.Core.Model;
using FM21.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FM21.Service
{
    public interface IInstructionGroupMasterService
    {
        Task<GeneralResponse<ICollection<InstructionGroupMaster>>> GetAll();
        Task<GeneralResponse<ICollection<InstructionGroupMasterModel>>> GetGroupBySiteProductAndCategory(int siteProductMapID, int categoryID);
        Task<GeneralResponse<bool>> Create(InstructionGroupMasterModel instructionGroupModel);
        Task<GeneralResponse<bool>> Update(InstructionGroupMasterModel instructionGroupModel);
        Task<GeneralResponse<bool>> Delete(int id);
    }
}