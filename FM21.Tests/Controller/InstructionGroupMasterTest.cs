using FluentValidation.TestHelper;
using FM21.API.Controllers;
using FM21.Core;
using FM21.Core.Localization;
using FM21.Core.Model;
using FM21.Core.Validator;
using FM21.Entities;
using FM21.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace FM21.Tests
{
    [TestFixture]
    public class InstructionGroupMasterTest
    {
        private IStringLocalizer localizer;
        private InstructionGroupMasterController instructionGroupMasterController;
        private Mock<IInstructionGroupMasterService> instructionGroupMasterService;
        private InstructionGroupMasterValidator validator;

        [SetUp]
        public void SetUp()
        {
            localizer = new JsonStringLocalizer();
            instructionGroupMasterService = new Mock<IInstructionGroupMasterService>();
            validator = new InstructionGroupMasterValidator(localizer);
        }

        [Test]
        public async Task Should_Return_All_Active_Records()
        {
            var returnObject = GetRecordListMock();
            instructionGroupMasterService = new Mock<IInstructionGroupMasterService>();
            instructionGroupMasterService.Setup(t => t.GetAll()).ReturnsAsync(returnObject);

            instructionGroupMasterController = new InstructionGroupMasterController(instructionGroupMasterService.Object);
            var response = await instructionGroupMasterController.GetAll() as JsonResult;

            Assert.IsNotNull(response);
            Assert.AreEqual(returnObject.Data, ((GeneralResponse<ICollection<InstructionGroupMaster>>)response.Value).Data);
            Assert.AreEqual(returnObject.Data.Count, ((GeneralResponse<ICollection<InstructionGroupMaster>>)response.Value).Data.Count);
            Assert.AreEqual(returnObject.Result, ((GeneralResponse<ICollection<InstructionGroupMaster>>)response.Value).Result);
        }

        [Test]
        public async Task Should_Return_GroupBySiteAndCategory_When_Get_Record_By_Valid_Parameter()
        {
            var returnObject = GetModelRecordListMock();
            instructionGroupMasterService = new Mock<IInstructionGroupMasterService>();
            instructionGroupMasterService.Setup(t => t.GetGroupBySiteProductAndCategory(1, 1)).ReturnsAsync(returnObject);

            instructionGroupMasterController = new InstructionGroupMasterController(instructionGroupMasterService.Object);
            var response = await instructionGroupMasterController.GetGroupBySiteProductAndCategory(1, 1) as JsonResult;

            Assert.IsNotNull(response);
            Assert.AreEqual(returnObject.Data, ((GeneralResponse<ICollection<InstructionGroupMasterModel>>)response.Value).Data);
            Assert.AreEqual(returnObject.Data.Count, ((GeneralResponse<ICollection<InstructionGroupMasterModel>>)response.Value).Data.Count);
            Assert.AreEqual(returnObject.Result, ((GeneralResponse<ICollection<InstructionGroupMasterModel>>)response.Value).Result);
        }

        [Test]
        public async Task Should_Not_Return_Data_When_Get_GroupBySiteAndCategory_With_InValid_Parameter()
        {
            var returnObject = new GeneralResponse<ICollection<InstructionGroupMasterModel>>();
            returnObject.Data = new Collection<InstructionGroupMasterModel>();
            instructionGroupMasterService = new Mock<IInstructionGroupMasterService>();
            instructionGroupMasterService.Setup(t => t.GetGroupBySiteProductAndCategory(1, 99)).ReturnsAsync(returnObject);

            instructionGroupMasterController = new InstructionGroupMasterController(instructionGroupMasterService.Object);
            var response = await instructionGroupMasterController.GetGroupBySiteProductAndCategory(1, 99) as JsonResult;

            Assert.IsNotNull(response);
            Assert.AreEqual(returnObject.Data, ((GeneralResponse<ICollection<InstructionGroupMasterModel>>)response.Value).Data);
            Assert.AreEqual(returnObject.Data.Count, ((GeneralResponse<ICollection<InstructionGroupMasterModel>>)response.Value).Data.Count);
            Assert.AreEqual(returnObject.Result, ((GeneralResponse<ICollection<InstructionGroupMasterModel>>)response.Value).Result);
        }

        [Test]
        public async Task Should_Create_New_Data()
        {
            GeneralResponse<bool> returnObject = new GeneralResponse<bool>() { Data = true };
            var data = GetMockObjectModel();
            instructionGroupMasterService = new Mock<IInstructionGroupMasterService>();
            instructionGroupMasterService.Setup(t => t.Create(data)).ReturnsAsync(returnObject);

            instructionGroupMasterController = new InstructionGroupMasterController(instructionGroupMasterService.Object);
            var response = await instructionGroupMasterController.PostInstructionGroup(data);

            Assert.IsNotNull(response);
            Assert.AreEqual(((GeneralResponse<bool>)((JsonResult)response).Value).Result, ResultType.Success);
        }

        [Test]
        public async Task Should_Update_When_Data_Is_Valid()
        {
            int recordID = 1;
            GeneralResponse<bool> returnObject = new GeneralResponse<bool>() { Data = true };
            var data = GetMockObjectModel(1);
            instructionGroupMasterService = new Mock<IInstructionGroupMasterService>();
            instructionGroupMasterService.Setup(t => t.Update(data)).ReturnsAsync(returnObject);

            instructionGroupMasterController = new InstructionGroupMasterController(instructionGroupMasterService.Object);
            var response = await instructionGroupMasterController.PutInstructionGroup(recordID, data) as JsonResult;

            Assert.IsNotNull(response);
            Assert.AreEqual(((GeneralResponse<bool>)response.Value).Result, ResultType.Success);
        }

        [Test]
        [SetUICulture("en-us")]
        public void Should_Not_Update_When_Data_IsInvalid()
        {
            InstructionGroupMasterModel obj = GetMockObjectModel(0);
            var result = validator.TestValidate(obj, "Edit,New");
            result.ShouldHaveValidationErrorFor(x => x.InstructionGroupID);
        }

        [Test]
        public async Task Should_Delete_Data()
        {
            int recordID = 1;
            var returnObject = new GeneralResponse<bool>();

            instructionGroupMasterService = new Mock<IInstructionGroupMasterService>();
            instructionGroupMasterService.Setup(t => t.Delete(recordID)).ReturnsAsync(returnObject);

            instructionGroupMasterController = new InstructionGroupMasterController(instructionGroupMasterService.Object);
            var response = await instructionGroupMasterController.DeleteInstructionGroup(recordID) as JsonResult;

            Assert.That(response.Value, Is.EqualTo(returnObject));
            Assert.IsNotNull(returnObject);
        }

        private InstructionGroupMaster GetMockObject(int? id = null)
        {
            var obj = new InstructionGroupMaster()
            {
                InstructionGroupName = "Powder",
                IsActive = true,
                CreatedBy = 8888,
                CreatedOn = DateTime.Now,
                InstructionGroupID = 1,
                InstructionMaster = new List<InstructionMaster>()
            };
            if (id != null)
            {
                obj.InstructionGroupID = Convert.ToInt32(id);
            }
            return obj;
        }

        private GeneralResponse<ICollection<InstructionGroupMasterModel>> GetModelRecordListMock()
        {
            var response = new GeneralResponse<ICollection<InstructionGroupMasterModel>>();
            response.Data = new List<InstructionGroupMasterModel>();
            response.Data.Add(GetMockObjectModel(1));
            response.Data.Add(GetMockObjectModel(2));
            return response;
        }

        private GeneralResponse<ICollection<InstructionGroupMaster>> GetRecordListMock()
        {
            var response = new GeneralResponse<ICollection<InstructionGroupMaster>>();
            response.Data = new List<InstructionGroupMaster>();
            response.Data.Add(GetMockObject(1));
            response.Data.Add(GetMockObject(2));
            return response;
        }

        private InstructionGroupMasterModel GetMockObjectModel(int? id = null)
        {
            var obj = new InstructionGroupMasterModel()
            {
                InstructionGroupName = "Powder"
            };
            if (id != null)
            {
                obj.InstructionGroupID = Convert.ToInt32(id);
            }
            return obj;
        }
    }
}