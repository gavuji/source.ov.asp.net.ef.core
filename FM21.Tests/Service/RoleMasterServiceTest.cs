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
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FM21.Tests
{
    [TestFixture]
    public class RoleMasterServiceTest : TestBase
    {
        private IRoleMasterService roleMasterService;
        private Mock<IRepository<RoleMaster>> roleMasterRepository;
        private IRepository<RolePermissionMapping> rolePermissionRepository;

        [SetUp]
        [SetUICulture("en-us")]
        public void SetUp()
        {
            roleMasterRepository = SetupRoleMasterRepository();
            rolePermissionRepository = SetupRolePermissionRepository();
            roleMasterService = new RoleMasterService(serviceProvider.Object, exceptionHandler.Object, mapper, unitOfWork.Object, null, roleMasterRepository.Object, rolePermissionRepository);
        }

        [Test]
        public async Task Service_Should_Return_All_ActiveAnd_NotDeleted_Data_By_SortColumn()
        {
            var response = await roleMasterService.GetAll();

            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
        }

        [Test]
        public async Task Service_Should_Return_All_Active_Data()
        {
            var returnObject = GetGeneralRoleMockPagedData();
            var response = await roleMasterService.GetPageWiseData(new SearchFilter() { PageIndex = 1, PageSize = 10 });

            Assert.That(response.Result, Is.EqualTo(returnObject.Result));
            Assert.IsNotNull(response.Data);
        }

        [Test]
        public async Task Service_Should_Not_Return_Data_When_Exception()
        {
            roleMasterRepository.Setup(r => r.GetManyAsync(o => o.IsActive && !o.IsDeleted, true)).Throws(new Exception("something went wrong"));
            var response = await roleMasterService.GetAll();
            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [TestCase("RoleName")]
        [TestCase("RoleDescription")]
        [TestCase("CreatedBy")]
        public async Task Service_Should_Return_All_Active_Data_With_Sorting_By_SortColumn(string sortColumn)
        {
            var returnObject = GetGeneralRoleMockPagedData();
            var response = await roleMasterService.GetPageWiseData(new SearchFilter() { PageIndex = 1, PageSize = 10, SortColumn = sortColumn,Search= "site manager" });

            Assert.That(response.Result, Is.EqualTo(returnObject.Result));
        }

        [Test]
        public async Task Service_Should_Not_Return_RoleData_When_Exception()
        {
            roleMasterRepository.Setup(r => r.Query(true)).Throws(new Exception("something went wrong"));
            var response = await roleMasterService.GetPageWiseData(new SearchFilter());

            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_Return_By_Valid_PrimaryKey()
        {
            var returnObject = GetSingleRoleObjectMockData();
            var response = await roleMasterService.Get(1);

            Assert.That(response.Result, Is.EqualTo(returnObject.Result));
            Assert.AreEqual(response.Data.RoleID, returnObject.Data.RoleID);
        }

        [Test]
        public async Task Service_Should_Return_RecordNotExist_On_Invalid_PrimaryKey()
        {
            var response = await roleMasterService.Get(5);

            Assert.IsNull(response.Data);
            Assert.AreEqual(response.Message, localizer["msgRecordNotExist", new string[] { "Role" }].Value);
        }

        [Test]
        public async Task Service_Should_Not_Return_Role_When_Exception()
        {
            roleMasterRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>())).Throws(new Exception("something went wrong"));
            var response = await roleMasterService.Get(1);

            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_CreateNew_Data()
        {
            var model = GetModelObjectMock();
            var response = await roleMasterService.Create(model);

            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
            Assert.AreEqual(response.Message, localizer["msgInsertSuccess", new string[] { "Role" }].Value);
        }

        [Test]
        public async Task Service_Should_Not_Create_When_Data_InValid()
        {
            var model = GetModelObjectMock();
            model.RoleName = string.Empty;

            var response = await roleMasterService.Create(model);

            Assert.That(response.Result, Is.EqualTo(ResultType.Warning));
            Assert.Greater(response.ExtraData.Count, 0);
        }
        
        [Test]
        public async Task Service_Should_Not_CreateNew_RoleMaster_When_RoleName_Already_Exist()
        {
            var model = GetModelObjectMock();
            string expectedResult = localizer["msgDuplicateRecord", new string[] { "Role" }].Value;
            roleMasterRepository.Setup(r => r.Any(x => x.RoleName.ToLower().Trim() == model.RoleName.ToLower().Trim())).Returns(true);
            var actualResult = await roleMasterService.Create(model);

            Assert.That(actualResult.Result, Is.EqualTo(ResultType.Warning));
            Assert.AreEqual(actualResult.Message, expectedResult);
        }

        [Test]
        public async Task Service_Should_Not_Create_New_Role_When_Excption()
        {
            var model = GetModelObjectMock();
            roleMasterRepository.Setup(r => r.Any(It.IsAny<Expression<Func<RoleMaster, bool>>>())).Throws(new Exception("something went wrong"));
            var response = await roleMasterService.Create(model);
            Assert.AreEqual(response.Result, ResultType.Error);
        }

        [Test]
        public async Task Service_Should_Update_Data()
        {
            var model = GetModelObjectMock();
            model.RoleID = 1;
            model.RoleName = "Role edited";

            var response = await roleMasterService.Update(model);

            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
        }

        [Test]
        public async Task Service_Should_Not_Update_When_PrimaryKey_InValid()
        {
            var model = GetModelObjectMock();
            var response = await roleMasterService.Update(model);

            Assert.That(response.Result, Is.EqualTo(ResultType.Warning));
            Assert.Greater(response.ExtraData.Count, 0);
            Assert.AreEqual("RoleID", response.ExtraData[0].Key);
            Assert.AreEqual(localizer["msgMustBeGreaterThenZero"].Value, response.ExtraData[0].Value);
        }

        [Test]
        public async Task Service_Should_Not_Update_Data_When_Values_Are_InValid()
        {
            var model = GetModelObjectMock();
            model.RoleID = 1;
            model.RoleName = string.Empty;

            var response = await roleMasterService.Update(model);

            Assert.That(response.Result, Is.EqualTo(ResultType.Warning));
            Assert.Greater(response.ExtraData.Count, 0);
            Assert.AreEqual("RoleName", response.ExtraData[0].Key);
            Assert.AreEqual(localizer["msgRequiredField"].Value, response.ExtraData[0].Value);
        }
        
        [Test]
        public async Task Service_Should_Not_Update_When_Role_Is_Duplicate()
        {
            var model = GetModelObjectMock();
            model.RoleID = 1;
            string expectedResult = localizer["msgDuplicateRecord", new string[] { "Role" }].Value;
            roleMasterRepository.Setup(r => r.Any(It.IsAny<Expression<Func<RoleMaster, bool>>>())).Returns(true);
            var actualResult = await roleMasterService.Update(model);

            Assert.That(actualResult.Result, Is.EqualTo(ResultType.Warning));
            Assert.AreEqual(actualResult.Message, expectedResult);
        }

        [Test]
        public async Task Service_Should_Not_Update_Role_When_Exception()
        {
            var model = GetModelObjectMock();
            model.RoleID = 1;
            roleMasterRepository.Setup(r => r.Any(It.IsAny<Expression<Func<RoleMaster, bool>>>())).Throws(new Exception("something went wrong"));
            var response = await roleMasterService.Update(model);

            Assert.AreEqual(response.Result, ResultType.Error);
        }

        [Test]
        public async Task Service_Should_Delete_Data_When_PrimaryKey_Valid()
        {
            var model = GetModelObjectMock();
            model.RoleID = 1;

            var response = await roleMasterService.Delete(model.RoleID);

            Assert.AreEqual(response.Result, ResultType.Success);
        }

        [Test]
        public async Task Service_Should_Not_Delete_Data_When_PrimaryKey_InValid()
        {
            var model = GetModelObjectMock();
            model.RoleID = 5;

            var response = await roleMasterService.Delete(model.RoleID);

            Assert.AreEqual(response.Result, ResultType.Warning);
            Assert.AreEqual(response.Message, localizer["msgRecordNotExist", new string[] { "Role" }].Value);
        }
        
        [Test]
        public async Task Service_Should_Not_Delete_When_Role_Used_By_Other_Place()
        {
            var model = GetModelObjectMock();
            model.RoleID = 1;
            roleMasterRepository.Setup(r => r.Any(It.IsAny<Expression<Func<RoleMaster, bool>>>())).Returns(true);
            var response = await roleMasterService.Delete(model.RoleID);

            Assert.AreEqual(response.Result, ResultType.Warning);
            Assert.AreEqual(response.Message, localizer["msgRoleDeleteFail"].Value);
        }

        [Test]
        public async Task Service_Should_Delete_When_Exception()
        {
            roleMasterRepository.Setup(r => r.GetByIdAsync(1)).Throws(new Exception("something went wrong"));
            var response = await roleMasterService.Delete(1);
            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        #region Setup Dummy Data & Repository
        private Mock<IRepository<RoleMaster>> SetupRoleMasterRepository()
        {
            var repo = new Mock<IRepository<RoleMaster>>();
            var responseSingle = GetSingleRoleObjectMockData();
            var recordList = GetGeneralRoleMockData();

            var mockData = GetGeneralRoleMockPagedData();
            IQueryable<RoleMaster> queryableData = mockData.Data.AsQueryable();

            repo.Setup(r => r.GetManyAsync(o => o.IsActive && !o.IsDeleted)).Returns(recordList);
            repo.Setup(r => r.GetManyAsync(o => o.IsActive && !o.IsDeleted, true)).Returns(recordList);
            repo.Setup(r => r.AddAsync(It.IsAny<RoleMaster>()))
             .Callback(new Action<RoleMaster>(entity =>
             {
                 entity.RoleName = "Test Role";
                 entity.RoleDescription = "Test Role";
                 entity.CreatedOn = DateTime.Now;
             }));

            repo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(responseSingle.Data);
            repo.Setup(r => r.Any(x => x.RoleID != responseSingle.Data.RoleID && x.RoleName == string.Empty)).Returns(true);
            repo.Setup(r => r.Query(true)).Returns(queryableData);
            repo.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(() => { responseSingle.Data = null; return responseSingle.Data; }); // Get data from Invalid ID
            return repo;
        }

        private IRepository<RolePermissionMapping> SetupRolePermissionRepository()
        {
            var repo = new Mock<IRepository<RolePermissionMapping>>();
            IQueryable<RolePermissionMapping> querableRolePermissionMapping = GetRolePermissionMappingData().Data.AsQueryable();
            repo.Setup(r => r.Query(true)).Returns(querableRolePermissionMapping);
            return repo.Object;
        }

        private async Task<ICollection<RoleMaster>> GetGeneralRoleMockData()
        {
            List<RoleMaster> recordList = new List<RoleMaster>();
            recordList.Add(GetObjectMock(1));
            recordList.Add(GetObjectMock(2));
            recordList[1].RoleName = "formulator";
            return await Task.FromResult(recordList);
        }

        private PagedEntityResponse<RoleMaster> GetGeneralRoleMockPagedData()
        {
            var response = new PagedEntityResponse<RoleMaster>();
            response.Data = new List<RoleMaster>();
            response.Data.Add(GetObjectMock(1));
            response.Data[0].RolePermissionMapping = new Collection<RolePermissionMapping>() { new RolePermissionMapping() { RoleID = 1, PermissionID = 2, PermissionType = 1 } };
            response.Data.Add(GetObjectMock(2));
            response.Data[1].RolePermissionMapping = new Collection<RolePermissionMapping>() { new RolePermissionMapping() { RoleID = 2, PermissionID = 2, PermissionType = 1 } };
            response.Data[1].RoleName = "formulator";
            return response;
        }

        private GeneralResponse<RoleMaster> GetSingleRoleObjectMockData()
        {
            var response = new GeneralResponse<RoleMaster>();
            response.Data = GetObjectMock(1);
            return response;
        }

        private RoleMaster GetObjectMock(int? id = null)
        {
            RoleMaster obj = new RoleMaster()
            {
                RoleName = "site manager",
                RoleDescription = "test",
                IsActive = true,
                IsDeleted = false
            };
            if (id != null)
            {
                obj.RoleID = Convert.ToInt32(id);
            }
            return obj;
        }

        private RoleMasterModel GetModelObjectMock()
        {
            RoleMasterModel model = new RoleMasterModel();
            model.RoleName = "Test Role";
            model.RoleDescription = "Test Role";
            return model;
        }

        private PagedEntityResponse<RolePermissionMapping> GetRolePermissionMappingData()
        {
            var response = new PagedEntityResponse<RolePermissionMapping>();
            response.Data = new List<RolePermissionMapping>();
            response.Data.Add(new RolePermissionMapping()
            {
                RoleID = 1,
                PermissionID = 1,
                PermissionType = 1
            });
            response.Data.Add(new RolePermissionMapping()
            {
                RoleID = 1,
                PermissionID = 2,
                PermissionType = 1
            });
            return response;
        }
        #endregion
    }
}