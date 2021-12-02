using FM21.Core;
using FM21.Core.Model;
using FM21.Service;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace FM21.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [SwaggerTag("All User and permission related API methods")]
    //[ApiExplorerSettings(GroupName = "v1.0")]
    [ApiController]
    public class UserMasterController : ControllerBase
    {
        private readonly IUserMasterService userMasterService;

        public UserMasterController(IUserMasterService userMasterService)
        {
            this.userMasterService = userMasterService;
        }

        [SwaggerOperation(Summary = "Get current window login user information", Tags = new string[] { "UserMaster" })]
        [HttpGet("GetCurrentUser")]
        public async Task<IActionResult> GetCurrentUser()
        {
            Log.Information("[Information] Message :" + User.Identity.Name);
            var response = await userMasterService.GetCurrentUser(User.Identity.Name);
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Get list of AD user along with assgined roles.", Tags = new string[] { "UserMaster" })]
        [HttpGet("GetAllADUsersAlongWithRoles")]
        public async Task<IActionResult> GetAllADUsersAlongWithRoles([FromQuery] SearchFilter searchFilter)
        {
            var response = await userMasterService.GetAllADUsersAlongWithRoles(searchFilter);
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Update existing user.", Tags = new string[] { "UserMaster" })]
        [HttpPut("UpdateUserRolePermission")]
        public async Task<IActionResult> PutUpdateUserRolePermission(UserMasterModel model)
        {
            var response = await userMasterService.UpdateUserRolePermission(model);
            return new JsonResult(response);
        }
    }
}