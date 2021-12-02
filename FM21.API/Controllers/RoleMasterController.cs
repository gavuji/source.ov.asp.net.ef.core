using FM21.Core.Model;
using FM21.Service;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace FM21.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [SwaggerTag("All Role master related API methods")]
    //[ApiExplorerSettings(GroupName = "v1.0")]
    [ApiController]
    public class RoleMasterController : ControllerBase
    {
        private readonly IRoleMasterService roleMasterService;

        public RoleMasterController(IRoleMasterService roleMasterService)
        {
            this.roleMasterService = roleMasterService;
        }

        [SwaggerOperation(Summary = "Get list of all roles.", Tags = new string[] { "RoleMaster" })]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var response = await roleMasterService.GetAll();
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Get roles list for search (page wise, sort, filter).", Tags = new string[] { "RoleMaster" })]
        [HttpGet("GetSearchList")]
        public async Task<IActionResult> GetSearchList([FromQuery] SearchFilter searchFilter)
        {
            if (ModelState.IsValid)
            {
                var response = await roleMasterService.GetPageWiseData(searchFilter);
                return new JsonResult(response);
            }
            else
            {
                return BadRequest();
            }
        }

        [SwaggerOperation(Summary = "Get role by RoleID.", Tags = new string[] { "RoleMaster" })]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRole(int id)
        {
            var obj = await roleMasterService.Get(id);
            if (obj == null)
            {
                return NotFound();
            }
            return new JsonResult(obj);
        }

        [SwaggerOperation(Summary = "Add new Role.", Tags = new string[] { "RoleMaster" })]
        [HttpPost]
        public async Task<IActionResult> PostRole(RoleMasterModel model)
        {
            var response = await roleMasterService.Create(model);
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Update existing Role.", Tags = new string[] { "RoleMaster" })]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRole(int id, RoleMasterModel model)
        {
            model.RoleID = id;
            var response = await roleMasterService.Update(model);
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Delete the Role.", Tags = new string[] { "RoleMaster" })]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var response = await roleMasterService.Delete(id);
            return new JsonResult(response);
        }
    }
}