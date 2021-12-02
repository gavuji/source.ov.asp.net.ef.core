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
    public class RegulatoryMasterTest
    {
        private Mock<IRegulatoryMasterService> regulatoryService;
        private RegulatoryMasterController regulatoryController;
        private IStringLocalizer localizer;
        private SearchFilter searchFilter;
        private RegulatoryMasterValidator validator;

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
            validator = new RegulatoryMasterValidator(localizer);
            regulatoryService = new Mock<IRegulatoryMasterService>();
        }

        [Test]
        public async Task RegulatoryMaster_Controller_Should_Return_All_Active_Regulatory_Data()
        {
            var response = GetGeneralRegulatoryMockData();

            regulatoryService.Setup(t => t.GetAll()).ReturnsAsync(response);
            regulatoryController = new RegulatoryMasterController(regulatoryService.Object);
            var regulatoryList = await regulatoryController.GetAll() as JsonResult;
            Assert.IsNotNull(regulatoryList);
            Assert.AreEqual(response.Data, ((GeneralResponse<ICollection<RegulatoryMaster>>)regulatoryList.Value).Data);
            Assert.AreEqual(response.Data.Count, ((GeneralResponse<ICollection<RegulatoryMaster>>)regulatoryList.Value).Data.Count);
            Assert.AreEqual(response.Result, ((GeneralResponse<ICollection<RegulatoryMaster>>)regulatoryList.Value).Result);
        }

        [Test]
        public async Task RegulatoryMaster_Controller_Should_Return_All_From_GetSearchList_With_Ok_Status()
        {

            var response = GetRegulatoryMockData();
            regulatoryService.Setup(t => t.GetPageWiseData(searchFilter.Search, searchFilter.PageIndex, searchFilter.PageSize, searchFilter.SortColumn, searchFilter.SortDirection)).ReturnsAsync(response);

            regulatoryController = new RegulatoryMasterController(regulatoryService.Object);
            var regulatoryList = await regulatoryController.GetSearchList(searchFilter) as JsonResult;
            Assert.IsNotNull(regulatoryList);
            Assert.AreEqual(response.Data, ((PagedEntityResponse<RegulatoryModel>)regulatoryList.Value).Data);
            Assert.AreEqual(response.Data.Count, ((PagedEntityResponse<RegulatoryModel>)regulatoryList.Value).Data.Count);
            Assert.AreEqual(response.Result, ((PagedEntityResponse<RegulatoryModel>)regulatoryList.Value).Result);

        }

        [Test]
        public async Task RegulatoryMaster_Controller_Should_Return_Null_From_GetSearchList_When_SearchFilter_Model_IsInvalid()
        {
            searchFilter = new SearchFilter();// This will cause modelInvalid
            regulatoryController = new RegulatoryMasterController(regulatoryService.Object);

            var context = new ValidationContext(searchFilter, null, null);
            var results = new List<ValidationResult>();
            if (!Validator.TryValidateObject(searchFilter, context, results, true))
            {
                regulatoryController.ModelState.Clear();
                foreach (ValidationResult result in results)
                {
                    var key = result.MemberNames.FirstOrDefault() ?? "";
                    regulatoryController.ModelState.AddModelError(key, result.ErrorMessage);
                }
            }

            var response = await regulatoryController.GetSearchList(searchFilter) as JsonResult;
            Assert.IsNull(response);
        }

        [Test]
        public async Task RegulatoryMaster_Controller_Should_Return_Regulatory_By_Valid_RegulatoryId()
        {
            var objRegulatory = GetRegulatoryMaster();
            regulatoryService = new Mock<IRegulatoryMasterService>();
            regulatoryService.Setup(t => t.Get(objRegulatory.Data.RegulatoryId)).ReturnsAsync(objRegulatory);
            regulatoryController = new RegulatoryMasterController(regulatoryService.Object);

            var regulatoryList = await regulatoryController.GetRegulatory(objRegulatory.Data.RegulatoryId) as JsonResult;
            var actualResult = regulatoryList.Value;

            regulatoryService.Verify(c => c.Get(It.IsAny<int>()), Times.Once);
            Assert.AreEqual(objRegulatory.Data.NutrientId, ((GeneralResponse<RegulatoryMaster>)actualResult).Data.NutrientId);
            Assert.AreEqual(objRegulatory.Data.OldUsa, ((GeneralResponse<RegulatoryMaster>)actualResult).Data.OldUsa);
            Assert.AreEqual(objRegulatory.Data.CanadaNi, ((GeneralResponse<RegulatoryMaster>)actualResult).Data.CanadaNi);
            Assert.AreEqual(objRegulatory.Data.CanadaNf, ((GeneralResponse<RegulatoryMaster>)actualResult).Data.CanadaNf);
            Assert.AreEqual(objRegulatory.Data.NewUsRdi, ((GeneralResponse<RegulatoryMaster>)actualResult).Data.NewUsRdi);
            Assert.AreEqual(objRegulatory.Data.EU, ((GeneralResponse<RegulatoryMaster>)actualResult).Data.EU);
            Assert.AreEqual(objRegulatory.Data.UnitPerMg, ((GeneralResponse<RegulatoryMaster>)actualResult).Data.UnitPerMg);
        }

        [Test]
        public async Task RegulatoryMaster_Controller_Should_Return_Null_When_InValid_RegulatoryId()
        {
            int regulatoryId = 100;
            var objRegulatory = GetRegulatoryMaster();
            regulatoryService.Setup(t => t.Get(objRegulatory.Data.RegulatoryId)).ReturnsAsync(objRegulatory);
            regulatoryController = new RegulatoryMasterController(regulatoryService.Object);
            var regulatoryObject = await regulatoryController.GetRegulatory(regulatoryId) as NotFoundResult;
            regulatoryService.Verify(c => c.Get(It.IsAny<int>()), Times.Once);
            Assert.AreEqual(HttpStatusCode.NotFound, (HttpStatusCode)regulatoryObject.StatusCode);
        }

        [Test]
        public async Task RegulatoryMaster_Controller_Should_CreateNew_RegulatoryData()
        {
            GeneralResponse<bool> objRegulatory = new GeneralResponse<bool>() { Data = true };
            var createModel = GetRegulatoryModel();
            regulatoryService.Setup(t => t.Create(createModel.Data)).ReturnsAsync(objRegulatory);
            regulatoryController = new RegulatoryMasterController(regulatoryService.Object);
            var regulatoryObject = await regulatoryController.PostRegulatory(createModel.Data);
            Assert.IsNotNull(regulatoryObject);
            Assert.AreEqual(((GeneralResponse<bool>)((JsonResult)regulatoryObject.Result).Value).Result, ResultType.Success); //return true
        }

        [Test]
        public async Task RegulatoryMaster_Controller_Should_Update_Regulatory_When_Data_Is_Valid()
        {
            int RegulatoryId = 1;
            GeneralResponse<bool> objRegulatory = new GeneralResponse<bool>() { Data = true };
            var objRegulatoryModel = GetRegulatoryModel();
            regulatoryService.Setup(t => t.Update(objRegulatoryModel.Data)).ReturnsAsync(objRegulatory);
            regulatoryController = new RegulatoryMasterController(regulatoryService.Object);
            var regulatoryObject = await regulatoryController.PutRegulatory(RegulatoryId, objRegulatoryModel.Data) as JsonResult;
            Assert.That(
            regulatoryObject.Value,
            Is.EqualTo(objRegulatory));
            Assert.IsNotNull(regulatoryObject);
        }

        [Test]
        [SetUICulture("en-us")]
        public void RegulatoryMaster_Controller_Should_Not_Update_Regulatory_When_Data_IsInvalid()
        {
            RegulatoryModel Data = GetRegulatoryModel().Data;
            Data.NutrientId = 0;
            Data.RegulatoryId = 0;
            var result = validator.TestValidate(Data, ruleSet: "New,Edit");
            result.ShouldHaveValidationErrorFor(x => x.NutrientId);
            result.ShouldHaveValidationErrorFor(x => x.RegulatoryId);
        }

        [Test]
        public async Task Regulatory_Controller_Should_Delete_RegulatoryData()
        {
            int RegulatoryId = 1;
            var response = new GeneralResponse<bool>();
            regulatoryService.Setup(t => t.Delete(RegulatoryId)).ReturnsAsync(response);
            regulatoryController = new RegulatoryMasterController(regulatoryService.Object);
            var regulatoryObject = await regulatoryController.DeleteRegulatory(RegulatoryId) as JsonResult;

            Assert.That(
            regulatoryObject.Value,
            Is.EqualTo(response));
            Assert.IsNotNull(regulatoryObject);
        }

        [Test]
        public async Task RegulatoryMaster_Controller_Should_Return_All_Active_UnitOfMeasurement_Data()
        {
            var response = GetUnitOfMeasurementMockData();
            regulatoryService.Setup(t => t.GetUnitOfMeasurement()).ReturnsAsync(response);
            regulatoryController = new RegulatoryMasterController(regulatoryService.Object);
            var regulatoryList = await regulatoryController.GetUnitOfMeasurement() as JsonResult;
            Assert.IsNotNull(regulatoryList);
            Assert.AreEqual(response.Data, ((GeneralResponse<ICollection<UnitOfMeasurementMaster>>)regulatoryList.Value).Data);
            Assert.AreEqual(response.Data.Count, ((GeneralResponse<ICollection<UnitOfMeasurementMaster>>)regulatoryList.Value).Data.Count);
            Assert.AreEqual(response.Result, ((GeneralResponse<ICollection<UnitOfMeasurementMaster>>)regulatoryList.Value).Result);
        }

        private PagedEntityResponse<RegulatoryModel> GetRegulatoryMockData()
        {
            var response = new PagedEntityResponse<RegulatoryModel>();
            List<RegulatoryModel> regulatoryMaster = new List<RegulatoryModel>();
            regulatoryMaster.Add(new RegulatoryModel()
            {
                RegulatoryId = 1,
                NutrientId = 2,
                OldUsa = 1000,
                CanadaNi = 5,
                CanadaNf = 100,
                NewUsRdi = 5,
                EU = 500,
                Unit = "mg",
                UnitPerMg = 100,
                IsActive = true,
                IsDeleted = false
            });
            regulatoryMaster.Add(new RegulatoryModel()
            {
                RegulatoryId = 2,
                NutrientId = 1,
                OldUsa = 1000,
                CanadaNi = 5,
                CanadaNf = 100,
                NewUsRdi = 5,
                EU = 500,
                Unit = "mg",
                UnitPerMg = 50,
                IsActive = true,
                IsDeleted = false
            });
            response.Data = regulatoryMaster;
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

        private GeneralResponse<ICollection<RegulatoryMaster>> GetGeneralRegulatoryMockData()
        {
            var response = new GeneralResponse<ICollection<RegulatoryMaster>>();
            List<RegulatoryMaster> regulatoryMaster = new List<RegulatoryMaster>();
            regulatoryMaster.Add(new RegulatoryMaster()
            {
                RegulatoryId = 1,
                NutrientId = 2,
                OldUsa = 1000,
                CanadaNi = 5,
                CanadaNf = 100,
                NewUsRdi = 5,
                EU = 500,
                UnitPerMg = 100,
                IsActive = true,
                IsDeleted = false,
                CreatedBy = 25412,
                CreatedOn = DateTime.Now,
                UpdatedBy = null,
                UpdatedOn = DateTime.Now
            });
            regulatoryMaster.Add(new RegulatoryMaster()
            {
                RegulatoryId = 2,
                NutrientId = 1,
                OldUsa = 1000,
                CanadaNi = 5,
                CanadaNf = 100,
                NewUsRdi = 5,
                EU = 500,
                UnitPerMg = 50,
                IsActive = true,
                IsDeleted = false,
                CreatedBy = 25412,
                CreatedOn = DateTime.Now,
                UpdatedBy = null,
                UpdatedOn = DateTime.Now
            });
            response.Data = regulatoryMaster;
            return response;
        }

        private GeneralResponse<RegulatoryModel> GetRegulatoryModel()
        {
            GeneralResponse<RegulatoryModel> objRegulatory = new GeneralResponse<RegulatoryModel>()
            {
                Data = new RegulatoryModel()
                {
                    RegulatoryId = 0,
                    NutrientId = 1,
                    OldUsa = 1000,
                    CanadaNi = 5,
                    CanadaNf = 100,
                    NewUsRdi = 5,
                    EU = 500,
                    Unit = "mg",
                    UnitPerMg = 100,
                    IsActive = true,
                    IsDeleted = false,
                }
            };
            return objRegulatory;
        }

        private GeneralResponse<RegulatoryMaster> GetRegulatoryMaster()
        {
            GeneralResponse<RegulatoryMaster> objRegulatory = new GeneralResponse<RegulatoryMaster>()
            {
                Data = new RegulatoryMaster()
                {
                    RegulatoryId = 50,
                    NutrientId = 1,
                    OldUsa = 1000,
                    CanadaNi = 5,
                    CanadaNf = 100,
                    NewUsRdi = 5,
                    EU = 500,
                    UnitPerMg = 100,
                    IsActive = true,
                    IsDeleted = false,
                }
            };
            return objRegulatory;
        }

        private GeneralResponse<ICollection<UnitOfMeasurementMaster>> GetUnitOfMeasurementMockData()
        {
            var response = new GeneralResponse<ICollection<UnitOfMeasurementMaster>>();
            response.Data = new List<UnitOfMeasurementMaster>();
            response.Data.Add(new UnitOfMeasurementMaster()
            {
                UnitOfMeasurementID = 1,
                MeasurementUnit = "g",
                IsActive = true,
                IsDeleted = false,
                CreatedBy = 1,
                CreatedOn = DateTime.Now,
                UpdatedBy = null,
                UpdatedOn = DateTime.Now
            });
            response.Data.Add(new UnitOfMeasurementMaster()
            {
                UnitOfMeasurementID = 2,
                MeasurementUnit = "kcal",
                IsActive = true,
                IsDeleted = false,
                CreatedBy = 1,
                CreatedOn = DateTime.Now,
                UpdatedBy = null,
                UpdatedOn = DateTime.Now
            });
            return response;
        }
    }
}