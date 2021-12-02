using FM21.Core.Model;
using FM21.Service;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace FM21.API.Controllers
{

    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [SwaggerTag("All Regulatory Data related API methods")]
    
    [ApiController]
    public class RegulatoryMasterController : ControllerBase
    {
        private readonly IRegulatoryMasterService regulatoryMasterService;

        public RegulatoryMasterController(IRegulatoryMasterService regulatoryMasterService)
        {
            this.regulatoryMasterService = regulatoryMasterService;
        }

        [SwaggerOperation(Summary = "Get list of all regulatory data.", Tags = new string[] { "RegulatoryMaster" })]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var response = await regulatoryMasterService.GetAll();
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Get regulatory data list for search (page wise, sort, filter).", Tags = new string[] { "RegulatoryMaster" })]
        [HttpGet("GetSearchList")]
        public async Task<IActionResult> GetSearchList([FromQuery] SearchFilter searchFilter)
        {
            if (ModelState.IsValid)
            {
                var response = await regulatoryMasterService.GetPageWiseData(searchFilter.Search, searchFilter.PageIndex, searchFilter.PageSize, searchFilter.SortColumn, searchFilter.SortDirection);
                return new JsonResult(response);
            }
            else
            {
                return BadRequest();
            }
        }

        [SwaggerOperation(Summary = "Get regulatory data by RegulatoryId.", Tags = new string[] { "RegulatoryMaster" })]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRegulatory(int id)
        {
            var obj = await regulatoryMasterService.Get(id);
            if (obj == null)
            {
                return NotFound();
            }
            return new JsonResult(obj);
        }

        [SwaggerOperation(Summary = "Add new regulatory data.", Tags = new string[] { "RegulatoryMaster" })]
        [HttpPost]
        public async Task<ActionResult<bool>> PostRegulatory(RegulatoryModel model)
        {
            var response = await regulatoryMasterService.Create(model);
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Update existing regulatory data.", Tags = new string[] { "RegulatoryMaster" })]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRegulatory(int id, RegulatoryModel model)
        {
            model.RegulatoryId = id;
            var response = await regulatoryMasterService.Update(model);
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Delete the regulatory data.", Tags = new string[] { "RegulatoryMaster" })]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRegulatory(int id)
        {
            var response = await regulatoryMasterService.Delete(id);
            return new JsonResult(response);
        }
        [SwaggerOperation(Summary = "Get all configured unit of measurement", Tags = new string[] { "RegulatoryMaster" })]
        [HttpGet("GetUnitOfMeasurement")]
        public async Task<IActionResult> GetUnitOfMeasurement()
        {
            var response = await regulatoryMasterService.GetUnitOfMeasurement();
            return new JsonResult(response);
        }
    }
}
