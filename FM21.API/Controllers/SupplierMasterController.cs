using FM21.Core.Model;
using FM21.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace FM21.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [SwaggerTag("All Supplier Master related API methods")]
    //[ApiExplorerSettings(GroupName = "v1.0")]
    [ApiController]
    public class SupplierMasterController : ControllerBase
    {
        private readonly ISupplierMasterService supplierMasterService;

        public SupplierMasterController(ISupplierMasterService supplierMasterService)
        {
            this.supplierMasterService = supplierMasterService;
        }

        [SwaggerOperation(Summary = "Get list of all Supplier List.", Tags = new string[] { "SupplierMaster" })]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var response = await supplierMasterService.GetAll();
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Get Supplier list for search (page wise, sort, filter).", Tags = new string[] { "SupplierMaster" })]
        [HttpGet("GetSearchList")]
        public async Task<IActionResult> GetSearchList([FromQuery] SearchFilter searchFilter)
        {
            if (ModelState.IsValid)
            {
                var response = await supplierMasterService.GetPageWiseData(searchFilter.Search, searchFilter.PageIndex, searchFilter.PageSize, searchFilter.SortColumn, searchFilter.SortDirection);
                return new OkObjectResult(response);
            }
            else
            {
                return BadRequest();
            }
        }

        [SwaggerOperation(Summary = "Get Supplier by SupplierID.", Tags = new string[] { "SupplierMaster" })]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSupplier(int id)
        {
            var obj = await supplierMasterService.Get(id);
            if (obj == null)
            {
                return NotFound();
            }
            return new OkObjectResult(obj);
        }

        [SwaggerOperation(Summary = "Add new Supplier.", Tags = new string[] { "SupplierMaster" })]
        [HttpPost]
        public async Task<ActionResult<bool>> PostSupplier(SupplierMasterModel model)
        {
            var response = await supplierMasterService.Create(model);
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Update existing Supplier.", Tags = new string[] { "SupplierMaster" })]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSupplier(int id, SupplierMasterModel model)
        {
            model.SupplierId = id;
            var response = await supplierMasterService.Update(model);
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Delete the Supplier.", Tags = new string[] { "SupplierMaster" })]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            var response = await supplierMasterService.Delete(id);
            return new JsonResult(response);
        }
    }
}