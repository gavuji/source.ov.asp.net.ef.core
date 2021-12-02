using FM21.Core;
using FM21.Core.Model;
using FM21.Data.Infrastructure;
using FM21.Entities;
using FM21.Service;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace FM21.Tests
{
    [TestFixture]
    public class UserMasterServiceTest : TestBase
    {
        private IUserMasterService userMasterService;
        private Mock<IRepository<UserMaster>> userMasterRepository;
        private Mock<IRepository<UserRole>> userRoleRepository;

        [SetUp]
        [SetUICulture("en-us")]
        public void SetUp()
        {
            userMasterRepository = SetupUserMasterRepository();
            userRoleRepository = SetupUserRoleRepository();
            ApplicationConstants.Domain = "pip.local";
            ApplicationConstants.ADUserGroup = "Domain Users";
            userMasterService = new UserMasterService(serviceProvider.Object, exceptionHandler.Object, mapper, unitOfWork.Object, null, userMasterRepository.Object, userRoleRepository.Object);
        }

        [Test]
        [Ignore("Ignore as is ad user retrieves")]
        public async Task Service_Should_Return_All_Active_Data()
        {
            var returnObject = GetGeneralUserMockPagedData();

            var response = await userMasterService.GetAllADUsersAlongWithRoles(new SearchFilter() { PageIndex = 1, PageSize = 10 });

            Assert.That(response.Result, Is.EqualTo(returnObject.Result));
            Assert.IsNotNull(response.Data);
        }

        [Test]
        [Ignore("Ignore as is ad user retrieves")]
        public async Task Service_Should_Return_All_Active_Data_By_SortColumn()
        {
            var response = await userMasterService.GetAllADUsersAlongWithRoles(new SearchFilter() { PageIndex = 1, PageSize = 10, SortColumn = "DisplayName" });

            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
        }

        [Test]
        public async Task Service_Should_CreateNew_Data()
        {
            var model = GetModelObjectMock();

            var response = await userMasterService.UpdateUserRolePermission(model);

            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
            Assert.AreEqual(response.Message, localizer["msgUpdateSuccess", new string[] { "User" }].Value);
        }

        [Test]
        public async Task Service_Should_Return_UserInformation_When_UserName_Exist()
        {
            var returnObject = GetUserMasterMockObject();
            string currentUserName = returnObject.DomainFullName = "pip.local\\Administrator";
            var response = await userMasterService.GetCurrentUser(currentUserName);

            Assert.AreEqual(ResultType.Success, response.Result);
            Assert.IsNotNull(response.Data);
        }

        [Test]
        public async Task Service_Should_Return_User_Not_Exist_Message_When_UserName_Invalid()
        {
            var response = await userMasterService.GetCurrentUser("pip.local\\test");

            Assert.AreEqual(ResultType.Success, response.Result);
            Assert.AreEqual(response.Message, localizer["msgRecordNotExist", new string[] { "User" }].Value);
        }

        [Test]
        public async Task Service_Should_Not_Return_User_When_Exception()
        {
            var response = await userMasterService.GetCurrentUser("test");

            Assert.AreEqual(ResultType.Error, response.Result);
        }

        [Test]
        public async Task Service_Should_NotCreate_When_Data_InValid()
        {
            var model = GetModelObjectMock();
            model.DomainFullName = string.Empty;

            var response = await userMasterService.UpdateUserRolePermission(model);

            Assert.That(response.Result, Is.EqualTo(ResultType.Warning));
            Assert.Greater(response.ExtraData.Count, 0);
        }

        [Test]
        public async Task Service_Should_Update_User_And_Add_New_Role_In_Edit_Mode()
        {
            var userMasterModel = GetModelObjectMock(1);
            userMasterModel.DisplayName = "User edited";
            userMasterRepository.Setup(r => r.GetAsync(x => x.DomainFullName.ToLower() == userMasterModel.DomainFullName.ToLower())).ReturnsAsync(GetUserMasterMockObject(1));
            userRoleRepository.Setup(r => r.Get(It.IsAny<System.Linq.Expressions.Expression<Func<UserRole, bool>>>())).Returns(new UserRole() { RoleID = 99, UserID = 1 });
            var response = await userMasterService.UpdateUserRolePermission(userMasterModel);

            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
        }

        [Test]
        public async Task Service_Should_Update_User_With_Role()
        {
            UserMaster user = new UserMaster() { UserID = 1 };
            var userMasterModel = GetModelObjectMock(1);
            userMasterRepository.Setup(r => r.GetAsync(x => x.DomainFullName.ToLower() == userMasterModel.DomainFullName.ToLower())).ReturnsAsync(GetUserMasterMockObject(1));
            (new int[] { 99 }).ToList().ForEach(roleID => {
                userRoleRepository.Setup(r => r.Get(x => x.UserID == user.UserID && x.RoleID == roleID)).Returns(new UserRole() { RoleID = 99, UserID = 1 });
            });
            var response = await userMasterService.UpdateUserRolePermission(userMasterModel);

            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
        }

        [Test]
        public async Task Service_Should_Not_Update_User_When_Exception()
        {
            var userMasterModel = GetModelObjectMock(1);
            userMasterModel.AssignedRoleList = null;
            userMasterRepository.Setup(r => r.GetAsync(x => x.DomainFullName.ToLower() == userMasterModel.DomainFullName.ToLower())).ReturnsAsync(GetUserMasterMockObject(1));
            userRoleRepository.Setup(r => r.Delete(It.IsAny<System.Linq.Expressions.Expression<Func<UserRole, bool>>>())).Throws(new Exception("something went wrong"));
            var response = await userMasterService.UpdateUserRolePermission(userMasterModel);

            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_Not_Update_Data_When_Values_Are_InValid()
        {
            var model = GetModelObjectMock();
            model.UserID = 1;
            model.DomainFullName = string.Empty;

            var response = await userMasterService.UpdateUserRolePermission(model);

            Assert.That(response.Result, Is.EqualTo(ResultType.Warning));
            Assert.Greater(response.ExtraData.Count, 0);
            Assert.AreEqual("DomainFullName", response.ExtraData[0].Key);
            Assert.AreEqual(localizer["msgRequiredField"].Value, response.ExtraData[0].Value);
        }

        #region Setup Dummy Data & Repository
        private Mock<IRepository<UserMaster>> SetupUserMasterRepository()
        {
            var repo = new Mock<IRepository<UserMaster>>();
            var responseSingle = GetSingleUserObjectMockData();
            var recordList = GetGeneralUserMockData();

            IQueryable<UserMaster> queryableData = recordList.Result.AsQueryable();

            repo.Setup(r => r.GetManyAsync(o => !o.IsDeleted)).Returns(recordList);
            repo.Setup(r => r.AddAsync(It.IsAny<UserMaster>()))
             .Callback(new Action<UserMaster>(entity =>
             {
                 entity.DisplayName = "Test User";
                 entity.DomainFullName = "pip.local/Administrator";
                 entity.CreatedOn = DateTime.Now;
             }));

            repo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(responseSingle.Data);
            repo.Setup(r => r.Any(x => x.UserID != responseSingle.Data.UserID && x.DomainFullName == string.Empty)).Returns(true);
            repo.Setup(r => r.Query(true)).Returns(queryableData);

            return repo;
        }

        private Mock<IRepository<UserRole>> SetupUserRoleRepository()
        {
            var repo = new Mock<IRepository<UserRole>>();
            List<UserRole> recordList = new List<UserRole>();
            recordList.Add(new UserRole { UserID = 1, RoleID = 1 });

            IQueryable<UserRole> querableData = recordList.AsQueryable();
            repo.Setup(r => r.Query(true)).Returns(querableData);

            IQueryable<UserRole> queryableData = recordList.AsQueryable();

            repo.Setup(r => r.GetManyAsync(o => o.UserID == 1)).ReturnsAsync(recordList);
            repo.Setup(r => r.AddAsync(It.IsAny<UserRole>()))
                             .Callback(new Action<UserRole>(entity =>
                             {
                                 entity.UserID = 1;
                                 entity.RoleID = 1;
                             }));
            repo.Setup(r => r.Any()).Returns(true);
            repo.Setup(r => r.Query(true)).Returns(queryableData);
            return repo;
        }

        private async Task<ICollection<UserMaster>> GetGeneralUserMockData()
        {
            List<UserMaster> recordList = new List<UserMaster>();
            recordList.Add(GetUserMasterMockObject(1));
            recordList.Add(GetUserMasterMockObject(2));
            return await Task.FromResult(recordList);
        }

        private PagedEntityResponse<UserMasterModel> GetGeneralUserMockPagedData()
        {
            var response = new PagedEntityResponse<UserMasterModel>();
            response.Data = new List<UserMasterModel>();
            response.Data.Add(GetModelObjectMock(1));
            response.Data.Add(GetModelObjectMock(2));
            return response;
        }

        private GeneralResponse<UserMaster> GetSingleUserObjectMockData()
        {
            var response = new GeneralResponse<UserMaster>();
            response.Data = GetUserMasterMockObject(1);
            return response;
        }

        private UserMaster GetUserMasterMockObject(int? id = null)
        {
            UserMaster obj = new UserMaster()
            {
                UserID = 1,
                DisplayName = "Test User",
                DomainFullName = "pip.local/Administrator",
                IsDeleted = false,
                UserRole = new Collection<UserRole>() { 
                    new UserRole() { UserRoleID = 1, UserID = 1, RoleID  = 1, Role = new RoleMaster() { RoleName = "Admin" } } 
                }
            };
            if (id != null)
            {
                obj.UserID = Convert.ToInt32(id);
            }
            return obj;
        }

        private UserMasterModel GetModelObjectMock(int? id = null)
        {
            UserMasterModel model = new UserMasterModel();
            model.DisplayName = "Test User";
            model.DomainFullName = "pip.local/Administrator";
            model.AssignedRoleList = new int[] { 1, 99 }; 
            if (id != null)
            {
                model.UserID = Convert.ToInt32(id);
            }
            return model;
        }
        #endregion
    }
}