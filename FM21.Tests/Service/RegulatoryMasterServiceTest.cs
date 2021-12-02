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
    public class RegulatoryMasterServiceTest : TestBase
    {
        private Mock<IRepository<RegulatoryMaster>> regulatoryMasterRepository;
        private IRepository<NutrientMaster> nutrientMasterRepository;
        private IRegulatoryMasterService regulatoryMasterService;
        private Mock<IRepository<UnitOfMeasurementMaster>> unitOfMeasurementMasterRepository;
        List<RegulatoryMaster> regulatoryModel;
        RegulatoryModel regModel;

        [SetUp]
        public void SetUp()
        {
            regulatoryModel = GetMockDataForCreateRegulatoryData();
            regulatoryMasterRepository = SetupRegulatoryRepository();
            nutrientMasterRepository = SetupNutrientRepository();
            regModel = GetSignleObject();            
            unitOfMeasurementMasterRepository = SetupUnitOfMeasurementMasterRepository();
            regulatoryMasterService = new RegulatoryMasterService(serviceProvider.Object, exceptionHandler.Object, mapper, unitOfWork.Object, null,
                regulatoryMasterRepository.Object, nutrientMasterRepository, unitOfMeasurementMasterRepository.Object);
        }

        [Test]
        public async Task Service_Should_Return_All_ActiveAnd_NotDeleted_RegulatoryData_By_SortColumn()
        {
            var regulatory = await regulatoryMasterService.GetAll();
            Assert.That(regulatory.Result, Is.EqualTo(ResultType.Success));
        }

        [Test]
        public async Task Service_Should_Not_Return_Data_When_Exception()
        {
            regulatoryMasterRepository.Setup(r => r.GetManyAsync(o => o.IsActive && !o.IsDeleted, true)).Throws(new Exception("something went wrong"));
            var response = await regulatoryMasterService.GetAll();
            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_Return_All_Active_RegulatoryData()
        {
            var response = GetGeneralRegulatoryMockPagedData();

            var regulatory = await regulatoryMasterService.GetPageWiseData(filter: null, pageIndex: 1, pageSize: 10, sortColumn: null, sortDirection: null);

            Assert.That(regulatory.Result, Is.EqualTo(response.Result));
            Assert.IsNotNull(regulatory.ExtraData);
        }

        [TestCase("nutrient")]
        [TestCase("OldUsa")]
        [TestCase("CanadaNi")]
        [TestCase("CanadaNf")]
        [TestCase("NewUsRdi")]
        [TestCase("EU")]
        [TestCase("Unit")]
        [TestCase("UnitPerMg")]
        [TestCase("CreatedBy")]
        [TestCase("")]
        public async Task Service_Should_Return_All_Active_RegulatoryData_By_SortColumn(string sortColumn)
        {
            var response = GetGeneralRegulatoryMockPagedData();
            var regulatory = await regulatoryMasterService.GetPageWiseData(filter: null, pageIndex: 1, pageSize: 10, sortColumn: sortColumn, sortDirection: null);

            Assert.That(regulatory.Result, Is.EqualTo(response.Result));
        }

        [Test]
        public async Task Service_Should_Not_Return_RegulatoryData_When_Exception()
        {
            regulatoryMasterRepository.Setup(r => r.Query(true)).Throws(new Exception("something went wrong"));
            var response = await regulatoryMasterService.GetPageWiseData(filter: null, pageIndex: 1, pageSize: 10, sortColumn: null, sortDirection: null);

            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_Return_Regulatory_By_Valid_RegulatoryId()
        {
            var response = GetSingleRegulatoryObjectMockData();
            var regulatory = await regulatoryMasterService.Get(1);
            Assert.That(regulatory.Result, Is.EqualTo(response.Result));
            Assert.AreEqual(regulatory.Data.RegulatoryId, response.Data.RegulatoryId);
        }

        [Test]
        public async Task Service_Should_Return_RecordNotExist_On_Invalid_RegulatoryId()
        {
            var response = await regulatoryMasterService.Get(5);
            
            Assert.IsNull(response.Data);
            Assert.AreEqual(response.Message, localizer["msgRecordNotExist", new string[] { "Regulatory" }].Value);
        }

        [Test]
        public async Task Service_Should_Return_Record_When_Exception()
        {
            regulatoryMasterRepository.Setup(r => r.GetByIdAsync(6)).Throws(new Exception("something went wrong"));
            var response = await regulatoryMasterService.Get(6);

            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_CreateNew_RegulatoryData()
        {
            int _maxRegIDBeforeAdd = regulatoryModel.Max(a => a.RegulatoryId);
            var response = await regulatoryMasterService.Create(regModel);
            Assert.That(_maxRegIDBeforeAdd + 1, Is.EqualTo(regulatoryModel.Last().RegulatoryId));
            Assert.AreEqual(response.Result, ResultType.Success);
        }

        [Test]
        public async Task Service_Should_NotCreate_When_RegulatoryData_InValid()
        {
            regModel.NutrientId = 0;           
            var response = await regulatoryMasterService.Create(regModel);

            Assert.That(response.Result, Is.EqualTo(ResultType.Warning));
            Assert.Greater(response.ExtraData.Count, 0);
        }

        [Test]
        public async Task Service_Should_Create_ValidaionMessage_When_Unit_IsEmpty()
        {
            regModel.Unit = string.Empty;
            var response = await regulatoryMasterService.Create(regModel);

            Assert.That(response.Result, Is.EqualTo(ResultType.Warning));
            Assert.Greater(response.ExtraData.Count, 0);
        }
        
        [Test]
        public async Task Service_Should_Not_Create_When_RegulatoryData_Exist()
        {
            regulatoryMasterRepository.Setup(r => r.Any(It.IsAny<Expression<Func<RegulatoryMaster, bool>>>())).Returns(true);
            var response = await regulatoryMasterService.Create(regModel);

            Assert.That(response.Result, Is.EqualTo(ResultType.Warning));
            Assert.AreEqual(response.Message, localizer["msgDuplicateRecord", new string[] { "Regulatory" }].Value);
        }

        [Test]
        public async Task Service_Should_Not_CreateNew_RegulatoryData_When_Excption()
        {
            regulatoryMasterRepository.Setup(r => r.AddAsync(It.IsAny<RegulatoryMaster>())).Throws(new Exception("something went wrong"));
            var response = await regulatoryMasterService.Create(regModel);
            Assert.AreEqual(response.Result, ResultType.Error);
        }

        [Test]
        public async Task Service_Should_Update_RegulatoryData()
        {
            int? OldUsaValue = regModel.OldUsa;
            regModel.OldUsa = 210;
            regModel.RegulatoryId = 1;
            var response = await regulatoryMasterService.Update(regModel);

            Assert.That(regModel.OldUsa, Is.Not.EqualTo(OldUsaValue));
            Assert.That(regModel.RegulatoryId, Is.EqualTo(1));
            Assert.AreEqual(response.Result, ResultType.Success);
        }

        [Test]
        public async Task Service_Should_Not_Update_When_RegulatoryId_InValid()
        {
            var response = await regulatoryMasterService.Update(regModel);
           
            Assert.Greater(response.ExtraData.Count, 0);
            Assert.AreEqual("RegulatoryId", response.ExtraData[0].Key);
            Assert.AreEqual("Must be greater than zero.", response.ExtraData[0].Value);
        }

        [Test]
        public async Task Service_Should_Not_Update_Regulatory_When_NutrientId_InValid()
        {
            regModel.NutrientId = 0;
            regModel.RegulatoryId = 1;
            var response = await regulatoryMasterService.Update(regModel);
           
            Assert.Greater(response.ExtraData.Count, 0);
            Assert.AreEqual("NutrientId", response.ExtraData[0].Key);
            Assert.AreEqual("Must be greater than zero.", response.ExtraData[0].Value);
        }

        [Test]
        public async Task Service_Should_Not_Update_When_Nutrient_And_RegulatoryItem_Already_Exist_In_The_System()
        {
            regModel.RegulatoryId = 1;
            regModel.NutrientId = 5;
            regulatoryMasterRepository.Setup(r => r.Any(It.IsAny<Expression<Func<RegulatoryMaster, bool>>>())).Returns(true);
            var response = await regulatoryMasterService.Update(regModel);

            Assert.That(response.Result, Is.EqualTo(ResultType.Warning));
            Assert.AreEqual(response.Message, localizer["msgDuplicateRecord", new string[] { "Regulatory" }].Value);
        }
        
        [Test]
        public async Task Service_Should_Not_Update_When_Regulatory_Not_Exist_In_The_System()
        {
            var responseSingle = GetSingleRegulatoryObjectMockData();
            responseSingle.Data = null;
            regModel.RegulatoryId = 1;
            regulatoryMasterRepository.Setup(r => r.GetByIdAsync(regModel.RegulatoryId)).ReturnsAsync(responseSingle.Data);
            var response = await regulatoryMasterService.Update(regModel);

            Assert.That(response.Result, Is.EqualTo(ResultType.Warning));
            Assert.AreEqual(response.Message, localizer["msgRecordNotExist", new string[] { "Regulatory" }].Value);
        }

        [Test]
        public async Task Service_Should_Not_Update_RegulatoryData_When_Exception()
        {
            regModel.RegulatoryId = 9;
            regulatoryMasterRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>())).Throws(new Exception("something went wrong"));
            var response = await regulatoryMasterService.Update(regModel);

            Assert.AreEqual(response.Result, ResultType.Error);
        }

        [Test]
        public async Task Service_Should_Delete_RegulatoryData_When_RegulatoryId_Valid()
        {
            regModel.RegulatoryId = 1;
            var response = await regulatoryMasterService.Delete(regModel.RegulatoryId);
            Assert.AreEqual(response.Result, ResultType.Success);
        }

        [Test]
        public async Task Service_Should_Not_Delete_RegulatoryData_When_RegulatoryId_InValid()
        {
            regModel.RegulatoryId = 5;
            var response = await regulatoryMasterService.Delete(regModel.RegulatoryId);
            Assert.AreEqual(response.Result, ResultType.Warning);
        }

        [Test]
        public async Task Service_Should_Delete_When_Exception()
        {
            regulatoryMasterRepository.Setup(r => r.GetByIdAsync(1)).Throws(new Exception("something went wrong"));
            var response = await regulatoryMasterService.Delete(1);
            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_Return_All_UnitOfMeasurementMaster()
        {
            var response = await regulatoryMasterService.GetUnitOfMeasurement();
            Assert.Greater(response.Data.Count, 0);
        }

        [Test]
        public async Task Service_Should_Not_Return_UnitOfMeasurementMaster_When_Exception()
        {
            unitOfMeasurementMasterRepository.Setup(r => r.GetManyAsync(o => o.IsActive && !o.IsDeleted, true)).Throws(new Exception("something went wrong"));
            var response = await regulatoryMasterService.GetUnitOfMeasurement();
            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        #region Setup Dummy Data & Repository
        private Mock<IRepository<RegulatoryMaster>> SetupRegulatoryRepository()
        {
            var repo = new Mock<IRepository<RegulatoryMaster>>();
            var responseSingle = GetSingleRegulatoryObjectMockData();
            var response = GetGeneralRegulatoryMockData();
            var getRegMockData = GetGeneralRegulatoryMockPagedData();
            IQueryable<RegulatoryMaster> queryableRegulatory = getRegMockData.Data.AsQueryable();

            repo.Setup(r => r.GetManyAsync(o => o.IsActive && !o.IsDeleted)).Returns(response);
            repo.Setup(r => r.AddAsync(It.IsAny<RegulatoryMaster>()))
             .Callback(new Action<RegulatoryMaster>(newRegulatory => {
                 dynamic maxRegularID = regulatoryModel.Last().RegulatoryId;
                 dynamic nextRegularID = maxRegularID + 1;
                 newRegulatory.RegulatoryId = nextRegularID;
                 newRegulatory.NutrientId = 1;

                 newRegulatory.CreatedOn = DateTime.Now;
                 regulatoryModel.Add(newRegulatory);
             }));
            repo.Setup(r => r.UpdateAsync(It.IsAny<RegulatoryMaster>()))
             .Callback(new Action<RegulatoryMaster>(x => {
                 var oldRegulatory = regulatoryModel.Find(a => a.RegulatoryId == x.RegulatoryId);
                 oldRegulatory.UpdatedOn = DateTime.Now;
                 oldRegulatory = x;

             }));

            repo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(responseSingle.Data);
            repo.Setup(r => r.Any(x => x.RegulatoryId != responseSingle.Data.RegulatoryId &&
             x.NutrientId == 0)).Returns(true);


            repo.Setup(r => r.Query(true)).Returns(queryableRegulatory);
            repo.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(() => { responseSingle.Data = null; return responseSingle.Data; }); // Get data from Invalid ID
            
            return repo;
        }
        
        private Mock<IRepository<UnitOfMeasurementMaster>> SetupUnitOfMeasurementMasterRepository()
        {
            var repo = new Mock<IRepository<UnitOfMeasurementMaster>>();
            var lstUnitOfMeasurment = GetUnitOfMeasurementMasterMockData();           
            repo.Setup(r => r.GetManyAsync(o => o.IsActive && !o.IsDeleted, true)).ReturnsAsync(lstUnitOfMeasurment);
            return repo;
        }

        private IRepository<NutrientMaster> SetupNutrientRepository()
        {
            var repo = new Mock<IRepository<NutrientMaster>>();
            IQueryable<NutrientMaster> querableNutrMaster = GetNutrientData().Data.AsQueryable();
            repo.Setup(r => r.Query(true)).Returns(querableNutrMaster);
            return repo.Object;
        }

        private RegulatoryModel GetSignleObject()
        {
            RegulatoryModel model = new RegulatoryModel();
            model.NutrientId = 2;
            model.OldUsa = 1000;
            model.CanadaNi = 5;
            model.CanadaNf = 100;
            model.NewUsRdi = 5;
            model.EU = 500;
            model.Unit = "mg";
            model.UnitPerMg = 100;
            model.IsActive = true;
            model.IsDeleted = false;
            return model;
        }
        
        private List<RegulatoryMaster> GetMockDataForCreateRegulatoryData()
        {
            List<RegulatoryMaster> model = new List<RegulatoryMaster>();
            model.Add(new RegulatoryMaster()
            {
                RegulatoryId = 1,
                NutrientId = 2,
                OldUsa = 1000,
                CanadaNi = 5,
                CanadaNf = 100,
                NewUsRdi = 5,
                EU = 500,
                //Unit = "mg",
                UnitPerMg = 100,
                IsActive = true,
                IsDeleted = false,
                NutrientMaster = new NutrientMaster()
            });
            return model;
        }

        private async Task<ICollection<RegulatoryMaster>> GetGeneralRegulatoryMockData()
        {
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
                //Unit = "mg",
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
               // Unit = "mg",
                UnitPerMg = 50,
                IsActive = true,
                IsDeleted = false,
                CreatedBy = 25412,
                CreatedOn = DateTime.Now,
                UpdatedBy = null,
                UpdatedOn = DateTime.Now
            });
            return await Task.FromResult(regulatoryMaster);
        }

        private PagedEntityResponse<RegulatoryMaster> GetGeneralRegulatoryMockPagedData()
        {
            List<RegulatoryMaster> lst = new List<RegulatoryMaster>();
            var response = new PagedEntityResponse<RegulatoryMaster>();
            lst.Add(new RegulatoryMaster()
            {
                RegulatoryId = 10,
                NutrientId = 2,
                OldUsa = 1000,
                CanadaNi = 5,
                CanadaNf = 100,
                NewUsRdi = 5,
                EU = 500,
              //  Unit = "mg",
                UnitPerMg = 100,
                IsActive = true,
                IsDeleted = false,
                NutrientMaster = new NutrientMaster()
                {
                    Name = "BBiotin",
                    NutrientID = 1,
                    NutrientTypeID = 1
                }
            });
            lst.Add(new RegulatoryMaster()
            {
                RegulatoryId = 2,
                NutrientId = 1,
                OldUsa = 1000,
                CanadaNi = 5,
                CanadaNf = 100,
                NewUsRdi = 5,
                EU = 500,
               // Unit = "mg",
                UnitPerMg = 50,
                IsActive = true,
                IsDeleted = false,
                NutrientMaster = new NutrientMaster()
                {
                    Name = "ABiotin1",
                    NutrientID = 2,
                    NutrientTypeID = 1
                }
            });
            response.Data = lst;
            return response;
        }

        private GeneralResponse<RegulatoryMaster> GetSingleRegulatoryObjectMockData()
        {
            var response = new GeneralResponse<RegulatoryMaster>();
            response.Data = new RegulatoryMaster()
            {
                RegulatoryId = 1,
                NutrientId = 2,
                OldUsa = 1000,
                CanadaNi = 5,
                CanadaNf = 100,
                NewUsRdi = 5,
                EU = 500,
               // Unit = "mg",
                UnitPerMg = 100,
                IsActive = true,
                IsDeleted = false,
                CreatedBy = 25412,
                CreatedOn = DateTime.Now,
                UpdatedBy = null,
                UpdatedOn = DateTime.Now,

            };


            return response;
        }
        
        private PagedEntityResponse<NutrientMaster> GetNutrientData()
        {
            List<NutrientMaster> lst = new List<NutrientMaster>();
            var response = new PagedEntityResponse<NutrientMaster>();
            lst.Add(new NutrientMaster()
            {
                Name = "BBiotin",
                NutrientID = 1,
                NutrientTypeID = 1,
                IsActive = true,
                IsDeleted = false
            });
            lst.Add(new NutrientMaster()
            {
                Name = "ABiotin1",
                NutrientID = 2,
                NutrientTypeID = 1,
                IsActive = true,
                IsDeleted = false
            });
            response.Data = lst;
            return response;
        }
        
        private List<UnitOfMeasurementMaster> GetUnitOfMeasurementMasterMockData()
        {
            List<UnitOfMeasurementMaster> lstUnitOfMeasurementMaster = new List<UnitOfMeasurementMaster>();

            lstUnitOfMeasurementMaster.Add(new UnitOfMeasurementMaster()
            {
                UnitOfMeasurementID =1,
                MeasurementUnit ="g",
                IsActive = true,
                IsDeleted=false,
                CreatedBy=8888,
                CreatedOn=DateTime.Now
            });
            lstUnitOfMeasurementMaster.Add(new UnitOfMeasurementMaster()
            {
                UnitOfMeasurementID = 2,
                MeasurementUnit = "kcal",
                IsActive = true,
                IsDeleted = false,
                CreatedBy = 8888,
                CreatedOn = DateTime.Now
            });
           
            return lstUnitOfMeasurementMaster;
        }
        #endregion
    }
}