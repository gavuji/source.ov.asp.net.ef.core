using FM21.Core;
using FM21.Core.Model;
using FM21.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FM21.Service
{
    public interface ICommonService
    {
        Task<GeneralResponse<ICollection<SiteMaster>>> GetAllSite();
        Task<GeneralResponse<ICollection<ProductTypeMaster>>> GetAllProductType();
        Task<GeneralResponse<ICollection<SiteProductTypeModel>>> GetAllSiteProductType(bool retrieveInActive);
        Task<GeneralResponse<ICollection<InstructionCategoryMaster>>> GetAllInstructionCategory();
        Task<GeneralResponse<ICollection<InstructionCategoryMaster>>> GetAllInstructionCategoryBySiteProductMapID(int siteProductMapID);
        Task<GeneralResponse<ICollection<BrokerMaster>>> GetAllBroker();
        Task<GeneralResponse<ICollection<KosherCodeMaster>>> GetAllKosherCode();
        Task<GeneralResponse<ICollection<HACCPMaster>>> GetAllHACCPData(string hACCPType = "");
        Task<GeneralResponse<ICollection<RMStatusMaster>>> GetAllRMStatus(string rMStatusType = "");
        Task<GeneralResponse<ICollection<StorageConditionMaster>>> GetAllStorageCondition();
        Task<GeneralResponse<ICollection<CountryMaster>>> GetAllCountry();
        Task<GeneralResponse<ICollection<ProductionLineMasterModel>>> GetAllProductionLineBySite(int siteID, int formulaID);
        Task<GeneralResponse<ICollection<ProductionLineMixerModel>>> GetAllProductionLineMixerBySite(int siteID);
        Task<GeneralResponse<ICollection<ReleaseAgentMaster>>> GetAllReleaseAgent();
        Task<GeneralResponse<ICollection<PkoPercentageMaster>>> GetAllPkoPercentage();
        Task<GeneralResponse<ICollection<BarFormatMaster>>> GetAllBarFormatMaster();
        Task<GeneralResponse<ICollection<BarFormatCodeMaster>>> GetAllBarFormatCodeMaster();
        Task<GeneralResponse<ICollection<FormulaTypeMaster>>> GetAllFormulaTypeMaster();
        Task<GeneralResponse<ICollection<SitterWidthMaster>>> GetAllSitterWidth(int siteID);
        Task<GeneralResponse<ICollection<ExtrusionDieMaster>>> GetAllExtrusionDie();
        Task<GeneralResponse<ICollection<InternalQCMAVLookUpMaster>>> GetAllInternalQCMAVLookUp();
    }
}