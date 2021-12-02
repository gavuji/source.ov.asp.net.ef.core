using FM21.API.Controllers;
using FM21.Core;
using FM21.Core.Model;
using FM21.Entities;
using FM21.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FM21.Tests
{
    [TestFixture]
    public class SupplierMasterTest
    {
        private SupplierMasterController supplierMasterController;
        private SearchFilter searchFilter;
        Mock<ISupplierMasterService> supplierDataService;

        [SetUp]
        public void SetUp()
        {
            supplierDataService = new Mock<ISupplierMasterService>();
            searchFilter = Mock.Of<SearchFilter>(m =>
                     m.PageSize == 10 &&
                     m.PageIndex == 1);
        }

        [Test]
        public void Test_Get_AllSupplier()
        {

            supplierMasterController = new SupplierMasterController(supplierDataService.Object);
            var supplierList = supplierMasterController.GetAll();
            Assert.IsNotNull(supplierList);
            Assert.That(supplierList, Is.TypeOf<System.Threading.Tasks.Task<IActionResult>>()); //Tests exact type

        }

        [Test]
        public async Task Test_GetSearchList_OkResult()
        {
            var response = getSupplierMockData();
            supplierDataService.Setup(t => t.GetPageWiseData(searchFilter.Search, searchFilter.PageIndex, searchFilter.PageSize, searchFilter.SortColumn, searchFilter.SortDirection)).ReturnsAsync(response);

            supplierMasterController = new SupplierMasterController(supplierDataService.Object);
            var supplierList = await supplierMasterController.GetSearchList(searchFilter) as OkObjectResult;
            Assert.IsNotNull(supplierList);
            Assert.AreEqual(response.Data, ((PagedEntityResponse<SupplierMaster>)supplierList.Value).Data);
            Assert.AreEqual(response.Data.Count, ((PagedEntityResponse<SupplierMaster>)supplierList.Value).Data.Count);
            Assert.AreEqual(response.Result, ((PagedEntityResponse<SupplierMaster>)supplierList.Value).Result);
        }

        [Test]
        public async Task Test_GetSearchList_BadRequestResult()
        {
            searchFilter = new SearchFilter();// This will cause modelInvalid
            supplierMasterController = new SupplierMasterController(supplierDataService.Object);

            var context = new ValidationContext(searchFilter, null, null);
            var results = new List<ValidationResult>();
            if (!Validator.TryValidateObject(searchFilter, context, results, true))
            {
                supplierMasterController.ModelState.Clear();
                foreach (ValidationResult result in results)
                {
                    var key = result.MemberNames.FirstOrDefault() ?? "";
                    supplierMasterController.ModelState.AddModelError(key, result.ErrorMessage);
                }
            }

            var response = await supplierMasterController.GetSearchList(searchFilter) as OkObjectResult;
            Assert.IsNull(response);
        }

        [Test]
        public async Task Test_GetSupplierAsync_With_ValidSupplierId()
        {
            GeneralResponse<SupplierMaster> objSupplier = new GeneralResponse<SupplierMaster>()
            {
                Data = new SupplierMaster()
                {
                    SupplierID = 2,
                    SupplierName = "Santosh",
                    Address = "Ahmedabad",
                    PhoneNumber = "12345678",
                    Email = "FM@gmail.com",
                    SupplierAbbreviation1 = "This is Abbreviation 1",
                    SupplierAbbreviation2 = "This is Abbreviation 2"
                }
            };

            
            supplierDataService.Setup(t => t.Get(objSupplier.Data.SupplierID)).ReturnsAsync(objSupplier);

            supplierMasterController = new SupplierMasterController(supplierDataService.Object);

            var supplierList = await supplierMasterController.GetSupplier(objSupplier.Data.SupplierID) as OkObjectResult;
            var actualResult = supplierList.Value;

            supplierDataService.Verify(c => c.Get(It.IsAny<int>()), Times.Once);
            Assert.AreEqual(objSupplier.Data.SupplierID, ((GeneralResponse<SupplierMaster>)actualResult).Data.SupplierID);
            Assert.AreEqual(objSupplier.Data.SupplierName, ((GeneralResponse<SupplierMaster>)actualResult).Data.SupplierName);
            Assert.AreEqual(objSupplier.Data.SupplierAbbreviation1, ((GeneralResponse<SupplierMaster>)actualResult).Data.SupplierAbbreviation1);
            Assert.AreEqual(objSupplier.Data.SupplierAbbreviation2, ((GeneralResponse<SupplierMaster>)actualResult).Data.SupplierAbbreviation2);
            Assert.AreEqual(objSupplier.Data.Address, ((GeneralResponse<SupplierMaster>)actualResult).Data.Address);
            Assert.AreEqual(objSupplier.Data.PhoneNumber, ((GeneralResponse<SupplierMaster>)actualResult).Data.PhoneNumber);
            Assert.AreEqual(objSupplier.Data.Email, ((GeneralResponse<SupplierMaster>)actualResult).Data.Email);
            Assert.AreEqual(HttpStatusCode.OK, (HttpStatusCode)supplierList.StatusCode);
        }

        [Test]
        public async Task Test_GetSupplierAsync_With_InValidSupplierId()
        {
            GeneralResponse<SupplierMaster> objSupplier = new GeneralResponse<SupplierMaster>()
            {
                Data = new SupplierMaster()
                {
                    SupplierID = 1212,
                    SupplierName = "Santosh",
                    Address = "Ahmedabad",
                    PhoneNumber = "12345678",
                    Email = "FM@gmail.com",
                    SupplierAbbreviation1 = "This is Abbreviation 1",
                    SupplierAbbreviation2 = "This is Abbreviation 2"
                }
            };

            supplierDataService.Setup(t => t.Get(objSupplier.Data.SupplierID)).ReturnsAsync(objSupplier);

            supplierMasterController = new SupplierMasterController(supplierDataService.Object);

            var customerObject = await supplierMasterController.GetSupplier(99) as NotFoundResult;

            supplierDataService.Verify(c => c.Get(It.IsAny<int>()), Times.Once);
            Assert.AreEqual(customerObject.StatusCode, HttpStatusCode.NotFound.GetHashCode());
        }

        [Test]
        public async Task Test_CreateSupplierAsync_With_ValidData()
        {
            GeneralResponse<bool> objSupplier1 = new GeneralResponse<bool>()
            {
                Data =true
            };

            GeneralResponse<SupplierMasterModel> objSupplier2 = new GeneralResponse<SupplierMasterModel>()
            {
                Data = new SupplierMasterModel()
                {

                    SupplierName = "Ameet",
                    Address = "Ahmedabad",
                    PhoneNumber = "4455454",
                    Email = "a@gmail.com",
                    SupplierAbbreviation1 = "This is Abbreviation 1",
                    SupplierAbbreviation2 = "This is Abbreviation 2"
                }
            };


            supplierDataService.Setup(t => t.Create(objSupplier2.Data)).ReturnsAsync(objSupplier1);

            supplierMasterController = new SupplierMasterController(supplierDataService.Object);

            var supplierObject = await supplierMasterController.PostSupplier(objSupplier2.Data) ;
            Assert.AreEqual(((GeneralResponse<bool>)((JsonResult)supplierObject.Result).Value).Result, ResultType.Success); //return true
            Assert.IsNotNull(supplierObject);
        }

        [Test]
        public async Task Test_DeleteSupplier_OkResult()
        {
            GeneralResponse<bool> response = new GeneralResponse<bool>();
            supplierDataService.Setup(t => t.Delete(1)).ReturnsAsync(response);

            supplierMasterController = new SupplierMasterController(supplierDataService.Object);

            var result = await supplierMasterController.DeleteSupplier(1) as JsonResult;
            Assert.IsNotNull(result.Value);


        }

        [Test]
        public async Task Test_DeleteSupplier_NotFoundResult()
        {
            GeneralResponse<bool> response = new GeneralResponse<bool>();
            supplierDataService.Setup(t => t.Delete(100)).ReturnsAsync(response);

            supplierMasterController = new SupplierMasterController(supplierDataService.Object);

            var result = await supplierMasterController.DeleteSupplier(1) as JsonResult;
            Assert.IsNull(result.Value);


        }

        [Test]
        public async Task Test_UpdateSupplierAsync_With_ValidData()
        {
            int SupplierId = 1;
            GeneralResponse<bool> objSupplier1 = new GeneralResponse<bool>()
            {
                Data =true
            };

            GeneralResponse<SupplierMasterModel> objSupplier2 = new GeneralResponse<SupplierMasterModel>()
            {
                Data = new SupplierMasterModel()
                {

                    SupplierName = "Ameet",
                    Address = "Ahmedabad",
                    PhoneNumber = "4455454",
                    Email = "a@gmail.com",
                    SupplierAbbreviation1 = "This is Abbreviation 1",
                    SupplierAbbreviation2 = "This is Abbreviation 2"
                }
            };
            supplierDataService.Setup(t => t.Update(objSupplier2.Data)).ReturnsAsync(objSupplier1);

            supplierMasterController = new SupplierMasterController(supplierDataService.Object);

            var supplierObject = await supplierMasterController.PutSupplier(SupplierId, objSupplier2.Data) as JsonResult;

            Assert.That(
            supplierObject.Value,
            Is.EqualTo(objSupplier1));
            Assert.IsNotNull(supplierObject);
        }

        private PagedEntityResponse<SupplierMaster> getSupplierMockData()
        {
            var response = new PagedEntityResponse<SupplierMaster>();
            List<SupplierMaster> supplierMasters = new List<SupplierMaster>();
            supplierMasters.Add(new SupplierMaster()
            {
                SupplierID = 1,
                SupplierName = "Santosh",
                Address = "Ahmedabad",
                PhoneNumber = "12345678",
                Email = "FM@gmail.com",
                SupplierAbbreviation1 = "This is Abbreviation 1",
                SupplierAbbreviation2 = "This is Abbreviation 2",
                IsActive = true,
                IsDeleted = false
            });
            supplierMasters.Add(new SupplierMaster()
            {
                SupplierID = 2,
                SupplierName = "Santosh",
                Address = "Ahmedabad",
                PhoneNumber = "12345678",
                Email = "FM@gmail.com",
                SupplierAbbreviation1 = "This is Abbreviation 1",
                SupplierAbbreviation2 = "This is Abbreviation 2",
                IsActive = true,
                IsDeleted = false

            });
            response.Data = supplierMasters;
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

    }
}
