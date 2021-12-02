using FM21.Core;
using FM21.Core.Model;
using FM21.Data.Infrastructure;
using FM21.Entities;
using FM21.Service;
using FM21.Service.Interface;
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
    public class SupplierMasterServiceTest : TestBase
    {
        private Mock<IRepository<SupplierMaster>> supplierMasterRepository;
        private ISupplierMasterService supplierMasterService;
        List<SupplierMaster> supplierMaster;

        [SetUp]
        public void SetUp()
        {
            supplierMaster = GetMockDataForCreateSupplierData();
            supplierMasterRepository = SetupRegulatoryRepository();
            supplierMasterService = new SupplierMasterService(serviceProvider.Object, exceptionHandler.Object, mapper, unitOfWork.Object, cacheProvider.Object, supplierMasterRepository.Object);
        }

        [Test]
        public async Task Service_Should_Return_All_ActiveAnd_NotDeleted_SupplierData()
        {
            var response = await supplierMasterService.GetAll();
            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
        }

        [Test]
        public async Task Service_Should_Return_Exception_When_SupplierData_IsNull()
        {

            List<SupplierMaster> nullObject = null;
            supplierMasterRepository.Setup(r => r.GetManyAsync(It.IsAny<Expression<Func<SupplierMaster, bool>>>(), true)).ReturnsAsync(nullObject);
            var response = await supplierMasterService.GetAll();
            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }


        [Test]
        public async Task Service_Should_Not_Return_Data_When_Exception()
        {
            supplierMasterRepository.Setup(r => r.GetManyAsync(o => o.IsDeleted == false && o.IsActive == true, true)).Throws(new Exception("something went wrong"));
            var response = await supplierMasterService.GetAll();
            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_Return_All_Active_SupplierData()
        {
            var response = await supplierMasterService.GetPageWiseData(filter: "test supplier", pageIndex: 1, pageSize: 10, sortColumn: null, sortDirection: null);

            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
        }

        [TestCase("Address")]
        [TestCase("suppliername")]
        [TestCase("Email")]
        [TestCase("PhoneNumber")]
        [TestCase("SupplierAbbreviation1")]
        [TestCase("SupplierAbbreviation2")]
        [TestCase("SupplierID")]
        public async Task Service_Should_Return_All_Active_SupplierData_By_SortColumn(string sortColumn)
        {
            var response = await supplierMasterService.GetPageWiseData(filter: "test supplier", pageIndex: 1, pageSize: 10, sortColumn: sortColumn, sortDirection: null);
            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
        }

        [Test]
        public async Task Service_Should_Not_Return_SupplierData_When_Exception()
        {
            supplierMasterRepository.Setup(r => r.Query(true)).Throws(new Exception("something went wrong"));
            var response = await supplierMasterService.GetPageWiseData(filter: null, pageIndex: 1, pageSize: 10, sortColumn: "", sortDirection: null);
            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_Return_Supplier_By_Valid_SupplierId()
        {
            var returnObject = GetSingleSupplierObjectMockData();
            var response = await supplierMasterService.Get(1);
            Assert.That(response.Result, Is.EqualTo(returnObject.Result));
            Assert.AreEqual(response.Data.SupplierID, returnObject.Data.SupplierID);
        }

        [Test]
        public async Task Service_Should_Return_RecordNotExist_On_Invalid_SupplierId()
        {
            var response = await supplierMasterService.Get(5);

            Assert.IsNull(response.Data);
            Assert.AreEqual(response.Message, localizer["msgRecordNotExist", new string[] { "Supplier" }].Value);
        }

        [Test]
        public async Task Service_Should_Not_Return_Supplier_When_Exception()
        {
            supplierMasterRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>())).Throws(new Exception("something went wrong"));
            var response = await supplierMasterService.Get(1);

            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_CreateNew_SupplierData()
        {
            SupplierMasterModel supplierMasterModel = GetSignleObject();
            int _maxRegIDBeforeAdd = supplierMaster.Max(a => a.SupplierID);
            supplierMasterRepository.Setup(r => r.Any(x => x.SupplierName.ToLower().Trim() == supplierMasterModel.SupplierName.ToLower().Trim())).Returns(false);
            var response = await supplierMasterService.Create(supplierMasterModel);

            Assert.That(_maxRegIDBeforeAdd + 1, Is.EqualTo(supplierMaster.Last().SupplierID));
            Assert.AreEqual(response.Message, localizer["msgInsertSuccess", new string[] { "Supplier" }].Value);
        }

        [Test]
        public async Task Service_Should_Not_CreateNew_Supplier_When_SupplierName_Already_Exist()
        {
            SupplierMasterModel supplierMasterModel = GetSignleObject();
            supplierMasterRepository.Setup(r => r.Any(x => x.SupplierName.ToLower().Trim() == supplierMasterModel.SupplierName.ToLower().Trim())).Returns(true);
            var response = await supplierMasterService.Create(supplierMasterModel);

            Assert.AreEqual(response.Message, localizer["msgDuplicateRecord", new string[] { "Supplier" }].Value);
        }

        [Test]
        public async Task Service_Should_NotCreate_When_SupplierData_InValid()
        {
            SupplierMasterModel supplierMasterModel = GetSignleObject();
            supplierMasterModel.SupplierName = null;

            var supplier = await supplierMasterService.Create(supplierMasterModel);

            Assert.That(supplier.Result, Is.EqualTo(ResultType.Warning));
            Assert.Greater(supplier.ExtraData.Count, 0);
        }

        [Test]
        public async Task Service_Should_Not_Create_New_Supplier_When_Excption()
        {
            var model = GetSignleObject();
            supplierMasterRepository.Setup(r => r.Any(It.IsAny<Expression<Func<SupplierMaster, bool>>>())).Throws(new Exception("something went wrong"));
            var response = await supplierMasterService.Create(model);
            Assert.AreEqual(response.Result, ResultType.Error);
        }

        [Test]
        public async Task Service_Should_Update_SupplierData()
        {
            var _firstSupplier = GetSignleObject();
            string oldName = _firstSupplier.SupplierName;
            _firstSupplier.SupplierName = "Paresh";
            _firstSupplier.SupplierId = 1;

            var response = await supplierMasterService.Update(_firstSupplier);

            Assert.That(_firstSupplier.SupplierName, Is.Not.EqualTo(oldName));
            Assert.That(_firstSupplier.SupplierId, Is.EqualTo(1));
            Assert.AreEqual(response.Message, localizer["msgUpdateSuccess", new string[] { "Supplier" }].Value);
        }

        [Test]
        public async Task Service_Should_Not_Update_When_SupplierId_InValid()
        {
            var _firstSupplier = GetSignleObject();

            var supplier = await supplierMasterService.Update(_firstSupplier);

            Assert.AreEqual(supplier.Result, ResultType.Warning);
            Assert.Greater(supplier.ExtraData.Count, 0);
        }
        
        [Test]
        public async Task Service_Should_Not_Update_When_Supplier_Is_Duplicate()
        {
            var _firstSupplier = GetSignleObject();
            _firstSupplier.SupplierId = 1;
            string expectedResult = localizer["msgDuplicateRecord", new string[] { "Supplier" }];
            supplierMasterRepository.Setup(r => r.Any(It.IsAny<Expression<Func<SupplierMaster, bool>>>())).Returns(true);
            var actualResult = await supplierMasterService.Update(_firstSupplier);

            Assert.AreEqual(actualResult.Result, ResultType.Warning);
            Assert.That(actualResult.Message, Is.EqualTo(expectedResult));
        }

        [Test]
        public async Task Service_Should_Not_Update_Supplier_When_Exception()
        {
            var model = GetSignleObject();
            model.SupplierId = 1;
            supplierMasterRepository.Setup(r => r.Any(It.IsAny<Expression<Func<SupplierMaster, bool>>>())).Throws(new Exception("something went wrong"));
            var response = await supplierMasterService.Update(model);

            Assert.AreEqual(response.Result, ResultType.Error);
        }

        [Test]
        public async Task Service_Should_Delete_SupplierData_When_SupplierId_Valid()
        {
            var _firstSupplier = GetSignleObject();
            _firstSupplier.SupplierId = 1;

            var response = await supplierMasterService.Delete(_firstSupplier.SupplierId);

            Assert.AreEqual(response.Result, ResultType.Success);
            Assert.AreEqual(response.Message, localizer["msgDeleteSuccess", new string[] { "Supplier" }].Value);
        }

        [Test]
        public async Task Service_Should_Not_Delete_When_SupplierData_Used_By_Other_Place()
        {
            int SupplierId = 2;
            var responseSingle = GetSingleSupplierObjectMockData();
            supplierMasterRepository.Setup(r => r.GetByIdAsync(SupplierId)).ReturnsAsync(responseSingle.Data);
            var response = await supplierMasterService.Delete(SupplierId);

            Assert.AreEqual(response.Result, ResultType.Warning);
            Assert.AreEqual(response.Message, localizer["msgDeleteFailAsUsedByOthers"].Value);
        }

        [Test]
        public async Task Service_Should_Delete_SupplierData_When_SupplierId_InValid()
        {
            var _firstSupplier = GetSignleObject();
            _firstSupplier.SupplierId = 10;

            var response = await supplierMasterService.Delete(_firstSupplier.SupplierId);
            Assert.AreEqual(response.Result, ResultType.Warning);
        }

        [Test]
        public async Task Service_Should_Delete_When_Exception()
        {
            supplierMasterRepository.Setup(r => r.GetByIdAsync(1)).Throws(new Exception("something went wrong"));
            var response = await supplierMasterService.Delete(1);
            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        #region Setup Dummy Data & Repository
        private Mock<IRepository<SupplierMaster>> SetupRegulatoryRepository()
        {
            var repo = new Mock<IRepository<SupplierMaster>>();
            var responseSingle = GetSingleSupplierObjectMockData();
            var response = GetGeneralSupplierMockData();
            var getsupplierMockData = GetSupplierMasterPagedMockData();
            IQueryable<SupplierMaster> queryableRegulatory = getsupplierMockData.Data.AsQueryable();

            repo.Setup(r => r.GetManyAsync(o => o.IsDeleted == false && o.IsActive == true)).Returns(response);
            repo.Setup(r => r.GetManyAsync(o => o.IsDeleted == false && o.IsActive == true, true)).Returns(response);
            repo.Setup(r => r.AddAsync(It.IsAny<SupplierMaster>()))
             .Callback(new Action<SupplierMaster>(newSupplier =>
             {
                 dynamic maxRegularID = supplierMaster.Last().SupplierID;
                 dynamic nextRegularID = maxRegularID + 1;
                 newSupplier.SupplierID = nextRegularID;
                 newSupplier.CreatedOn = DateTime.Now;
                 supplierMaster.Add(newSupplier);
             }));
            repo.Setup(r => r.UpdateAsync(It.IsAny<SupplierMaster>()))
             .Callback(new Action<SupplierMaster>(x =>
             {
             }));
            repo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(responseSingle.Data);
            repo.Setup(r => r.Any(x => x.SupplierID != responseSingle.Data.SupplierID)).Returns(true);
            repo.Setup(r => r.Query(true)).Returns(queryableRegulatory);
            repo.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(() => { responseSingle.Data = null; return responseSingle.Data; }); // Get data from Invalid ID
            return repo;
        }

        private SupplierMasterModel GetSignleObject()
        {
            SupplierMasterModel supplierMasterModel = new SupplierMasterModel();
            supplierMasterModel.SupplierName = "test supplier";
            supplierMasterModel.Address = "Ahmedabad";
            supplierMasterModel.PhoneNumber = "12345678";
            supplierMasterModel.Email = "FM@gmail.com";
            supplierMasterModel.SupplierAbbreviation1 = "This is Abbreviation 1";
            supplierMasterModel.SupplierAbbreviation2 = "This is Abbreviation 2";
            supplierMasterModel.IsActive = true;
            supplierMasterModel.IsDeleted = false;
            return supplierMasterModel;
        }

        private List<SupplierMaster> GetMockDataForCreateSupplierData()
        {
            List<SupplierMaster> supplierMasters = new List<SupplierMaster>();
            supplierMasters.Add(new SupplierMaster()
            {
                SupplierID = 1,
                SupplierName = "test supplier",
                Address = "Ahmedabad",
                PhoneNumber = "12345678",
                Email = "FM@gmail.com",
                SupplierAbbreviation1 = "This is Abbreviation 1",
                SupplierAbbreviation2 = "This is Abbreviation 2",
                IsActive = true,
                IsDeleted = false
            });
            return supplierMasters;
        }

        private async Task<ICollection<SupplierMaster>> GetGeneralSupplierMockData()
        {
            List<SupplierMaster> supplierMasters = new List<SupplierMaster>();
            supplierMasters.Add(new SupplierMaster()
            {
                SupplierID = 1,
                SupplierName = "test supplier",
                Address = "Ahmedabad",
                PhoneNumber = "12345678",
                Email = "FM@gmail.com",
                SupplierAbbreviation1 = "This is Abbreviation 1",
                SupplierAbbreviation2 = "This is Abbreviation 2",
                IsActive = true,
                IsDeleted = false,
                CreatedBy = 25412,
                CreatedOn = DateTime.Now,
                UpdatedBy = null,
                UpdatedOn = DateTime.Now
            });
            supplierMasters.Add(new SupplierMaster()
            {
                SupplierID = 2,
                SupplierName = "test supplier",
                Address = "Pune",
                PhoneNumber = "8889997774",
                Email = "test@gmail.com",
                SupplierAbbreviation1 = "This is Abbreviation 1",
                SupplierAbbreviation2 = "This is Abbreviation 2",
                IsActive = true,
                IsDeleted = false,
                CreatedBy = 25412,
                CreatedOn = DateTime.Now,
                UpdatedBy = null,
                UpdatedOn = DateTime.Now
            });
            return await Task.FromResult(supplierMasters);
        }

        private PagedEntityResponse<SupplierMaster> GetSupplierMasterPagedMockData()
        {
            var response = new PagedEntityResponse<SupplierMaster>();
            List<SupplierMaster> supplierMasterModels = new List<SupplierMaster>();
            supplierMasterModels.Add(new SupplierMaster()
            {
                SupplierID = 1,
                SupplierName = "test supplier",
                Address = "Ahmedabad",
                PhoneNumber = "12345678",
                Email = "FM@gmail.com",
                SupplierAbbreviation1 = "This is Abbreviation 1",
                SupplierAbbreviation2 = "This is Abbreviation 2",
                IsActive = true,
                IsDeleted = false
            });
            supplierMasterModels.Add(new SupplierMaster()
            {
                SupplierID = 2,
                SupplierName = "test supplier",
                Address = "Pune",
                PhoneNumber = "8889997774",
                Email = "test@gmail.com",
                SupplierAbbreviation1 = "This is Abbreviation 1",
                SupplierAbbreviation2 = "This is Abbreviation 2",
                IsActive = true,
                IsDeleted = false,
                IngredientSupplierMapping = new IngredientSupplierMapping[] { new IngredientSupplierMapping() { IngredientID = 1, IngredientSupplierID = 1, ManufactureID = 2 } }
            });
            response.Data = supplierMasterModels;
            return response;
        }

        private GeneralResponse<SupplierMaster> GetSingleSupplierObjectMockData()
        {
            var response = new GeneralResponse<SupplierMaster>();
            response.Data = new SupplierMaster()
            {
                SupplierID = 1,
                SupplierName = "test supplier",
                Address = "Ahmedabad",
                PhoneNumber = "12345678",
                Email = "FM@gmail.com",
                SupplierAbbreviation1 = "This is Abbreviation 1",
                SupplierAbbreviation2 = "This is Abbreviation 2",
                IsActive = true,
                IsDeleted = false
            };
            return response;
        }
        #endregion
    }
}
