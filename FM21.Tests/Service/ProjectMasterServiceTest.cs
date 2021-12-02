using FM21.Core;
using FM21.Core.Model;
using FM21.Data.Infrastructure;
using FM21.Entities;
using FM21.Service;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FM21.Tests
{
    [TestFixture]
    public class ProjectMasterServiceTest : TestBase
    {
        private Mock<IRepository<ProjectMaster>> projectMasterRepository;
        private IProjectMasterService projectMasterService;
        private SearchFilter searchFilter;
        ProjectMasterModel projModel;
        List<ProjectMaster> projectModel;

        [SetUp]
        public void SetUp()
        {
            projectModel = GetProjectListMaster();
            projectMasterRepository = SetupProjectRepository();
            projModel = GetProjectMasterModelList().Last();
            searchFilter = Mock.Of<SearchFilter>(m =>
                    m.PageSize == 10 &&
                    m.PageIndex == 1 &&
                    m.Search == "FM" &&
                    m.SortColumn == "" &&
                    m.SortDirection == "");
            projectMasterService = new ProjectMasterService(serviceProvider.Object, exceptionHandler.Object, mapper, unitOfWork.Object, null, projectMasterRepository.Object);
        }

        [Test]
        public async Task Service_Should_Return_All_Active_ProjectInformation()
        {
            var response = await projectMasterService.GetAll();
            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
        }

        [Test]
        public async Task Service_Should_Not_Return_Data_When_Exception()
        {
            projectMasterRepository.Setup(r => r.Query(true)).Throws(new Exception("something went wrong"));
            var response = await projectMasterService.GetAll();
            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_Return_ProjectInformation_Sorting_By_ProjectCode()
        {
            searchFilter.SortColumn = "projectcode";
            var response = await projectMasterService.GetPageWiseData(searchFilter);
            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
        }

        [TestCase("projectcode")]
        [TestCase("npicode")]
        [TestCase("customername")]
        [TestCase("ProjectDescription")]
        [TestCase("CreatedBy")]
        [TestCase("")]
        public async Task Service_Should_Return_ProjectInformation_Sorting_By_Sort_Column(string sortColumn)
        {
            searchFilter.SortColumn = sortColumn;
            var response = await projectMasterService.GetPageWiseData(searchFilter);
            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
        }

        [Test]
        public async Task Service_Should_Not_Return_ProjectInformation_When_Exception()
        {
            projectMasterRepository.Setup(r => r.Query(true)).Throws(new Exception("something went wrong"));
            var response = await projectMasterService.GetPageWiseData(searchFilter);
            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_Return_ProjectInformation_By_Valid_ProjectId()
        {
            var returnObject = GetSingleProjectObjectMockData();
            var response = await projectMasterService.Get(1);
            Assert.That(response.Result, Is.EqualTo(returnObject.Result));
            Assert.AreEqual(response.Data.ProjectId, returnObject.Data.ProjectId);
        }

        [Test]
        public async Task Service_Should_Return_RecordNotExist_On_InValid_ProjectId()
        {
            var response = await projectMasterService.Get(5);

            Assert.IsNull(response.Data);
            Assert.AreEqual(response.Message, localizer["msgRecordNotExist", new string[] { "Project" }].Value);
        }

        [Test]
        public async Task Service_Should_Return_Project_Information_When_Exception()
        {
            projectMasterRepository.Setup(r => r.GetByIdAsync(5)).Throws(new Exception("something went wrong"));
            var response = await projectMasterService.Get(5);

            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_Create_New_Project()
        {
            int _maxRegIDBeforeAdd = projectModel.Max(a => a.ProjectId);
            projModel.ProjectCode = 100003;
            var repo = SetUpProjectInformationMock();

            var service = new ProjectMasterService(serviceProvider.Object, exceptionHandler.Object, mapper, unitOfWork.Object, null, repo.Object);

            var response = await service.Create(projModel);
            Assert.That(_maxRegIDBeforeAdd + 1, Is.EqualTo(projectModel.Last().ProjectId));
            Assert.AreEqual(response.Result, ResultType.Success);
        }

        [Test]
        public async Task Service_Should_Not_Create_New_Project_When_Project_Model_Is_Invalid()
        { 
            projModel.ProjectCode = 0;
            projModel.CustomerId = 0;
            var response = await projectMasterService.Create(projModel);

            Assert.That(response.Result, Is.EqualTo(ResultType.Warning));
            Assert.Greater(response.ExtraData.Count, 0);
        }

        [Test]
        public async Task Service_Should_Not_Create_New_Project_And_Return_Validaion_Message_When_ProjectCode_Is_100000()
        {
            projModel.ProjectCode = 100000;
            projModel.CustomerId = 5;
            var response = await projectMasterService.Create(projModel);

            Assert.That(response.Result, Is.EqualTo(ResultType.Warning));
            Assert.That(response.Message, Is.EqualTo(localizer["msgReserveProjectCode100000"].Value));
        }

        [Test]
        public async Task Service_Should_Not_Create_New_Project_And_Return_Validaion_Message_When_ProjectCode_Already_Exist()
        {
            projModel.ProjectCode = 100002;
            projModel.CustomerId = 5;
            var response = await projectMasterService.Create(projModel);

            Assert.That(response.Result, Is.EqualTo(ResultType.Warning));
            Assert.That(response.Message, Is.EqualTo(localizer["msgDuplicateRecord", new string[] { "Project" }].Value));
        }

        [Test]
        public async Task Service_Should_Not_Create_New_Project_And_Return_Validaion_Message_When_ProjectDescription_Is_Empty()
        {
            projModel.ProjectDescription = string.Empty;
            var response = await projectMasterService.Create(projModel);

            Assert.That(response.Result, Is.EqualTo(ResultType.Warning));
            Assert.Greater(response.ExtraData.Count, 0);
        }

        [Test]
        public async Task Service_Should_Create_New_Project_When_Exception()
        {
            projectMasterRepository.Setup(r => r.Any(It.IsAny<Expression<Func<ProjectMaster, bool>>>())).Throws(new Exception("something went wrong"));
            var response = await projectMasterService.Create(projModel);
            Assert.AreEqual(response.Result, ResultType.Error);
        }

        [Test]
        public async Task Service_Should_Update_ProjectInformation()
        {
            string oldProjectDescription = projModel.ProjectDescription;
            projModel.ProjectDescription = "ONT Bar \\ for the choc 2020";
            projModel.ProjectId = 1;
            projModel.ProjectCode = 100009;
           
            projectMasterRepository.Setup(repository => repository.Any(It.IsAny<Expression<Func<ProjectMaster, bool>>>()))
                .Callback<Expression<Func<ProjectMaster, bool>>>(
                    expression =>
                    {
                        expression.Compile();
                    })
                .Returns(false);

            var response = await projectMasterService.Update(projModel);
            Assert.That(projModel.ProjectDescription, Is.Not.EqualTo(oldProjectDescription));
            Assert.That(projModel.ProjectId, Is.EqualTo(1));
            Assert.AreEqual(response.Result, ResultType.Success);
        }
        
        [Test]
        public async Task Service_Should_Not_Update_ProjectInformation_When_ProjectCode_Is_100000_And_Return_ValidationMessage()
        {
            projModel.ProjectDescription = "ONT Bar \\ for the choc 2020";
            projModel.ProjectCode = 100000;
            var response = await projectMasterService.Update(projModel);

            Assert.That(response.Result, Is.EqualTo(ResultType.Warning));
            Assert.That(response.Message, Is.EqualTo(localizer["msgReserveProjectCode100000"].Value));
        }

        [Test]
        public async Task Service_Should_Not_Update_ProjectInformation_When_ProjectId_Is_Invalid_And_Return_ValidationMessage()
        {
            projModel.ProjectId = 0;
            var response = await projectMasterService.Update(projModel);

            Assert.Greater(response.ExtraData.Count, 0);
            Assert.AreEqual("ProjectId", response.ExtraData[0].Key);
            Assert.AreEqual("Must be greater than zero.", response.ExtraData[0].Value);
        }
        
        [Test]
        public async Task Service_Should_Not_Update_When_Project_Information_Is_NotExist()
        {
            projModel.ProjectDescription = "ONT Bar \\ for the choc 2020";
            projModel.ProjectCode = 100008;
            string expectedResult = localizer["msgRecordNotExist", new string[] { "Project" }];
            projectMasterRepository.Setup(r => r.Any(It.IsAny<Expression<Func<ProjectMaster, bool>>>())).Returns(true);
            var actualResult = await projectMasterService.Update(projModel);

            Assert.AreEqual(actualResult.Result, ResultType.Warning);
            Assert.That(actualResult.Message, Is.EqualTo(expectedResult));
        }
        
        [Test]
        public async Task Service_Should_Not_Update_When_Project_Information_Is_Duplicate_In_The_System()
        {
         
            projModel.ProjectCode = 100008;
            projModel.ProjectId = 2;
            var responseSingle = GetSingleProjectObjectMockData();

            string expectedResult = localizer["msgDuplicateRecord", new string[] { "Project" }];
            projectMasterRepository.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(responseSingle.Data);
            
            projectMasterRepository.Setup(r => r.Any(It.IsAny<Expression<Func<ProjectMaster, bool>>>())).Returns(true);
            var actualResult = await projectMasterService.Update(projModel);

            Assert.AreEqual(actualResult.Result, ResultType.Warning);
            Assert.That(actualResult.Message, Is.EqualTo(expectedResult));
        }
        
        [Test]
        public async Task Service_ProjectInformation_ThrowsException_On_Update()
        {
          
            projectMasterRepository.Setup(r => r.GetByIdAsync(2)).Throws(new Exception("something went wrong"));
            var actualResult = await projectMasterService.Update(projModel);

            Assert.AreEqual(actualResult.Result, ResultType.Error);
        }

        [Test]
        public async Task Service_Should_Delete_ProjectInformation_When_ProjectId_Is_Valid()
        {
            projModel.ProjectId = 1;
            var response = await projectMasterService.Delete(projModel.ProjectId);
            Assert.AreEqual(response.Result, ResultType.Success);
        }

        [Test]
        public async Task Service_ProjectInformation_ThrowsException_On_Delete()
        {
            projModel.ProjectId = 1;
            projectMasterRepository.Setup(r => r.GetByIdAsync(1)).Throws(new Exception("something went wrong"));
            var response = await projectMasterService.Delete(projModel.ProjectId);
            Assert.AreEqual(response.Result, ResultType.Error);
        }

        [Test]
        public async Task Service_Should_Not_Delete_ProjectInformation_When_ProjectId_Is_InValid()
        {
            projModel.ProjectId = 100;
            var response = await projectMasterService.Delete(projModel.ProjectId);
            Assert.AreEqual(response.Result, ResultType.Warning);
        }

        [Test]
        public async Task Service_Should_Return_New_Auto_Generate_Project_Code_When_New_Project_Created()
        {
            int nextProjectCode = 100003;
            int previousProjectCode = 100002;
            var response = await projectMasterService.GetNextProjectCode();

            Assert.AreNotEqual(nextProjectCode, previousProjectCode);
            Assert.AreEqual(response.Data, nextProjectCode);
        }

        [Test]
        public async Task Service_Should_Not_Return_New_Auto_Generate_Project_Code_When_Exception()
        {
            projectMasterRepository.Setup(r => r.GetAllAsync()).Throws(new Exception("something went wrong"));
            var response = await projectMasterService.GetNextProjectCode();

            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        #region Setup Dummy Data & Repository
        private Mock<IRepository<ProjectMaster>> SetupProjectRepository()
        {
            var repo = new Mock<IRepository<ProjectMaster>>();
            var responseSingle = GetSingleProjectObjectMockData();
            var response = GetGeneralprojectMockData();

            var getRegMockData = GetGeneralprojectMockPagedData();
            IQueryable<ProjectMaster> queryableProject = getRegMockData.Data.AsQueryable();

            repo.Setup(r => r.GetManyAsync(o => o.IsActive && !o.IsDeleted)).Returns(response);
            repo.Setup(r => r.AddAsync(It.IsAny<ProjectMaster>()))
             .Callback(new Action<ProjectMaster>(newProject => {
                 dynamic maxProjectID = projectModel.Last().ProjectId;
                 dynamic nextProjectID = maxProjectID + 1;
                 newProject.ProjectId = nextProjectID;
                 newProject.CustomerId =3;

                 newProject.CreatedOn = DateTime.Now;
                 projectModel.Add(newProject);
             }));
            repo.Setup(r => r.UpdateAsync(It.IsAny<ProjectMaster>()))
             .Callback(new Action<ProjectMaster>(x => {
                 var oldProject = projectModel.Find(a => a.ProjectId == x.ProjectId);
                 oldProject.UpdatedOn = DateTime.Now;
                 oldProject = x;

             }));
          
            repo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(responseSingle.Data);

            repo.Setup(r => r.Query(true)).Returns(queryableProject);
           
            repo.Setup(repository => repository.Any(It.IsAny<Expression<Func<ProjectMaster, bool>>>()))
                .Callback<Expression<Func<ProjectMaster, bool>>>(
                    expression =>
                    {
                        expression.Compile();
                    })
                .Returns(true);

            repo.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(() => { responseSingle.Data = null; return responseSingle.Data; }); // Get data from Invalid ID
            repo.Setup(r => r.GetAllAsync()).Returns(response);

            return repo;
        }

        private Mock<IRepository<ProjectMaster>> SetUpProjectInformationMock()
        {
            var repo = new Mock<IRepository<ProjectMaster>>();
            bool IsExist = false;
            repo.Setup(r => r.AddAsync(It.IsAny<ProjectMaster>()))
             .Callback(new Action<ProjectMaster>(newProject => {
                 dynamic maxProjectID = projectModel.Last().ProjectId;
                 dynamic nextProjectID = maxProjectID + 1;
                 newProject.ProjectId = nextProjectID;
                 newProject.CustomerId = 3;

                 newProject.CreatedOn = DateTime.Now;
                 projectModel.Add(newProject);
             }));


            repo.Setup(repository => repository.Any(It.IsAny<Expression<Func<ProjectMaster, bool>>>()))
                .Callback<Expression<Func<ProjectMaster, bool>>>(
                    expression =>
                    {
                        expression.Compile();
                        IsExist = false;
                    })
                .Returns(IsExist);

            return repo;
        }

        private async Task<ICollection<ProjectMaster>> GetGeneralprojectMockData()
        {
            return await Task.FromResult(GetProjectListMaster());
        }

        private PagedEntityResponse<ProjectMaster> GetGeneralprojectMockPagedData()
        {
            var response = new PagedEntityResponse<ProjectMaster>();            
            response.Data = GetProjectListMaster();
            return response;
        }

        private GeneralResponse<ProjectMaster> GetSingleProjectObjectMockData()
        {
            var response = new GeneralResponse<ProjectMaster>();
            response.Data = GetProjectListMaster().Last();
            return response;
        }
        
        private PagedEntityResponse<Customer> GetNutrientData()
        {
            List<Customer> lst = new List<Customer>();
            var response = new PagedEntityResponse<Customer>();
            lst.Add(new Customer()
            {
                CustomerId = 3,
                Name = "FM Nessloson 2",
                Email = "FM@gmail.com",
                Address = "address",
                PhoneNumber = "7896541254",
                IsActive = true,
                IsDeleted = false,
                CreatedBy = 25412,
                CreatedOn = DateTime.Now,
                CustomerAbbreviation1 = "12342134",
                CustomerAbbreviation2 = "241234"
            });
            lst.Add(new Customer()
            {
                CustomerId = 4,
                Name = "Tom",
                Email = "FM4@gmail.com",
                Address = "address test4",
                PhoneNumber = "7896541254",
                IsActive = true,
                IsDeleted = false,
                CreatedBy = 25412,
                CreatedOn = DateTime.Now,
                CustomerAbbreviation1 = "abb14",
                CustomerAbbreviation2 = "abb24"
            });
            response.Data = lst;
            return response;
        }

        private List<ProjectMaster> GetProjectListMaster()
        {
            List<ProjectMaster> ProjectMaster = new List<ProjectMaster>();
            ProjectMaster.Add(new ProjectMaster()
            {
                ProjectId = 1,
                ProjectCode = 10000,
                NPICode = null,
                ProjectDescription = "Not assigned",
                CustomerId = 3,
                IsActive = true,
                IsDeleted = false,
                CreatedBy = 8888,
                CreatedOn = DateTime.Now,
                UpdatedBy = null,
                UpdatedOn = DateTime.Now,
                CustomerMaster = GetNutrientData().Data[0]

            });
            ProjectMaster.Add(new ProjectMaster()
            {

                ProjectId = 2,
                ProjectCode = 100001,
                NPICode = null,
                ProjectDescription = "This is the LAC\\123\\Project Description test",
                CustomerId = 4,
                IsActive = true,
                IsDeleted = false,
                CreatedBy = 8888,
                CreatedOn = DateTime.Now,
                UpdatedBy = null,
                UpdatedOn = DateTime.Now,
                CustomerMaster = GetNutrientData().Data[1]

            });
            ProjectMaster.Add(new ProjectMaster()
            {

                ProjectId = 3,
                ProjectCode = 100002,
                NPICode = null,
                ProjectDescription = "This is the LAC\\123\\Project Description test of Project ID 3",
                CustomerId = 4,
                IsActive = true,
                IsDeleted = false,
                CreatedBy = 8888,
                CreatedOn = DateTime.Now,
                UpdatedBy = null,
                UpdatedOn = DateTime.Now,
                CustomerMaster = GetNutrientData().Data[1]

            });
            return ProjectMaster;
        }
        
        private List<ProjectMasterModel> GetProjectMasterModelList()
        {
            List<ProjectMasterModel> ProjectMaster = new List<ProjectMasterModel>();
            ProjectMaster.Add(new ProjectMasterModel()
            {
                ProjectId = 1,
                ProjectCode = 100000,
                NPICode = null,
                ProjectDescription = "Not assigned",
                CustomerId = 2,
                IsActive = true,
                IsDeleted = false,
                 


            });
            ProjectMaster.Add(new ProjectMasterModel()
            {
                ProjectId = 2,
                ProjectCode = 100001,
                NPICode = null,
                ProjectDescription = "This is the LAC\\123\\Project Description test",
                CustomerId = 2,
                IsActive = true,
                IsDeleted = false
            });
            return ProjectMaster;
        }
        #endregion
    }
}