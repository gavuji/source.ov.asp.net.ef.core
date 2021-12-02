using FM21.API.Controllers;
using FM21.Core;
using FM21.Core.Model;
using FM21.Entities;
using FM21.Service.Interface;
using Microsoft.AspNetCore.Mvc;
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
    public class AllergenMasterTest
    {
        private AllergenMasterController allergenMasterController;
        private Mock<IAllergenMasterService> allergenDataService;
        private SearchFilter searchFilter;

        [SetUp]
        public void SetUp()
        {
            allergenDataService = new Mock<IAllergenMasterService>();
            searchFilter = Mock.Of<SearchFilter>(m =>
                     m.PageSize == 10 &&
                     m.PageIndex == 1);
        }

        [Test]
        public void Test_Get_AllAllergen()
        {

            allergenMasterController = new AllergenMasterController(allergenDataService.Object);
            var allergenList = allergenMasterController.GetAll();
            Assert.IsNotNull(allergenList);
            Assert.That(allergenList, Is.TypeOf<System.Threading.Tasks.Task<IActionResult>>()); //Tests exact type

        }
        [Test]
        public void Test_And_Pass_Value_To_Alert_Master_Entities()
        {

            AllergenMaster Data = new AllergenMaster()
            {
                AllergenID = 1,
                AllergenName = "Shrimp",
                AllergenCode = "L",
                AllergenDescription_En = "Crustacean",
                AllergenDescription_Fr = "Crustacés",
                CreatedBy = 1,
                CreatedOn = DateTime.Now,
                AllergenDescription_Es = "Spanish Desc",
                IsActive = true,
                IsCANADAAllergen = true,
                IsDeleted = false,
                IsUSAAllergen = true,
                UpdatedBy = 8888,
                UpdatedOn = DateTime.Now
            };
            Assert.IsNotNull(Data);
        }

        [Test]
        public async Task Test_GetSearchList_OkResult()
        {
            var response = getAllergenMasterMockData();
            allergenDataService = new Mock<IAllergenMasterService>();

            allergenDataService.Setup(t => t.GetPageWiseData(searchFilter.Search, searchFilter.PageIndex, searchFilter.PageSize, searchFilter.SortColumn, searchFilter.SortDirection)).ReturnsAsync(response);

            allergenMasterController = new AllergenMasterController(allergenDataService.Object);
            var allergenList = await allergenMasterController.GetSearchList(searchFilter) as JsonResult;
            Assert.IsNotNull(allergenList);
            Assert.AreEqual(response.Data, ((PagedEntityResponse<AllergenMaster>)allergenList.Value).Data);
            Assert.AreEqual(response.Data.Count, ((PagedEntityResponse<AllergenMaster>)allergenList.Value).Data.Count);
            Assert.AreEqual(response.Result, ((PagedEntityResponse<AllergenMaster>)allergenList.Value).Result);
        }

        [Test]
        public async Task Test_GetSearchList_BadRequestResult()
        {
            searchFilter = new SearchFilter();// This will cause modelInvalid
            allergenMasterController = new AllergenMasterController(allergenDataService.Object);

            var context = new ValidationContext(searchFilter, null, null);
            var results = new List<ValidationResult>();
            if (!Validator.TryValidateObject(searchFilter, context, results, true))
            {
                allergenMasterController.ModelState.Clear();
                foreach (ValidationResult result in results)
                {
                    var key = result.MemberNames.FirstOrDefault() ?? "";
                    allergenMasterController.ModelState.AddModelError(key, result.ErrorMessage);
                }
            }

            var response = await allergenMasterController.GetSearchList(searchFilter) as JsonResult;
            Assert.IsNull(response);
        }

        [Test]
        public async Task Test_GetAllergenAsync_With_ValidAllergenId()
        {
            GeneralResponse<AllergenMaster> objAllergen = new GeneralResponse<AllergenMaster>()
            {
                Data = GetAllergenMasterData()
            };

            allergenDataService.Setup(t => t.Get(objAllergen.Data.AllergenID)).ReturnsAsync(objAllergen);

            allergenMasterController = new AllergenMasterController(allergenDataService.Object);

            var allergenList = await allergenMasterController.GetAllergen(objAllergen.Data.AllergenID) as OkObjectResult;
            var actualResult = allergenList.Value;

            allergenDataService.Verify(c => c.Get(It.IsAny<int>()), Times.Once);
            Assert.AreEqual(objAllergen.Data.AllergenID, ((GeneralResponse<AllergenMaster>)actualResult).Data.AllergenID);
            Assert.AreEqual(objAllergen.Data.AllergenName, ((GeneralResponse<AllergenMaster>)actualResult).Data.AllergenName);
            Assert.AreEqual(objAllergen.Data.AllergenCode, ((GeneralResponse<AllergenMaster>)actualResult).Data.AllergenCode);
            Assert.AreEqual(objAllergen.Data.AllergenDescription_En, ((GeneralResponse<AllergenMaster>)actualResult).Data.AllergenDescription_En);
            Assert.AreEqual(objAllergen.Data.AllergenDescription_Fr, ((GeneralResponse<AllergenMaster>)actualResult).Data.AllergenDescription_Fr);
            Assert.AreEqual(HttpStatusCode.OK, (HttpStatusCode)allergenList.StatusCode);

        }

        [Test]
        public async Task Test_GetAllergenAsync_With_InValidAllergenId()
        {
            GeneralResponse<AllergenMaster> objAllergen = new GeneralResponse<AllergenMaster>()
            {
                Data = new AllergenMaster()
                {
                    AllergenID = 1220,
                    AllergenName = "Shrimp",
                    AllergenCode = "L",
                    AllergenDescription_En = "Crustacean",
                    AllergenDescription_Fr = "Crustacés"
                }
            };
            allergenDataService.Setup(t => t.Get(objAllergen.Data.AllergenID)).ReturnsAsync(objAllergen);

            allergenMasterController = new AllergenMasterController(allergenDataService.Object);

            var allergenObject = await allergenMasterController.GetAllergen(99) as NotFoundResult;

            allergenDataService.Verify(c => c.Get(It.IsAny<int>()), Times.Once);
            Assert.IsNull(allergenObject);
        }

        [Test]
        public async Task Test_CreateAllergenAsync_With_ValidData()
        {
            GeneralResponse<bool> objAllergen = new GeneralResponse<bool>()
            {
                Data = true
            };

            GeneralResponse<AllergenMasterModel> objAllergenModel = new GeneralResponse<AllergenMasterModel>()
            {
                Data = new AllergenMasterModel()
                {
                    AllergenName = "Shrimp",
                    AllergenCode = "L",
                    AllergenDescription_En = "Crustacean",
                    AllergenDescription_Fr = "Crustacés"
                }
            };



            allergenDataService.Setup(t => t.Create(objAllergenModel.Data)).ReturnsAsync(objAllergen);

            allergenMasterController = new AllergenMasterController(allergenDataService.Object);

            var allergenObject = await allergenMasterController.PostAllergen(objAllergenModel.Data);
            Assert.AreEqual(((GeneralResponse<bool>)((JsonResult)allergenObject.Result).Value).Result, ResultType.Success); //return true
            Assert.IsNotNull(allergenObject);
        }

        [Test]
        public async Task Test_CreateAllergenAsync_With_InValidData()
        {
            GeneralResponse<bool> objAllergen = new GeneralResponse<bool>()
            {
                Data = false
            };

            GeneralResponse<AllergenMasterModel> objAllergenModel = new GeneralResponse<AllergenMasterModel>()
            {
                Data = new AllergenMasterModel()
                {
                    AllergenName = "Shrimp",
                    AllergenCode = "L",
                    AllergenDescription_En = "Crustacean",
                    AllergenDescription_Fr = "Crustacés"
                }
            };

            allergenDataService.Setup(t => t.Create(objAllergenModel.Data)).ReturnsAsync(objAllergen);

            allergenMasterController = new AllergenMasterController(allergenDataService.Object);

            var allergenObject = await allergenMasterController.PostAllergen(objAllergenModel.Data);

            Assert.IsFalse(allergenObject.Value);
        }
        [Test]
        public async Task Test_UpdateAllergenAsync_With_ValidData()
        {
            int AllergenID = 10;
            GeneralResponse<bool> objAllergen = new GeneralResponse<bool>()
            {
                Data = true
            };

            GeneralResponse<AllergenMasterModel> objAllergenModel = new GeneralResponse<AllergenMasterModel>()
            {
                Data = new AllergenMasterModel()
                {
                    AllergenID = 1,
                    AllergenName = "Shrimp",
                    AllergenCode = "L",
                    AllergenDescription_En = "Crustacean",
                    AllergenDescription_Fr = "Crustacés"
                }
            };
            allergenDataService.Setup(t => t.Update(objAllergenModel.Data)).ReturnsAsync(objAllergen);

            allergenMasterController = new AllergenMasterController(allergenDataService.Object);

            var allergenObject = await allergenMasterController.PutAllergen(AllergenID, objAllergenModel.Data) as JsonResult;

            Assert.That(
            allergenObject.Value,
            Is.EqualTo(objAllergen));
            Assert.IsNotNull(allergenObject);
        }

        [Test]
        public async Task Test_UpdateAllergenAsync_With_InValidData()
        {
            int AllergenID = 1;
            GeneralResponse<AllergenMaster> objAllergen = new GeneralResponse<AllergenMaster>()
            {
                Data = new AllergenMaster()
                {
                    AllergenID = 1,
                    AllergenName = "Shrimp",
                    AllergenCode = "LDDDDDDDDDDDDDDDDDDDDDD",
                    AllergenDescription_En = "Crustacean",
                    AllergenDescription_Fr = "Crustacés"
                }
            };

            GeneralResponse<AllergenMasterModel> objAllergenModel = new GeneralResponse<AllergenMasterModel>()
            {
                Data = new AllergenMasterModel()
                {
                    AllergenID = 1,
                    AllergenName = "Shrimp",
                    AllergenCode = "LDDDDDDDDDDDDDDDDDDDDDD",
                    AllergenDescription_En = "Crustacean",
                    AllergenDescription_Fr = "Crustacés"
                }
            };


            allergenDataService.Setup(t => t.Get(objAllergen.Data.AllergenID)).ReturnsAsync(objAllergen);

            allergenMasterController = new AllergenMasterController(allergenDataService.Object);

            var allergenObject = await allergenMasterController.PutAllergen(AllergenID, objAllergenModel.Data) as JsonResult;

            Assert.IsNull(allergenObject.Value);
        }
        [Test]
        public async Task Test_DeleteAllergen_OkResult()
        {
            GeneralResponse<bool> response = new GeneralResponse<bool>();


            allergenDataService.Setup(t => t.Delete(1)).ReturnsAsync(response);

            allergenMasterController = new AllergenMasterController(allergenDataService.Object);

            var result = await allergenMasterController.DeleteAllergen(1) as JsonResult;
            Assert.IsNotNull(result.Value);


        }

        [Test]
        public async Task Test_DeleteSupplier_NotFoundResult()
        {
            GeneralResponse<bool> response = new GeneralResponse<bool>();


            allergenDataService.Setup(t => t.Delete(100)).ReturnsAsync(response);

            allergenMasterController = new AllergenMasterController(allergenDataService.Object);

            var result = await allergenMasterController.DeleteAllergen(10) as JsonResult;
            Assert.IsNull(result.Value);
        }

        private PagedEntityResponse<AllergenMaster> getAllergenMasterMockData()
        {
            var response = new PagedEntityResponse<AllergenMaster>();
            List<AllergenMaster> allergenMasterModels = new List<AllergenMaster>();
            allergenMasterModels.Add(new AllergenMaster()
            {
                AllergenCode = "Code",
                AllergenID = 1,
                AllergenName = "se",
                AllergenDescription_En = "English Desc",
                AllergenDescription_Fr = "French Desc",
                AllergenDescription_Es = "Spanish Desc",
                IsActive = true,
                IsDeleted = false
            });
            allergenMasterModels.Add(new AllergenMaster()
            {
                AllergenCode = "L",
                AllergenID = 2,
                AllergenName = "Crab",
                AllergenDescription_En = "English Desc",
                AllergenDescription_Fr = "French Desc",
                AllergenDescription_Es = "Spanish Desc",
                IsActive = true,
                IsDeleted = false

            });
            response.Data = allergenMasterModels;
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
        private AllergenMaster GetAllergenMasterData()
        {

            AllergenMaster Data = new AllergenMaster()
            {
                AllergenID = 1,
                AllergenName = "Shrimp",
                AllergenCode = "L",
                AllergenDescription_En = "Crustacean",
                AllergenDescription_Fr = "Crustacés",
                CreatedBy = 1,
                CreatedOn = DateTime.Now,
                AllergenDescription_Es = "Spanish Desc",
                IsActive = true,
                IsCANADAAllergen = true,
                IsDeleted = false,
                IsUSAAllergen = true,
                UpdatedBy = 8888,
                UpdatedOn = DateTime.Now
            };
            return Data;
        }
    }
}
