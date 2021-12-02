using FM21.Core.Model;
using FM21.Service;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace FM21.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [SwaggerTag("All Permission master related API methods")]
    //[ApiExplorerSettings(GroupName = "v1.0")]
    [ApiController]
   
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionMasterService permissionMasterService;

        public PermissionController(IPermissionMasterService permissionMasterService)
        {
            this.permissionMasterService = permissionMasterService;
        }
        
        [SwaggerOperation(Summary = "Get Role Permission matrix.", Tags = new string[] { "Permission" })]
        [HttpGet("GetRolePermissionMatrix")]
        public async Task<IActionResult> GetRolePermissionMatrix()
        {
            var response = await permissionMasterService.GetRolePermissionMatrix();
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Add new Permission.", Tags = new string[] { "Permission" })]
        [HttpPost("PostRolePermissionMatrix")]
        public async Task<ActionResult<List<bool>>> PostRolePermissionMatrix([FromBody] List<RolePermissionAccess> roleAccessList)
        {
             var response = await permissionMasterService.UpdateRoleMatrix(roleAccessList);
            return new JsonResult(response);
        }
    }
}
