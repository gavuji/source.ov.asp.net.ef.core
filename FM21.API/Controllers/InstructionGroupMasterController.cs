using FM21.Core.Model;
using FM21.Service;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading.Tasks;

namespace FM21.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [SwaggerTag("All Instruction Group master related API methods")]
    //[ApiExplorerSettings(GroupName = "v1.0")]
    [ApiController]
    public class InstructionGroupMasterController : ControllerBase
    {
        private readonly IInstructionGroupMasterService instructionGroupService;

        public InstructionGroupMasterController(IInstructionGroupMasterService instructionGroupService)
        {
            this.instructionGroupService = instructionGroupService;
        }

        [SwaggerOperation(Summary = "Get list of all the Instruction group.", Tags = new string[] { "InstructionGroupMaster" })]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var response = await instructionGroupService.GetAll();
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Get list of Instruction group by site, product and category.", Tags = new string[] { "InstructionGroupMaster" })]
        [HttpGet("GetGroupBySiteProductAndCategory")]
        public async Task<IActionResult> GetGroupBySiteProductAndCategory(int siteProductMapID, int categoryID)
        {
            var response = await instructionGroupService.GetGroupBySiteProductAndCategory(siteProductMapID, categoryID);
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Add new Instruction Group.", Tags = new string[] { "InstructionGroupMaster" })]
        [HttpPost]
        public async Task<IActionResult> PostInstructionGroup(InstructionGroupMasterModel model)
        {
            var response = await instructionGroupService.Create(model);
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Update existing Instruction Group.", Tags = new string[] { "InstructionGroupMaster" })]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInstructionGroup(int id, InstructionGroupMasterModel model)
        {
            model.InstructionGroupID = id;
            var response = await instructionGroupService.Update(model);
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Delete the Instruction group.", Tags = new string[] { "InstructionGroupMaster" })]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInstructionGroup(int id)
        {
            var response = await instructionGroupService.Delete(id);
            return new JsonResult(response);
        }
    }
}