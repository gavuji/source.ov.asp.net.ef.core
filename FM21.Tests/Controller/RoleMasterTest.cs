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
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FM21.Tests
{
    [TestFixture]
    public class RoleMasterTest
    {
        private IStringLocalizer localizer;
        private RoleMasterController roleMasterController;
        private Mock<IRoleMasterService> roleMasterService;
        private RoleMasterValidator validator;

        [SetUp]
        public void SetUp()
        {
            localizer = new JsonStringLocalizer();
            roleMasterService = new Mock<IRoleMasterService>();
            validator = new RoleMasterValidator(localizer);
        }

        [Test]
        public async Task Should_Return_All_Active_Records()
        {
            var returnObject = GetGeneralRoleMockData();
            roleMasterService = new Mock<IRoleMasterService>();
            roleMasterService.Setup(t => t.GetAll()).ReturnsAsync(returnObject);

            roleMasterController = new RoleMasterController(roleMasterService.Object);
            var response = await roleMasterController.GetAll() as JsonResult;
            Assert.IsNotNull(response);
            Assert.AreEqual(returnObject.Data, ((GeneralResponse<ICollection<RoleMaster>>)response.Value).Data);
            Assert.AreEqual(returnObject.Data.Count, ((GeneralResponse<ICollection<RoleMaster>>)response.Value).Data.Count);
            Assert.AreEqual(returnObject.Result, ((GeneralResponse<ICollection<RoleMaster>>)response.Value).Result);
        }

        [Test]
        public async Task Should_Return_All_From_GetSearchList_With_Ok_Status()
        {
            SearchFilter searchFilter = new SearchFilter() { PageIndex = 1, PageSize = 10 };
            var returnObject = GetRoleMockData();
            roleMasterService = new Mock<IRoleMasterService>();
            roleMasterService.Setup(t => t.GetPageWiseData(searchFilter)).ReturnsAsync(returnObject);

            roleMasterController = new RoleMasterController(roleMasterService.Object);
            var response = await roleMasterController.GetSearchList(searchFilter) as JsonResult;
            Assert.IsNotNull(response);
            Assert.AreEqual(returnObject.Data, ((PagedEntityResponse<RoleMaster>)response.Value).Data);
            Assert.AreEqual(returnObject.Data.Count, ((PagedEntityResponse<RoleMaster>)response.Value).Data.Count);
            Assert.AreEqual(returnObject.Result, ((PagedEntityResponse<RoleMaster>)response.Value).Result);
        }

        [Test]
        public async Task Should_Return_Null_From_GetSearchList_When_Model_IsInvalid()
        {
            SearchFilter searchFilter = new SearchFilter();// This will cause modelInvalid
            roleMasterController = new RoleMasterController(roleMasterService.Object);

            var context = new ValidationContext(searchFilter, null, null);
            var results = new List<ValidationResult>();
            if (!Validator.TryValidateObject(searchFilter, context, results, true))
            {
                roleMasterController.ModelState.Clear();
                foreach (ValidationResult result in results)
                {
                    var key = result.MemberNames.FirstOrDefault() ?? "";
                    roleMasterController.ModelState.AddModelError(key, result.ErrorMessage);
                }
            }

            var response = await roleMasterController.GetSearchList(searchFilter) as JsonResult;
            Assert.IsNull(response);
        }

        [Test]
        public async Task Should_Return_Data_When_Get_Record_By_Valid_PrimaryKey()
        {
            GeneralResponse<RoleMaster> obj = new GeneralResponse<RoleMaster>()
            {
                Data = GetRoleMasterMockObject("Role Test", 2)
            };

            roleMasterService = new Mock<IRoleMasterService>();
            roleMasterService.Setup(t => t.Get(obj.Data.RoleID)).ReturnsAsync(obj);

            roleMasterController = new RoleMasterController(roleMasterService.Object);
            var response = await roleMasterController.GetRole(obj.Data.RoleID) as JsonResult;
            var responseResult = response.Value as GeneralResponse<RoleMaster>;

            roleMasterService.Verify(c => c.Get(It.IsAny<int>()), Times.Once);
            Assert.AreEqual(obj.Data.RoleName, responseResult.Data.RoleName);
            Assert.AreEqual(obj.Data.RoleDescription, responseResult.Data.RoleDescription);
        }

        [Test]
        public async Task Should_Return_Null_When_Get_Record_With_InValid_PrimaryKey()
        {
            GeneralResponse<RoleMaster> returnObject = new GeneralResponse<RoleMaster>()
            {
                Data = GetRoleMasterMockObject("Role Test", 1)
            };

            roleMasterService = new Mock<IRoleMasterService>();
            roleMasterService.Setup(t => t.Get(returnObject.Data.RoleID)).ReturnsAsync(returnObject);

            roleMasterController = new RoleMasterController(roleMasterService.Object);

            var response = await roleMasterController.GetRole(9999) as NotFoundResult;

            roleMasterService.Verify(c => c.Get(It.IsAny<int>()), Times.Once);
            Assert.AreEqual(HttpStatusCode.NotFound, (HttpStatusCode)response.StatusCode);
        }

        [Test]
        public async Task Should_Create_New_Data()
        {
            GeneralResponse<bool> returnObject = new GeneralResponse<bool>() { Data = true };
            var data = new RoleMasterModel() { RoleName = "Role Test" };
            roleMasterService = new Mock<IRoleMasterService>();
            roleMasterService.Setup(t => t.Create(data)).ReturnsAsync(returnObject);

            roleMasterController = new RoleMasterController(roleMasterService.Object);
            var response = await roleMasterController.PostRole(data);

            Assert.IsNotNull(response);
            Assert.AreEqual(((GeneralResponse<bool>)((JsonResult)response).Value).Result, ResultType.Success);
        }

        [Test]
        public async Task Should_Update_When_Data_Is_Valid()
        {
            GeneralResponse<bool> returnObject = new GeneralResponse<bool>() { Data = true };
            int recordID = 1;
            var data = new RoleMasterModel()
            {
                RoleID = recordID,
                RoleName = "test role",
                RoleDescription = "desc"
            };
            roleMasterService = new Mock<IRoleMasterService>();
            roleMasterService.Setup(t => t.Update(data)).ReturnsAsync(returnObject);

            roleMasterController = new RoleMasterController(roleMasterService.Object);

            var response = await roleMasterController.PutRole(recordID, data);

            Assert.IsNotNull(response);
            Assert.AreEqual(((GeneralResponse<bool>)((JsonResult)response).Value).Result, ResultType.Success);
        }

        [Test]
        [SetUICulture("en-us")]
        public void Should_Not_Update_When_Data_IsInvalid()
        {
            RoleMasterModel objRoleMaster = new RoleMasterModel() { RoleID = 0, RoleName = "" };
            var result = validator.TestValidate(objRoleMaster, "Edit,New");
            result.ShouldHaveValidationErrorFor(x => x.RoleID);
            result.ShouldHaveValidationErrorFor(x => x.RoleName);
        }

        [Test]
        public async Task Should_Delete_Data()
        {
            int recordID = 1;
            var returnObject = new GeneralResponse<bool>();

            roleMasterService = new Mock<IRoleMasterService>();
            roleMasterService.Setup(t => t.Delete(recordID)).ReturnsAsync(returnObject);

            roleMasterController = new RoleMasterController(roleMasterService.Object);

            var response = await roleMasterController.DeleteRole(recordID) as JsonResult;

            Assert.That(response.Value, Is.EqualTo(returnObject));
            Assert.IsNotNull(response);
        }

        private GeneralResponse<ICollection<RoleMaster>> GetGeneralRoleMockData()
        {
            var response = new GeneralResponse<ICollection<RoleMaster>>();
            List<RoleMaster> rolePermissionMatrix = new List<RoleMaster>();
            rolePermissionMatrix.Add(GetRoleMasterMockObject("Role Test", 99999));
            rolePermissionMatrix.Add(GetRoleMasterMockObject("Role Test2", 44444));
            response.Data = rolePermissionMatrix;
            return response;
        }

        private PagedEntityResponse<RoleMaster> GetRoleMockData()
        {
            var response = new PagedEntityResponse<RoleMaster>();
            List<RoleMaster> roleMasterObj = new List<RoleMaster>();
            roleMasterObj.Add(GetRoleMasterMockObject("Role Test1", 99999));
            roleMasterObj.Add(GetRoleMasterMockObject("Role Test2", 44444));
            response.Data = roleMasterObj;
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

        private RoleMaster GetRoleMasterMockObject(string roleName, int? id = null)
        {
            RoleMaster obj = new RoleMaster()
            {
                RoleName = roleName,
                RoleDescription = "desc"
            };
            if(id != null)
            {
                obj.RoleID = Convert.ToInt32(id);
            }
            return obj;
        }
    }
}