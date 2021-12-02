using FM21.Service;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace FM21.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [SwaggerTag("All the common API methods")]
    //[ApiExplorerSettings(GroupName = "v1.0")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly ICommonService commonService;

        public CommonController(ICommonService commonService)
        {
            this.commonService = commonService;
        }

        [SwaggerOperation(Summary = "Get list of all the sites.", Tags = new string[] { "Common" })]
        [HttpGet("GetAllSite")]
        public async Task<IActionResult> GetAllSite()
        {
            var response = await commonService.GetAllSite();
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Get list of all the product types.", Tags = new string[] { "Common" })]
        [HttpGet("GetAllProductType")]
        public async Task<IActionResult> GetAllProductType()
        {
            var response = await commonService.GetAllProductType();
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Get list of all the sites along with product type.", Tags = new string[] { "Common" })]
        [HttpGet("GetAllSiteProductType")]
        public async Task<IActionResult> GetAllSiteProductType(bool retrieveInActive = false)
        {
            var response = await commonService.GetAllSiteProductType(retrieveInActive);
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Get list of all the Instruction category.", Tags = new string[] { "InstructionMaster" })]
        [HttpGet("GetAllInstructionCategory")]
        public async Task<IActionResult> GetAllInstructionCategory()
        {
            var response = await commonService.GetAllInstructionCategory();
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Get list of Instruction category by site and product type.", Tags = new string[] { "InstructionMaster" })]
        [HttpGet("GetAllInstructionCategoryBySiteProductMapID")]
        public async Task<IActionResult> GetAllInstructionCategoryBySiteProductMapID(int siteProductMapID)
        {
            var response = await commonService.GetAllInstructionCategoryBySiteProductMapID(siteProductMapID);
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Get list of all the Brokers.", Tags = new string[] { "Common" })]
        [HttpGet("GetAllBroker")]
        public async Task<IActionResult> GetAllBroker()
        {
            var response = await commonService.GetAllBroker();
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Get list of all the Kosher Code.", Tags = new string[] { "Common" })]
        [HttpGet("GetAllKosherCode")]
        public async Task<IActionResult> GetAllKosherCode()
        {
            var response = await commonService.GetAllKosherCode();
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Get list of all the HACCP  records based on supplied type of it.", Tags = new string[] { "Common" })]
        [HttpGet("GetAllHACCPData")]
        public async Task<IActionResult> GetAllHACCPData(string hACCPType = "")
        {
            var response = await commonService.GetAllHACCPData(hACCPType);
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Get list of all the RM Status records based on supplied type of it. Pass blank string to get all.", Tags = new string[] { "Common" })]
        [HttpGet("GetAllRMStatus")]
        public async Task<IActionResult> GetAllRMStatus(string rMStatusType = "")
        {
            var response = await commonService.GetAllRMStatus(rMStatusType);
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Get list of all the Storage Condition.", Tags = new string[] { "Common" })]
        [HttpGet("GetAllStorageCondition")]
        public async Task<IActionResult> GetAllStorageCondition()
        {
            var response = await commonService.GetAllStorageCondition();
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Get list of all the Country.", Tags = new string[] { "Common" })]
        [HttpGet("GetAllCountry")]
        public async Task<IActionResult> GetAllCountry()
        {
            var response = await commonService.GetAllCountry();
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Get list of all Production Line by site.", Tags = new string[] { "Common" })]
        [HttpGet("GetAllProductionLineBySite")]
        public async Task<IActionResult> GetAllProductionLineBySite(int siteID, int formulaID)
        {
            var response = await commonService.GetAllProductionLineBySite(siteID, formulaID);
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Get list of all Production Line Mixer by site.", Tags = new string[] { "Common" })]
        [HttpGet("GetAllProductionLineMixerBySite")]
        public async Task<IActionResult> GetAllProductionLineMixerBySite(int siteID)
        {
            var response = await commonService.GetAllProductionLineMixerBySite(siteID);
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Get list of all the Release Agent.", Tags = new string[] { "Common" })]
        [HttpGet("GetAllReleaseAgent")]
        public async Task<IActionResult> GetAllReleaseAgent()
        {
            var response = await commonService.GetAllReleaseAgent();
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Get list of all the Pko Percentage.", Tags = new string[] { "Common" })]
        [HttpGet("GetAllPkoPercentage")]
        public async Task<IActionResult> GetAllPkoPercentage()
        {
            var response = await commonService.GetAllPkoPercentage();
            return new JsonResult(response);
        }
        
        [SwaggerOperation(Summary = "Get All Bar Format", Tags = new string[] { "Common" })]
        [HttpGet("GetAllBarFormatMaster")]
        public async Task<IActionResult> GetAllBarFormatMaster()
        {
            var response = await commonService.GetAllBarFormatMaster();
            return new JsonResult(response);
        }
        
        [SwaggerOperation(Summary = "Get All Bar Format Code Master", Tags = new string[] { "Common" })]
        [HttpGet("GetAllBarFormatCodeMaster")]
        public async Task<IActionResult> GetAllBarFormatCodeMaster()
        {
            var response = await commonService.GetAllBarFormatCodeMaster();
            return new JsonResult(response);
        }
        
        [SwaggerOperation(Summary = "Get All Formula Type Master", Tags = new string[] { "Common" })]
        [HttpGet("GetAllFormulaTypeMaster")]
        public async Task<IActionResult> GetAllFormulaTypeMaster()
        {
            var response = await commonService.GetAllFormulaTypeMaster();
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Get All Sitter Width Master data. Pass site to get site specific data or zero to get all site data.", Tags = new string[] { "Common" })]
        [HttpGet("GetAllSitterWidth")]
        public async Task<IActionResult> GetAllSitterWidth(int siteID)
        {
            var response = await commonService.GetAllSitterWidth(siteID);
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Get All Extrusion Die Master data", Tags = new string[] { "Common" })]
        [HttpGet("GetAllExtrusionDie")]
        public async Task<IActionResult> GetAllExtrusionDie()
        {
            var response = await commonService.GetAllExtrusionDie();
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Get All Internal QC MAV LookUp Master data", Tags = new string[] { "Common" })]
        [HttpGet("GetAllInternalQCMAVLookUp")]
        public async Task<IActionResult> GetAllInternalQCMAVLookUp()
        {
            var response = await commonService.GetAllInternalQCMAVLookUp();
            return new JsonResult(response);
        }
    }

}