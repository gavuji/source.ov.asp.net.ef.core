using FM21.API.Extensions;
using FM21.Core;
using FM21.Data.Infrastructure;
using FM21.Entities;
using FM21.Service;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FM21.Tests
{
    [TestFixture]
    public class CommonServiceTest : TestBase
    {
        private ICommonService commonService;
        private Mock<IRepository<SiteMaster>> siteMasterRepository;
        private Mock<IRepository<ProductTypeMaster>> productTypeMasterRepository;
        private Mock<IRepository<SiteProductTypeMapping>> siteProductTypeMappingRepository;
        private Mock<IRepository<InstructionCategoryMaster>> instructionCategoryMasterRepository;
        private Mock<IRepository<BrokerMaster>> brokerMasterRepository;
        private Mock<IRepository<KosherCodeMaster>> kosherCodeMasterRepository;
        private Mock<IRepository<HACCPMaster>> hACCPMasterRepository;
        private Mock<IRepository<RMStatusMaster>> rMStatusMasterRepository;
        private Mock<IRepository<StorageConditionMaster>> storageConditionMasterRepository;
        private Mock<IRepository<CountryMaster>> countryMasterRepository;
        private Mock<IRepository<SiteProductionLineMapping>> siteProductionLineMapRepository;
        private Mock<IRepository<ProductionLineMixerMapping>> productionLineMixerMapRepository;
        private Mock<IRepository<ReleaseAgentMaster>> releaseAgentMasterRepository;
        private Mock<IRepository<PkoPercentageMaster>> pkoPercentageMasterRepository;
        private Mock<IRepository<BarFormatMaster>> barFormatMasterRepository;
        private Mock<IRepository<BarFormatCodeMaster>> barFormatCodeMasterRepository;
        private Mock<IRepository<FormulaTypeMaster>> formulaTypeMasterRepository;
        private Mock<IRepository<SitterWidthMaster>> sitterWidthMasterRepository;
        private Mock<IRepository<ExtrusionDieMaster>> extrusionDieMasterRepository;
        private Mock<IRepository<InternalQCMAVLookUpMaster>> internalQCMAVLookUpMasterRepository;

        [SetUp]
        [SetUICulture("en-us")]
        public void SetUp()
        {
            siteMasterRepository = SetupSiteMasterRepository();
            productTypeMasterRepository = SetupProductTypeMasterRepository();
            siteProductTypeMappingRepository = SetupSiteProductTypeRepository();
            instructionCategoryMasterRepository = SetupInstructionCategoryRepository();
            brokerMasterRepository = SetupBrokerMasterRepository();
            kosherCodeMasterRepository = SetupKosherCodeMasterRepository();
            hACCPMasterRepository = SetupHACCPMasterRepository();
            rMStatusMasterRepository = SetupRMStatusMasterRepository();
            storageConditionMasterRepository = SetupStorageConditionMasterRepository();
            countryMasterRepository = SetupCountryMasterRepository();
            siteProductionLineMapRepository = SetupSiteProductionLineMappingRepository();
            productionLineMixerMapRepository = SetupProductionLineMixerMappingRepository();
            releaseAgentMasterRepository = SetupReleaseAgentMasterRepository();
            pkoPercentageMasterRepository = SetupPkoPercentageMasterRepository();
            barFormatMasterRepository = SetupBarFormatMasterRepository();
            barFormatCodeMasterRepository = SetupBarFormatCodeMasterRepository();
            formulaTypeMasterRepository = SetupFormulaTypeMasterRepository();
            sitterWidthMasterRepository = SetupSitterWidthMasterRepository();
            extrusionDieMasterRepository = SetupExtrusionDieMasterRepository();
            internalQCMAVLookUpMasterRepository = SetupInternalQCMAVLookUpMasterRepository();

            commonService = new CommonService(serviceProvider.Object, exceptionHandler.Object, mapper, unitOfWork.Object, null,
                                    siteMasterRepository.Object, productTypeMasterRepository.Object, siteProductTypeMappingRepository.Object,
                                    instructionCategoryMasterRepository.Object, brokerMasterRepository.Object, kosherCodeMasterRepository.Object,
                                    hACCPMasterRepository.Object, rMStatusMasterRepository.Object, storageConditionMasterRepository.Object,
                                    countryMasterRepository.Object, siteProductionLineMapRepository.Object, productionLineMixerMapRepository.Object,
                                    releaseAgentMasterRepository.Object, pkoPercentageMasterRepository.Object, barFormatMasterRepository.Object, 
                                    barFormatCodeMasterRepository.Object, formulaTypeMasterRepository.Object, sitterWidthMasterRepository.Object,
                                    extrusionDieMasterRepository.Object, internalQCMAVLookUpMasterRepository.Object);
        }

        [Test]
        public async Task Service_Should_Return_All_Site_Data()
        {
            var response = await commonService.GetAllSite();

            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
            Assert.IsNotNull(response.Data);
        }

        [Test]
        public async Task Service_Should_Not_Return_When_Exception()
        {
            siteMasterRepository.Setup(r => r.GetManyAsync(o => o.IsActive, true)).Throws(new Exception("something went wrong"));
            var response = await commonService.GetAllSite();

            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_Return_All_Product_Type_Data()
        {
            var response = await commonService.GetAllProductType();

            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
            Assert.IsNotNull(response.Data);
        }

        [Test]
        public async Task Service_Should_Not_Return_All_ProductType_Data_When_Exception()
        {
            productTypeMasterRepository.Setup(r => r.GetManyAsync(o => o.IsActive, true)).Throws(new Exception("something went wrong"));
            var response = await commonService.GetAllProductType();

            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_Return_All_SiteProductType_Data()
        {
            var response = await commonService.GetAllSiteProductType(false);

            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
            Assert.IsNotNull(response.Data);
        }

        [Test]
        public async Task Service_Should_Not_Return_All_SiteProductType_Data_When_Exception()
        {
            siteProductTypeMappingRepository.Setup(r => r.Query(true)).Throws(new Exception("something went wrong"));
            var response = await commonService.GetAllSiteProductType(false);

            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_Return_All_InstructionCategory_Data()
        {
            var response = await commonService.GetAllInstructionCategory();

            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
            Assert.IsNotNull(response.Data);
        }

        [Test]
        public async Task Service_Should_Not_Return_All_InstructionCategory_Data_When_Exception()
        {
            instructionCategoryMasterRepository.Setup(r => r.GetManyAsync(o => o.IsActive, true)).Throws(new Exception("something went wrong"));
            var response = await commonService.GetAllInstructionCategory();

            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_Return_All_InstructionCategoryBySiteProductMapID_Data()
        {
            var response = await commonService.GetAllInstructionCategoryBySiteProductMapID(1);

            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
            Assert.IsNotNull(response.Data);
        }

        [Test]
        public async Task Service_Should_Not_Return_All_InstructionCategoryBySiteProductMapID_Data_When_Exception()
        {
            siteProductTypeMappingRepository.Setup(r => r.Query(true)).Throws(new Exception("something went wrong"));
            var response = await commonService.GetAllInstructionCategoryBySiteProductMapID(1);

            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_Return_All_BrokerMaster_Data()
        {

            var response = await commonService.GetAllBroker();

            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
            Assert.IsNotNull(response.Data);
        }

        [Test]
        public async Task Service_Should_Not_Return_All_BrokerMaster_Data_When_Exception()
        {
            brokerMasterRepository.Setup(r => r.GetManyAsync(o => o.IsActive && !o.IsDeleted, true)).Throws(new Exception("something went wrong"));
            var response = await commonService.GetAllBroker();

            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_Return_All_KosherCodeMaster_Data()
        {
            var response = await commonService.GetAllKosherCode();

            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
            Assert.IsNotNull(response.Data);
        }

        [Test]
        public async Task Service_Should_Not_Return_All_KosherCodeMaster_Data_When_Exception()
        {
            kosherCodeMasterRepository.Setup(r => r.GetManyAsync(o => o.IsActive && !o.IsDeleted, true)).Throws(new Exception("something went wrong"));
            var response = await commonService.GetAllKosherCode();

            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_Return_All_HACCPMaster_Data()
        {
            var response = await commonService.GetAllHACCPData();

            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
            Assert.IsNotNull(response.Data);
        }

        [Test]
        public async Task Service_Should_Not_Return_All_HACCPMaster_Dat_When_Exception()
        {
            string hACCPType = string.Empty;
            hACCPMasterRepository.Setup(r => r.GetManyAsync(o => o.IsActive && !o.IsDeleted && (hACCPType == string.Empty || o.HACCPType.ToLower() == hACCPType), true)).Throws(new Exception("something went wrong"));
            var response = await commonService.GetAllHACCPData();

            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_Return_All_RMStatus_Data()
        {
            var response = await commonService.GetAllRMStatus();

            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
            Assert.IsNotNull(response.Data);
        }

        [Test]
        public async Task Service_Should_Not_Return_All_RMStatus_Data_When_Exception()
        {
            string rMStatusType = string.Empty;
            rMStatusMasterRepository.Setup(r => r.GetManyAsync(o => o.IsActive && !o.IsDeleted && (rMStatusType == string.Empty || o.RMStatusType.ToLower() == rMStatusType), true)).Throws(new Exception("something went wrong"));
            var response = await commonService.GetAllRMStatus();

            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_Return_All_StorageCondition_Data()
        {
            var response = await commonService.GetAllStorageCondition();

            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
            Assert.IsNotNull(response.Data);
        }

        [Test]
        public async Task Service_Should_Not_Return_All_StorageCondition_Data_When_Exception()
        {
            storageConditionMasterRepository.Setup(r => r.GetManyAsync(o => o.IsActive && !o.IsDeleted, true)).Throws(new Exception("something went wrong"));
            var response = await commonService.GetAllStorageCondition();

            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_Return_All_Country()
        {
            var response = await commonService.GetAllCountry();

            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
            Assert.IsNotNull(response.Data);
        }

        [Test]
        public async Task Service_Should_Not_Return_All_Country_When_Exception()
        {
            countryMasterRepository.Setup(r => r.GetManyAsync(o => o.IsActive && !o.IsDeleted, true)).Throws(new Exception("something went wrong"));
            var response = await commonService.GetAllCountry();

            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_Return_All_Production_Line_By_Site()
        {
            var response = await commonService.GetAllProductionLineBySite(1, 1);

            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
            Assert.IsNotNull(response.Data);
        }

        [Test]
        public async Task Service_Should_Not_Return_All_Production_Line_By_Site_When_Exception()
        {
            siteProductionLineMapRepository.Setup(r => r.Query(true)).Throws(new Exception("something went wrong"));
            var response = await commonService.GetAllProductionLineBySite(1, 1);

            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_Return_All_Production_Line_Mixer_By_Site()
        {
            var response = await commonService.GetAllProductionLineMixerBySite(1);

            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
            Assert.IsNotNull(response.Data);
        }

        [Test]
        public async Task Service_Should_Not_Return_All_Production_Line_Mixer_By_Site_When_Exception()
        {
            productionLineMixerMapRepository.Setup(r => r.Query(true)).Throws(new Exception("something went wrong"));
            var response = await commonService.GetAllProductionLineMixerBySite(1);

            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_Return_All_ReleaseAgentMaster_Data()
        {
            var response = await commonService.GetAllReleaseAgent();

            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
            Assert.IsNotNull(response.Data);
        }

        [Test]
        public async Task Service_Should_Not_Return_Release_Agent_When_Exception()
        {
            releaseAgentMasterRepository.Setup(r => r.GetManyAsync(o => o.IsActive && !o.IsDeleted, true)).Throws(new Exception("something went wrong"));
            var response = await commonService.GetAllReleaseAgent();

            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_Return_All_PkoPercentageMaster_Data()
        {
            var response = await commonService.GetAllPkoPercentage();

            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
            Assert.IsNotNull(response.Data);
        }

        [Test]
        public async Task Service_Should_Not_Return_PkoPercentageMaster_Data_When_Exception()
        {
            pkoPercentageMasterRepository.Setup(r => r.GetManyAsync(o => o.IsActive && !o.IsDeleted, true)).Throws(new Exception("something went wrong"));
            var response = await commonService.GetAllPkoPercentage();

            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public void Service_Should_Pass_Value_To_BrokerMaster_Entities()
        {
            BrokerMaster Data = new BrokerMaster()
            {
                BrokerID = 1,
                BrokerName = "broker 1",
                Address = "",
                CreatedBy = 8888,
                CreatedOn = DateTime.Now,
                Email = "Nellson@nellson.com",
                IsActive = false,
                IsDeleted = false,
                PhoneNumber = "7985641235",
                UpdatedBy = 1,
                UpdatedOn = DateTime.Now
            };
            Assert.IsNotNull(Data);
        }

        [Test]
        public void Service_Should_Pass_Values_To_HACCPMaster_Entities()
        {
            HACCPMaster obj = new HACCPMaster()
            {
                HACCPID = 2,
                HACCPType = "CHEMICAL",
                HACCPDescription = "Allergen - Milk",
                IsActive = true,
                IsDeleted = false,
                CreatedBy = 8888,
                CreatedOn = DateTime.Now,
                UpdatedBy = 8888,
                UpdatedOn = DateTime.Now
            };

            Assert.IsNotNull(obj);
        }

        [Test]
        public async Task Service_Should_Pass_Values_To_CountryMaster_Entities()
        {
            CountryMaster Data = new CountryMaster()
            {
                CountryID = 1,
                CountryName = "None",
                UpdatedOn = DateTime.Now,
                UpdatedBy = 8888,
                CreatedBy = 8888,
                CreatedOn = DateTime.Now,
                IsActive = false,
                IsDeleted = false
            };
            await Task.FromResult(Data);
            Assert.IsNotNull(Data);
        }

        [Test]
        public void Service_Should_Dispose_Object()
        {
            var objectMock = new Mock<DatabaseFactory>();
            using (var databaseFactory = new DatabaseFactory())
            {
                databaseFactory.Get();
            }
            objectMock.Verify();
        }

        [TestCase("en-US")]
        [TestCase("UserID")]
        public void Service_Should_Check_HttpContext_Headers_On_Requests(string headerKey)
        {
            var context = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            context.Response.Body = new MemoryStream();
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            if (!string.IsNullOrEmpty(headerKey) && headerKey.Contains("UserID"))
            {
                context.Request.Headers["UserID"] = "8888";
            }
            else
            {
                context.Request.Headers["Content-Language"] = headerKey;
            }
            // Arrange
            MiddlewareExtensions.WebWorker(null, context);
            Assert.IsNotNull(context);
        }
        
        [Test]
        public async Task Service_Should_Return_All_BarFormatMaster_Data()
        {
            var response = await commonService.GetAllBarFormatMaster();

            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
            Assert.IsNotNull(response.Data);
        }

        [Test]
        public async Task Service_Should_Not_Return_BarFormatMaster_Data_When_Exception()
        {
            barFormatMasterRepository.Setup(r => r.GetManyAsync(o => o.IsActive && !o.IsDeleted, true)).Throws(new Exception("something went wrong"));
            var response = await commonService.GetAllBarFormatMaster();

            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_Return_All_BarFormatCodeMaster_Data()
        {
            var response = await commonService.GetAllBarFormatCodeMaster();

            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
            Assert.IsNotNull(response.Data);
        }
        
        [Test]
        public async Task Service_Should_Not_Return_All_BarFormatCodeMaster_Data_When_Exception()
        {
            barFormatCodeMasterRepository.Setup(r => r.GetManyAsync(o => o.IsActive && !o.IsDeleted, true)).Throws(new Exception("something went wrong"));
            var response = await commonService.GetAllBarFormatCodeMaster();

            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_Return_All_FormulaType()
        {
            var response = await commonService.GetAllFormulaTypeMaster();

            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
            Assert.IsNotNull(response.Data);
        }

        [Test]
        public async Task Service_Should_Not_Return_FormulaType_When_Exception()
        {
            formulaTypeMasterRepository.Setup(r => r.GetManyAsync(o => o.IsActive && !o.IsDeleted, true)).Throws(new Exception("something went wrong"));
            var response = await commonService.GetAllFormulaTypeMaster();

            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_Return_All_SitterWidth()
        {
            var response = await commonService.GetAllSitterWidth(0);

            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
            Assert.IsNotNull(response.Data);
        }

        [Test]
        public async Task Service_Should_Not_Return_SitterWidth_When_Exception()
        {
            int siteID = 0;
            sitterWidthMasterRepository.Setup(r => r.GetManyAsync(o => o.IsActive && !o.IsDeleted && (siteID == 0 || o.SiteID == siteID), true)).Throws(new Exception("something went wrong"));
            var response = await commonService.GetAllSitterWidth(0);

            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_Return_All_ExtrusionDie()
        {
            var response = await commonService.GetAllExtrusionDie();

            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
            Assert.IsNotNull(response.Data);
        }

        [Test]
        public async Task Service_Should_Not_Return_ExtrusionDie_When_Exception()
        {
            extrusionDieMasterRepository.Setup(r => r.GetManyAsync(o => o.IsActive && !o.IsDeleted, true)).Throws(new Exception("something went wrong"));
            var response = await commonService.GetAllExtrusionDie();

            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }
        [Test]
        public async Task Service_Should_Return_All_InternalQCMAVLookUpMaster()
        {
            var response = await commonService.GetAllInternalQCMAVLookUp();

            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
            Assert.IsNotNull(response.Data);
        }

        [Test]
        public async Task Service_Should_Not_Return_InternalQCMAVLookUpMaster_When_Exception()
        {
            internalQCMAVLookUpMasterRepository.Setup(r => r.GetManyAsync(o => o.IsActive, true)).Throws(new Exception("something went wrong"));
            var response = await commonService.GetAllInternalQCMAVLookUp();

            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        #region Setup Dummy Data & Repository - SiteMaster
        private Mock<IRepository<SiteMaster>> SetupSiteMasterRepository()
        {
            var repo = new Mock<IRepository<SiteMaster>>();
            var recordList = GetSiteMasterMockObjectAsync();
            repo.Setup(r => r.GetManyAsync(o => o.IsActive, true)).Returns(recordList);
            return repo;
        }

        private async Task<ICollection<SiteMaster>> GetSiteMasterMockObjectAsync()
        {
            var recordList = GetSiteMasterMockObject();
            return await Task.FromResult(recordList);
        }

        private ICollection<SiteMaster> GetSiteMasterMockObject()
        {
            ICollection<SiteMaster> recordList = new Collection<SiteMaster>()
            {
                new SiteMaster(){  SiteID = 1, SiteCode = "ONT", SiteDescription = "Ontario" },
                new SiteMaster(){  SiteID = 2, SiteCode = "LAC", SiteDescription ="LAC" }
            };
            return recordList;
        }
        #endregion

        #region Setup Dummy Data & Repository - ProductTypeMaster
        private Mock<IRepository<ProductTypeMaster>> SetupProductTypeMasterRepository()
        {
            var repo = new Mock<IRepository<ProductTypeMaster>>();
            var recordList = GetProductTypeMasterMockObjectAsync();
            repo.Setup(r => r.GetManyAsync(o => o.IsActive, true)).Returns(recordList);
            return repo;
        }

        private async Task<ICollection<ProductTypeMaster>> GetProductTypeMasterMockObjectAsync()
        {
            var recordList = GetProductTypeMasterMockObject();
            return await Task.FromResult(recordList);
        }

        private ICollection<ProductTypeMaster> GetProductTypeMasterMockObject()
        {
            ICollection<ProductTypeMaster> recordList = new Collection<ProductTypeMaster>()
            {
                new ProductTypeMaster(){ ProductTypeID = 1, ProductType = "Bar" },
                new ProductTypeMaster(){ ProductTypeID = 2, ProductType = "Powder" }
            };
            return recordList;
        }
        #endregion

        #region Setup Dummy Data & Repository - SiteProductTypeMapping
        private Mock<IRepository<SiteProductTypeMapping>> SetupSiteProductTypeRepository()
        {
            var repo = new Mock<IRepository<SiteProductTypeMapping>>();
            IQueryable<SiteProductTypeMapping> queryableData = GetSiteProductTypeMockObject().AsQueryable();
            repo.Setup(r => r.Query(true)).Returns(queryableData);
            return repo;
        }

        private ICollection<SiteProductTypeMapping> GetSiteProductTypeMockObject()
        {
            var recordList = new Collection<SiteProductTypeMapping>()
            {
                new SiteProductTypeMapping()
                {
                    SiteID = 1, ProductTypeID = 1, SiteProductMapID = 1,
                    Site = new SiteMaster{ SiteID = 1, SiteCode = "ONT",
                        SiteInstructionCategoryMapping = new List<SiteInstructionCategoryMapping>(){ new SiteInstructionCategoryMapping() { InstructionCategory = new InstructionCategoryMaster() { InstructionCategoryID = 1, InstructionCategory = "Liquid" } } } },
                    ProductType= new ProductTypeMaster() { ProductTypeID = 1, ProductType = "Bar" }
                },
                new SiteProductTypeMapping()
                {
                    SiteID = 1, ProductTypeID = 2, SiteProductMapID = 2,
                    Site = new SiteMaster{ SiteID = 1, SiteCode = "ONT",
                        SiteInstructionCategoryMapping = new List<SiteInstructionCategoryMapping>(){ new SiteInstructionCategoryMapping() { InstructionCategory = new InstructionCategoryMaster() { InstructionCategoryID = 1, InstructionCategory = "Liquid" } } } },
                    ProductType= new ProductTypeMaster() { ProductTypeID = 2, ProductType = "Powder" }
                }
            };
            return recordList;
        }
        #endregion

        #region Setup Dummy Data & Repository - InstructionCategoryMaster
        private Mock<IRepository<InstructionCategoryMaster>> SetupInstructionCategoryRepository()
        {
            var repo = new Mock<IRepository<InstructionCategoryMaster>>();
            var recordList = GetInstructionCategoryMockObjectAsync();
            repo.Setup(r => r.GetManyAsync(o => o.IsActive, true)).Returns(recordList);
            return repo;
        }

        private async Task<ICollection<InstructionCategoryMaster>> GetInstructionCategoryMockObjectAsync()
        {
            var recordList = GetInstructionCategoryMockObject();
            return await Task.FromResult(recordList);
        }

        private ICollection<InstructionCategoryMaster> GetInstructionCategoryMockObject()
        {
            var recordList = new Collection<InstructionCategoryMaster>()
            {
                new InstructionCategoryMaster(){  InstructionCategoryID = 1, InstructionCategory = "Cat 1" },
                new InstructionCategoryMaster(){  InstructionCategoryID = 2, InstructionCategory = "Cat 1" }
            };
            return recordList;
        }
        #endregion

        #region Setup Dummy Data & Repository - BrokerMaster
        private Mock<IRepository<BrokerMaster>> SetupBrokerMasterRepository()
        {
            var repo = new Mock<IRepository<BrokerMaster>>();
            var recordList = GetBrokerMasterMockObjectAsync();
            repo.Setup(r => r.GetManyAsync(o => o.IsActive && !o.IsDeleted, true)).Returns(recordList);
            return repo;
        }

        private async Task<ICollection<BrokerMaster>> GetBrokerMasterMockObjectAsync()
        {
            var recordList = GetBrokerMasterMockObject();
            return await Task.FromResult(recordList);
        }

        private ICollection<BrokerMaster> GetBrokerMasterMockObject()
        {
            var recordList = new Collection<BrokerMaster>()
            {
                new BrokerMaster(){ BrokerID = 1, BrokerName = "broker 1" },
                new BrokerMaster(){ BrokerID = 2, BrokerName = "broker 2" }
            };
            return recordList;
        }
        #endregion

        #region Setup Dummy Data & Repository - KosherCodeMaster
        private Mock<IRepository<KosherCodeMaster>> SetupKosherCodeMasterRepository()
        {
            var repo = new Mock<IRepository<KosherCodeMaster>>();
            var recordList = GetKosherCodeMasterMockObjectAsync();
            repo.Setup(r => r.GetManyAsync(o => o.IsActive && !o.IsDeleted, true)).Returns(recordList);
            return repo;
        }

        private async Task<ICollection<KosherCodeMaster>> GetKosherCodeMasterMockObjectAsync()
        {
            var recordList = GetKosherCodeMasterMockObject();
            return await Task.FromResult(recordList);
        }

        private ICollection<KosherCodeMaster> GetKosherCodeMasterMockObject()
        {
            var recordList = new Collection<KosherCodeMaster>()
            {
                new KosherCodeMaster(){  KosherCodeID = 1, KosherCode = "K001" },
                new KosherCodeMaster(){  KosherCodeID = 2, KosherCode = "K002" }
            };
            return recordList;
        }
        #endregion

        #region Setup Dummy Data & Repository - HACCPMaster
        private Mock<IRepository<HACCPMaster>> SetupHACCPMasterRepository()
        {
            string hACCPType = string.Empty;
            var repo = new Mock<IRepository<HACCPMaster>>();
            var recordList = GetHACCPMasterMockObjectAsync();
            repo.Setup(r => r.GetManyAsync(o => o.IsActive && !o.IsDeleted && (hACCPType == string.Empty || o.HACCPType.ToLower() == hACCPType), true)).Returns(recordList);
            return repo;
        }

        private async Task<ICollection<HACCPMaster>> GetHACCPMasterMockObjectAsync()
        {
            var recordList = GetHACCPMasterMockObject();
            return await Task.FromResult(recordList);
        }

        private ICollection<HACCPMaster> GetHACCPMasterMockObject()
        {
            var recordList = new Collection<HACCPMaster>()
            {
                new HACCPMaster(){ HACCPID = 1, HACCPType = "BIOLOGICAL", HACCPDescription = "cGMP" },
                new HACCPMaster(){ HACCPID = 2, HACCPType = "CHEMICAL", HACCPDescription = "Allergen - Milk" }
            };
            return recordList;
        }
        #endregion

        #region Setup Dummy Data & Repository - RMStatusMaster
        private Mock<IRepository<RMStatusMaster>> SetupRMStatusMasterRepository()
        {
            string rMStatusType = string.Empty;
            var repo = new Mock<IRepository<RMStatusMaster>>();
            var recordList = GetRMStatusMasterMockObjectAsync();
            repo.Setup(r => r.GetManyAsync(o => o.IsActive && !o.IsDeleted && (rMStatusType == string.Empty || o.RMStatusType.ToLower() == rMStatusType), true)).Returns(recordList);
            return repo;
        }

        private async Task<ICollection<RMStatusMaster>> GetRMStatusMasterMockObjectAsync()
        {
            var recordList = GetRMStatusMasterMockObject();
            return await Task.FromResult(recordList);
        }

        private ICollection<RMStatusMaster> GetRMStatusMasterMockObject()
        {
            var recordList = new Collection<RMStatusMaster>()
            {
                new RMStatusMaster(){ RMStatusMasterID = 1, RMStatusType = "STERILIZATION", RMStatus = "Steam" },
                new RMStatusMaster(){ RMStatusMasterID = 2, RMStatusType = "REGULATORY", RMStatus = "GRAS" }
            };
            return recordList;
        }
        #endregion

        #region Setup Dummy Data & Repository - StorageConditionMaster
        private Mock<IRepository<StorageConditionMaster>> SetupStorageConditionMasterRepository()
        {
            var repo = new Mock<IRepository<StorageConditionMaster>>();
            var recordList = GetStorageConditionMasterMockObjectAsync();
            repo.Setup(r => r.GetManyAsync(o => o.IsActive && !o.IsDeleted, true)).Returns(recordList);
            return repo;
        }

        private async Task<ICollection<StorageConditionMaster>> GetStorageConditionMasterMockObjectAsync()
        {
            var recordList = GetStorageConditionMasterMockObject();
            return await Task.FromResult(recordList);
        }

        private ICollection<StorageConditionMaster> GetStorageConditionMasterMockObject()
        {
            var recordList = new Collection<StorageConditionMaster>()
            {
                new StorageConditionMaster(){ StorageConditionID = 1, StorageDescription = "Cold Room" },
                new StorageConditionMaster(){ StorageConditionID = 2, StorageDescription = "Room Temp" }
            };
            return recordList;
        }
        #endregion

        #region Setup Dummy Data & Repository - CountryMaster
        private Mock<IRepository<CountryMaster>> SetupCountryMasterRepository()
        {
            var repo = new Mock<IRepository<CountryMaster>>();
            var recordList = GetCountryMasterMockObjectAsync();
            repo.Setup(r => r.GetManyAsync(o => o.IsActive && !o.IsDeleted, true)).Returns(recordList);
            return repo;
        }

        private async Task<ICollection<CountryMaster>> GetCountryMasterMockObjectAsync()
        {
            var recordList = GetCountryMasterMockObject();
            return await Task.FromResult(recordList);
        }

        private ICollection<CountryMaster> GetCountryMasterMockObject()
        {
            var recordList = new Collection<CountryMaster>()
            {
                new CountryMaster(){ CountryID = 1, CountryName = "None" },
                new CountryMaster(){ CountryID = 2, CountryName = "USA" },
                new CountryMaster(){ CountryID = 3, CountryName = "Argentina" },
                new CountryMaster(){ CountryID = 4, CountryName = "Australia" },
            };
            return recordList;
        }
        #endregion

        #region Setup Dummy Data & Repository - SiteProductionLineMapping
        private Mock<IRepository<SiteProductionLineMapping>> SetupSiteProductionLineMappingRepository()
        {
            var repo = new Mock<IRepository<SiteProductionLineMapping>>();
            IQueryable<SiteProductionLineMapping> queryableData = GetSiteProductionLineMockObject().AsQueryable();
            repo.Setup(r => r.Query(true)).Returns(queryableData);
            return repo;
        }

        private ICollection<SiteProductionLineMapping> GetSiteProductionLineMockObject()
        {
            var recordList = new Collection<SiteProductionLineMapping>()
            {
                new SiteProductionLineMapping()
                {
                    SiteProductionLineMapID = 1, ProductionLineID = 1, SiteID = 1,
                    ProductionLine = new ProductionLineMaster{ ProductionLineID = 1, LineCode = "L1", LineDescription = "Line 1", IsActive = true, IsDeleted = false }
                },
                new SiteProductionLineMapping()
                {
                    SiteProductionLineMapID = 2, ProductionLineID = 2, SiteID = 1,
                    ProductionLine = new ProductionLineMaster{ ProductionLineID = 2, LineCode = "L2", LineDescription = "Line 2", IsActive = true, IsDeleted = false }
                }
            };
            return recordList;
        }
        #endregion

        #region Setup Dummy Data & Repository - ProductionLineMixerMapping
        private Mock<IRepository<ProductionLineMixerMapping>> SetupProductionLineMixerMappingRepository()
        {
            var repo = new Mock<IRepository<ProductionLineMixerMapping>>();
            IQueryable<ProductionLineMixerMapping> queryableData = GetProductionLineMixerMockObject().AsQueryable();
            repo.Setup(r => r.Query(true)).Returns(queryableData);
            return repo;
        }

        private ICollection<ProductionLineMixerMapping> GetProductionLineMixerMockObject()
        {
            var recordList = new Collection<ProductionLineMixerMapping>()
            {
                new ProductionLineMixerMapping()
                {
                    ProductionLineMixerMapID = 1, ProductionMixerID = 1, SiteProductionLineID = 1,
                    ProductionMixerMaster = new ProductionMixerMaster(){ ProductionMixerID = 1, MixerDescription = "Mixer 1", IsActive = true, IsDeleted = false },
                    SiteProductionLine= new SiteProductionLineMapping()
                    {
                        SiteProductionLineMapID = 1, ProductionLineID = 1, SiteID = 1,
                        ProductionLine = new ProductionLineMaster(){ ProductionLineID = 1, LineCode = "L3", LineDescription = "Line 3", IsActive = true, IsDeleted = false }
                    }
                },
                new ProductionLineMixerMapping()
                {
                    ProductionLineMixerMapID = 2, ProductionMixerID = 2, SiteProductionLineID = 1,
                    ProductionMixerMaster = new ProductionMixerMaster(){ ProductionMixerID = 2, MixerDescription = "Mixer 2", IsActive = true, IsDeleted = false },
                    SiteProductionLine= new SiteProductionLineMapping()
                    {
                        SiteProductionLineMapID = 2, ProductionLineID = 1, SiteID = 1,
                        ProductionLine = new ProductionLineMaster(){ ProductionLineID = 1, LineCode = "L3", LineDescription = "Line 3", IsActive = true, IsDeleted = false }
                    }
                }
            };
            return recordList;
        }
        #endregion

        #region Setup Dummy Data & Repository - ReleaseAgentMaster
        private Mock<IRepository<ReleaseAgentMaster>> SetupReleaseAgentMasterRepository()
        {
            var repo = new Mock<IRepository<ReleaseAgentMaster>>();
            var recordList = GetReleaseAgentMasterMockObjectAsync();
            repo.Setup(r => r.GetManyAsync(o => o.IsActive && !o.IsDeleted, true)).Returns(recordList);
            return repo;
        }

        private async Task<ICollection<ReleaseAgentMaster>> GetReleaseAgentMasterMockObjectAsync()
        {
            var recordList = GetReleaseAgentMasterMockObject();
            return await Task.FromResult(recordList);
        }

        private ICollection<ReleaseAgentMaster> GetReleaseAgentMasterMockObject()
        {
            ICollection<ReleaseAgentMaster> recordList = new Collection<ReleaseAgentMaster>()
            {
                new ReleaseAgentMaster(){ ReleaseAgentID = 1, ReleaseAgentDescription = "Agent 1" },
                new ReleaseAgentMaster(){ ReleaseAgentID = 2, ReleaseAgentDescription = "Agent 2" }
            };
            return recordList;
        }
        #endregion

        #region Setup Dummy Data & Repository - PkoPercentageMaster
        private Mock<IRepository<PkoPercentageMaster>> SetupPkoPercentageMasterRepository()
        {
            var repo = new Mock<IRepository<PkoPercentageMaster>>();
            var recordList = GetPkoPercentageMasterMockObjectAsync();
            repo.Setup(r => r.GetManyAsync(o => o.IsActive && !o.IsDeleted, true)).Returns(recordList);
            return repo;
        }

        private async Task<ICollection<PkoPercentageMaster>> GetPkoPercentageMasterMockObjectAsync()
        {
            var recordList = GetPkoPercentageMasterMockObject();
            return await Task.FromResult(recordList);
        }

        private ICollection<PkoPercentageMaster> GetPkoPercentageMasterMockObject()
        {
            ICollection<PkoPercentageMaster> recordList = new Collection<PkoPercentageMaster>()
            {
                new PkoPercentageMaster(){ PkoPercentageMasterID = 1, PkoPercentageDescription = "1%" },
                new PkoPercentageMaster(){ PkoPercentageMasterID = 2, PkoPercentageDescription = "2%" }
            };
            return recordList;
        }
        #endregion

        #region Setup Dummy Data & Repository - BarFormatMaster
        private Mock<IRepository<BarFormatMaster>> SetupBarFormatMasterRepository()
        {
            var repo = new Mock<IRepository<BarFormatMaster>>();
            var recordList = GetBarFormatMasterMockObjectAsync();
            repo.Setup(r => r.GetManyAsync(o => o.IsActive && !o.IsDeleted, true)).Returns(recordList);
            return repo;
        }

        private Mock<IRepository<BarFormatCodeMaster>> SetupBarFormatCodeMasterRepository()
        {
            var repo = new Mock<IRepository<BarFormatCodeMaster>>();
            var recordListBarFormatCode = GetBarFormatCodeMasterMockObjectAsync();
            repo.Setup(r => r.GetManyAsync(o => o.IsActive && !o.IsDeleted, true)).Returns(recordListBarFormatCode);
            return repo;
        }

        private async Task<ICollection<BarFormatMaster>> GetBarFormatMasterMockObjectAsync()
        {
            var recordList = GetBarFormatMasterMockObject();
            return await Task.FromResult(recordList);
        }

        private async Task<ICollection<BarFormatCodeMaster>> GetBarFormatCodeMasterMockObjectAsync()
        {
            var recordList = GetBarFormatCodeMasterMockObject();
            return await Task.FromResult(recordList);
        }

        private ICollection<BarFormatMaster> GetBarFormatMasterMockObject()
        {
            ICollection<BarFormatMaster> recordList = new Collection<BarFormatMaster>()
            {
                new BarFormatMaster(){ BarFormatCode ="1", BarFormatDescription="N/A", BarFormatID=1, BarFormatType="Equipment", DisplayOrder=1  },
                new BarFormatMaster(){ BarFormatCode ="2", BarFormatDescription="WEB 1", BarFormatID=1, BarFormatType="Equipment", DisplayOrder=2  },
                new BarFormatMaster(){ BarFormatCode ="3", BarFormatDescription="WEB 1 + Readco", BarFormatID=1, BarFormatType="Equipment", DisplayOrder=2  }
            };
            return recordList;
        }
        
        private ICollection<BarFormatCodeMaster> GetBarFormatCodeMasterMockObject()
        {
            ICollection<BarFormatCodeMaster> recordList = new Collection<BarFormatCodeMaster>()
            {
                new BarFormatCodeMaster(){ BarFormatCode="1111", BarFormatCodeID=1, BarFormatDescription="Unspecified Format", IsActive=true  },
                new BarFormatCodeMaster(){ BarFormatCode="2212", BarFormatCodeID=1, BarFormatDescription="Single dough layer", IsActive=true  },
                new BarFormatCodeMaster(){ BarFormatCode="2213", BarFormatCodeID=1, BarFormatDescription="Single cereal layer", IsActive=true  },
            };
            return recordList;
        }
        #endregion

        #region Setup Dummy Data & Repository - FormulaTypeMaster      
        private Mock<IRepository<FormulaTypeMaster>> SetupFormulaTypeMasterRepository()
        {
            var repo = new Mock<IRepository<FormulaTypeMaster>>();
            var recordList = GetFormulaTypeMasterMockObjectAsync();
            repo.Setup(r => r.GetManyAsync(o => o.IsActive && !o.IsDeleted, true)).Returns(recordList);
            return repo;
        }

        private async Task<ICollection<FormulaTypeMaster>> GetFormulaTypeMasterMockObjectAsync()
        {
            var recordList = GetFormulaTypeMasterMockObject();
            return await Task.FromResult(recordList);
        }

        private ICollection<FormulaTypeMaster> GetFormulaTypeMasterMockObject()
        {
            var recordList = new Collection<FormulaTypeMaster>()
            {
                new FormulaTypeMaster()
                {
                    FormulaTypeCode = "S10",
                    FormulaDescription = "Complete S10_",
                    CreatedBy = 2, CreatedOn = DateTime.Now,
                    IsActive = true, IsDeleted = false, FormulaTypeID = 1,
                    FormulaChangeCode = new List<FormulaChangeCode>(),
                    FormulaMaster = new List<FormulaMaster>(),
                    FormulaTypeProductMapping = new List<FormulaTypeProductMapping>(),
                    UpdatedBy = 8,
                    UpdatedOn = DateTime.Now
                }
            };
            return recordList;
        }
        #endregion

        #region Setup Dummy Data & Repository - SitterWidthMaster
        private Mock<IRepository<SitterWidthMaster>> SetupSitterWidthMasterRepository()
        {
            int siteID = 0;
            var repo = new Mock<IRepository<SitterWidthMaster>>();
            var recordList = GetSitterWidthMasterMockObjectAsync();
            repo.Setup(r => r.GetManyAsync(o => o.IsActive && !o.IsDeleted && (siteID == 0 || o.SiteID == siteID), true)).Returns(recordList);
            return repo;
        }

        private async Task<ICollection<SitterWidthMaster>> GetSitterWidthMasterMockObjectAsync()
        {
            var recordList = GetSitterWidthMasterMockObject();
            return await Task.FromResult(recordList);
        }

        private ICollection<SitterWidthMaster> GetSitterWidthMasterMockObject()
        {
            var recordList = new Collection<SitterWidthMaster>()
            {
                new SitterWidthMaster(){ SitterWidthID = 1, SiteID = 1, SitterWidth = "ONT: 22" },
                new SitterWidthMaster(){ SitterWidthID = 2, SiteID = 1, SitterWidth = "ONT: 28" }
            };
            return recordList;
        }
        #endregion

        #region Setup Dummy Data & Repository - ExtrusionDieMaster
        private Mock<IRepository<ExtrusionDieMaster>> SetupExtrusionDieMasterRepository()
        {
            var repo = new Mock<IRepository<ExtrusionDieMaster>>();
            var recordList = GetExtrusionDieMasterMockObjectAsync();
            repo.Setup(r => r.GetManyAsync(o => o.IsActive && !o.IsDeleted, true)).Returns(recordList);
            return repo;
        }

        private async Task<ICollection<ExtrusionDieMaster>> GetExtrusionDieMasterMockObjectAsync()
        {
            var recordList = GetExtrusionDieMasterMockObject();
            return await Task.FromResult(recordList);
        }

        private ICollection<ExtrusionDieMaster> GetExtrusionDieMasterMockObject()
        {
            var recordList = new Collection<ExtrusionDieMaster>()
            {
                new ExtrusionDieMaster(){ ExtrusionDieID = 1, ExtrusionDie = "125" },
                new ExtrusionDieMaster(){ ExtrusionDieID = 2, ExtrusionDie = "130" }
            };
            return recordList;
        }
        #endregion
        #region Setup Dummy Data & Repository - InternalQCMAVLookUpMaster
        private Mock<IRepository<InternalQCMAVLookUpMaster>> SetupInternalQCMAVLookUpMasterRepository()
        {
            var repo = new Mock<IRepository<InternalQCMAVLookUpMaster>>();
            var recordList = GetInternalQCMAVLookUpMasterMockObjectAsync();
            repo.Setup(r => r.GetManyAsync(o => o.IsActive, true)).Returns(recordList);
            return repo;
        }

        private async Task<ICollection<InternalQCMAVLookUpMaster>> GetInternalQCMAVLookUpMasterMockObjectAsync()
        {
            var recordList = GetInternalQCMAVLookUpMasterMockObject();
            return await Task.FromResult(recordList);
        }

        private ICollection<InternalQCMAVLookUpMaster> GetInternalQCMAVLookUpMasterMockObject()
        {
            var recordList = new Collection<InternalQCMAVLookUpMaster>()
            {
                new InternalQCMAVLookUpMaster(){ Subtract=13, TotalBarWeight=125, CreatedBy=1, InternalQCMAVLookUpMasterID=1, IsActive =true},
                new InternalQCMAVLookUpMaster(){ Subtract = 43, TotalBarWeight = 1200, CreatedBy = 1, InternalQCMAVLookUpMasterID = 2, IsActive = true }
            };
            return recordList;
        }
        #endregion
    }
}