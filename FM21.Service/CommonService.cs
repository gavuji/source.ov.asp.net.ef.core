using AutoMapper;
using FM21.Core;
using FM21.Core.Model;
using FM21.Data.Infrastructure;
using FM21.Entities;
using FM21.Service.Caching;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FM21.Service
{
    public class CommonService : BaseService, ICommonService
    {
        private readonly IRepository<SiteMaster> siteMasterRepository;
        private readonly IRepository<ProductTypeMaster> productTypeMasterRepository;
        private readonly IRepository<SiteProductTypeMapping> siteProductTypeMappingRepository;
        private readonly IRepository<InstructionCategoryMaster> instructionCategoryMasterRepository;
        private readonly IRepository<BrokerMaster> brokerMasterRepository;
        private readonly IRepository<KosherCodeMaster> kosherCodeMasterRepository;
        private readonly IRepository<HACCPMaster> hACCPMasterRepository;
        private readonly IRepository<RMStatusMaster> rMStatusMasterRepository;
        private readonly IRepository<StorageConditionMaster> storageConditionMasterRepository;
        private readonly IRepository<CountryMaster> countryMasterRepository;
        private readonly IRepository<SiteProductionLineMapping> siteProductionLineMapRepository;
        private readonly IRepository<ProductionLineMixerMapping> productionLineMixerMapRepository;
        private readonly IRepository<ReleaseAgentMaster> releaseAgentMasterRepository;
        private readonly IRepository<PkoPercentageMaster> pkoPercentageMasterRepository;
        private readonly IRepository<BarFormatMaster> barFormatMasterRepository;
        private readonly IRepository<BarFormatCodeMaster> barFormatCodeMasterRepository;
        private readonly IRepository<FormulaTypeMaster> formulaTypeMasterRepository;
        private readonly IRepository<SitterWidthMaster> sitterWidthMasterRepository;
        private readonly IRepository<ExtrusionDieMaster> extrusionDieMasterRepository;
        private readonly IRepository<InternalQCMAVLookUpMaster> internalQCMAVLookUpMasterRepository;
        
        public CommonService(IServiceProvider provider, IExceptionHandler exceptionHandler, IMapper mapper, IUnitOfWork unitOfWork, ICacheProvider cacheProvider,
            IRepository<SiteMaster> siteMasterRepository, IRepository<ProductTypeMaster> productTypeMasterRepository, IRepository<SiteProductTypeMapping> siteProductTypeMappingRepository, 
            IRepository<InstructionCategoryMaster> instructionCategoryMasterRepository, IRepository<BrokerMaster> brokerMasterRepository, IRepository<KosherCodeMaster> kosherCodeMasterRepository, 
            IRepository<HACCPMaster> hACCPMasterRepository, IRepository<RMStatusMaster> rMStatusMasterRepository, IRepository<StorageConditionMaster> storageConditionMasterRepository,
            IRepository<CountryMaster> countryMasterRepository, IRepository<SiteProductionLineMapping> siteProductionLineMapRepository, IRepository<ProductionLineMixerMapping> productionLineMixerRepository,
            IRepository<ReleaseAgentMaster> releaseAgentMasterRepository, IRepository<PkoPercentageMaster> pkoPercentageMasterRepository, IRepository<BarFormatMaster> barFormatMasterRepository, 
            IRepository<BarFormatCodeMaster> barFormatCodeMasterRepository, IRepository<FormulaTypeMaster> formulaTypeMasterRepository, IRepository<SitterWidthMaster> sitterWidthMasterRepository,
            IRepository<ExtrusionDieMaster> extrusionDieMasterRepository, IRepository<InternalQCMAVLookUpMaster> internalQCMAVLookUpMasterRepository)
            : base(provider, exceptionHandler, mapper, unitOfWork, cacheProvider)
        {
            this.siteMasterRepository = siteMasterRepository;
            this.productTypeMasterRepository = productTypeMasterRepository;
            this.siteProductTypeMappingRepository = siteProductTypeMappingRepository;
            this.instructionCategoryMasterRepository = instructionCategoryMasterRepository;
            this.brokerMasterRepository = brokerMasterRepository;
            this.kosherCodeMasterRepository = kosherCodeMasterRepository;
            this.hACCPMasterRepository = hACCPMasterRepository;
            this.rMStatusMasterRepository = rMStatusMasterRepository;
            this.storageConditionMasterRepository = storageConditionMasterRepository;
            this.countryMasterRepository = countryMasterRepository;
            this.siteProductionLineMapRepository = siteProductionLineMapRepository;
            this.productionLineMixerMapRepository = productionLineMixerRepository;
            this.releaseAgentMasterRepository = releaseAgentMasterRepository;
            this.pkoPercentageMasterRepository = pkoPercentageMasterRepository;
            this.barFormatMasterRepository = barFormatMasterRepository;
            this.barFormatCodeMasterRepository = barFormatCodeMasterRepository;
            this.formulaTypeMasterRepository = formulaTypeMasterRepository;
            this.sitterWidthMasterRepository = sitterWidthMasterRepository;
            this.extrusionDieMasterRepository = extrusionDieMasterRepository;
            this.internalQCMAVLookUpMasterRepository = internalQCMAVLookUpMasterRepository;
        }

        public async Task<GeneralResponse<ICollection<SiteMaster>>> GetAllSite()
        {
            var response = new GeneralResponse<ICollection<SiteMaster>>();
            try
            {
                response.Data = await siteMasterRepository.GetManyAsync(o => o.IsActive, true);
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }

        public async Task<GeneralResponse<ICollection<ProductTypeMaster>>> GetAllProductType()
        {
            var response = new GeneralResponse<ICollection<ProductTypeMaster>>();
            try
            {
                response.Data = await productTypeMasterRepository.GetManyAsync(o => o.IsActive, true);
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }

        public async Task<GeneralResponse<ICollection<SiteProductTypeModel>>> GetAllSiteProductType(bool retrieveInActive)
        {
            var response = new GeneralResponse<ICollection<SiteProductTypeModel>>();
            try
            {
                await Task.Run(() =>
                {
                    var arrList = siteProductTypeMappingRepository.Query(true)
                                        .Include(o => o.Site)
                                        .Include(o => o.ProductType)
                                        .Where(o => o.Site.IsActive || retrieveInActive)
                                        .ToList();
                    response.Data = mapper.Map<IList<SiteProductTypeMapping>, IList<SiteProductTypeModel>>(arrList);
                });
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }

        public async Task<GeneralResponse<ICollection<InstructionCategoryMaster>>> GetAllInstructionCategory()
        {
            var response = new GeneralResponse<ICollection<InstructionCategoryMaster>>();
            try
            {
                response.Data = await instructionCategoryMasterRepository.GetManyAsync(o => o.IsActive, true);
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }
        public async Task<GeneralResponse<ICollection<InstructionCategoryMaster>>> GetAllInstructionCategoryBySiteProductMapID(int siteProductMapID)
        {
            var response = new GeneralResponse<ICollection<InstructionCategoryMaster>>();
            try
            {
               await Task.Run(() =>
               {
                    response.Data = siteProductTypeMappingRepository.Query(true)
                                        .Where(o => o.SiteProductMapID == Convert.ToInt32(siteProductMapID))
                                        .Include(o => o.Site)
                                        .ThenInclude(o => o.SiteInstructionCategoryMapping)
                                        .ThenInclude(o => o.InstructionCategory)
                                        .FirstOrDefault()
                                        .Site
                                        .SiteInstructionCategoryMapping
                                        .Select(o => new InstructionCategoryMaster
                                        {
                                            InstructionCategoryID = o.InstructionCategory.InstructionCategoryID,
                                            InstructionCategory = o.InstructionCategory.InstructionCategory
                                        })
                                        .ToList();
               }); 
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }

        public async Task<GeneralResponse<ICollection<BrokerMaster>>> GetAllBroker()
        {
            var response = new GeneralResponse<ICollection<BrokerMaster>>();
            try
            {
                response.Data = (await brokerMasterRepository.GetManyAsync(o => o.IsActive && !o.IsDeleted, true))
                                                .OrderBy(o => o.BrokerName).ToList();
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }

        public async Task<GeneralResponse<ICollection<KosherCodeMaster>>> GetAllKosherCode()
        {
            var response = new GeneralResponse<ICollection<KosherCodeMaster>>();
            try
            {
                response.Data = (await kosherCodeMasterRepository.GetManyAsync(o => o.IsActive && !o.IsDeleted, true))
                                            .OrderBy(o => o.KosherCodeDescription).ToList();
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }

        public async Task<GeneralResponse<ICollection<HACCPMaster>>> GetAllHACCPData(string hACCPType = "") 
        {
            var response = new GeneralResponse<ICollection<HACCPMaster>>();
            try
            {
                hACCPType = hACCPType.ToLower();
                response.Data = (await hACCPMasterRepository.GetManyAsync(o => o.IsActive && !o.IsDeleted 
                                                                            && (hACCPType == string.Empty || o.HACCPType.ToLower() == hACCPType), true))
                                            .OrderBy(o => o.HACCPDescription).ToList();
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }

        public async Task<GeneralResponse<ICollection<RMStatusMaster>>> GetAllRMStatus(string rMStatusType = "")
        {
            var response = new GeneralResponse<ICollection<RMStatusMaster>>();
            try
            {
                rMStatusType = rMStatusType.ToLower();
                response.Data = (await rMStatusMasterRepository.GetManyAsync(o => o.IsActive && !o.IsDeleted
                                                && (rMStatusType == string.Empty || o.RMStatusType.ToLower() == rMStatusType), true))
                                            .OrderBy(o => o.RMStatusType)
                                            .ThenBy(p => p.DisplayOrder)
                                            .ToList();
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }

        public async Task<GeneralResponse<ICollection<StorageConditionMaster>>> GetAllStorageCondition()
        {
            var response = new GeneralResponse<ICollection<StorageConditionMaster>>(); 
            try
            {
                response.Data = (await storageConditionMasterRepository.GetManyAsync(o => o.IsActive && !o.IsDeleted, true))
                                            .OrderBy(o => o.StorageType)
                                            .ThenBy(o => o.StorageGroupNumber).ToList();
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }

        public async Task<GeneralResponse<ICollection<CountryMaster>>> GetAllCountry()
        {
            var response = new GeneralResponse<ICollection<CountryMaster>>();
            try
            {
                response.Data = (await countryMasterRepository.GetManyAsync(o => o.IsActive && !o.IsDeleted, true))
                                            .OrderBy(o => o.CountryName).ToList();
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }

        public async Task<GeneralResponse<ICollection<ProductionLineMasterModel>>> GetAllProductionLineBySite(int siteID, int formulaID)
        {
            var response = new GeneralResponse<ICollection<ProductionLineMasterModel>>();
            try
            {
                response.Data = siteProductionLineMapRepository.Query(true)
                    .Include(o => o.ProductionLine).ThenInclude(o => o.FormulaProductionLineMapping)
                    .Where(o => o.ProductionLine.IsActive && !o.ProductionLine.IsDeleted && o.SiteID == siteID)
                    .Select(o => new
                    {
                        o.ProductionLineID,
                        o.ProductionLine.LineCode,
                        o.ProductionLine.LineDescription,
                        FormulaProductionLineMapping = o.ProductionLine.FormulaProductionLineMapping.FirstOrDefault(x => x.FormulaID == formulaID && x.ProductionLineID == o.ProductionLineID)
                    })
                    .Select(o => new ProductionLineMasterModel()
                    {
                        ProductionLineID = o.ProductionLineID,
                        LineCode = o.LineCode,
                        LineDescription = o.LineDescription,
                        ProductionMixerID = o.FormulaProductionLineMapping == null ? 0 : o.FormulaProductionLineMapping.ProductionMixerID,
                        BatchWeight = o.FormulaProductionLineMapping == null ? null : o.FormulaProductionLineMapping.Weight,
                        SiteID = siteID
                    })
                    .OrderBy(o => o.LineCode)
                    .ToList();
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return await Task.FromResult(response);
        }

        public async Task<GeneralResponse<ICollection<ProductionLineMixerModel>>> GetAllProductionLineMixerBySite(int siteID)
        {
            var response = new GeneralResponse<ICollection<ProductionLineMixerModel>>();
            try
            {
                response.Data = productionLineMixerMapRepository.Query(true)
                    .Include(o => o.ProductionMixerMaster)
                    .Include(o => o.SiteProductionLine).ThenInclude(o => o.ProductionLine)
                    .Where(o => o.SiteProductionLine.ProductionLine.IsActive && !o.SiteProductionLine.ProductionLine.IsDeleted
                                && o.ProductionMixerMaster.IsActive && !o.ProductionMixerMaster.IsDeleted 
                                && o.SiteProductionLine.SiteID == siteID)
                    .Select(o => new ProductionLineMixerModel()
                    {
                        ProductionLineID = o.SiteProductionLine.ProductionLineID,
                        ProductionMixerID = o.ProductionMixerID,
                        MixerDescription = o.ProductionMixerMaster.MixerDescription
                    })
                    .OrderBy(o => o.MixerDescription)
                    .ToList();
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return await Task.FromResult(response);
        }

        public async Task<GeneralResponse<ICollection<ReleaseAgentMaster>>> GetAllReleaseAgent()
        {
            var response = new GeneralResponse<ICollection<ReleaseAgentMaster>>();
            try
            {
                response.Data = await releaseAgentMasterRepository.GetManyAsync(o => o.IsActive && !o.IsDeleted, true);
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }

        public async Task<GeneralResponse<ICollection<PkoPercentageMaster>>> GetAllPkoPercentage()
        {
            var response = new GeneralResponse<ICollection<PkoPercentageMaster>>();
            try
            {
                response.Data = await pkoPercentageMasterRepository.GetManyAsync(o => o.IsActive && !o.IsDeleted, true);
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }
        
        public async Task<GeneralResponse<ICollection<BarFormatMaster>>> GetAllBarFormatMaster()
        {
            var response = new GeneralResponse<ICollection<BarFormatMaster>>();
            try
            {
                response.Data = await barFormatMasterRepository.GetManyAsync(o => o.IsActive && !o.IsDeleted, true);
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }
        
        public async Task<GeneralResponse<ICollection<BarFormatCodeMaster>>> GetAllBarFormatCodeMaster()
        {
            var response = new GeneralResponse<ICollection<BarFormatCodeMaster>>();
            try
            {
                response.Data = await barFormatCodeMasterRepository.GetManyAsync(o => o.IsActive && !o.IsDeleted, true);
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }
       
        public async Task<GeneralResponse<ICollection<FormulaTypeMaster>>> GetAllFormulaTypeMaster()
        {
            var response = new GeneralResponse<ICollection<FormulaTypeMaster>>();
            try
            {
                response.Data = await formulaTypeMasterRepository.GetManyAsync(o => o.IsActive && !o.IsDeleted, true);
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }

        public async Task<GeneralResponse<ICollection<SitterWidthMaster>>> GetAllSitterWidth(int siteID)
        {
            var response = new GeneralResponse<ICollection<SitterWidthMaster>>();
            try
            {
                response.Data = await sitterWidthMasterRepository.GetManyAsync(o => o.IsActive && !o.IsDeleted && (siteID == 0 || o.SiteID == siteID), true);
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }

        public async Task<GeneralResponse<ICollection<ExtrusionDieMaster>>> GetAllExtrusionDie()
        {
            var response = new GeneralResponse<ICollection<ExtrusionDieMaster>>();
            try
            {
                response.Data = await extrusionDieMasterRepository.GetManyAsync(o => o.IsActive && !o.IsDeleted, true);
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }

        public async Task<GeneralResponse<ICollection<InternalQCMAVLookUpMaster>>> GetAllInternalQCMAVLookUp()
        {
            var response = new GeneralResponse<ICollection<InternalQCMAVLookUpMaster>>();
            try
            {
                response.Data = await internalQCMAVLookUpMasterRepository.GetManyAsync(o => o.IsActive, true);
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }
    }
}