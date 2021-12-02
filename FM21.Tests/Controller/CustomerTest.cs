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
    public class CustomerTest
    {
        private Mock<ICustomerService> customerService;
        private CustomerController custController;
        private IStringLocalizer localizer;
        private SearchFilter searchFilter;
        Mock<ICustomerService> mydata;
        private CustomerValidator validator;

        [SetUp]
        public void SetUp()
        {
            localizer = new JsonStringLocalizer();
            customerService = new Mock<ICustomerService>();
            validator = new CustomerValidator(localizer);
            searchFilter = Mock.Of<SearchFilter>(m =>
                     m.PageSize == 10 &&
                     m.PageIndex == 1 &&
                     m.Search == "FM" &&
                     m.SortColumn == "" &&
                     m.SortDirection == "");
        }

        [Test]
        public async Task Customer_Controller_Should_Return_All_Active_CustomerData()
        {

            var response = GetGeneralCustomerMockData();
            mydata = new Mock<ICustomerService>();
            mydata.Setup(t => t.GetAll()).ReturnsAsync(response);

            custController = new CustomerController(mydata.Object);
            var customerList = await custController.GetAll() as JsonResult;
            Assert.IsNotNull(customerList);           
            Assert.AreEqual(response.Data, ((GeneralResponse<ICollection<Customer>>)customerList.Value).Data);
            Assert.AreEqual(response.Data.Count, ((GeneralResponse<ICollection<Customer>>)customerList.Value).Data.Count);
            Assert.AreEqual(response.Result, ((GeneralResponse<ICollection<Customer>>)customerList.Value).Result);


        }
        
        [Test]
        public async Task Customer_Controller_Should_Return_All_From_GetSearchList_With_Ok_Status()
        {
            var response = GetCustomerMockData();
            mydata = new Mock<ICustomerService>();
            mydata.Setup(t => t.GetPageWiseData(searchFilter.Search, searchFilter.PageIndex, searchFilter.PageSize, searchFilter.SortColumn, searchFilter.SortDirection)).ReturnsAsync(response);

            custController = new CustomerController(mydata.Object);
           
            var customerList = await custController.GetSearchList(searchFilter) as OkObjectResult;
            Assert.IsNotNull(customerList);
            Assert.AreEqual(HttpStatusCode.OK, (HttpStatusCode)customerList.StatusCode);
            Assert.AreEqual(response.Data, ((PagedEntityResponse<Customer>)customerList.Value).Data);
            Assert.AreEqual(response.Data.Count, ((PagedEntityResponse<Customer>)customerList.Value).Data.Count);
            Assert.AreEqual(response.Result, ((PagedEntityResponse<Customer>)customerList.Value).Result);
        }

        [Test]
        public async Task Customer_Controller_Should_Return_Null_From_GetSearchList_When_SearchFilter_Model_IsInvalid()
        {
            searchFilter = new SearchFilter();// This will cause modelInvalid
            custController = new CustomerController(customerService.Object);
            var context = new ValidationContext(searchFilter, null, null);
            var results = new List<ValidationResult>();
            if (!Validator.TryValidateObject(searchFilter, context, results, true))
            {
                custController.ModelState.Clear();
                foreach (ValidationResult result in results)
                {
                    var key = result.MemberNames.FirstOrDefault() ?? "";
                    custController.ModelState.AddModelError(key, result.ErrorMessage);
                }
            }
            var customerList = await custController.GetSearchList(searchFilter) as JsonResult;
            Assert.IsNull(customerList);
        }

        [Test]
        public async Task Customer_Controller_Should_Return_Customer_By_Valid_CustomerId()
        {
            GeneralResponse<Customer> objCustomer = new GeneralResponse<Customer>()
            {
                Data = new Customer()
                {
                    CustomerId = 2,
                    Address = "FM Nessloson",
                    PhoneNumber = "12345678",
                    Email = "FM@gmail.com"
                }
            };

            mydata = new Mock<ICustomerService>();
            mydata.Setup(t => t.Get(objCustomer.Data.CustomerId)).ReturnsAsync(objCustomer);

            custController = new CustomerController(mydata.Object);

            var customerList = await custController.GetCustomer(objCustomer.Data.CustomerId) as OkObjectResult;
            var actualResult = customerList.Value;

            mydata.Verify(c => c.Get(It.IsAny<int>()), Times.Once);
            Assert.AreEqual(objCustomer.Data.Address, ((GeneralResponse<Customer>)actualResult).Data.Address);
            Assert.AreEqual(objCustomer.Data.PhoneNumber, ((GeneralResponse<Customer>)actualResult).Data.PhoneNumber);
            Assert.AreEqual(objCustomer.Data.Email, ((GeneralResponse<Customer>)actualResult).Data.Email);
            Assert.AreEqual(HttpStatusCode.OK, (HttpStatusCode)customerList.StatusCode);

        }

        [Test]
        public async Task Customer_Controller_Should_Return_Null_When_InValid_CustomerId()
        {
            GeneralResponse<Customer> objCustomer = new GeneralResponse<Customer>()
            {
                Data = new Customer()
                {
                    CustomerId = 1520,
                    Address = "FM Nessloson",
                    PhoneNumber = "12345678",
                    Email = "FM@gmail.com"
                }
            };

            mydata = new Mock<ICustomerService>();
            mydata.Setup(t => t.Get(objCustomer.Data.CustomerId)).ReturnsAsync(objCustomer);

            custController = new CustomerController(mydata.Object);

            var customerObject = await custController.GetCustomer(99) as NotFoundResult;

            mydata.Verify(c => c.Get(It.IsAny<int>()), Times.Once);
            Assert.IsNull(customerObject);
        }

        [Test]
        public async Task Customer_Controller_Should_CreateNew_Customer()
        {
            GeneralResponse<bool> objCustomer = new GeneralResponse<bool>()
            {
                Data = true
            };
            GeneralResponse<CustomerModel> objCustomer1 = new GeneralResponse<CustomerModel>()
            {
                Data = new CustomerModel()
                {

                    Address = "FM Nessloson",
                    PhoneNumber = "12345678",
                    Email = "FM@gmail.com"
                }
            };
            mydata = new Mock<ICustomerService>();
            mydata.Setup(t => t.Create(objCustomer1.Data)).ReturnsAsync(objCustomer);
            custController = new CustomerController(mydata.Object);
            var customerObject = await custController.PostCustomer(objCustomer1.Data);
            Assert.AreEqual(((GeneralResponse<bool>)((JsonResult)customerObject.Result).Value).Result , ResultType.Success); //return true
            Assert.IsNotNull(customerObject);
        }

        [Test]
        public async Task Customer_Controller_Should_Update_Customer_When_Data_Is_Valid()
        {
            int CustomerId = 1;
            GeneralResponse<bool> objCustomer = new GeneralResponse<bool>(){Data = true };
            GeneralResponse<CustomerModel> objCustomer1 = new GeneralResponse<CustomerModel>()
            {
                Data = new CustomerModel()
                {

                    Address = "FM Nessloson",
                    PhoneNumber = "12345678",
                    Email = "FM@gmail.com"
                }
            };
            mydata = new Mock<ICustomerService>();
            mydata.Setup(t => t.Update(objCustomer1.Data)).ReturnsAsync(objCustomer);

            custController = new CustomerController(mydata.Object);
            var customerObject = await custController.PutCustomer(CustomerId, objCustomer1.Data) as JsonResult;
            Assert.That(
            customerObject.Value,
            Is.EqualTo(objCustomer));
            Assert.IsNotNull(customerObject);
        }

        [Test]
        [SetUICulture("en-us")]
        public void Customer_Controller_Should_Not_Update_Customer_When_Data_IsInvalid()
        {
            CustomerModel objRoleMaster = new CustomerModel() { Address = "FM Nessloson"};
            var result = validator.TestValidate(objRoleMaster, "Edit,New");
            result.ShouldHaveValidationErrorFor(x => x.CustomerId);
            result.ShouldHaveValidationErrorFor(x => x.Name);
            result.ShouldHaveValidationErrorFor(x => x.PhoneNumber);
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Test]
        public async Task Customer_Controller_Should_Delete_CustomerData()
        {
            int CustomerId = 1;
            var response = new GeneralResponse<bool>();

            mydata = new Mock<ICustomerService>();
            mydata.Setup(t => t.Delete(CustomerId)).ReturnsAsync(response);

            custController = new CustomerController(mydata.Object);

            var customerObject = await custController.DeleteCustomer(CustomerId) as JsonResult;

            Assert.That(
            customerObject.Value,
            Is.EqualTo(response));
            Assert.IsNotNull(customerObject);
        }

        private PagedEntityResponse<Customer> GetCustomerMockData()
        {
            var response = new PagedEntityResponse<Customer>();
            List<Customer> customerPermissionMatrix = new List<Customer>();
            customerPermissionMatrix.Add(new Customer()
            {
                CustomerId = 2,
                Name = "FM Nessloson 1",
                Email = "FM@gmail.com",
                Address = "address",
                PhoneNumber = "12345678",
                IsActive = true,
                IsDeleted=false,
                CreatedBy = 25412,
                CreatedOn = DateTime.Now,
                UpdatedBy = null,
                UpdatedOn = DateTime.Now,
                CustomerAbbreviation1 = null,
                CustomerAbbreviation2 = null
            });
            customerPermissionMatrix.Add(new Customer()
            {
                CustomerId = 2,
                Name = "FM Nessloson 3",
                Email = "FM@gmail.com",
                Address = "address",
                PhoneNumber = "12345678",
                IsActive = true,
                IsDeleted = false,
                CreatedBy = 25412,
                CreatedOn = DateTime.Now,
                UpdatedBy = null,
                UpdatedOn = DateTime.Now,
                CustomerAbbreviation1 = "CustomerAbbreviation1 Value",
                CustomerAbbreviation2 = "CustomerAbbreviation1 Value"
            });
            response.Data = customerPermissionMatrix;
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

        private GeneralResponse<ICollection<Customer>> GetGeneralCustomerMockData()
        {
            var response = new GeneralResponse<ICollection<Customer>>();
            List<Customer> customerPermissionMatrix = new List<Customer>();
            customerPermissionMatrix.Add(new Customer()
            {
                CustomerId = 2,
                Name = "FM Nessloson 1",
                Email = "FM@gmail.com",
                Address = "address",
                PhoneNumber = "12345678",
                IsActive = true,
                IsDeleted = false,
                CreatedBy = 25412,
                CreatedOn = DateTime.Now,
                UpdatedBy = null,
                UpdatedOn = DateTime.Now,
                CustomerAbbreviation1 = null,
                CustomerAbbreviation2 = null
            });
            customerPermissionMatrix.Add(new Customer()
            {
                CustomerId = 2,
                Name = "FM Nessloson 3",
                Email = "FM@gmail.com",
                Address = "address",
                PhoneNumber = "12345678",
                IsActive = true,
                IsDeleted = false,
                CreatedBy = 25412,
                CreatedOn = DateTime.Now,
                UpdatedBy = null,
                UpdatedOn = DateTime.Now,
                CustomerAbbreviation1 = "CustomerAbbreviation1 Value",
                CustomerAbbreviation2 = "CustomerAbbreviation1 Value"
            });
            response.Data = customerPermissionMatrix;          

            return response;
        }
    }
}