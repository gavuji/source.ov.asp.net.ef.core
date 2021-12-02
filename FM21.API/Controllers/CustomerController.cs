using FM21.Core.Model;
using FM21.Service;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace FM21.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [SwaggerTag("All User and permission related API methods")]
    //[ApiExplorerSettings(GroupName = "v1.0")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService customerService;

        public CustomerController(ICustomerService customerService)
        {
            this.customerService = customerService;
        }        

        [SwaggerOperation(Summary = "Get list of all Customer.", Tags = new string[] { "Customer" })]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var response = await customerService.GetAll();
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Get Customer list for search (page wise, sort, filter).", Tags = new string[] { "Customer" })]
        [HttpGet("GetSearchList")]
        public async Task<IActionResult> GetSearchList([FromQuery] SearchFilter searchFilter)
        {
            if (ModelState.IsValid)
            {
                var response = await customerService.GetPageWiseData(searchFilter.Search, searchFilter.PageIndex, searchFilter.PageSize, searchFilter.SortColumn, searchFilter.SortDirection);
                return new OkObjectResult(response);
            }
            else
            {
                return BadRequest();
            }
        }

        [SwaggerOperation(Summary = "Get Customer by CustomerId.", Tags = new string[] { "Customer" })]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomer(int id)
        {
            var obj = await customerService.Get(id);
            if (obj == null)
            {
                return NotFound(obj);
            }
            return new OkObjectResult(obj);
        }

        [SwaggerOperation(Summary = "Add new Customer.", Tags = new string[] { "Customer" })]
        [HttpPost]
        public async Task<ActionResult<bool>> PostCustomer(CustomerModel model)
        {
            var response = await customerService.Create(model);
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Update existing Customer.", Tags = new string[] { "Customer" })]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, CustomerModel model)
        {
            model.CustomerId = id;
            var response = await customerService.Update(model);
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Delete the Customer.", Tags = new string[] { "Customer" })]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var response = await customerService.Delete(id);
            return new JsonResult(response);
        }
    }
}
