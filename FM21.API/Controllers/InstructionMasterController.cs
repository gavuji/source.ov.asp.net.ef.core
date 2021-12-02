using FM21.Core;
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
    [SwaggerTag("All Instruction master related API methods")]
    //[ApiExplorerSettings(GroupName = "v1.0")]
    [ApiController]
    public class InstructionMasterController : ControllerBase
    {
        private readonly IInstructionMasterService instructionService;

        public InstructionMasterController(IInstructionMasterService instructionService)
        {
            this.instructionService = instructionService;
        }

        [SwaggerOperation(Summary = "Get list of all Instructions.", Tags = new string[] { "InstructionMaster" })]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var response = await instructionService.GetAll();
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Get Instruction list for search (page wise, sort, search).", Tags = new string[] { "InstructionMaster" })]
        [HttpGet("GetSearchList")]
        public async Task<IActionResult> GetSearchList([FromQuery] SearchFilter searchFilter)
        {
            if (ModelState.IsValid)
            {
                var response = await instructionService.GetPageWiseData(searchFilter);
                return new JsonResult(response);
            }
            else
            {
                return BadRequest();
            }
        }

        [SwaggerOperation(Summary = "Get Instruction list for search (page wise, sort, search, & filter).", Tags = new string[] { "InstructionMaster" })]
        [HttpGet("GetSearchListWithFilter")]
        public async Task<IActionResult> GetSearchListWithFilter([FromQuery] SearchFilter searchFilter, int siteProductMapID, int instructionCategoryID)
        {
            if (ModelState.IsValid)
            {
                var response = await instructionService.GetSearchListWithFilter(searchFilter, siteProductMapID, instructionCategoryID);
                return new JsonResult(response);
            }
            else
            {
                return BadRequest();
            }
        }

        [SwaggerOperation(Summary = "Get Instruction by InstructionMasterID.", Tags = new string[] { "InstructionMaster" })]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetInstruction(int id)
        {
            var response = await instructionService.Get(id);
            if (response == null)
            {
                return NotFound();
            }
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Get list of Instruction group by site, product, category and group.", Tags = new string[] { "InstructionMaster" })]
        [HttpGet("GetBySiteProductCategoryAndGroup")]
        public async Task<IActionResult> GetBySiteProductCategoryAndGroup(int siteProductMapID, int categoryID, int groupID)
        {
            var response = await instructionService.GetBySiteProductCategoryAndGroup(siteProductMapID, categoryID, groupID);
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Add new Instruction.", Tags = new string[] { "InstructionMaster" })]
        [HttpPost]
        public async Task<IActionResult> PostInstruction(InstructionMasterModel model)
        {
            var response = await instructionService.Create(model);
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Update existing Instruction.", Tags = new string[] { "InstructionMaster" })]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInstruction(int id, InstructionMasterModel model)
        {
            model.InstructionMasterID = id;
            var response = await instructionService.Update(model);
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Delete the Instruction.", Tags = new string[] { "InstructionMaster" })]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInstruction(int id)
        {
            var response = await instructionService.Delete(id);
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Update Instruction Group order.", Tags = new string[] { "InstructionMaster" })]
        [HttpPut("UpdateInstructionGroupOrder")]
        public async Task<IActionResult> UpdateInstructionGroupOrder(int siteProductMapID, int categoryID, List<InstructionGroupMasterModel> lstInstructionGroup)
        {
            var response = await instructionService.UpdateInstructionGroupOrder(siteProductMapID, categoryID, lstInstructionGroup);
            return new JsonResult(response);
        }

        [SwaggerOperation(Summary = "Update Instruction order.", Tags = new string[] { "InstructionMaster" })]
        [HttpPut("UpdateInstructionOrder")]
        public async Task<IActionResult> UpdateInstructionOrder(List<InstructionMasterModel> lstInstruction)
        {
            var response = await instructionService.UpdateInstructionOrder(lstInstruction);
            return new JsonResult(response);
        }
    }
}