using FM21.Core;
using FM21.Core.Model;
using FM21.Entities;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace FM21.Service
{
    public interface IFormulaMasterService
    {
        Task<GeneralResponse<FormulaModel>> GetFormulaByFormulaID(int formulaID);
        Task<GeneralResponse<ICollection<FormulaClaimsModel>>> GetClaimsByFormulaID(int formulaID);
        Task<GeneralResponse<ICollection<FormulaCriteriaModel>>> GetCriteriaByFormulaID(int formulaID);
        Task<GeneralResponse<ICollection<NutrientModel>>> GetAminoAcidInfo(FormulaIngredient[] arrIngredients);
        Task<GeneralResponse<PDCAASInfo>> GetPDCAASInfo(FormulaIngredient[] formulaIngredients);
        Task<GeneralResponse<CarbohydrateInfo>> GetCarbohydrateInfo(FormulaIngredient[] formulaIngredients);
        Task<GeneralResponse<ICollection<DSActualInfo>>> GetDSActualInfo(FormulaIngredient[] arrIngredients);
        Task<GeneralResponse<ICollection<IngredientListInfo>>> GetIngredientListInfo(FormulaIngredient[] formulaIngredients);
        Task<GeneralResponse<ICollection<ReconstitutionMaster>>> GetAllReconstitutionMaster();
        Task<GeneralResponse<ICollection<PowderLiquidMaster>>> GetAllPowderLiquidMaster();
        Task<GeneralResponse<ICollection<PowderBlenderMasterModel>>> GetAllPowderBlenderMaster(int siteID);
        Task<GeneralResponse<ICollection<UnitServingMasterModel>>> GetAllPowderUnitMaster(int siteID);
        Task<GeneralResponse<ICollection<UnitServingMasterModel>>> GetAllPowderUnitServingMaster();
        Task<PagedTableResponse<DataTable>> SearchFormula(FormulaSearchFilter searchFilter);
        Task<GeneralResponse<bool>> UpdateFormula(FormulaModel formula);
        Task<GeneralResponse<bool>> SaveClaims(int formulaID, List<ClaimModel> lstFormulaClaims);
        Task<GeneralResponse<bool>> SaveCriteria(int formulaID, int[] lstCriteria);
        Task<GeneralResponse<bool>> SaveFormula(FormulaModel formulaModel);
        Task<GeneralResponse<ICollection<FormulaRegulatoryCategoryMaster>>> GetFormulaRegulatoryCategoryMaster();
        Task<GeneralResponse<ICollection<DatasheetFormatMaster>>> GetDatasheetFormatMaster();
        Task<GeneralResponse<bool>> SaveYieldLoss(int formulaID, FormulaMasterModel formulaMasterModel);
        Task<GeneralResponse<DataTable>> GetFormulaYieldLossFactorDefaultValue(int siteId, string mixerType);
        Task<GeneralResponse<DataTable>> GetFormulaHomePageHeaderInfo(int formulaID);
        Task<GeneralResponse<ICollection<FormulaSearchHistory>>> GetFormulaSearchHistory(int userID);
        Task<GeneralResponse<bool>> UpdateFormulaSearchHistory(int userID, string searchData);
        Task<GeneralResponse<bool>> DeleteFormula(int[] formulaIDs);
        Task<GeneralResponse<DataTable>> GetFormulaChangeCodeList(FormulaChangeCodeParam formulaChangeCodeParam);
        Task<GeneralResponse<bool>> IsFormulaCodeExist(string formulaCode);
        Task<GeneralResponse<ICollection<FormulaStatusMaster>>> GetFormulaStatusMaster();
        Task<GeneralResponse<FormulaMasterModel>> ImportInternalQCInfoByFormulaCode(string formulaCode);
        Task<GeneralResponse<List<FormulaTargetModel>>> GetTargetInfo(FormulaTargetInfoParam formulaTargetInfoParam);
    }
}