using FM21.Core.Model;
using FM21.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace FM21.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [SwaggerTag("All Allergern Master related API methods")]
    //[ApiExplorerSettings(GroupName = "v1.0")]
    [ApiController]
    public class AllergenMasterController : ControllerBase
    {
        private readonly IAllergenMasterService allergenMasterService;

        public AllergenMasterController(IAllergenMasterService allergenMasterService)
        {
            this.allergenMasterService = allergenMasterService;
        }

        [SwaggerOperation(Summary = "Get Allergen list for search (page wise, sort, filter).", Tags = new string[] { "AllergenMaster" })]
        [HttpGet("GetSearchList")]
        public async Task<IActionResult> GetSearchList([FromQuery] SearchFilter searchFilter)
        {
            if (ModelState.IsValid)
            {
                var response = await allergenMasterService.GetPageWiseData(searchFilter.Search, searchFilter.PageIndex, searchFilter.PageSize, searchFilter.SortColumn, searchFilter.SortDirection);
                return new JsonResult(response);
            }
            else
            {
                return BadRequest();
            }
        }

        [SwaggerOperation(Summary = "Get list of all Allergen List.", Tags = new string[] { "AllergenMaster" })]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var response = await allergenMasterService.GetAll();
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Get Allergen by AllergenId.", Tags = new string[] { "AllergenMaster" })]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAllergen(int id)
        {
            var obj = await allergenMasterService.Get(id);
            if (obj == null)
            {
                return NotFound(obj);
            }
            return new OkObjectResult(obj);
        }

        [SwaggerOperation(Summary = "Add new Allergen.", Tags = new string[] { "AllergenMaster" })]
        [HttpPost]
        public async Task<ActionResult<bool>> PostAllergen(AllergenMasterModel model)
        {
            var response = await allergenMasterService.Create(model);
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Update existing Allergen.", Tags = new string[] { "AllergenMaster" })]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAllergen(int id, AllergenMasterModel model)
        {
            model.AllergenID = id;
            var response = await allergenMasterService.Update(model);
            return new JsonResult(response);
        }
        [SwaggerOperation(Summary = "Delete the Allergen.", Tags = new string[] { "AllergenMaster" })]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAllergen(int id)
        {
            var response = await allergenMasterService.Delete(id);
            return new JsonResult(response);
        }
    }
}
