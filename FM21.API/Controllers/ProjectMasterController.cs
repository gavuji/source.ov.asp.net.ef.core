using FM21.Core.Model;
using FM21.Service;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;


namespace FM21.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [SwaggerTag("All Project Information related API methods")]

    [ApiController]
    public class ProjectMasterController : ControllerBase
    {
        private readonly IProjectMasterService projectMasterService;

        public ProjectMasterController(IProjectMasterService projectMasterService)
        {
            this.projectMasterService = projectMasterService;
        }

        [SwaggerOperation(Summary = "Get list of all Project Information.", Tags = new string[] { "ProjectMaster" })]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var response = await projectMasterService.GetAll();
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Get Project Information list for search (page wise, sort, filter).", Tags = new string[] { "ProjectMaster" })]
        [HttpGet("GetSearchList")]
        public async Task<IActionResult> GetSearchList([FromQuery] SearchFilter searchFilter)
        {
            var response = await projectMasterService.GetPageWiseData(searchFilter);
            return new JsonResult(response);
        }
        [SwaggerOperation(Summary = "Get Next Project Code.", Tags = new string[] { "ProjectMaster" })]
        [HttpGet("GetNextProjectCode")]
        public async Task<IActionResult> GetNextProjectCode()
        {
            var response = await projectMasterService.GetNextProjectCode();
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Get project information by ProjectId.", Tags = new string[] { "ProjectMaster" })]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProjectInformation(int id)
        {
            var obj = await projectMasterService.Get(id);
            if (obj == null)
            {
                return NotFound();
            }
            return new JsonResult(obj);
        }
        [SwaggerOperation(Summary = " Create new project.", Tags = new string[] { "ProjectMaster" })]
        [HttpPost]
        public async Task<ActionResult<bool>> Postproject(ProjectMasterModel model)
        {
            var response = await projectMasterService.Create(model);
            return new JsonResult(response);
        }


        [SwaggerOperation(Summary = "Update existing project information.", Tags = new string[] { "ProjectMaster" })]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProjectMaster(int id, ProjectMasterModel model)
        {
            model.ProjectId = id;
            var response = await projectMasterService.Update(model);
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Delete the project information.", Tags = new string[] { "ProjectMaster" })]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProjectMaster(int id)
        {
            var response = await projectMasterService.Delete(id);
            return new JsonResult(response);
        }


    }
}
