using AutoMapper;
using FluentValidation;
using FM21.Core;
using FM21.Core.Model;
using FM21.Core.Validator;
using FM21.Data.Infrastructure;
using FM21.Entities;
using FM21.Service.Caching;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace FM21.Service
{
    public class FormulaMasterService : BaseService, IFormulaMasterService
    {
        private readonly IRepository<FormulaMaster> formulaMasterRepository;
        private readonly IRepository<FormulaClaimMapping> formulaClaimMappingRepository;
        private readonly IRepository<FormulaCriteriaMapping> formulaCriteriaMappingRepository;
        private readonly IRepository<ClaimMaster> claimMasterRepository;
        private readonly IRepository<CriteriaMaster> criteriaMasterRepository;
        private readonly IRepository<FormulaRegulatoryCategoryMaster> formulaRegulatoryCategoryMasterRepository;
        private readonly IRepository<DatasheetFormatMaster> datasheetFormatMasterRepository;
        private readonly IRepository<NutrientMaster> nutrientMasterRepository;
        private readonly IRepository<IngredientMaster> ingredientMasterRepository;
        private readonly IRepository<ReconstitutionMaster> reconstitutionMasterRepository;
        private readonly IRepository<PowderLiquidMaster> powderLiquidMasterRepository;
        private readonly IRepository<PowderBlenderSiteMapping> powderBlenderSiteMappingRepository;
        private readonly IRepository<UnitServingMaster> unitServingMasterRepository;
        private readonly IRepository<PowderUnitServingSiteMapping> powderUnitServingSiteMappingRepository;
        private readonly IRepository<FormulaSearchHistory> formulaSearchHistoryRepository;
        private readonly IRepository<FormulaChangeCode> formulaChangeCodeRepository;
        private readonly IRepository<FormulaRevision> formulaRevisionRepository;
        private readonly IRepository<FormulaTypeMaster> formulaTypeMasterRepository;
        private readonly IRepository<FormulaStatusMaster> formulaStatusMasterRepository;
        private readonly IRepository<UserMaster> userMasterRepository;
        private readonly IRepository<FormulaRossCode> formulaRossCodeRepository;

        public FormulaMasterService(IServiceProvider provider, IExceptionHandler exceptionHandler, IMapper mapper, IUnitOfWork unitOfWork, ICacheProvider cacheProvider,
            IRepository<FormulaMaster> formulaMasterRepository, IRepository<FormulaClaimMapping> formulaClaimMappingRepository, IRepository<FormulaCriteriaMapping> formulaCriteriaMappingRepository,
            IRepository<ClaimMaster> claimMasterRepository, IRepository<CriteriaMaster> criteriaMasterRepository, IRepository<FormulaRegulatoryCategoryMaster> formulaRegulatoryCategoryMasterRepository,
            IRepository<DatasheetFormatMaster> datasheetFormatMasterRepository, IRepository<NutrientMaster> nutrientMasterRepository, IRepository<IngredientMaster> ingredientMasterRepository,
            IRepository<ReconstitutionMaster> reconstitutionMasterRepository, IRepository<PowderLiquidMaster> powderLiquidMasterRepository, IRepository<PowderBlenderSiteMapping> powderBlenderSiteMappingRepository,
            IRepository<UnitServingMaster> unitServingMasterRepository, IRepository<PowderUnitServingSiteMapping> powderUnitServingSiteMappingRepository, IRepository<FormulaSearchHistory> formulaSearchHistoryRepository, 
            IRepository<FormulaChangeCode> formulaChangeCodeRepository, IRepository<FormulaRevision> formulaRevisionRepository, IRepository<FormulaTypeMaster> formulaTypeMasterRepository, 
            IRepository<FormulaStatusMaster> formulaStatusMasterRepository, IRepository<UserMaster> userMasterRepository, IRepository<FormulaRossCode> formulaRossCodeRepository)
            : base(provider, exceptionHandler, mapper, unitOfWork, cacheProvider)
        {
            this.formulaMasterRepository = formulaMasterRepository;
            this.formulaClaimMappingRepository = formulaClaimMappingRepository;
            this.formulaCriteriaMappingRepository = formulaCriteriaMappingRepository;
            this.claimMasterRepository = claimMasterRepository;
            this.criteriaMasterRepository = criteriaMasterRepository;
            this.formulaRegulatoryCategoryMasterRepository = formulaRegulatoryCategoryMasterRepository;
            this.datasheetFormatMasterRepository = datasheetFormatMasterRepository;
            this.nutrientMasterRepository = nutrientMasterRepository;
            this.ingredientMasterRepository = ingredientMasterRepository;
            this.reconstitutionMasterRepository = reconstitutionMasterRepository;
            this.powderLiquidMasterRepository = powderLiquidMasterRepository;
            this.powderBlenderSiteMappingRepository = powderBlenderSiteMappingRepository;
            this.unitServingMasterRepository = unitServingMasterRepository;
            this.powderUnitServingSiteMappingRepository = powderUnitServingSiteMappingRepository;
            this.formulaSearchHistoryRepository = formulaSearchHistoryRepository;
            this.formulaChangeCodeRepository = formulaChangeCodeRepository;
            this.formulaRevisionRepository = formulaRevisionRepository;
            this.formulaTypeMasterRepository = formulaTypeMasterRepository;
            this.formulaStatusMasterRepository = formulaStatusMasterRepository;
            this.userMasterRepository = userMasterRepository;
            this.formulaRossCodeRepository = formulaRossCodeRepository;
        }

        #region Retrieve API
        public async Task<GeneralResponse<FormulaModel>> GetFormulaByFormulaID(int formulaID)
        {
            var response = new GeneralResponse<FormulaModel>();
            try
            {
                if (formulaID == 0)
                {
                    DataTable dtFormulaDetails = await formulaMasterRepository.GetFromStoredProcedureAsync("GetFormulaDetailsByFormulaID", ("formulaID", formulaID));
                    var jsonData = JsonConvert.SerializeObject(dtFormulaDetails);

                    response.Data = new FormulaModel();
                    response.Data.FormulaMaster = new FormulaMasterModel();
                    response.Data.FormulaDetails = JsonConvert.DeserializeObject<List<FormulaDetailsModel>>(jsonData);
                }
                else
                {
                    var obj = formulaMasterRepository.Query(true)
                            .Where(o => o.FormulaID == formulaID)
                            .Include(o => o.SiteProductMap)
                            .Include(o => o.FormulaProject).Include(o => o.FormulaProject.CustomerMaster)
                            .Include(o => o.NutrientFormat)
                            .Include(o => o.FormulaClaimMapping).ThenInclude(o => o.ClaimMaster)
                            .Include(o => o.FormulaCriteriaMapping)
                            .Include(o => o.RegulatoryCategory)
                            .FirstOrDefault();

                    if (obj != null)
                    {
                        DataTable dtFormulaDetails = await formulaMasterRepository.GetFromStoredProcedureAsync("GetFormulaDetailsByFormulaID", ("formulaID", formulaID));
                        var jsonData = JsonConvert.SerializeObject(dtFormulaDetails);

                        response.Data = new FormulaModel();
                        response.Data.FormulaMaster = mapper.Map<FormulaMasterModel>(obj);
                        response.Data.FormulaMaster.LastChangedBy = GetUserByUserID(obj.UpdatedBy, obj.UpdatedOn);
                        response.Data.ClaimInfo = obj.FormulaClaimMapping.Select(x => new ClaimModel() { ClaimID = x.ClaimID, ClaimCode = x.ClaimMaster.ClaimCode, ClaimDescription = x.ClaimMaster.ClaimDescription, Description = x.Description }).ToList();
                        response.Data.CriteriaInfo = obj.FormulaCriteriaMapping.Select(x => x.CriteriaID).ToArray();
                        response.Data.FormulaDetails = JsonConvert.DeserializeObject<List<FormulaDetailsModel>>(jsonData);
                    }
                    else
                    {
                        response.Message = localizer["msgRecordNotExist", new string[] { "Formula" }];
                    }
                }
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }

        public async Task<GeneralResponse<ICollection<FormulaClaimsModel>>> GetClaimsByFormulaID(int formulaID)
        {
            var response = new GeneralResponse<ICollection<FormulaClaimsModel>>();
            try
            {
                response.Data = new Collection<FormulaClaimsModel>();
                claimMasterRepository.Query(true)
                               .Include(o => o.FormulaClaimMapping)
                               .Where(o => o.IsActive && !o.IsDeleted)
                               .ToList()
                               .ForEach(o =>
                               {
                                   var obj = new FormulaClaimsModel()
                                   {
                                       ClaimID = o.ClaimID,
                                       FormulaID = formulaID,
                                       ClaimCode = o.ClaimCode,
                                       ClaimDescription = o.ClaimDescription,
                                       ClaimGroupType = o.ClaimGroupType,
                                       HasImpact = o.HasImpact,
                                       FormulaClaimMapID = 0

                                   };
                                   var mappingData = o.FormulaClaimMapping.FirstOrDefault(x => x.FormulaID == formulaID);
                                   if (mappingData != null)
                                   {
                                       obj.Description = mappingData.Description;
                                       obj.FormulaClaimMapID = mappingData.FormulaClaimMapID;
                                   }
                                   response.Data.Add(obj);
                               });
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return await Task.FromResult(response);
        }

        public async Task<GeneralResponse<ICollection<FormulaCriteriaModel>>> GetCriteriaByFormulaID(int formulaID)
        {
            var response = new GeneralResponse<ICollection<FormulaCriteriaModel>>();
            try
            {
                await Task.Run(() =>
                {
                    response.Data = new Collection<FormulaCriteriaModel>();
                    criteriaMasterRepository.Query(true)
                                    .Where(o => o.IsActive && !o.IsDeleted)
                                    .Include(o => o.FormulaCriteriaMapping)
                                    .OrderBy(o => o.CriteriaOrder)
                                    .ToList()
                                    .ForEach(o =>
                                    {
                                        var obj = new FormulaCriteriaModel()
                                        {
                                            FormulaID = formulaID,
                                            CriteriaID = o.CriteriaID,
                                            CriteriaDescription = o.CriteriaDescription,
                                            ColorCode = o.ColorCode,
                                            CriteriaOrder = o.CriteriaOrder,
                                            FormulaCriteriaMapID = o.FormulaCriteriaMapping.Where(x => x.FormulaID == formulaID).Select(x => x.FormulaCriteriaMapID).FirstOrDefault()
                                        };
                                        response.Data.Add(obj);
                                    });
                });
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }

        public async Task<GeneralResponse<ICollection<FormulaRegulatoryCategoryMaster>>> GetFormulaRegulatoryCategoryMaster()
        {
            var response = new GeneralResponse<ICollection<FormulaRegulatoryCategoryMaster>>();
            try
            {
                response.Data = formulaRegulatoryCategoryMasterRepository.Query(true)
                                                                        .Where(o => o.IsActive && !o.IsDeleted)
                                                                        .ToList();
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return await Task.FromResult(response);
        }

        public async Task<GeneralResponse<ICollection<DatasheetFormatMaster>>> GetDatasheetFormatMaster()
        {
            var response = new GeneralResponse<ICollection<DatasheetFormatMaster>>();
            try
            {
                response.Data = datasheetFormatMasterRepository.Query(true)
                                                                .Where(o => o.IsActive && !o.IsDeleted)
                                                                .ToList();
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return await Task.FromResult(response);
        }

        public async Task<GeneralResponse<ICollection<NutrientModel>>> GetAminoAcidInfo(FormulaIngredient[] arrIngredients)
        {
            var response = new GeneralResponse<ICollection<NutrientModel>>();
            try
            {
                int[] ingredientIDs = new int[0];
                if (arrIngredients != null)
                {
                    ingredientIDs = arrIngredients.Select(o => o.IngredientID).ToArray();
                }
                var nutrients = await nutrientMasterRepository.GetManyAsync(o => o.IsActive && !o.IsDeleted && o.IsAminoAcid, true);
                var data = mapper.Map<ICollection<NutrientMaster>, ICollection<NutrientModel>>(nutrients);

                var ingredients = ingredientMasterRepository.Query(true)
                                            .Include(o => o.IngredientNutrientMapping)
                                            .Where(o => ingredientIDs.Contains(o.IngredientID)).ToList();

                foreach (var aminoAcids in data)
                {
                    aminoAcids.NutrientValue = 0;
                    ingredients.ForEach(i =>
                    {
                        var oValue = i.IngredientNutrientMapping.Where(x => x.NutrientID == aminoAcids.NutrientId);
                        if (oValue.FirstOrDefault() != null)
                        {
                            decimal? amount = arrIngredients.Where(x => x.IngredientID == i.IngredientID).Select(o => o.Amount).Sum();
                            aminoAcids.NutrientValue += (Convert.ToDecimal(oValue.First().NutrientValue) / 100) * Convert.ToDecimal(amount);
                        }
                    });
                    aminoAcids.NutrientValue = Math.Round(aminoAcids.NutrientValue, 2);
                }
                response.Data = data;
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }

        public async Task<GeneralResponse<PDCAASInfo>> GetPDCAASInfo(FormulaIngredient[] formulaIngredients)
        {
            var response = new GeneralResponse<PDCAASInfo>() { Data = new PDCAASInfo() };
            try
            {
                int[] ingIDsList = new int[0];
                Dictionary<int, decimal?> formulaIngList = new Dictionary<int, decimal?>();
                List<string> missingAA = new List<string>();
                List<string> missingDigest = new List<string>();
                if (formulaIngredients != null)
                {
                    formulaIngList = formulaIngredients.GroupBy(o => o.IngredientID)
                                            .ToDictionary(o => o.Key, o => o.Sum(x => x.Amount));
                    ingIDsList = formulaIngredients.Select(o => o.IngredientID).Distinct().ToArray();
                }

                var ingList = ingredientMasterRepository.Query(true)
                                            .Include(o => o.IngredientNutrientMapping).ThenInclude(o => o.Nutrient)
                                            .Where(o => ingIDsList.Contains(o.IngredientID))
                                            .Select(o => new
                                            {
                                                o.IngredientID,
                                                o.RMDescription,
                                                o.IngredientNutrientMapping,
                                                Amount = formulaIngList[o.IngredientID]
                                            }).ToList();

                // Calculate Protein value
                ingList.ForEach(i =>
                {
                    var oValue = i.IngredientNutrientMapping.FirstOrDefault(x => x.Nutrient.Name == NutrientName.Protein);
                    if (oValue != null)
                    {
                        response.Data.ProteinAmount += (Convert.ToDecimal(oValue.NutrientValue) / 100) * Convert.ToDecimal(i.Amount);
                    }
                });

                //Fetch Protein Digestibility
                response.Data.ProteinDigestibility = ingList.Select(o => o.IngredientNutrientMapping
                                                            .Where(n => n.Nutrient.Name == NutrientName.ProteinDigestibility)
                                                            .Select(n => n.NutrientValue)
                                                            .FirstOrDefault())
                                                            .Average() ?? 0;

                //Calculate Amino Acids nutrient values
                foreach (PDCAASNutrient AminoAcids in response.Data.AminoAcids)
                {
                    string[] nutrientList = AminoAcids.NutrientName.Split(',');
                    AminoAcids.NutrientValue = 0;
                    foreach (var nutrientName in nutrientList)
                    {
                        ingList.ForEach(i =>
                        {
                            var oValue = i.IngredientNutrientMapping.FirstOrDefault(x => x.Nutrient.Name == nutrientName.Trim());
                            if (oValue != null)
                            {
                                AminoAcids.NutrientValue += ((Convert.ToDecimal(oValue.NutrientValue) / 100) * Convert.ToDecimal(i.Amount)) * 1000;
                            }
                            else
                            {
                                missingAA.Add(i.RMDescription);
                            }
                        });
                    }
                    AminoAcids.NutrientValue = (response.Data.ProteinAmount > 0 ? Math.Round(AminoAcids.NutrientValue / response.Data.ProteinAmount, 2) : 0);
                    AminoAcids.UncorrectedAAScore = Math.Round(AminoAcids.NutrientValue / AminoAcids.FAOProtein, 2);
                    AminoAcids.CorrectedAAScore = Math.Round(AminoAcids.UncorrectedAAScore * ((response.Data.ProteinDigestibility ?? 0) / 100), 2);
                }

                //Fetch ProteinDigestibility missing information
                ingList.ForEach(i =>
                {
                    var oValue = i.IngredientNutrientMapping.FirstOrDefault(x => x.Nutrient.Name == NutrientName.ProteinDigestibility);
                    if (oValue == null)
                    {
                        missingDigest.Add(i.RMDescription);
                    }
                });

                response.Data.MissingAAInfo = string.Join(", ", missingAA.Distinct());
                response.Data.MissingDigestInfo = string.Join(", ", missingDigest.Distinct());

                var pDCAASValue = response.Data.AminoAcids.OrderBy(o => o.CorrectedAAScore).First();
                response.Data.PDCAAS = (pDCAASValue.CorrectedAAScore < 1 ? pDCAASValue.CorrectedAAScore : 1);
                response.Data.AdjustedProtein = response.Data.ProteinAmount * response.Data.PDCAAS;
                response.Data.RDIPercent = response.Data.AdjustedProtein / 50;
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return await Task.FromResult(response);
        }

        public async Task<GeneralResponse<CarbohydrateInfo>> GetCarbohydrateInfo(FormulaIngredient[] formulaIngredients)
        {
            var response = new GeneralResponse<CarbohydrateInfo>() { Data = new CarbohydrateInfo() };
            try
            {
                if (formulaIngredients != null)
                {
                    int[] ingIDsList = formulaIngredients.Select(o => o.IngredientID).Distinct().ToArray();
                    Dictionary<int, decimal?> formulaIngList = formulaIngredients.GroupBy(o => o.IngredientID)
                                                                .ToDictionary(o => o.Key, o => o.Sum(x => x.Amount));

                    var ingList = ingredientMasterRepository.Query(true)
                                                .Include(o => o.IngredientNutrientMapping).ThenInclude(o => o.Nutrient)
                                                .Where(o => ingIDsList.Contains(o.IngredientID))
                                                .Select(o => new
                                                {
                                                    o.IngredientID,
                                                    o.IngredientNutrientMapping,
                                                    o.IngredientList,
                                                    o.IngredientBreakDown,
                                                    Amount = formulaIngList[o.IngredientID]
                                                }).ToList();

                    response.Data.TotalCarbs = ingList.Select(ing => ing.IngredientNutrientMapping
                                                                    .Where(n => n.Nutrient.Name == NutrientName.TotalCarbohydrate)
                                                                    .Select(n => (n.NutrientValue / 100) * ing.Amount)
                                                                    .FirstOrDefault() ?? 0
                                                        ).Sum();

                    response.Data.TotalFiber = ingList.Select(ing => ing.IngredientNutrientMapping
                                                                    .Where(n => n.Nutrient.Name == NutrientName.TotalFiber)
                                                                    .Select(n => (n.NutrientValue / 100) * ing.Amount)
                                                                    .FirstOrDefault() ?? 0
                                                        ).Sum();

                    response.Data.TotalSugars = ingList.Select(ing => ing.IngredientNutrientMapping
                                                                    .Where(n => n.Nutrient.Name == NutrientName.TotalSugars)
                                                                    .Select(n => (n.NutrientValue / 100) * ing.Amount)
                                                                    .FirstOrDefault() ?? 0
                                                        ).Sum();

                    ingList.ForEach(ing =>
                    {
                        var attributeList = (ing.IngredientList ?? "").Split(new string[] { ",\\" }, StringSplitOptions.None).ToList();
                        var attributeValues = (ing.IngredientBreakDown ?? "").Split('\\');
                        response.Data.Maltodextrin += CalculateAttributeValue(attributeList, attributeValues, ing.Amount, "maltodextrin");
                        response.Data.PolydextroseSolids += CalculateAttributeValue(attributeList, attributeValues, ing.Amount, "polydextrose");
                        response.Data.Sucralose += CalculateAttributeValue(attributeList, attributeValues, ing.Amount, "sucralose");
                        response.Data.Aspartame += CalculateAttributeValue(attributeList, attributeValues, ing.Amount, "aspartame");
                        response.Data.Acesulfame += CalculateAttributeValue(attributeList, attributeValues, ing.Amount, "acesulfame");
                        response.Data.Neotame += CalculateAttributeValue(attributeList, attributeValues, ing.Amount, "neotame");
                        response.Data.Isomalt += CalculateAttributeValue(attributeList, attributeValues, ing.Amount, "isomalt");
                        response.Data.Lactitol += CalculateAttributeValue(attributeList, attributeValues, ing.Amount, "lactitol");
                        response.Data.Mannitol += CalculateAttributeValue(attributeList, attributeValues, ing.Amount, "mannitol");
                        response.Data.Maltitol += CalculateAttributeValue(attributeList, attributeValues, ing.Amount, "maltitol");
                        response.Data.MaltitolSyrup += CalculateAttributeValue(attributeList, attributeValues, ing.Amount, "maltitol syrup");
                        response.Data.Sorbitol += CalculateAttributeValue(attributeList, attributeValues, ing.Amount, "sorbitol");
                        response.Data.LongChainPolyols += CalculateAttributeValue(attributeList, attributeValues, ing.Amount, "long chain polyols");
                        response.Data.Xylitol += CalculateAttributeValue(attributeList, attributeValues, ing.Amount, "xylitol");
                        response.Data.Erythritol += CalculateAttributeValue(attributeList, attributeValues, ing.Amount, "erythritol");
                        response.Data.Glycerine += CalculateAttributeValue(attributeList, attributeValues, ing.Amount, "glycerine");
                    });
                    response.Data.PolydextroseFiber70 = response.Data.PolydextroseFiber70 * Convert.ToDecimal(0.7);
                    response.Data.PolydextroseFiber80 = response.Data.PolydextroseFiber80 * Convert.ToDecimal(0.8);
                    response.Data.PolydextroseFiber90 = response.Data.PolydextroseFiber90 * Convert.ToDecimal(0.9);
                    response.Data.Glycerine = response.Data.Glycerine * Convert.ToDecimal(0.997);
                    response.Data.TotalMaltitol = response.Data.Maltitol + response.Data.MaltitolSyrup;
                    response.Data.TotSugAlc = response.Data.Lactitol + response.Data.Mannitol + response.Data.TotalMaltitol
                                            + response.Data.Sorbitol + response.Data.LongChainPolyols + response.Data.Xylitol
                                            + response.Data.Erythritol + response.Data.Glycerine;
                    response.Data.TotSugAlcDB = response.Data.TotalSugars;
                    response.Data.CHO_FIB_SugAlc = response.Data.TotalCarbs - response.Data.TotalFiber - response.Data.TotalSugars;

                    // Round attributes value
                    response.Data.TotalCarbs = Math.Round(response.Data.TotalCarbs, 2);
                    response.Data.TotalFiber = Math.Round(response.Data.TotalFiber, 2);
                    response.Data.TotalSugars = Math.Round(response.Data.TotalSugars, 2);
                    response.Data.Maltodextrin = Math.Round(response.Data.Maltodextrin, 3);
                    response.Data.PolydextroseSolids = Math.Round(response.Data.PolydextroseSolids, 2);
                    response.Data.Sucralose = Math.Round(response.Data.Sucralose, 4);
                    response.Data.Aspartame = Math.Round(response.Data.Aspartame, 2);
                    response.Data.Acesulfame = Math.Round(response.Data.Acesulfame, 2);
                    response.Data.Neotame = Math.Round(response.Data.Neotame, 2);
                    response.Data.Isomalt = Math.Round(response.Data.Isomalt, 2);
                    response.Data.Lactitol = Math.Round(response.Data.Lactitol, 2);
                    response.Data.Mannitol = Math.Round(response.Data.Mannitol, 2);
                    response.Data.Maltitol = Math.Round(response.Data.Maltitol, 2);
                    response.Data.MaltitolSyrup = Math.Round(response.Data.MaltitolSyrup, 2);
                    response.Data.Sorbitol = Math.Round(response.Data.Sorbitol, 2);
                    response.Data.LongChainPolyols = Math.Round(response.Data.LongChainPolyols, 2);
                    response.Data.Xylitol = Math.Round(response.Data.Xylitol, 2);
                    response.Data.Erythritol = Math.Round(response.Data.Erythritol, 2);
                    response.Data.Glycerine = Math.Round(response.Data.Glycerine, 2);
                    response.Data.PolydextroseFiber70 = Math.Round(response.Data.PolydextroseFiber70, 2);
                    response.Data.PolydextroseFiber80 = Math.Round(response.Data.PolydextroseFiber80, 2);
                    response.Data.PolydextroseFiber90 = Math.Round(response.Data.PolydextroseFiber90, 2);
                    response.Data.TotalMaltitol = Math.Round(response.Data.TotalMaltitol, 2);
                    response.Data.TotSugAlc = Math.Round(response.Data.TotSugAlc, 2);
                    response.Data.TotSugAlcDB = Math.Round(response.Data.TotSugAlcDB, 2);
                    response.Data.CHO_FIB_SugAlc = Math.Round(response.Data.CHO_FIB_SugAlc, 2);
                }
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return await Task.FromResult(response);
        }

        public async Task<GeneralResponse<ICollection<DSActualInfo>>> GetDSActualInfo(FormulaIngredient[] arrIngredients)
        {
            var response = new GeneralResponse<ICollection<DSActualInfo>>();
            try
            {
                var data = new Collection<DSActualInfo>();
                int[] ingredientIDs = new int[0];
                Dictionary<int, decimal?> formulaIngList = new Dictionary<int, decimal?>();
                Dictionary<string, decimal?> nutrientValues = new Dictionary<string, decimal?>();

                if (arrIngredients != null)
                {
                    ingredientIDs = arrIngredients.Select(o => o.IngredientID).ToArray();

                    formulaIngList = arrIngredients.GroupBy(o => o.IngredientID)
                                                    .ToDictionary(o => o.Key, o => o.Sum(x => x.Amount));
                }

                var nutrientList = nutrientMasterRepository.Query(true)
                                                            .Include(o => o.UnitOfMeasurement)
                                                            .Where(o => o.IsActive && !o.IsDeleted)
                                                            .OrderBy(o => o.DisplayColumnOrder)
                                                            .ThenBy(o => o.DisplayItemOrder);

                var ingredientList = ingredientMasterRepository.Query(true)
                                            .Include(o => o.IngredientNutrientMapping).ThenInclude(o => o.Nutrient)
                                            .Where(o => ingredientIDs.Contains(o.IngredientID))
                                            .Select(o => new
                                            {
                                                o.IngredientID,
                                                o.IngredientNutrientMapping,
                                                Amount = formulaIngList[o.IngredientID]
                                            })
                                            .ToList();

                foreach (var nutrient in nutrientList)
                {
                    DSActualInfo dSActualInfo = new DSActualInfo()
                    {
                        NutrientID = nutrient.NutrientID,
                        Name = nutrient.Name,
                        MeasurementUnit = nutrient.UnitOfMeasurement.MeasurementUnit,
                        DisplayColumnOrder = nutrient.DisplayColumnOrder,
                        DisplayItemOrder = nutrient.DisplayItemOrder,
                        IsAminoAcid = nutrient.IsAminoAcid,
                        PerServingValue = 0,
                        Per100GramValue = 0
                    };
                    ingredientList.ForEach(ing =>
                    {
                        var nutrientValue = ing.IngredientNutrientMapping.Where(n => n.NutrientID == nutrient.NutrientID)
                                                                        .Select(o => o.NutrientValue)
                                                                        .FirstOrDefault() ?? 0;
                        dSActualInfo.PerServingValue += (nutrientValue / 100) * ing.Amount;
                        dSActualInfo.Per100GramValue += nutrientValue;
                    });
                    dSActualInfo.PerServingValue = Math.Round(Convert.ToDecimal(dSActualInfo.PerServingValue), 2);
                    dSActualInfo.Per100GramValue = Math.Round(Convert.ToDecimal(dSActualInfo.Per100GramValue), 2);
                    data.Add(dSActualInfo);

                    switch (dSActualInfo.Name.Trim())
                    {
                        case NutrientName.Protein:
                        case NutrientName.TotalFat:
                        case NutrientName.TotalFiber:
                        case NutrientName.TotalSugars:
                        case NutrientName.TotalCarbohydrate:
                        case NutrientName.FATkcalFactor:
                        case NutrientName.CarbkcalFactor:
                        case NutrientName.PROkcalFactor:
                        case NutrientName.NPNkcalFactor:
                        case NutrientName.Polyunsaturated:
                        case NutrientName.NonProximateNutrient:
                        case NutrientName.ProteinDigestibility:
                        case NutrientName.L_Lysine_Lys:
                        case NutrientName.L_Threonine_Thr:
                        case NutrientName.L_Leucine_Leu:
                        case NutrientName.L_Tryptophan_Trp:
                        case NutrientName.L_Phenylalanine_Phe:
                        case NutrientName.L_Tyrosine_Tyr:
                        case NutrientName.L_Histidine_His:
                        case NutrientName.L_Methionine_Met:
                        case NutrientName.L_Cystine_Cys:
                        case NutrientName.L_Valine_Val:
                        case NutrientName.L_Isoleucine_Ile:
                            nutrientValues.Add(dSActualInfo.Name, dSActualInfo.PerServingValue);
                            break;
                    }
                }

                // Calculate total calories, incl insol
                decimal? totalCalories = nutrientValues[NutrientName.TotalCarbohydrate] * nutrientValues[NutrientName.CarbkcalFactor];
                totalCalories += nutrientValues[NutrientName.TotalFat] * nutrientValues[NutrientName.FATkcalFactor];
                totalCalories += nutrientValues[NutrientName.Protein] * nutrientValues[NutrientName.PROkcalFactor];
                totalCalories += nutrientValues[NutrientName.NonProximateNutrient] * nutrientValues[NutrientName.NPNkcalFactor];
                data.Insert(0, GetDSActualObject("Total Calories, incl insol", totalCalories, 1, 0));

                // Calculate PDCAAS & Corrected Protien
                PDCAASInfo oPDCAASInfo = new PDCAASInfo();
                foreach (PDCAASNutrient AminoAcids in oPDCAASInfo.AminoAcids)
                {
                    string[] lstNnutrient = AminoAcids.NutrientName.Split(',');
                    AminoAcids.NutrientValue = 0;
                    foreach (var nutrientName in lstNnutrient)
                    {
                        AminoAcids.NutrientValue += Convert.ToDecimal(nutrientValues[nutrientName.Trim()]) * 1000;
                    }
                    AminoAcids.NutrientValue = (nutrientValues[NutrientName.Protein] > 0 ? Math.Round(AminoAcids.NutrientValue / Convert.ToDecimal(nutrientValues[NutrientName.Protein]), 2) : 0);
                    AminoAcids.UncorrectedAAScore = Math.Round(AminoAcids.NutrientValue / AminoAcids.FAOProtein, 2);
                    AminoAcids.CorrectedAAScore = Math.Round(AminoAcids.UncorrectedAAScore * ((nutrientValues[NutrientName.ProteinDigestibility] ?? 0) / 100), 2);
                }
                var pDCAASValue = oPDCAASInfo.AminoAcids.OrderBy(o => o.CorrectedAAScore).First();
                oPDCAASInfo.PDCAAS = (pDCAASValue.CorrectedAAScore < 1 ? pDCAASValue.CorrectedAAScore : 1);
                oPDCAASInfo.AdjustedProtein = Convert.ToDecimal(nutrientValues[NutrientName.Protein]) * oPDCAASInfo.PDCAAS;
                int idxProteinDigestibility = data.IndexOf(data.First(o => o.Name == NutrientName.ProteinDigestibility));
                data.Insert(++idxProteinDigestibility, GetDSActualObject("PDCAAS", oPDCAASInfo.PDCAAS, 1, 0));
                data.Insert(++idxProteinDigestibility, GetDSActualObject("Corrected Protein", oPDCAASInfo.AdjustedProtein, 1, 0));

                // Set Moisture & Ash order
                var moisture = data.First(o => o.Name == NutrientName.Moisture);
                var ash = data.First(o => o.Name == NutrientName.Ash);
                data.Remove(moisture);
                data.Remove(ash);
                moisture.DisplayColumnOrder = 3;
                ash.DisplayColumnOrder = 3;
                data.Add(moisture);
                data.Add(ash);

                // Calculate total amino acid percentage
                decimal? totalAminoAcidPercent = data.Where(o => o.IsAminoAcid).Select(o => o.PerServingValue).Sum();
                totalAminoAcidPercent = nutrientValues[NutrientName.Protein] > 0 ? Math.Round(Convert.ToDecimal((totalAminoAcidPercent / nutrientValues[NutrientName.Protein]) * 100), 2) : 0;
                data.Add(GetDSActualObject("AA % of Total", totalAminoAcidPercent, 5, 0));

                // Calculate total fat percentage
                string[] fattyAcidsList = new string[] { NutrientName.SaturatedFat, NutrientName.Monounsaturated, NutrientName.Polyunsaturated };
                decimal? totalFatPercent = data.Where(o => fattyAcidsList.Contains(o.Name)).Select(o => o.PerServingValue).Sum();
                totalFatPercent = nutrientValues[NutrientName.TotalFat] > 0 ? Math.Round(Convert.ToDecimal((totalFatPercent / nutrientValues[NutrientName.TotalFat]) * 100), 0) : 0;
                data.Add(GetDSActualObject("FA % of Total", totalFatPercent, 5, 2));

                // Calculate total linoleic percentage
                string[] linolenicList = new string[] { NutrientName.Linoleic_18_2, NutrientName.Linolenic_18_3 };
                decimal? totalLinoleicPercent = data.Where(o => linolenicList.Contains(o.Name)).Select(o => o.PerServingValue).Sum();
                totalLinoleicPercent = nutrientValues[NutrientName.Polyunsaturated] > 0 ? Math.Round(Convert.ToDecimal((totalLinoleicPercent / nutrientValues[NutrientName.Polyunsaturated]) * 100), 2) : 0;
                data.Add(GetDSActualObject("18:2, 18:3 % of Total", totalLinoleicPercent, 5, 0));

                // Calculate total fiber percentage
                string[] fiberComponentList = new string[] { NutrientName.SolubleFiber, NutrientName.InsolubleFiber };
                decimal? totalFiberPercent = data.Where(o => fiberComponentList.Contains(o.Name)).Select(o => o.PerServingValue).Sum();
                totalFiberPercent = nutrientValues[NutrientName.TotalFiber] > 0 ? Math.Round(Convert.ToDecimal((totalFiberPercent / nutrientValues[NutrientName.TotalFiber]) * 100), 2) : 0;
                data.Add(GetDSActualObject("InSol/Sol % of Total", totalFiberPercent, 5, 0));

                // Calculate total carbohydrate percentage
                string[] carbComponentList = new string[] { NutrientName.TotalFat, NutrientName.Protein, NutrientName.Moisture, NutrientName.Ash, NutrientName.NonProximateNutrient };
                decimal? totalCarbPercent = data.Where(o => carbComponentList.Contains(o.Name)).Select(o => o.PerServingValue).Sum();
                totalCarbPercent = nutrientValues[NutrientName.TotalCarbohydrate] > 0 ? Math.Round(Convert.ToDecimal((totalCarbPercent / nutrientValues[NutrientName.TotalCarbohydrate]) * 100), 2) : 0;
                data.Add(GetDSActualObject("Carbohydrate % of Total", totalCarbPercent, 5, 0));

                // Calculate total sugar percentage
                string[] sugarComponentList = new string[] { NutrientName.Galactose, NutrientName.Glucose, NutrientName.Fructose, NutrientName.Lactose, NutrientName.Sucrose, NutrientName.Maltose };
                decimal? totalSugarPercent = data.Where(o => sugarComponentList.Contains(o.Name)).Select(o => o.PerServingValue).Sum();
                totalSugarPercent = nutrientValues[NutrientName.TotalSugars] > 0 ? Math.Round(Convert.ToDecimal((totalSugarPercent / nutrientValues[NutrientName.TotalSugars]) * 100), 2) : 0;
                data.Add(GetDSActualObject("Sugar % of Total", totalSugarPercent, 5, 0));

                response.Data = data;
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return await Task.FromResult(response);
        }

        public async Task<GeneralResponse<ICollection<IngredientListInfo>>> GetIngredientListInfo(FormulaIngredient[] formulaIngredients)
        {
            var response = new GeneralResponse<ICollection<IngredientListInfo>>();
            try
            {
                var data = new Collection<IngredientListInfo>();
                if (formulaIngredients != null)
                {
                    int[] ingIDsList = formulaIngredients.Select(o => o.IngredientID).Distinct().ToArray();

                    Dictionary<int, decimal?> formulaIngList = formulaIngredients.GroupBy(o => o.IngredientID)
                                                                .ToDictionary(o => o.Key, o => o.Sum(x => x.Amount));

                    Dictionary<int, decimal?> ingPercentInFormula = formulaIngredients.GroupBy(o => o.IngredientID)
                                                                .ToDictionary(o => o.Key, o => o.Sum(x => x.Percent));

                    var ingList = ingredientMasterRepository.Query(true)
                                                .Where(o => ingIDsList.Contains(o.IngredientID))
                                                .Select(o => new
                                                {
                                                    o.IngredientID,
                                                    o.IngredientCategoryID,
                                                    o.JDECode,
                                                    o.RMDescription,
                                                    o.IngredientList,
                                                    o.IngredientBreakDown,
                                                    Amount = formulaIngList[o.IngredientID],
                                                    PercentUsedInFormula = ingPercentInFormula[o.IngredientID]
                                                }).ToList();

                    decimal usedPercentInIng = 0, gramPerServe;
                    int ingIndex = 0;
                    ingList.ForEach(ing =>
                    {
                        var attributeList = (ing.IngredientList ?? "").Split(new string[] { ",\\" }, StringSplitOptions.None).ToList();
                        var attributeValues = (ing.IngredientBreakDown ?? "").Split('\\');
                        for (int i = 0; i < attributeList.Count; i++)
                        {
                            if (!string.IsNullOrWhiteSpace(attributeList[i]))
                            {
                                gramPerServe = 0;
                                if (i <= attributeValues.Count() && decimal.TryParse(Convert.ToString(attributeValues[i]), out usedPercentInIng))
                                {
                                    gramPerServe = (usedPercentInIng / 100) * (ing.Amount ?? 0);
                                }
                                var ingAttribute = new IngredientListInfo()
                                {
                                    IngredientID = ing.IngredientID,
                                    IngredientDescription = ing.RMDescription,
                                    IngredientCategoryID = ing.IngredientCategoryID,
                                    PartNumber = ing.JDECode,
                                    IngredientName = attributeList[i],
                                    IngUsedPercentInFormula = Convert.ToDecimal(ing.PercentUsedInFormula),
                                    IngUsedPercentInIngredient = Math.Round(usedPercentInIng, 7),
                                    GramPerServe = Math.Round(gramPerServe, 7),
                                    OrderNumber = ++ingIndex
                                };
                                data.Add(ingAttribute);
                            }
                        }
                    });
                    response.Data = data;
                }
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return await Task.FromResult(response);
        }

        public async Task<GeneralResponse<DataTable>> GetFormulaYieldLossFactorDefaultValue(int siteId, string mixerType)
        {
            var response = new GeneralResponse<DataTable>();
            try
            {
                if (siteId > 0 && !string.IsNullOrEmpty(mixerType))
                {
                    response.Data = await formulaMasterRepository.GetFromStoredProcedureAsync("GetFormulaYieldLossFactorDefaultValue", ("siteID", siteId), ("mixerType", mixerType));
                }
                else
                {
                    response.Message = localizer["msgRecordNotExist", new string[] { "Formula" }];
                }
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }

        public async Task<PagedTableResponse<DataTable>> SearchFormula(FormulaSearchFilter searchFilter)
        {
            var response = new PagedTableResponse<DataTable>() { CurrentPage = searchFilter.PageIndex, PageSize = searchFilter.PageSize };
            try
            {
                string columnList = string.Empty;
                if (!string.IsNullOrEmpty(searchFilter.FieldValue1))
                {
                    columnList = "," + searchFilter.FieldValue1;
                }
                if (!string.IsNullOrEmpty(searchFilter.FieldValue2))
                {
                    columnList += "," + searchFilter.FieldValue2;
                }
                if (!string.IsNullOrEmpty(searchFilter.FieldValue3))
                {
                    columnList += "," + searchFilter.FieldValue3;
                }
                if (!string.IsNullOrEmpty(searchFilter.FieldValue4))
                {
                    columnList += "," + searchFilter.FieldValue4;
                }
                if (!string.IsNullOrEmpty(columnList))
                {
                    columnList = columnList.Substring(1);
                }

                DataTable dtData = await formulaMasterRepository.GetFromStoredProcedureAsync("SearchFormula",
                            ("searchCol1", searchFilter.SearchField1), ("searchCol1Value", searchFilter.SearchText1),
                            ("searchCol2", searchFilter.SearchField2), ("searchCol2Value", searchFilter.SearchText2), ("searchCol2Condition", searchFilter.SearchCondition2),
                            ("searchCol3", searchFilter.SearchField3), ("searchCol3Value", searchFilter.SearchText3), ("searchCol3Condition", searchFilter.SearchCondition3),
                            ("siteID", searchFilter.SiteID), ("columnList", columnList),
                            ("sortColumn", searchFilter.SortColumn), ("sortDirection", searchFilter.SortDirection));

                //Pagination
                response.RowCount = dtData.Rows.Count;
                response.PageCount = (int)Math.Ceiling((double)response.RowCount / response.PageSize);
                var skip = (response.CurrentPage - 1) * response.PageSize;
                var data = dtData.Rows.Cast<DataRow>().Skip(skip).Take(response.PageSize);

                if (data != null && data.Any())
                {
                    DataTable dt = data.CopyToDataTable();
                    CheckAndCopyColumn(ref dt, searchFilter.FieldValue1, "FieldValue1");
                    CheckAndCopyColumn(ref dt, searchFilter.FieldValue2, "FieldValue2");
                    CheckAndCopyColumn(ref dt, searchFilter.FieldValue3, "FieldValue3");
                    CheckAndCopyColumn(ref dt, searchFilter.FieldValue4, "FieldValue4");
                    response.Data = dt;
                    response.Data.AcceptChanges();
                }

                if (searchFilter.EnableSearchHistory)
                {
                    string searchData = string.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}", searchFilter.SearchText1 ?? "", searchFilter.SearchField1 ?? "",
                       searchFilter.SearchCondition2 ?? "",
                       searchFilter.SearchText2 ?? "", searchFilter.SearchField2 ?? "", searchFilter.SearchCondition3 ?? "",
                       searchFilter.SearchText3 ?? "", searchFilter.SearchField3 ?? "");
                    await UpdateFormulaSearchHistory(Convert.ToInt32(RequestUserID), searchData);
                }
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }

        public async Task<GeneralResponse<ICollection<FormulaSearchHistory>>> GetFormulaSearchHistory(int userID)
        {
            var response = new GeneralResponse<ICollection<FormulaSearchHistory>>();
            try
            {
                response.Data = (await formulaSearchHistoryRepository.GetManyAsync(o => o.UserID == userID, true))
                                            .OrderByDescending(o => o.SearchDate).ToList();
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }

        public async Task<GeneralResponse<bool>> UpdateFormulaSearchHistory(int userID, string searchData)
        {
            var response = new GeneralResponse<bool>();
            try
            {
                if (formulaSearchHistoryRepository.GetMany(o => o.UserID == userID, true).Count() >= 5)
                {
                    var firstSearchRecord = formulaSearchHistoryRepository.GetMany(o => o.UserID == userID, true).OrderBy(o => o.SearchDate).First();
                    await formulaSearchHistoryRepository.DeleteAsync(o => o.FormulaSearchID == firstSearchRecord.FormulaSearchID);
                }

                var formulaSearch = formulaSearchHistoryRepository.GetMany(o => o.UserID == userID && o.SearchData == searchData).FirstOrDefault();
                if (formulaSearch == null)
                {
                    var oIngredientSearch = new FormulaSearchHistory() { UserID = userID, SearchData = searchData, SearchDate = DateTime.Now };
                    formulaSearchHistoryRepository.AddAsync(oIngredientSearch);
                }
                else
                {
                    formulaSearch.SearchDate = DateTime.Now;
                    formulaSearchHistoryRepository.UpdateAsync(formulaSearch);
                }
                await Save();
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }

        public async Task<GeneralResponse<ICollection<ReconstitutionMaster>>> GetAllReconstitutionMaster()
        {
            var response = new GeneralResponse<ICollection<ReconstitutionMaster>>();
            try
            {
                response.Data = await reconstitutionMasterRepository.GetManyAsync(o => o.IsActive && !o.IsDeleted, true);
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }

        public async Task<GeneralResponse<ICollection<PowderLiquidMaster>>> GetAllPowderLiquidMaster()
        {
            var response = new GeneralResponse<ICollection<PowderLiquidMaster>>();
            try
            {
                response.Data = await powderLiquidMasterRepository.GetManyAsync(o => o.IsActive && !o.IsDeleted, true);
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }

        public async Task<GeneralResponse<ICollection<PowderBlenderMasterModel>>> GetAllPowderBlenderMaster(int siteID)
        {
            var response = new GeneralResponse<ICollection<PowderBlenderMasterModel>>();
            try
            {
                response.Data = powderBlenderSiteMappingRepository.Query(true)
                                                    .Include(o => o.PowderBlender)
                                                    .Where(o => o.PowderBlender.IsActive && !o.PowderBlender.IsDeleted
                                                    && (siteID == 0 || o.SiteID == siteID))
                                                    .Select(o => new PowderBlenderMasterModel
                                                    {
                                                        PowderBlenderID = o.PowderBlenderID,
                                                        BlenderDescription = o.PowderBlender.BlenderDescription,
                                                        SiteID = o.SiteID
                                                    }).ToList();
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return await Task.FromResult(response);
        }

        public async Task<GeneralResponse<ICollection<UnitServingMasterModel>>> GetAllPowderUnitMaster(int siteID)
        {
            var response = new GeneralResponse<ICollection<UnitServingMasterModel>>();
            try
            {
                response.Data = powderUnitServingSiteMappingRepository.Query(true)
                                                    .Include(o => o.UnitServingMaster)
                                                    .Include(o => o.SiteProductMap)
                                                    .Where(o => o.UnitServingMaster.IsActive && !o.UnitServingMaster.IsDeleted
                                                        && o.UnitServingMaster.UnitServingType.ToUpper() == "UNIT"
                                                        && (siteID == 0 || o.SiteProductMap.SiteID == siteID))
                                                    .Select(o => new UnitServingMasterModel
                                                    {
                                                        UnitServingID = o.UnitServingMaster.UnitServingID,
                                                        UnitDescription = o.UnitServingMaster.UnitDescription,
                                                        SiteID = o.SiteProductMap.SiteID,
                                                        SiteProductMapID = o.SiteProductMapID
                                                    }).ToList();
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return await Task.FromResult(response);
        }

        public async Task<GeneralResponse<ICollection<UnitServingMasterModel>>> GetAllPowderUnitServingMaster()
        {
            var response = new GeneralResponse<ICollection<UnitServingMasterModel>>();
            try
            {
                response.Data = (await unitServingMasterRepository.GetManyAsync(o => o.IsActive && !o.IsDeleted
                                                        && o.UnitServingType.ToUpper() == "UNIT/SERVING", true))
                                                    .Select(o => new UnitServingMasterModel
                                                    {
                                                        UnitServingID = o.UnitServingID,
                                                        UnitDescription = o.UnitDescription
                                                    }).ToList();
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return await Task.FromResult(response);
        }

        public async Task<GeneralResponse<DataTable>> GetFormulaHomePageHeaderInfo(int formulaID)
        {
            var response = new GeneralResponse<DataTable>();
            try
            {
                if (formulaID > 0)
                {
                    response.Data = await formulaMasterRepository.GetFromStoredProcedureAsync("GetFormulaHomePageHeaderInfo", ("formulaID", formulaID));
                }
                else
                {
                    response.Message = localizer["msgRecordNotExist", new string[] { "Formula" }];
                }
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }

        public async Task<GeneralResponse<DataTable>> GetFormulaChangeCodeList(FormulaChangeCodeParam formulaChangeCodeParam)
        {
            var response = new GeneralResponse<DataTable>();
            try
            {
                if (formulaChangeCodeParam != null && formulaChangeCodeParam.CurrentSiteID > 0)
                {
                    response.Data = await formulaMasterRepository.GetFromStoredProcedureAsync("GetFormulaChangeCodes", ("FormulaTypeCode", formulaChangeCodeParam.FormulaTypeCode),
                        ("CurrentSiteId", formulaChangeCodeParam.CurrentSiteID), ("ChangeSiteId", formulaChangeCodeParam.ChangeSiteID),
                        ("FormulaCode", formulaChangeCodeParam.FormulaCode), ("ServingSize", formulaChangeCodeParam.ServingSize), ("FormulaId", formulaChangeCodeParam.FormulaID),
                        ("IsRossCode", (formulaChangeCodeParam.FormulaCode.Length == 17)));
                }
                else
                {
                    response.Message = localizer["msgRecordNotExist", new string[] { "Formula" }];
                }
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }

        public async Task<GeneralResponse<bool>> IsFormulaCodeExist(string formulaCode)
        {
            var response = new GeneralResponse<bool>();
            try
            {
                if (!string.IsNullOrEmpty(formulaCode))
                {
                    response.Data = formulaMasterRepository.Any(x => x.FormulaCode == formulaCode.Trim());
                    if (response.Data)
                    {
                        response.Message = localizer["msgDuplicateRecord", new string[] { "Formula Code" }];
                    }
                }
                else
                {
                    response.Message = localizer["msgInvalidCodeFormat"];
                }
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return await Task.FromResult(response);
        }

        public async Task<GeneralResponse<ICollection<FormulaStatusMaster>>> GetFormulaStatusMaster()
        {
            var response = new GeneralResponse<ICollection<FormulaStatusMaster>>();
            try
            {
                response.Data = formulaStatusMasterRepository.Query(true)
                                                            .Where(o => o.IsActive && !o.IsDeleted)
                                                            .ToList();
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return await Task.FromResult(response);
        }

        public async Task<GeneralResponse<FormulaMasterModel>> ImportInternalQCInfoByFormulaCode(string formulaCode)
        {
            var response = new GeneralResponse<FormulaMasterModel>();

            try
            {
                var obj = formulaMasterRepository.Query(true)
                        .Where(o => o.FormulaCode.Trim() == formulaCode.Trim())
                        .Include(o => o.SiteProductMap)
                        .Include(o => o.FormulaProject).Include(o => o.FormulaProject.CustomerMaster)
                        .Include(o => o.NutrientFormat)
                        .Include(o => o.FormulaClaimMapping).ThenInclude(o => o.ClaimMaster)
                        .Include(o => o.FormulaCriteriaMapping)
                        .Include(o => o.RegulatoryCategory)
                        .FirstOrDefault();

                if (obj != null)
                {
                    response.Data = new FormulaMasterModel();
                    response.Data = mapper.Map<FormulaMasterModel>(obj);
                }
                else
                {
                    response.Message = localizer["msgRecordNotExist", new string[] { "Formula" }];
                }

            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return await Task.FromResult(response);
        }

        public async Task<GeneralResponse<List<FormulaTargetModel>>> GetTargetInfo(FormulaTargetInfoParam formulaTargetInfoParam)
        {
            var response = new GeneralResponse<List<FormulaTargetModel>>();
            try
            {
                if (formulaTargetInfoParam != null)
                {
                    DataTable dtTargetDetails = await formulaMasterRepository.GetFromStoredProcedureAsync("GetTargetInfo", ("DatasheetFormatID", formulaTargetInfoParam.DatasheetFormatID),
                        ("IngredientID", formulaTargetInfoParam.IngredientID));
                    var jsonData = JsonConvert.SerializeObject(dtTargetDetails);

                    response.Data = new List<FormulaTargetModel>();
                    response.Data = JsonConvert.DeserializeObject<List<FormulaTargetModel>>(jsonData);
                }
                else
                {

                    response.Message = localizer["msgRecordNotExist", new string[] { "Formula" }];

                }
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }
        #endregion

        #region Insert, Update And Delete API 
        public async Task<GeneralResponse<bool>> UpdateFormula(FormulaModel formula)
        {
            var response = new GeneralResponse<bool>();
            try
            {
                var obj = await formulaMasterRepository.GetByIdAsync(formula.FormulaMaster.FormulaID);
                if (obj != null)
                {
                    mapper.Map<FormulaMasterModel, FormulaMaster>(formula.FormulaMaster, obj);
                    obj.UpdatedBy = RequestUserID;
                    obj.UpdatedOn = DateTime.Now;
                    formulaMasterRepository.UpdateAsync(obj);

                    var result = await SaveClaims(formula.FormulaMaster.FormulaID, formula.ClaimInfo);
                    if (result.Result == ResultType.Success)
                    {
                        await Save();
                    }
                    else
                    {
                        response.Exception = result.Exception;
                        return response;
                    }
                    response.Message = localizer["msgUpdateSuccess", new string[] { "Formula" }];
                }
                else
                {
                    response.Result = ResultType.Warning;
                    response.Message = localizer["msgRecordNotExist", new string[] { "Formula" }];
                }
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }

        public async Task<GeneralResponse<bool>> SaveFormula(FormulaModel formulaModel)
        {
            var response = new GeneralResponse<bool>();
            try
            {
                var validator = new FormulaMasterValidator(localizer);
                var results = validator.Validate(formulaModel.FormulaMaster, ruleSet: "New");
                if (results.IsValid)
                {
                    FormulaMaster formulaMaster = mapper.Map<FormulaMaster>(formulaModel.FormulaMaster);
                    if (formulaMaster != null)
                    {
                        formulaMaster.CreatedBy = RequestUserID;
                        formulaMaster.FormulaID = 0;
                        if (formulaModel.FormulaDetails != null && formulaModel.FormulaDetails.Count > 0)
                        {
                            formulaMaster.FormulaDetailMapping = mapper.Map<ICollection<FormulaDetailsModel>, ICollection<FormulaDetailMapping>>(formulaModel.FormulaDetails);
                            formulaMaster.FormulaDetailMapping.ToList().ForEach(c => { c.CreatedBy = RequestUserID; c.FormulaDetailMapID = 0; c.FormulaID = 0; });
                        }
                        if (formulaModel.ClaimInfo != null && formulaModel.ClaimInfo.Count > 0)
                        {
                            formulaMaster.FormulaClaimMapping = mapper.Map<ICollection<ClaimModel>, ICollection<FormulaClaimMapping>>(formulaModel.ClaimInfo);
                            formulaMaster.FormulaClaimMapping.ToList().ForEach(c => { c.CreatedBy = RequestUserID; c.FormulaClaimMapID = 0; c.FormulaID = 0; });
                        }
                        if (formulaModel.CriteriaInfo != null && formulaModel.CriteriaInfo.Length > 0)
                        {
                            List<FormulaCriteriaMapping> lstFormulaCriteriaMappings = new List<FormulaCriteriaMapping>();
                            formulaModel.CriteriaInfo.ToList().ForEach(criteriaID =>
                            {
                                FormulaCriteriaMapping formulaCriteriaMapping = new FormulaCriteriaMapping();
                                formulaCriteriaMapping.CriteriaID = criteriaID;
                                formulaCriteriaMapping.CreatedBy = RequestUserID;
                                lstFormulaCriteriaMappings.Add(formulaCriteriaMapping);
                            });
                            formulaMaster.FormulaCriteriaMapping = lstFormulaCriteriaMappings;
                        }
                        if (formulaModel.lstFormulaStatus != null && formulaModel.lstFormulaStatus.Count > 0)
                        {
                            foreach (var item in formulaModel.lstFormulaStatus)
                            {
                                var obj = await formulaMasterRepository.GetByIdAsync(item.FormulaID);
                                if (obj != null)
                                {
                                    obj.FormulaStatusCode = item.FormulaStatus;
                                    formulaMasterRepository.UpdateAsync(obj);
                                }
                            }
                        }
                        formulaMasterRepository.AddAsync(formulaMaster);

                        await Save();
                        await UpdateFormulaChangeCodeIncrements(formulaModel);
                        response.Message = localizer["msgInsertSuccess", new string[] { "Formula" }];
                    }
                    else
                    {
                        response.Result = ResultType.Warning;
                        response.Message = localizer["msgSomethingWentWrong"];
                    }
                }
                else
                {
                    response.Result = ResultType.Warning;
                    response.SetInfo(results);
                }
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }

        private async Task<GeneralResponse<bool>> UpdateFormulaChangeCodeIncrements(FormulaModel formulaModel)
        {
            var response = new GeneralResponse<bool>();
            try
            {
                string baseCode = string.Empty, spinCode = string.Empty, processCode = string.Empty, formulaTypeCode = string.Empty;

                FormulaMaster formulaMaster = await formulaMasterRepository.GetAsync(x => x.FormulaCode == formulaModel.FormulaMaster.FormulaCode);
                if (formulaMaster != null && formulaMaster.FormulaID > 0)
                {
                    if (formulaModel.FormulaMaster.FormulaCode.Length == 18) // JDE Code
                    {
                        var arr = formulaModel.FormulaMaster.FormulaCode.Split(new char[] { '.' });
                        if (arr.Length > 1)
                        {
                            baseCode = arr[0].Substring(4, 5);
                            spinCode = arr[1].Substring(0, 3);
                            processCode = arr[1].Substring(3, 1);
                            formulaTypeCode = arr[0].Substring(0, 3);
                        }
                        var result = formulaChangeCodeRepository.Query(true)
                                              .Include(x => x.FormulaTypeMaster)
                                              .Where(x => x.SiteID == formulaModel.FormulaMaster.SiteID &&
                                               x.IncrementNumber == Convert.ToInt32(baseCode) &&
                                              x.FormulaTypeMaster.FormulaTypeCode == formulaTypeCode).FirstOrDefault();
                        if (result == null)
                        {
                            int formulaTypeId = (await formulaTypeMasterRepository.GetAsync(x => x.FormulaTypeCode == formulaTypeCode)).FormulaTypeID;
                            FormulaChangeCode formulaChangeCode = new FormulaChangeCode();
                            formulaChangeCode.FormulaTypeID = formulaTypeId;
                            formulaChangeCode.SiteID = formulaModel.FormulaMaster.SiteID;
                            formulaChangeCode.IncrementNumber = Convert.ToInt32(baseCode);
                            formulaChangeCodeRepository.AddAsync(formulaChangeCode);
                        }
                    }
                    else
                    {
                        var arr = formulaModel.FormulaMaster.FormulaCode.Split(new char[] { '.' });
                        var RossBaseCode = arr[0].Substring(4, 4);
                        spinCode = arr[1].Substring(0, 3);
                        processCode = arr[1].Substring(3, 1);
                        var result = formulaRossCodeRepository.Query(true).Where(x => x.SiteID == formulaModel.FormulaMaster.SiteID &&
                                                  x.IncrementNumber == Convert.ToInt32(RossBaseCode)).FirstOrDefault();
                        if (result == null)
                        {

                            FormulaRossCode formulaRossCode = new FormulaRossCode();
                            formulaRossCode.SiteID = formulaModel.FormulaMaster.SiteID;
                            formulaRossCode.IncrementNumber = Convert.ToInt32(RossBaseCode);
                            formulaRossCode.CreatedOn = DateTime.Now;
                            formulaRossCode.CustomerName = arr[0].Substring(0, 3);
                            formulaRossCodeRepository.AddAsync(formulaRossCode);
                        }
                    }
                    FormulaRevision formulaRevision = new FormulaRevision()
                    {
                        FormulaID = formulaMaster.FormulaID,
                        RevisionNumber = Convert.ToInt32(spinCode),
                        CreatedOn = DateTime.Now,
                        ProcessCode = processCode
                    };
                    formulaRevisionRepository.AddAsync(formulaRevision);
                    await Save();
                    response.Data = true;
                }
            }
            catch (Exception ex)
            {
                exceptionHandler.LogError(ex);
                response.Exception = ex;
            }
            return response;
        }

        public async Task<GeneralResponse<bool>> SaveClaims(int formulaID, List<ClaimModel> lstFormulaClaims)
        {
            var response = new GeneralResponse<bool>();
            try
            {
                if (lstFormulaClaims != null && lstFormulaClaims.Count > 0)
                {
                    int[] claims = lstFormulaClaims.Select(o => o.ClaimID).ToArray();
                    formulaClaimMappingRepository.Delete(o => o.FormulaID == formulaID && !claims.Contains(o.ClaimID));

                    foreach (var claim in lstFormulaClaims)
                    {
                        var obj = await formulaClaimMappingRepository.GetAsync(x => x.FormulaID == formulaID && x.ClaimID == claim.ClaimID);
                        if (obj != null)
                        {
                            obj.Description = claim.Description;
                            obj.UpdatedBy = RequestUserID;
                            obj.UpdatedOn = DateTime.Now;
                            formulaClaimMappingRepository.UpdateAsync(obj);
                        }
                        else
                        {
                            FormulaClaimMapping formulaClaimMap = new FormulaClaimMapping();
                            formulaClaimMap.ClaimID = claim.ClaimID;
                            formulaClaimMap.FormulaID = formulaID;
                            formulaClaimMap.Description = claim.Description;
                            formulaClaimMap.CreatedBy = RequestUserID;
                            formulaClaimMappingRepository.AddAsync(formulaClaimMap);
                        }
                    }
                }
                else
                {
                    await formulaClaimMappingRepository.DeleteAsync(o => o.FormulaID == formulaID);
                }
                await Save();
                response.Message = localizer["msgUpdateSuccess", new string[] { "Claims" }];
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }

        public async Task<GeneralResponse<bool>> SaveCriteria(int formulaID, int[] lstCriteria)
        {
            var response = new GeneralResponse<bool>();
            try
            {
                if (lstCriteria != null && lstCriteria.Length > 0)
                {
                    await formulaCriteriaMappingRepository.DeleteAsync(o => o.FormulaID == formulaID && !lstCriteria.Contains(o.CriteriaID));

                    foreach (int criteriaID in lstCriteria)
                    {
                        var obj = await formulaCriteriaMappingRepository.GetAsync(x => x.FormulaID == formulaID && x.CriteriaID == criteriaID);
                        if (obj != null)
                        {
                            obj.UpdatedBy = RequestUserID;
                            obj.UpdatedOn = DateTime.Now;
                            formulaCriteriaMappingRepository.UpdateAsync(obj);
                        }
                        else
                        {
                            FormulaCriteriaMapping formulaCriteriaMap = new FormulaCriteriaMapping();
                            formulaCriteriaMap.CriteriaID = criteriaID;
                            formulaCriteriaMap.FormulaID = formulaID;
                            formulaCriteriaMap.CreatedBy = RequestUserID;
                            formulaCriteriaMappingRepository.AddAsync(formulaCriteriaMap);
                        }
                    }
                }
                else
                {
                    await formulaClaimMappingRepository.DeleteAsync(o => o.FormulaID == formulaID);
                }
                await Save();
                response.Message = localizer["msgUpdateSuccess", new string[] { "Criteria" }];
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }

        public async Task<GeneralResponse<bool>> SaveYieldLoss(int formulaID, FormulaMasterModel formulaMasterModel)
        {
            var response = new GeneralResponse<bool>();
            try
            {
                if (formulaMasterModel != null && formulaID > 0)
                {
                    var obj = await formulaMasterRepository.GetByIdAsync(formulaID);
                    if (obj != null)
                    {
                        FormulaMaster formulaMaster = SetYieldLoss(formulaMasterModel, obj);
                        formulaMasterRepository.UpdateAsync(formulaMaster);
                    }
                }
                await Save();
                response.Message = localizer["msgUpdateSuccess", new string[] { "Yield Loss" }];
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }

        public async Task<GeneralResponse<bool>> DeleteFormula(int[] formulaIDs)
        {
            var response = new GeneralResponse<bool>() { Data = false };
            try
            {
                if (formulaIDs != null && formulaIDs.Any())
                {
                    foreach (var formulaID in formulaIDs)
                    {
                        var objFormula = await formulaMasterRepository.GetByIdAsync(formulaID);
                        if (objFormula != null)
                        {
                            objFormula.UpdatedBy = RequestUserID;
                            objFormula.UpdatedOn = DateTime.Now;
                            objFormula.IsActive = false;
                            objFormula.IsDeleted = true;
                            formulaMasterRepository.UpdateAsync(objFormula);
                        }
                    }
                    await Save();
                }
                response.Message = localizer["msgDeleteSuccess", new string[] { "Formula" }];
                response.Data = true;
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }
        #endregion

        private FormulaMaster SetYieldLoss(FormulaMasterModel formulaMasterModel, FormulaMaster formulaMaster)
        {
            formulaMaster.S10CoreDoughYield = formulaMasterModel.S10CoreDoughYield;
            formulaMaster.S65SyrupYield = formulaMasterModel.S65SyrupYield;
            formulaMaster.S68_1LiquidBlendYield = formulaMasterModel.S68_1LiquidBlendYield;
            formulaMaster.S68_2LiquidBlendYield = formulaMasterModel.S68_2LiquidBlendYield;
            formulaMaster.S40CoreIngredientYield = formulaMasterModel.S40CoreIngredientYield;
            formulaMaster.S40ToppingIngredientYield = formulaMasterModel.S40ToppingIngredientYield;
            formulaMaster.S60DryBlendYield = formulaMasterModel.S60DryBlendYield;
            formulaMaster.S63DryBlendYield = formulaMasterModel.S63DryBlendYield;
            formulaMaster.S20LayerDoughYield = formulaMasterModel.S20LayerDoughYield;
            formulaMaster.S30CoatingYield = formulaMasterModel.S30CoatingYield;
            formulaMaster.S10CoreDoughYieldLoss = formulaMasterModel.S10CoreDoughYieldLoss;
            formulaMaster.S65SyrupYieldLoss = formulaMasterModel.S65SyrupYieldLoss;
            formulaMaster.S68_1LiquidBlendYieldLoss = formulaMasterModel.S68_1LiquidBlendYieldLoss;
            formulaMaster.S68_2LiquidBlendYieldLoss = formulaMasterModel.S68_2LiquidBlendYieldLoss;
            formulaMaster.S40CoreIngredientYieldLoss = formulaMasterModel.S40CoreIngredientYieldLoss;
            formulaMaster.S40ToppingIngredientYieldLoss = formulaMasterModel.S40ToppingIngredientYieldLoss;
            formulaMaster.S60DryBlendYieldLoss = formulaMasterModel.S60DryBlendYieldLoss;
            formulaMaster.S63DryBlendYieldLoss = formulaMasterModel.S63DryBlendYieldLoss;
            formulaMaster.S20LayerDoughYieldLoss = formulaMasterModel.S20LayerDoughYieldLoss;
            formulaMaster.S30CoatingYieldLoss = formulaMasterModel.S30CoatingYieldLoss;
            formulaMaster.UpdatedOn = DateTime.Now;
            formulaMaster.UpdatedBy = RequestUserID;
            return formulaMaster;
        }

        private void CheckAndCopyColumn(ref DataTable dt, string columnName, string displayColName)
        {
            string[] fixedColumns = new string[] { "FormulaReference", "AltCode", "CustomerCode", "S70Code", "PowderXRefCode", "FormulaPurpose",
                    "Claims", "AllergenCode", "BarFormatDescription", "FormulaStatusCode", "CustomerName", "ProductDescription", "FlavourDescription" };
            if (!string.IsNullOrEmpty(columnName))
            {
                if (fixedColumns.Contains(columnName))
                {
                    DataColumn col = new DataColumn();
                    col.DataType = dt.Columns[columnName].DataType;
                    col.ColumnName = displayColName;
                    col.Expression = columnName;
                    dt.Columns.Add(col);
                }
                else
                {
                    dt.Columns[columnName].ColumnName = displayColName;
                }
            }
            else
            {
                DataColumn col = new DataColumn();
                col.DataType = typeof(string);
                col.ColumnName = displayColName;
                dt.Columns.Add(col);
            }
        }

        private string GetUserByUserID(int? userID, DateTime? updatedOn)
        {
            string displayName = string.Empty;
            if (userID.HasValue)
            {
                var objUser = userMasterRepository.Get(x => x.UserID == userID.Value);
                if (objUser != null)
                {
                    displayName = string.Format("{0} on {1}", objUser.DisplayName, Convert.ToDateTime(updatedOn).ToString("dd/MM/yy hh:mm"));
                }
            }
            else if (updatedOn != null)
            {
                displayName += Convert.ToDateTime(updatedOn).ToString("dd/MM/yy hh:mm");
            }
            return displayName;
        }

        private decimal CalculateAttributeValue(List<string> attributeList, string[] attributeValues, decimal? ingredientAmount, string attributeName)
        {
            decimal value = 0;
            var attributeIndex = attributeList.IndexOf(attributeName);
            if (attributeIndex >= 0 && attributeIndex <= attributeValues.Count() && decimal.TryParse(Convert.ToString(attributeValues[attributeIndex]), out value))
            {
                value = (value / 100) * (ingredientAmount ?? 0);
            }
            return value;
        }

        private DSActualInfo GetDSActualObject(string name, decimal? value, int displayColumnOrder, int displayItemOrder)
        {
            DSActualInfo dSActualInfo = new DSActualInfo()
            {
                NutrientID = 0,
                Name = name,
                MeasurementUnit = string.Empty,
                DisplayColumnOrder = displayColumnOrder,
                DisplayItemOrder = displayItemOrder,
                PerServingValue = value,
                Per100GramValue = 0
            };
            return dSActualInfo;
        }
    }
}