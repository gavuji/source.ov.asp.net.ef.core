using FM21.Core;
using FM21.Core.Localization;
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
    public class CustomerMasterServiceTest : TestBase
    {
        private Mock<IRepository<Customer>> customerMasterRepository;
        private ICustomerService customerMasterService;
        List<Customer> customerMaster;

        [SetUp]
        public void SetUp()
        {
            customerMaster = GetMockDataForCreatecustomerData();
            customerMasterRepository = SetupCustomerRepository();
            localizer = new JsonStringLocalizer();
            customerMasterService = new CustomerService(serviceProvider.Object, exceptionHandler.Object, mapper, unitOfWork.Object, null, customerMasterRepository.Object);
        }

        [Test]
        public async Task Service_Should_Return_All_Active_CustomerData()
        {
            var customer = await customerMasterService.GetAll();
            Assert.That(customer.Result, Is.EqualTo(ResultType.Success));
            Assert.Greater(customer.Data.Count, 0);
        }
        [Test]
        public async Task Service_Should_Not_Return_Data_When_Exception()
        {
            customerMasterRepository.Setup(r => r.GetManyAsync(o => o.IsActive == true && o.IsDeleted == false, true)).Throws(new Exception("something went wrong"));
            var customer = await customerMasterService.GetAll();
            Assert.That(customer.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_Return_All_Active_CustomerData_By_PagingResult()
        {
            var response = GetcustomerMasterPagedMockData();
            string filterText = GetCustomerMockData().FirstOrDefault().PhoneNumber;
            var customer = await customerMasterService.GetPageWiseData(filter: filterText, pageIndex: 1, pageSize: 10, sortColumn: null, sortDirection: null);
            Assert.That(customer.Result, Is.EqualTo(response.Result));
            Assert.Greater(customer.Data.Count(), 0);
        }

        [Test]
        public async Task Service_Should_Not_Return_All_Active_CustomerData_When_Exception()
        {
            customerMasterRepository.Setup(r => r.Query(true)).Throws(new Exception("something went wrong"));
            var customer = await customerMasterService.GetPageWiseData(filter: null, pageIndex: 1, pageSize: 10, sortColumn: null, sortDirection: null);
            Assert.That(customer.Result, Is.EqualTo(ResultType.Error));
        }

        [TestCase("name")]
        [TestCase("address")]
        [TestCase("email")]
        [TestCase("customerabbreviation1")]
        [TestCase("customerabbreviation2")]
        [TestCase("phonenumber")]
        [TestCase("createdby")]
        [TestCase("")] // the cover default sort by CustomerId
        public async Task Service_Should_Return_All_Active_CustomerData_By_PagingResult_SortColumn(string sortColumn)
        {
            var response = GetcustomerMasterPagedMockData();
            var customer = await customerMasterService.GetPageWiseData(filter: null, pageIndex: 1, pageSize: 10, sortColumn: sortColumn, sortDirection: null);
            Assert.That(customer.Result, Is.EqualTo(response.Result));
        }
        [Test]
        public async Task Service_Should_Return_Customer_By_Valid_CustomerId()
        {
            var response = GetSinglecustomerObjectMockData();
            var customer = await customerMasterService.Get(1);
            Assert.That(customer.Result, Is.EqualTo(response.Result));
            Assert.AreEqual(customer.Data.CustomerId, response.Data.CustomerId);
        }
        [Test]
        public async Task Service_Should_Not_Return_Customer_When_Exception()
        {
            customerMasterRepository.Setup(r => r.GetByIdAsync(1)).Throws(new Exception("something went wrong"));
            var customer = await customerMasterService.Get(1);
            Assert.That(customer.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_Return_RecordNotExist_On_Invalid_CustomerId()
        {
            var response = await customerMasterService.Get(5);
            Assert.IsNull(response.Data);
            Assert.AreEqual(response.Message, localizer["msgRecordNotExist", new string[] { "Customer" }].Value);
        }

        [Test]
        public async Task Service_Should_CreateNew_CustomerData()
        {
            CustomerModel customerMasterModel = GetSignleObject();

            int _maxRegIDBeforeAdd = customerMaster.Max(a => a.CustomerId);
            var response = await customerMasterService.Create(customerMasterModel);

            Assert.That(_maxRegIDBeforeAdd + 1, Is.EqualTo(customerMaster.Last().CustomerId));
            Assert.AreEqual(response.Result, response.Result);
        }

        [Test]
        public async Task Service_Should_NotCreate_When_CustomerData_InValid()
        {
            CustomerModel customerMasterModel = GetSignleObject();
            customerMasterModel.Name = "";

            var response = await customerMasterService.Create(customerMasterModel);

            Assert.That(response.Result, Is.EqualTo(ResultType.Warning));
            Assert.Greater(response.ExtraData.Count, 0);
        }

        [TestCase("name")]
        [TestCase("phonenumber")]
        [TestCase("email")]
        public async Task Service_Should_NotCreate_New_Customer_When_CustomerName_AlreadyExist(string testParams)
        {
            CustomerModel customerMasterModel = GetCustomerModel(testParams);
            customerMasterRepository.Setup(r => r.Any(It.IsAny<Expression<Func<Customer, bool>>>())).Returns(true);
            var customer = await customerMasterService.Create(customerMasterModel);

            Assert.That(customer.Result, Is.EqualTo(ResultType.Warning));
            Assert.AreEqual(customer.Message, localizer["msgDuplicateRecord", new string[] { "Customer" }].Value);
        }

        [Test]
        public async Task Service_Should_Not_Create_Customer_When_Exception()
        {
            CustomerModel customerMasterModel = GetSignleObject();
            customerMasterRepository.Setup(r => r.Any(It.IsAny<Expression<Func<Customer, bool>>>())).Throws(new Exception("something went wrong"));
            var response = await customerMasterService.Create(customerMasterModel);

            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_Update_CustomerData()
        {
            var _firstcustomer = GetSignleObject();
            string oldCode = _firstcustomer.CustomerAbbreviation1;
            _firstcustomer.CustomerId = 3;
            _firstcustomer.CustomerAbbreviation1 = "Changed Abbr";
            var response = await customerMasterService.Update(_firstcustomer);

            Assert.That(_firstcustomer.CustomerAbbreviation1, Is.Not.EqualTo(oldCode));
            Assert.AreEqual(response.Result, ResultType.Success);
        }

        [Test]
        public async Task Service_Should_Not_Update_When_CustomerId_InValid()
        {
            var _firstcustomer = GetSignleObject();
            var customer = await customerMasterService.Update(_firstcustomer);
            Assert.AreEqual(customer.Result, ResultType.Warning);
            Assert.Greater(customer.ExtraData.Count, 0);
        }
        [Test]
        public async Task Service_Should_Not_Update_When_Duplicate_Record_Exist()
        {
            var _firstcustomer = GetSignleObject();
            _firstcustomer.CustomerId = 1;
            customerMasterRepository.Setup(r => r.Any(It.IsAny<Expression<Func<Customer, bool>>>())).Returns(true);
            var customer = await customerMasterService.Update(_firstcustomer);
            Assert.That(customer.Result, Is.EqualTo(ResultType.Warning));
            Assert.AreEqual(customer.Message, localizer["msgDuplicateRecord", new string[] { "Customer" }].Value);
        }

        [Test]
        public async Task Service_Should_Not_Update_Customer_When_Exception()
        {
            var obj = GetSignleObject();
            obj.CustomerId = 3;
            customerMasterRepository.Setup(r => r.Any(It.IsAny<Expression<Func<Customer, bool>>>())).Throws(new Exception("something went wrong"));
            var response = await customerMasterService.Update(obj);

            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_Delete_CustomerData_When_CustomerId_Valid()
        {
            var _firstcustomer = GetSignleObject();
            _firstcustomer.CustomerId = 1;
            var customer = await customerMasterService.Delete(_firstcustomer.CustomerId);

            Assert.AreEqual(customer.Result, ResultType.Success);
            Assert.AreEqual(customer.Message, localizer["msgDeleteSuccess", new string[] { "Customer" }].Value);
        }

        [Test]
        public async Task Service_Should_Delete_CustomerData_When_CustomerId_InValid()
        {
            var _firstcustomer = GetSignleObject();
            _firstcustomer.CustomerId = 10;

            var response = await customerMasterService.Delete(_firstcustomer.CustomerId);

            Assert.AreEqual(response.Result, ResultType.Warning);
            Assert.AreEqual(response.Message, localizer["msgRecordNotExist", new string[] { "Customer" }].Value);
        }

        [Test]
        public async Task Service_Should_Not_Delete_CustomerData_When_Exception()
        {

            customerMasterRepository.Setup(r => r.GetByIdAsync(1)).Throws(new Exception("something went wrong"));
            var response = await customerMasterService.Delete(1);

            Assert.AreEqual(response.Result, ResultType.Error);
        }

        [Test]
        public async Task Service_Should_Pass_Value_To_Customer_Entities()
        {

            Customer customer = new Customer()
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
                CustomerAbbreviation2 = "241234",
                ProjectMaster = new Collection<ProjectMaster>() { new ProjectMaster() { ProjectId = 1, ProjectCode = 100001 } },
                UpdatedBy = 8888,
                UpdatedOn = DateTime.Now,

            };
            await Task.FromResult(customer);
            Assert.IsNotNull(customer);
        }

        #region Setup Dummy Data & Repository
        private Mock<IRepository<Customer>> SetupCustomerRepository()
        {
            var repo = new Mock<IRepository<Customer>>();
            var responseSingle = GetSinglecustomerObjectMockData();
            var response = GetGeneralcustomerMockData();

            var getcustomerMockData = GetcustomerMasterPagedMockData();
            IQueryable<Customer> queryableCustomer = getcustomerMockData.Data.AsQueryable();

            repo.Setup(r => r.GetManyAsync(o => o.IsActive == true && o.IsDeleted == false)).Returns(response);
            repo.Setup(r => r.GetManyAsync(o => o.IsActive == true && o.IsDeleted == false, true)).Returns(response);
            repo.Setup(r => r.AddAsync(It.IsAny<Customer>()))
             .Callback(new Action<Customer>(newcustomer =>
             {
                 dynamic maxCustomerID = customerMaster.Last().CustomerId;
                 dynamic nextCustomerID = maxCustomerID + 1;
                 newcustomer.CustomerId = nextCustomerID;
                 newcustomer.CreatedOn = DateTime.Now;
                 customerMaster.Add(newcustomer);
             }));
            repo.Setup(r => r.UpdateAsync(It.IsAny<Customer>()))
             .Callback(new Action<Customer>(x =>
             {
                 var oldCustomer = customerMaster.Find(a => a.CustomerId == x.CustomerId);
                 oldCustomer.UpdatedOn = DateTime.Now;
                 oldCustomer = x;

             }));

            repo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(responseSingle.Data);

            repo.Setup(r => r.Query(true)).Returns(queryableCustomer);
            repo.Setup(r => r.GetByIdAsync(3)).ReturnsAsync(responseSingle.Data);
            repo.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(() => { responseSingle.Data = null; return responseSingle.Data; }); // Get data from Invalid ID
            return repo;
        }

        private CustomerModel GetSignleObject()
        {
            CustomerModel customerMasterModel = new CustomerModel()
            {
                Name = "FM Nessloson 2",
                Email = "FM@gmail.com",
                Address = "address",
                PhoneNumber = "7896541254",
                IsActive = true,
                IsDeleted = false,
                CustomerAbbreviation1 = "12342134",
                CustomerAbbreviation2 = "241234"
            };
            return customerMasterModel;
        }
        private List<Customer> GetMockDataForCreatecustomerData()
        {
            List<Customer> customerMasters = new List<Customer>();
            customerMasters.Add(new Customer()
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
            return customerMasters;
        }

        private async Task<ICollection<Customer>> GetGeneralcustomerMockData()
        {
            return await Task.FromResult(GetCustomerMockData());
        }

        private List<Customer> GetCustomerMockData()
        {
            List<Customer> customerMasters = new List<Customer>();
            customerMasters.Add(new Customer()
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
            customerMasters.Add(new Customer()
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
            return customerMasters;
        }

        private PagedEntityResponse<Customer> GetcustomerMasterPagedMockData()
        {
            var response = new PagedEntityResponse<Customer>();
            response.Data = GetCustomerMockData();
            return response;
        }

        private GeneralResponse<Customer> GetSinglecustomerObjectMockData()
        {
            var response = new GeneralResponse<Customer>();
            response.Data = new Customer()
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
            };


            return response;

        }
        private CustomerModel GetCustomerModel(string testParams)
        {
            CustomerModel customerMasterModel = GetSignleObject();
            if (testParams == "name")
            {
                customerMasterModel.Name = "FM Nessloson 2";
            }
            if (testParams == "phonenumber")
            {
                customerMasterModel.PhoneNumber = "7896541254";
            }
            if (testParams == "email")
            {
                customerMasterModel.Email = "FM@gmail.com";
            }

            return customerMasterModel;
        }
        #endregion
    }
}