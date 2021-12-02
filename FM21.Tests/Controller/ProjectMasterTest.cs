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
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FM21.Tests
{
    [TestFixture]
    public class ProjectMasterTest
    {
        private Mock<IProjectMasterService> projectService;
        private ProjectMasterController projectController;
        private IStringLocalizer localizer;
        private SearchFilter searchFilter;
        private ProjectMasterValidator validator;

        [SetUp]
        public void SetUp()
        {
            localizer = new JsonStringLocalizer();
            searchFilter = Mock.Of<SearchFilter>(m =>
                     m.PageSize == 10 &&
                     m.PageIndex == 1 &&
                     m.Search == "FM" &&
                     m.SortColumn == "" &&
                     m.SortDirection == "");
            validator = new ProjectMasterValidator(localizer);
            projectService = new Mock<IProjectMasterService>();
        }

        [Test]
        public async Task ProjectMaster_Controller_Should_Return_All_Active_project_Data()
        {

            var response = GetGeneralProjectMockData();
            projectService.Setup(t => t.GetAll()).ReturnsAsync(response);
            projectController = new ProjectMasterController(projectService.Object);
            var projectList = await projectController.GetAll() as JsonResult;
            Assert.IsNotNull(projectList);
            Assert.AreEqual(response.Data.Count, ((GeneralResponse<ICollection<ProjectMasterModel>>)projectList.Value).Data.Count);
            Assert.AreEqual(response.Result, ((GeneralResponse<ICollection<ProjectMasterModel>>)projectList.Value).Result);
        }
        [Test]
        public async Task ProjectMaster_Controller_Should_Return_All_From_GetSearchList_With_Ok_Status()
        {

            var response = GetprojectMockDataModel();
            projectService.Setup(t => t.GetPageWiseData(searchFilter)).ReturnsAsync(response);
            projectController = new ProjectMasterController(projectService.Object);
            var projectList = await projectController.GetSearchList(searchFilter) as JsonResult;
            Assert.IsNotNull(projectList);
            Assert.AreEqual(response.Data, ((PagedEntityResponse<ProjectMasterModel>)projectList.Value).Data);
            Assert.AreEqual(response.Data.Count, ((PagedEntityResponse<ProjectMasterModel>)projectList.Value).Data.Count);
            Assert.AreEqual(response.Result, ((PagedEntityResponse<ProjectMasterModel>)projectList.Value).Result);

        }
        [Test]
        public async Task ProjectMaster_Controller_Should_Return_Null_From_GetSearchList_When_SearchFilter_Model_IsInvalid()
        {
            searchFilter = new SearchFilter();// This will cause modelInvalid
            projectController = new ProjectMasterController(projectService.Object);
            var projectList = await projectController.GetSearchList(searchFilter) as JsonResult;
            Assert.IsNull(projectList.Value);
        }

        [Test]
        public async Task ProjectMaster_Controller_Should_Return_project_By_Valid_projectId()
        {
            var objProject = GetProjectMaster();
            projectService.Setup(t => t.Get(objProject.Data.ProjectId)).ReturnsAsync(objProject);
            projectController = new ProjectMasterController(projectService.Object);

            var projectList = await projectController.GetProjectInformation(objProject.Data.ProjectId) as JsonResult;
            var actualResult = projectList.Value;

            projectService.Verify(c => c.Get(It.IsAny<int>()), Times.Once);
            Assert.AreEqual(objProject.Data.ProjectId, ((GeneralResponse<ProjectMaster>)actualResult).Data.ProjectId);
            Assert.AreEqual(objProject.Data.ProjectDescription, ((GeneralResponse<ProjectMaster>)actualResult).Data.ProjectDescription);
            Assert.AreEqual(objProject.Data.ProjectCode, ((GeneralResponse<ProjectMaster>)actualResult).Data.ProjectCode);
            Assert.AreEqual(objProject.Data.CustomerId, ((GeneralResponse<ProjectMaster>)actualResult).Data.CustomerId);

        }

        [Test]
        public async Task ProjectMaster_Controller_Should_Return_Null_When_InValid_projectId()
        {
            int projectId = 100;
            var objProject = GetProjectMaster();
            projectService.Setup(t => t.Get(objProject.Data.ProjectId)).ReturnsAsync(objProject);
            projectController = new ProjectMasterController(projectService.Object);
            var projectObject = await projectController.GetProjectInformation(projectId) as NotFoundResult;
            projectService.Verify(c => c.Get(It.IsAny<int>()), Times.Once);
            Assert.AreEqual(HttpStatusCode.NotFound, (HttpStatusCode)projectObject.StatusCode);

        }

        [Test]
        public async Task ProjectMaster_Controller_Should_CreateNew_projectData()
        {
            GeneralResponse<bool> objProject = new GeneralResponse<bool>() { Data = true };
            var createModel = GetprojectModel();
            projectService.Setup(t => t.Create(createModel.Data)).ReturnsAsync(objProject);
            projectController = new ProjectMasterController(projectService.Object);
            var projectObject = await projectController.Postproject(createModel.Data);
            Assert.IsNotNull(projectObject);
            Assert.AreEqual(((GeneralResponse<bool>)((JsonResult)projectObject.Result).Value).Result, ResultType.Success); //return true

        }

        [Test]
        public async Task ProjectMaster_Controller_Should_Update_project_When_Data_Is_Valid()
        {
            int projectId = 1;
            GeneralResponse<bool> objProject = new GeneralResponse<bool>() { Data = true };
            var objProjectModel = GetprojectModel();
            projectService.Setup(t => t.Update(objProjectModel.Data)).ReturnsAsync(objProject);
            projectController = new ProjectMasterController(projectService.Object);
            var projectObject = await projectController.PutProjectMaster(projectId, objProjectModel.Data) as JsonResult;
            Assert.That(
            projectObject.Value,
            Is.EqualTo(objProject));
            Assert.IsNotNull(projectObject);
        }

        [Test]
        public void ProjectMaster_Controller_Should_Not_Update_project_When_Data_IsInvalid()
        {
            ProjectMasterModel Data = GetprojectModel().Data;
            Data.ProjectCode =0;
            Data.ProjectDescription = "";
            Data.CustomerId = 0;
            var result = validator.TestValidate(Data, ruleSet: "New,Edit");
            result.ShouldHaveValidationErrorFor(x => x.ProjectCode);
            result.ShouldHaveValidationErrorFor(x => x.ProjectDescription);
            result.ShouldHaveValidationErrorFor(x => x.CustomerId);
        }

        [Test]
        public async Task project_Controller_Should_Delete_projectData()
        {
            int projectId = 1;
            var response = new GeneralResponse<bool>();
            projectService.Setup(t => t.Delete(projectId)).ReturnsAsync(response);
            projectController = new ProjectMasterController(projectService.Object);
            var projectObject = await projectController.DeleteProjectMaster(projectId) as JsonResult;

            Assert.That(
            projectObject.Value,
            Is.EqualTo(response));
            Assert.IsNotNull(projectObject);
        }
        [Test]
        public async Task project_Controller_Should_Create_NextProjectCode()
        {
           
            var response = new GeneralResponse<int>() { Data = 100006 };
            projectService.Setup(t => t.GetNextProjectCode()).ReturnsAsync(response);
            projectController = new ProjectMasterController(projectService.Object);
            var projectObject = await projectController.GetNextProjectCode() as JsonResult;

            Assert.IsNotNull(projectObject);
            Assert.That(projectObject.Value, Is.EqualTo(response));
        }

        private PagedEntityResponse<ProjectMasterModel> GetprojectMockDataModel()
        {
            var response = new PagedEntityResponse<ProjectMasterModel>();
            
            response.Data = GetProjectMasterModelList();
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

     
        private GeneralResponse<ICollection<ProjectMasterModel>> GetGeneralProjectMockData()
        {
            var response = new GeneralResponse<ICollection<ProjectMasterModel>>();
            response.Data = GetProjectMasterModelList();
            return response;
        }

        private GeneralResponse<ProjectMasterModel> GetprojectModel()
        {
            GeneralResponse<ProjectMasterModel> objProject = new GeneralResponse<ProjectMasterModel>()
            {
                Data = GetProjectMasterModelList().First()
            };
            return objProject;
        }

        private GeneralResponse<ProjectMaster> GetProjectMaster()
        {
            GeneralResponse<ProjectMaster> objProject = new GeneralResponse<ProjectMaster>()
            {
                Data = GetProjectListMaster().First()
            };
            return objProject;
        }

        private List<ProjectMaster> GetProjectListMaster()
        {
            List<ProjectMaster> ProjectMaster = new List<ProjectMaster>();
            ProjectMaster.Add(new ProjectMaster()
            {
                ProjectId = 1,
                ProjectCode = 100000,
                NPICode = null,
                ProjectDescription = "Not assigned",
                CustomerId = 3,
                IsActive = true,
                IsDeleted = false,
                CreatedBy = 8888,
                CreatedOn = DateTime.Now,
                UpdatedBy = null,
                UpdatedOn = DateTime.Now,

            });
            ProjectMaster.Add(new ProjectMaster()
            {

                ProjectId = 2,
                ProjectCode = 100001,
                NPICode = null,
                ProjectDescription = "This is the LAC\\123\\Project Description test",
                CustomerId = 2,
                IsActive = true,
                IsDeleted = false,
                CreatedBy = 8888,
                CreatedOn = DateTime.Now,
                UpdatedBy = null,
                UpdatedOn = DateTime.Now,

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
                IsDeleted = false


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

    }



}





