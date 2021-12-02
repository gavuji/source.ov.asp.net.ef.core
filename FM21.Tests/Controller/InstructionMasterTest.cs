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
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FM21.Tests
{
    [TestFixture]
    public class InstructionMasterTest
    {
        private IStringLocalizer localizer;
        private InstructionMasterController instructionMasterController;
        private Mock<IInstructionMasterService> instructionMasterService;
        private InstructionMasterValidator validator;

        [SetUp]
        public void SetUp()
        {
            localizer = new JsonStringLocalizer();
            instructionMasterService = new Mock<IInstructionMasterService>();
            validator = new InstructionMasterValidator(localizer);
        }

        [Test]
        public async Task Should_Return_All_Active_Records()
        {
            var returnObject = GetGeneralInstructionMockDataModel();
            instructionMasterService = new Mock<IInstructionMasterService>();
            instructionMasterService.Setup(t => t.GetAll()).ReturnsAsync(returnObject);

            instructionMasterController = new InstructionMasterController(instructionMasterService.Object);
            var response = await instructionMasterController.GetAll() as JsonResult;

            Assert.IsNotNull(response);
            Assert.AreEqual(returnObject.Data, ((GeneralResponse<ICollection<InstructionMasterModel>>)response.Value).Data);
            Assert.AreEqual(returnObject.Data.Count, ((GeneralResponse<ICollection<InstructionMasterModel>>)response.Value).Data.Count);
            Assert.AreEqual(returnObject.Result, ((GeneralResponse<ICollection<InstructionMasterModel>>)response.Value).Result);
        }

        [Test]
        public async Task Should_Return_All_From_GetSearchList_With_Ok_Status()
        {
            SearchFilter searchFilter = new SearchFilter() { PageIndex = 1, PageSize = 10 };
            var returnObject = GetInstructionMockDataModel();
            instructionMasterService = new Mock<IInstructionMasterService>();
            instructionMasterService.Setup(t => t.GetPageWiseData(searchFilter)).ReturnsAsync(returnObject);

            instructionMasterController = new InstructionMasterController(instructionMasterService.Object);
            var response = await instructionMasterController.GetSearchList(searchFilter) as JsonResult;

            Assert.IsNotNull(response);
            Assert.AreEqual(returnObject.Data, ((PagedEntityResponse<InstructionMasterModel>)response.Value).Data);
            Assert.AreEqual(returnObject.Data.Count, ((PagedEntityResponse<InstructionMasterModel>)response.Value).Data.Count);
            Assert.AreEqual(returnObject.Result, ((PagedEntityResponse<InstructionMasterModel>)response.Value).Result);
        }

        [Test]
        public async Task Should_Return_Null_From_GetSearchList_When_Model_IsInvalid()
        {
            SearchFilter searchFilter = new SearchFilter();// This will cause modelInvalid
            instructionMasterController = new InstructionMasterController(instructionMasterService.Object);

            var context = new ValidationContext(searchFilter, null, null);
            var results = new List<ValidationResult>();
            if (!Validator.TryValidateObject(searchFilter, context, results, true))
            {
                instructionMasterController.ModelState.Clear();
                foreach (ValidationResult result in results)
                {
                    var key = result.MemberNames.FirstOrDefault() ?? "";
                    instructionMasterController.ModelState.AddModelError(key, result.ErrorMessage);
                }
            }

            var response = await instructionMasterController.GetSearchList(searchFilter) as JsonResult;
            Assert.IsNull(response);
        }

        [Test]
        public async Task Should_Return_All_From_GetSearchListWithFilter_With_Ok_Status()
        {
            SearchFilter searchFilter = new SearchFilter() { PageIndex = 1, PageSize = 10000 };
            int SiteProductMapID = 1, categoryID = 1;
            var returnObject = GetInstructionMockDataModel();
            instructionMasterService = new Mock<IInstructionMasterService>();
            instructionMasterService.Setup(t => t.GetSearchListWithFilter(searchFilter, SiteProductMapID, categoryID)).ReturnsAsync(returnObject);

            instructionMasterController = new InstructionMasterController(instructionMasterService.Object);
            var response = await instructionMasterController.GetSearchListWithFilter(searchFilter, SiteProductMapID, categoryID) as JsonResult;

            Assert.IsNotNull(response);
            Assert.AreEqual(returnObject.Data, ((PagedEntityResponse<InstructionMasterModel>)response.Value).Data);
            Assert.AreEqual(returnObject.Data.Count, ((PagedEntityResponse<InstructionMasterModel>)response.Value).Data.Count);
            Assert.AreEqual(returnObject.Result, ((PagedEntityResponse<InstructionMasterModel>)response.Value).Result);
        }

        [Test]
        public async Task Should_Return_Null_From_GetSearchListWithFilter_When_Model_IsInvalid()
        {
            int SiteProductMapID = 1, categoryID = 1;
            SearchFilter searchFilter = new SearchFilter();// This will cause modelInvalid
            instructionMasterController = new InstructionMasterController(instructionMasterService.Object);

            var context = new ValidationContext(searchFilter, null, null);
            var results = new List<ValidationResult>();
            if (!Validator.TryValidateObject(searchFilter, context, results, true))
            {
                instructionMasterController.ModelState.Clear();
                foreach (ValidationResult result in results)
                {
                    var key = result.MemberNames.FirstOrDefault() ?? "";
                    instructionMasterController.ModelState.AddModelError(key, result.ErrorMessage);
                }
            }

            var response = await instructionMasterController.GetSearchListWithFilter(searchFilter, SiteProductMapID, categoryID) as JsonResult;
            Assert.IsNull(response);
        }

        [Test]
        public async Task Should_Return_Data_When_Get_Record_By_Valid_PrimaryKey()
        {
            GeneralResponse<InstructionMaster> returnObject = new GeneralResponse<InstructionMaster>()
            {
                Data = GetMockObject(2)
            };
            instructionMasterService = new Mock<IInstructionMasterService>();
            instructionMasterService.Setup(t => t.Get(returnObject.Data.InstructionMasterID)).ReturnsAsync(returnObject);

            instructionMasterController = new InstructionMasterController(instructionMasterService.Object);
            var response = await instructionMasterController.GetInstruction(returnObject.Data.InstructionMasterID) as JsonResult;
            var responseResult = response.Value;

            instructionMasterService.Verify(c => c.Get(It.IsAny<int>()), Times.Once);
            Assert.AreEqual(returnObject.Data.SiteProductMapID, ((GeneralResponse<InstructionMaster>)responseResult).Data.SiteProductMapID);
            Assert.AreEqual(returnObject.Data.InstructionCategoryID, ((GeneralResponse<InstructionMaster>)responseResult).Data.InstructionCategoryID);
            Assert.AreEqual(returnObject.Data.InstructionGroupID, ((GeneralResponse<InstructionMaster>)responseResult).Data.InstructionGroupID);
            Assert.AreEqual(returnObject.Data.DescriptionEn, ((GeneralResponse<InstructionMaster>)responseResult).Data.DescriptionEn);
            Assert.AreEqual(returnObject.Data.DescriptionFr, ((GeneralResponse<InstructionMaster>)responseResult).Data.DescriptionFr);
        }

        [Test]
        public async Task Should_Return_Null_When_Get_Record_With_InValid_PrimaryKey()
        {
            GeneralResponse<InstructionMaster> returnObject = new GeneralResponse<InstructionMaster>()
            {
                Data = GetMockObject(1)
            };
            instructionMasterService = new Mock<IInstructionMasterService>();
            instructionMasterService.Setup(t => t.Get(returnObject.Data.InstructionMasterID)).ReturnsAsync(returnObject);

            instructionMasterController = new InstructionMasterController(instructionMasterService.Object);
            var response = await instructionMasterController.GetInstruction(99) as NotFoundResult;

            instructionMasterService.Verify(c => c.Get(It.IsAny<int>()), Times.Once);
            Assert.AreEqual(HttpStatusCode.NotFound, (HttpStatusCode)response.StatusCode);
        }

        [Test]
        public async Task Should_Return_InstructionBySiteCategoryAndGroup_When_Get_Record_By_Valid_Parameter()
        {
            var returnObject = GetGeneralInstructionMockDataModel();
            instructionMasterService = new Mock<IInstructionMasterService>();
            instructionMasterService.Setup(t => t.GetBySiteProductCategoryAndGroup(1, 1, 1)).ReturnsAsync(returnObject);

            instructionMasterController = new InstructionMasterController(instructionMasterService.Object);
            var response = await instructionMasterController.GetBySiteProductCategoryAndGroup(1, 1, 1) as JsonResult;
            
            Assert.IsNotNull(response);
            Assert.AreEqual(returnObject.Data, ((GeneralResponse<ICollection<InstructionMasterModel>>)response.Value).Data);
            Assert.AreEqual(returnObject.Data.Count, ((GeneralResponse<ICollection<InstructionMasterModel>>)response.Value).Data.Count);
            Assert.AreEqual(returnObject.Result, ((GeneralResponse<ICollection<InstructionMasterModel>>)response.Value).Result);
        }

        [Test]
        public async Task Should_Not_Return_Data_When_Get_InstructionBySiteCategoryAndGroup_With_InValid_Parameter()
        {
            var returnObject = new GeneralResponse<ICollection<InstructionMasterModel>>();
            returnObject.Data = new Collection<InstructionMasterModel>();
            instructionMasterService = new Mock<IInstructionMasterService>();
            instructionMasterService.Setup(t => t.GetBySiteProductCategoryAndGroup(1, 1, 99)).ReturnsAsync(returnObject);

            instructionMasterController = new InstructionMasterController(instructionMasterService.Object);
            var response = await instructionMasterController.GetBySiteProductCategoryAndGroup(1, 1, 99) as JsonResult;

            Assert.IsNotNull(response);
            Assert.AreEqual(returnObject.Data, ((GeneralResponse<ICollection<InstructionMasterModel>>)response.Value).Data);
            Assert.AreEqual(returnObject.Data.Count, ((GeneralResponse<ICollection<InstructionMasterModel>>)response.Value).Data.Count);
            Assert.AreEqual(returnObject.Result, ((GeneralResponse<ICollection<InstructionMasterModel>>)response.Value).Result);
        }

        [Test]
        public async Task Should_Create_New_Data()
        {
            GeneralResponse<bool> returnObject = new GeneralResponse<bool>() { Data = true };
            var data = GetMockObjectModel();
            instructionMasterService = new Mock<IInstructionMasterService>();
            instructionMasterService.Setup(t => t.Create(data)).ReturnsAsync(returnObject);

            instructionMasterController = new InstructionMasterController(instructionMasterService.Object);
            var response = await instructionMasterController.PostInstruction(data);

            Assert.IsNotNull(response);
            Assert.AreEqual(((GeneralResponse<bool>)((JsonResult)response).Value).Result, ResultType.Success);
        }

        [Test]
        public async Task Should_Update_When_Data_Is_Valid()
        {
            int recordID = 1;
            GeneralResponse<bool> returnObject = new GeneralResponse<bool>() { Data = true };
            var data = GetMockObjectModel(1);
            instructionMasterService = new Mock<IInstructionMasterService>();
            instructionMasterService.Setup(t => t.Update(data)).ReturnsAsync(returnObject);

            instructionMasterController = new InstructionMasterController(instructionMasterService.Object);
            var response = await instructionMasterController.PutInstruction(recordID, data) as JsonResult;

            Assert.IsNotNull(response);
            Assert.AreEqual(((GeneralResponse<bool>)response.Value).Result, ResultType.Success);
        }

        [Test]
        [SetUICulture("en-us")]
        public void Should_Not_Update_When_Data_IsInvalid()
        {
            InstructionMasterModel obj = GetMockObjectModel(0);
            var result = validator.TestValidate(obj, "Edit,New");
            result.ShouldHaveValidationErrorFor(x => x.InstructionMasterID);
        }

        [Test]
        public async Task Should_Delete_Data()
        {
            int recordID = 1;
            var returnObject = new GeneralResponse<bool>();

            instructionMasterService = new Mock<IInstructionMasterService>();
            instructionMasterService.Setup(t => t.Delete(recordID)).ReturnsAsync(returnObject);

            instructionMasterController = new InstructionMasterController(instructionMasterService.Object);
            var response = await instructionMasterController.DeleteInstruction(recordID) as JsonResult;

            Assert.That(response.Value, Is.EqualTo(returnObject));
            Assert.IsNotNull(returnObject);
        }

        [Test]
        public async Task Should_Update_InstructionGroupOrder()
        {
            GeneralResponse<bool> returnObject = new GeneralResponse<bool>() { Data = true };        
            int SiteProductMapID = 1, categoryID = 1;
            IList<InstructionGroupMasterModel> recordList = GetInstructionGroupMasterModelMockdData();
            instructionMasterService = new Mock<IInstructionMasterService>();
            instructionMasterService.Setup(t => t.UpdateInstructionGroupOrder(SiteProductMapID, categoryID, recordList)).ReturnsAsync(returnObject);

            instructionMasterController = new InstructionMasterController(instructionMasterService.Object);
            var response = await instructionMasterController.UpdateInstructionGroupOrder(SiteProductMapID, categoryID, recordList.ToList()) as JsonResult;

            Assert.That(response.Value, Is.EqualTo(returnObject));
            Assert.IsNotNull(response);
        }

        [Test]
        public async Task Should_Update_InstructionOrder()
        {
            GeneralResponse<bool> returnObject = new GeneralResponse<bool>() { Data = true };
            IList<InstructionMasterModel> recordList = GetInstructionMasterModelMockdData();
            instructionMasterService = new Mock<IInstructionMasterService>();
            instructionMasterService.Setup(t => t.UpdateInstructionOrder(recordList)).ReturnsAsync(returnObject);

            instructionMasterController = new InstructionMasterController(instructionMasterService.Object);
            var response = await instructionMasterController.UpdateInstructionOrder(recordList.ToList()) as JsonResult;

            Assert.That(response.Value, Is.EqualTo(returnObject));
            Assert.IsNotNull(response);
        }

        private InstructionMaster GetMockObject(int? id = null)
        {
            var obj = new InstructionMaster()
            {
                SiteProductMapID = 1,
                InstructionCategoryID = 1,
                InstructionGroupID = 1,
                DescriptionEn = "desc en",
                DescriptionFr = "desc fr",
                GroupDisplayOrder = 1,
                GroupItemDisplayOrder = 1
            };
            if(id != null)
            {
                obj.InstructionMasterID = Convert.ToInt32(id);
            }
            return obj;
        }

        private GeneralResponse<ICollection<InstructionMasterModel>> GetGeneralInstructionMockDataModel()
        {
            var response = new GeneralResponse<ICollection<InstructionMasterModel>>();
            response.Data = new List<InstructionMasterModel>();
            response.Data.Add(GetMockObjectModel(99999));
            response.Data.Add(GetMockObjectModel(44444));
            return response;
        }

        private PagedEntityResponse<InstructionMasterModel> GetInstructionMockDataModel()
        {
            var response = new PagedEntityResponse<InstructionMasterModel>();
            response.Data = new List<InstructionMasterModel>();
            response.Data.Add(GetMockObjectModel(99999));
            response.Data.Add(GetMockObjectModel(44444));
            response.CurrentPage = 1;
            response.PageCount = 1;
            response.PageSize = 10;
            response.RowCount = 2;
            response.Message = null;
            response.Exception = null;
            response.Result = ResultType.Success;
            response.ExtraData = null;
            return response;
        }

        private InstructionMasterModel GetMockObjectModel(int? id = null)
        {
            var obj = new InstructionMasterModel()
            {
                SiteProductMapID = 1,
                InstructionCategoryID = 1,
                InstructionGroupID = 1,
                DescriptionEn = "desc en",
                DescriptionFr = "desc fr",
                GroupDisplayOrder = 1,
                GroupItemDisplayOrder = 1
            };
            if (id != null)
            {
                obj.InstructionMasterID = Convert.ToInt32(id);
            }
            return obj;
        }

        private IList<InstructionGroupMasterModel> GetInstructionGroupMasterModelMockdData()
        {
            List<InstructionGroupMasterModel> lstInstructionGroup = new List<InstructionGroupMasterModel>();
            lstInstructionGroup.Add(new InstructionGroupMasterModel()
            {
                GroupDisplayOrder = 1,
                InstructionGroupID = 1
            });
            lstInstructionGroup.Add(new InstructionGroupMasterModel()
            {
                GroupDisplayOrder = 1,
                InstructionGroupID = 2
            });
            return lstInstructionGroup;
        }

        private IList<InstructionMasterModel> GetInstructionMasterModelMockdData()
        {
            List<InstructionMasterModel> lstInstructionGroup = new List<InstructionMasterModel>();
            lstInstructionGroup.Add(GetMockObjectModel(1));
            lstInstructionGroup[0].GroupItemDisplayOrder = 2;
            lstInstructionGroup.Add(GetMockObjectModel(1));
            lstInstructionGroup[0].GroupItemDisplayOrder = 1;
            lstInstructionGroup.Add(GetMockObjectModel(1));
            lstInstructionGroup[0].GroupItemDisplayOrder = 3;
            return lstInstructionGroup;
        }
    }
}