using FluentValidation.TestHelper;
using FM21.API.Controllers;
using FM21.Core;
using FM21.Core.Localization;
using FM21.Core.Model;
using FM21.Core.Validator;
using FM21.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FM21.Tests
{
    [TestFixture]
    public class UserMasterTest
    {
        private IStringLocalizer localizer;
        private UserMasterController userMasterController;
        private Mock<IUserMasterService> userMasterService;
        private UserMasterValidator validator;

        [SetUp]
        public void SetUp()
        {
            localizer = new JsonStringLocalizer();
            userMasterService = new Mock<IUserMasterService>();
            validator = new UserMasterValidator(localizer);
        }

        [Test]
        public async Task Should_Return_Current_User_Info_With_Ok_Status()
        {
            string username = "Administrator";
            Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.SetupGet(p => p.User.Identity.Name).Returns(username);
            var objUser = new GeneralResponse<UserMasterModel>() { Data = GetUserMasterMockObject(username, 3) };
            userMasterService = new Mock<IUserMasterService>();
            userMasterService.Setup(t => t.GetCurrentUser(username)).ReturnsAsync(objUser);

            userMasterController = new UserMasterController(userMasterService.Object);
            userMasterController.ControllerContext = new ControllerContext()
            {
                HttpContext = mockHttpContext.Object
            };
            var response = await userMasterController.GetCurrentUser() as JsonResult;
            Assert.IsNotNull(response);
            Assert.AreEqual(ResultType.Success, ((GeneralResponse<UserMasterModel>)response.Value).Result);
        }

        [Test]
        public async Task Should_Return_Data_From_GetAllADUsersAlongWithRoles_With_Ok_Status()
        {
            SearchFilter searchFilter = new SearchFilter() { PageIndex = 1, PageSize = 10 };
            var returnObject = GetUserMockData();
            userMasterService = new Mock<IUserMasterService>();
            userMasterService.Setup(t => t.GetAllADUsersAlongWithRoles(searchFilter)).ReturnsAsync(returnObject);

            userMasterController = new UserMasterController(userMasterService.Object);
            var response = await userMasterController.GetAllADUsersAlongWithRoles(searchFilter) as JsonResult;
            Assert.IsNotNull(response);
            Assert.AreEqual(returnObject.Data, ((PagedEntityResponse<UserMasterModel>)response.Value).Data);
            Assert.AreEqual(returnObject.Data.Count, ((PagedEntityResponse<UserMasterModel>)response.Value).Data.Count);
            Assert.AreEqual(returnObject.Result, ((PagedEntityResponse<UserMasterModel>)response.Value).Result);
        }

        [Test]
        public async Task Should_Return_Null_From_GetAllADUsersAlongWithRoles_When_Model_IsInvalid()
        {
            SearchFilter searchFilter = new SearchFilter();// This will cause modelInvalid
            userMasterController = new UserMasterController(userMasterService.Object);
            var response = await userMasterController.GetAllADUsersAlongWithRoles(searchFilter) as JsonResult;
            Assert.IsNull(response.Value);
        }

        [Test]
        public async Task Should_Create_New_Data()
        {
            GeneralResponse<bool> returnObject = new GeneralResponse<bool>() { Data = true };
            var data = new UserMasterModel() { DomainFullName = "User 3", AssignedRoleList = new int[] { 1, 2 } };
            userMasterService = new Mock<IUserMasterService>();
            userMasterService.Setup(t => t.UpdateUserRolePermission(data)).ReturnsAsync(returnObject);

            userMasterController = new UserMasterController(userMasterService.Object);
            var response = await userMasterController.PutUpdateUserRolePermission(data);

            Assert.IsNotNull(response);
            Assert.AreEqual(((GeneralResponse<bool>)((JsonResult)response).Value).Result, ResultType.Success);
        }

        [Test]
        public async Task Should_Update_When_Data_Is_Valid()
        {
            GeneralResponse<bool> returnObject = new GeneralResponse<bool>() { Data = true };
            var data = GetUserMasterMockObject("Test User", 1);
            userMasterService = new Mock<IUserMasterService>();
            userMasterService.Setup(t => t.UpdateUserRolePermission(data)).ReturnsAsync(returnObject);

            userMasterController = new UserMasterController(userMasterService.Object);

            var response = await userMasterController.PutUpdateUserRolePermission(data);

            Assert.IsNotNull(response);
            Assert.AreEqual(((GeneralResponse<bool>)((JsonResult)response).Value).Result, ResultType.Success);
        }

        [Test]
        [SetUICulture("en-us")]
        public void Should_Not_Update_When_Data_IsInvalid()
        {
            var data = new UserMasterModel() { DomainFullName = "", DisplayName = "Test" };
            var result = validator.TestValidate(data, "Edit,New");
            result.ShouldHaveValidationErrorFor(x => x.DomainFullName);
        }

        private PagedEntityResponse<UserMasterModel> GetUserMockData()
        {
            var response = new PagedEntityResponse<UserMasterModel>();
            response.Data = new List<UserMasterModel>();
            response.Data.Add(GetUserMasterMockObject("User 1", 1));
            response.Data.Add(GetUserMasterMockObject("User 2", 2));
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

        private UserMasterModel GetUserMasterMockObject(string userName, int? id = null)
        {
            UserMasterModel obj = new UserMasterModel()
            {
                DisplayName = userName,
                DomainFullName = userName
            };
            if(id != null)
            {
                obj.UserID = Convert.ToInt32(id);
            }
            return obj;
        }
    }
}