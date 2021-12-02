using FM21.Core.Model;
using FM21.Service;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace FM21.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [SwaggerTag("All Formula master related API methods")]
    //[ApiExplorerSettings(GroupName = "v1.0")]
    [ApiController]
    public class FormulaMasterController : ControllerBase
    {
        private readonly IFormulaMasterService formulaMasterService;

        public FormulaMasterController(IFormulaMasterService formulaMasterService)
        {
            this.formulaMasterService = formulaMasterService;
        }

        [SwaggerOperation(Summary = "Get Formula details by FormulaID.", Tags = new string[] { "FormulaMaster" })]
        [HttpGet("GetFormulaByFormulaID/{formulaID}")]
        public async Task<IActionResult> GetFormulaByFormulaID(int formulaID)
        {
            var obj = await formulaMasterService.GetFormulaByFormulaID(formulaID);
            return new JsonResult(obj);
        }

        [SwaggerOperation(Summary = "Update existing Formula.", Tags = new string[] { "FormulaMaster" })]
        [HttpPut("UpdateFormula/{formulaID}")]
        public async Task<IActionResult> PutFormula(int formulaID, FormulaModel model)
        {
            model.FormulaMaster.FormulaID = formulaID;
            var response = await formulaMasterService.UpdateFormula(model);
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Search formula based on supplied parameters", Tags = new string[] { "FormulaMaster" })]
        [HttpGet("SearchFormula")]
        public async Task<IActionResult> SearchFormula([FromQuery] FormulaSearchFilter searchFilter)
        {
            var response = await formulaMasterService.SearchFormula(searchFilter);
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Update Formula search history for user.", Tags = new string[] { "FormulaMaster" })]
        [HttpPut("UpdateIngredientSearchHistory")]
        public async Task<IActionResult> UpdateFormulaSearchHistory(int userID, string searchData)
        {
            var response = await formulaMasterService.UpdateFormulaSearchHistory(userID, searchData);
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Get last 5 Formula search history by user", Tags = new string[] { "FormulaMaster" })]
        [HttpGet("GetFormulaSearchHistory")]
        public async Task<IActionResult> GetFormulaSearchHistory(int userID)
        {
            var response = await formulaMasterService.GetFormulaSearchHistory(userID);
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Get all claims along with formula mapping.", Tags = new string[] { "FormulaMaster" })]
        [HttpGet("GetClaimsByFormulaID/{formulaID}")]
        public async Task<IActionResult> GetClaimsByFormulaID(int formulaID)
        {
            var response = await formulaMasterService.GetClaimsByFormulaID(formulaID);
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Get all Criteria along with formula mapping.", Tags = new string[] { "FormulaMaster" })]
        [HttpGet("GetCriteriaByFormulaID/{formulaID}")]
        public async Task<IActionResult> GetCriteriaByFormulaID(int formulaID)
        {
            var response = await formulaMasterService.GetCriteriaByFormulaID(formulaID);
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Get list of all amino acid nutrients.", Tags = new string[] { "FormulaMaster" })]
        [HttpPut("GetAminoAcidNutrient")]
        public async Task<IActionResult> GetAminoAcidInfo([FromBody] FormulaIngredient[] arrIngredients)
        {
            var response = await formulaMasterService.GetAminoAcidInfo(arrIngredients);
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Get list of all PDCAAS information.", Tags = new string[] { "FormulaMaster" })]
        [HttpPut("GetPDCAASInfo")]
        public async Task<IActionResult> GetPDCAASInfo([FromBody] FormulaIngredient[] formulaIngredients)
        {
            var response = await formulaMasterService.GetPDCAASInfo(formulaIngredients);
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Get list of all Carbohydrate information.", Tags = new string[] { "FormulaMaster" })]
        [HttpPut("GetCarbohydrateInfo")]
        public async Task<IActionResult> GetCarbohydrateInfo([FromBody] FormulaIngredient[] formulaIngredients)
        {
            var response = await formulaMasterService.GetCarbohydrateInfo(formulaIngredients);
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Get list of all DS Actual information.", Tags = new string[] { "FormulaMaster" })]
        [HttpPut("GetDSActualInfo")]
        public async Task<IActionResult> GetDSActualInfo([FromBody] FormulaIngredient[] formulaIngredients)
        {
            var response = await formulaMasterService.GetDSActualInfo(formulaIngredients);
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Get Ingredient list information along with breakdown.", Tags = new string[] { "FormulaMaster" })]
        [HttpPut("GetIngredientListInfo")]
        public async Task<IActionResult> GetIngredientListInfo([FromBody] FormulaIngredient[] formulaIngredients)
        {
            var response = await formulaMasterService.GetIngredientListInfo(formulaIngredients);
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Get all Reconstitution master data.", Tags = new string[] { "FormulaMaster" })]
        [HttpGet("GetAllReconstitutionMaster")]
        public async Task<IActionResult> GetAllReconstitutionMaster()
        {
            var response = await formulaMasterService.GetAllReconstitutionMaster();
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Get all Powder Liquid Master data.", Tags = new string[] { "FormulaMaster" })]
        [HttpGet("GetAllPowderLiquidMaster")]
        public async Task<IActionResult> GetAllPowderLiquidMaster()
        {
            var response = await formulaMasterService.GetAllPowderLiquidMaster();
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Get all Powder Blender Master data, Pass zero for all site data.", Tags = new string[] { "FormulaMaster" })]
        [HttpGet("GetAllPowderBlenderMaster/{siteID}")]
        public async Task<IActionResult> GetAllPowderBlenderMaster(int siteID = 0)
        {
            var response = await formulaMasterService.GetAllPowderBlenderMaster(siteID);
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Get all Powder Unit Master data, Pass zero for all site data.", Tags = new string[] { "FormulaMaster" })]
        [HttpGet("GetAllPowderUnitMaster/{siteID}")]
        public async Task<IActionResult> GetAllPowderUnitMaster(int siteID = 0)
        {
            var response = await formulaMasterService.GetAllPowderUnitMaster(siteID);
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Get all Powder Unit Serving data.", Tags = new string[] { "FormulaMaster" })]
        [HttpGet("GetAllPowderUnitServingMaster")]
        public async Task<IActionResult> GetAllPowderUnitServingMaster()
        {
            var response = await formulaMasterService.GetAllPowderUnitServingMaster();
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Update Criteria details.", Tags = new string[] { "FormulaMaster" })]
        [HttpPut("UpdateCriteria")]
        public async Task<IActionResult> PutCriteria(int formulaID, int[] lstCriteria)
        {
            var response = await formulaMasterService.SaveCriteria(formulaID, lstCriteria);
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Add new Formula.", Tags = new string[] { "FormulaMaster" })]
        [HttpPost("SaveFormula")]
        public async Task<IActionResult> SaveFormula(FormulaModel model)
        {
            var response = await formulaMasterService.SaveFormula(model);
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Get Formula Regulatory CategoryMaster", Tags = new string[] { "FormulaMaster" })]
        [HttpGet("GetFormulaRegulatoryCategoryMaster")]
        public async Task<IActionResult> GetFormulaRegulatoryCategoryMaster()
        {

            var response = await formulaMasterService.GetFormulaRegulatoryCategoryMaster();
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Get Datasheet Format Master", Tags = new string[] { "FormulaMaster" })]
        [HttpGet("GetDatasheetFormatMaster")]
        public async Task<IActionResult> GetDatasheetFormatMaster()
        {

            var response = await formulaMasterService.GetDatasheetFormatMaster();
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Update Yield Loss", Tags = new string[] { "FormulaMaster" })]
        [HttpPut("UpdateYieldLoss")]
        public async Task<IActionResult> UpdateYieldLoss(int formulaID, FormulaMasterModel formulaMasterModel)
        {

            var response = await formulaMasterService.SaveYieldLoss(formulaID, formulaMasterModel);
            return new JsonResult(response);
        }
        
        [SwaggerOperation(Summary = "Get Yield Loss Default Values", Tags = new string[] { "FormulaMaster" })]
        [HttpGet("GetFormulaYieldLossFactorDefaultValue")]
        public async Task<IActionResult> GetFormulaYieldLossFactorDefaultValue(int siteID, string mixerType)
        {
            var response = await formulaMasterService.GetFormulaYieldLossFactorDefaultValue(siteID, mixerType);
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Get Formula Home Page Header Info", Tags = new string[] { "FormulaMaster" })]
        [HttpGet("GetFormulaHomePageHeaderInfo")]
        public async Task<IActionResult> GetFormulaHomePageHeaderInfo(int formulaID)
        {
            var response = await formulaMasterService.GetFormulaHomePageHeaderInfo(formulaID);
            return new JsonResult(response);
        }
        
        [SwaggerOperation(Summary = "Delete Formula", Tags = new string[] { "FormulaMaster" })]
        [HttpPost("DeleteFormula")]
        public async Task<IActionResult> DeleteFormula(int[] formulaID)
        {
            var response = await formulaMasterService.DeleteFormula(formulaID);
            return new JsonResult(response);
        }
        
        [SwaggerOperation(Summary = "Get Formula Change Code List", Tags = new string[] { "FormulaMaster" })]
        [HttpGet("GetFormulaChangeCodeList")]
        public async Task<IActionResult> GetFormulaChangeCodeList([FromQuery] FormulaChangeCodeParam  formulaChangeCodeParam)
        {
            var response = await formulaMasterService.GetFormulaChangeCodeList(formulaChangeCodeParam);
            return new JsonResult(response);
        }
        
        [SwaggerOperation(Summary = "Check If New Formula Code Exist ", Tags = new string[] { "FormulaMaster" })]
        [HttpGet("IsFormulaCodeExist")]
        public async Task<IActionResult> IsFormulaCodeExist(string formulaCode)
        {
            var response = await formulaMasterService.IsFormulaCodeExist(formulaCode);
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Get Formula Status List", Tags = new string[] { "FormulaMaster" })]
        [HttpGet("GetFormulaStatusMaster")]
        public async Task<IActionResult> GetFormulaStatusMaster()
        {
            var response = await formulaMasterService.GetFormulaStatusMaster();
            return new JsonResult(response);
        }
      
        [SwaggerOperation(Summary = "Import Internal QC Info By FormulaCode ", Tags = new string[] { "FormulaMaster" })]
        [HttpGet("ImportInternalQCInfoByFormulaCode")]
        public async Task<IActionResult> ImportInternalQCInfoByFormulaCode(string formulaCode)
        {
            var response = await formulaMasterService.ImportInternalQCInfoByFormulaCode(formulaCode);
            return new JsonResult(response);
        }
        [SwaggerOperation(Summary = "Get Target Info of Formula", Tags = new string[] { "FormulaMaster" })]
        [HttpGet("GetTargetInfo")]
        public async Task<IActionResult> GetTargetInfo([FromQuery] FormulaTargetInfoParam formulaTargetInfoParam)
        {
            var response = await formulaMasterService.GetTargetInfo(formulaTargetInfoParam);
            return new JsonResult(response);
        }
    }
}