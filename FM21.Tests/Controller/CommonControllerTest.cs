using FM21.API.Controllers;
using FM21.Core;
using FM21.Core.Model;
using FM21.Entities;
using FM21.Service;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FM21.Tests
{
    [TestFixture]
    public class CommonControllerTest
    {
        private CommonController commonController;
        private Mock<ICommonService> commonService;

        [SetUp]
        public void SetUp()
        {
            commonService = new Mock<ICommonService>();
        }

        [Test]
        public async Task Should_Return_All_Site_Records()
        {
            var returnObject = GetSiteMockData();
            commonService = new Mock<ICommonService>();
            commonService.Setup(t => t.GetAllSite()).ReturnsAsync(returnObject);

            commonController = new CommonController(commonService.Object);
            var response = await commonController.GetAllSite() as JsonResult;

            Assert.IsNotNull(response);
            Assert.AreEqual(returnObject.Data, ((GeneralResponse<ICollection<SiteMaster>>)response.Value).Data);
            Assert.AreEqual(returnObject.Data.Count, ((GeneralResponse<ICollection<SiteMaster>>)response.Value).Data.Count);
            Assert.AreEqual(returnObject.Result, ((GeneralResponse<ICollection<SiteMaster>>)response.Value).Result);
        }

        [Test]
        public async Task Should_Return_All_ProductType_Records()
        {
            var returnObject = GetProductTypeMockData();
            commonService = new Mock<ICommonService>();
            commonService.Setup(t => t.GetAllProductType()).ReturnsAsync(returnObject);

            commonController = new CommonController(commonService.Object);
            var response = await commonController.GetAllProductType() as JsonResult;

            Assert.IsNotNull(response);
            Assert.AreEqual(returnObject.Data, ((GeneralResponse<ICollection<ProductTypeMaster>>)response.Value).Data);
            Assert.AreEqual(returnObject.Data.Count, ((GeneralResponse<ICollection<ProductTypeMaster>>)response.Value).Data.Count);
            Assert.AreEqual(returnObject.Result, ((GeneralResponse<ICollection<ProductTypeMaster>>)response.Value).Result);
        }

        [Test]
        public async Task Should_Return_All_SiteProductType_Records()
        {
          
            var returnObject = new GeneralResponse<ICollection<SiteProductTypeModel>>();
            returnObject.Data = new List<SiteProductTypeModel>();
            returnObject.Data.Add(new SiteProductTypeModel() { SiteID = 1, ProductTypeID = 1, SiteProductType = "ONT Bar", ProductType="", SiteCode="", SiteName="ONT", SiteProductMapID=1 });
            returnObject.Data.Add(new SiteProductTypeModel() { SiteID = 2, ProductTypeID = 1, SiteProductType = "LAC Bar", ProductType = "", SiteCode = "", SiteName = "LAC", SiteProductMapID = 1 });
            commonService = new Mock<ICommonService>();
            commonService.Setup(t => t.GetAllSiteProductType(false)).ReturnsAsync(returnObject);

            commonController = new CommonController(commonService.Object);
            var response = await commonController.GetAllSiteProductType() as JsonResult;

            Assert.IsNotNull(response);
            Assert.AreEqual(returnObject.Data, ((GeneralResponse<ICollection<SiteProductTypeModel>>)response.Value).Data);
            Assert.AreEqual(returnObject.Data.Count, ((GeneralResponse<ICollection<SiteProductTypeModel>>)response.Value).Data.Count);
            Assert.AreEqual(returnObject.Result, ((GeneralResponse<ICollection<SiteProductTypeModel>>)response.Value).Result);
          
        }

        [Test]
        public async Task Should_Return_All_InstructionCategory_Records()
        {
            var returnObject = GetInstructionCategoryMockData();
            commonService = new Mock<ICommonService>();
            commonService.Setup(t => t.GetAllInstructionCategory()).ReturnsAsync(returnObject);

            commonController = new CommonController(commonService.Object);
            var response = await commonController.GetAllInstructionCategory() as JsonResult;

            Assert.IsNotNull(response);
            Assert.AreEqual(returnObject.Data, ((GeneralResponse<ICollection<InstructionCategoryMaster>>)response.Value).Data);
            Assert.AreEqual(returnObject.Data.Count, ((GeneralResponse<ICollection<InstructionCategoryMaster>>)response.Value).Data.Count);
            Assert.AreEqual(returnObject.Result, ((GeneralResponse<ICollection<InstructionCategoryMaster>>)response.Value).Result);
        }

        [Test]
        public async Task Should_Return_All_InstructionCategory_Record_BySiteProductMapID()
        {
            var returnObject = GetInstructionCategoryMockData();
            commonService = new Mock<ICommonService>();
            commonService.Setup(t => t.GetAllInstructionCategoryBySiteProductMapID(1)).ReturnsAsync(returnObject);

            commonController = new CommonController(commonService.Object);
            var response = await commonController.GetAllInstructionCategoryBySiteProductMapID(1) as JsonResult;

            Assert.IsNotNull(response);
            Assert.AreEqual(returnObject.Data, ((GeneralResponse<ICollection<InstructionCategoryMaster>>)response.Value).Data);
            Assert.AreEqual(returnObject.Data.Count, ((GeneralResponse<ICollection<InstructionCategoryMaster>>)response.Value).Data.Count);
            Assert.AreEqual(returnObject.Result, ((GeneralResponse<ICollection<InstructionCategoryMaster>>)response.Value).Result);
        }

        [Test]
        public async Task Should_Return_All_BrokerMaster_Records()
        {
            var returnObject = GetBrokerMasterMockData();
            commonService = new Mock<ICommonService>();
            commonService.Setup(t => t.GetAllBroker()).ReturnsAsync(returnObject);

            commonController = new CommonController(commonService.Object);
            var response = await commonController.GetAllBroker() as JsonResult;

            Assert.IsNotNull(response);
            Assert.AreEqual(returnObject.Data, ((GeneralResponse<ICollection<BrokerMaster>>)response.Value).Data);
            Assert.AreEqual(returnObject.Data.Count, ((GeneralResponse<ICollection<BrokerMaster>>)response.Value).Data.Count);
            Assert.AreEqual(returnObject.Result, ((GeneralResponse<ICollection<BrokerMaster>>)response.Value).Result);
        }

        [Test]
        public async Task Should_Return_All_KosherCodeMaster_Records()
        {
            var returnObject = GetKosherCodeMasterMockData();
            commonService = new Mock<ICommonService>();
            commonService.Setup(t => t.GetAllKosherCode()).ReturnsAsync(returnObject);

            commonController = new CommonController(commonService.Object);
            var response = await commonController.GetAllKosherCode() as JsonResult;

            Assert.IsNotNull(response);
            Assert.AreEqual(returnObject.Data, ((GeneralResponse<ICollection<KosherCodeMaster>>)response.Value).Data);
            Assert.AreEqual(returnObject.Data.Count, ((GeneralResponse<ICollection<KosherCodeMaster>>)response.Value).Data.Count);
            Assert.AreEqual(returnObject.Result, ((GeneralResponse<ICollection<KosherCodeMaster>>)response.Value).Result);
        }

        [Test]
        public async Task Should_Return_All_HACCPMaster_Records()
        {
            var returnObject = GetHACCPMasterMockData();
            commonService = new Mock<ICommonService>();
            commonService.Setup(t => t.GetAllHACCPData("")).ReturnsAsync(returnObject);

            commonController = new CommonController(commonService.Object);
            var response = await commonController.GetAllHACCPData() as JsonResult;

            Assert.IsNotNull(response);
            Assert.AreEqual(returnObject.Data, ((GeneralResponse<ICollection<HACCPMaster>>)response.Value).Data);
            Assert.AreEqual(returnObject.Data.Count, ((GeneralResponse<ICollection<HACCPMaster>>)response.Value).Data.Count);
            Assert.AreEqual(returnObject.Result, ((GeneralResponse<ICollection<HACCPMaster>>)response.Value).Result);
        }

        [Test]
        public async Task Should_Return_All_RMStatusMaster_Records()
        {
            var returnObject = GetRMStatusMasterMockData();
            commonService = new Mock<ICommonService>();
            commonService.Setup(t => t.GetAllRMStatus("")).ReturnsAsync(returnObject);

            commonController = new CommonController(commonService.Object);
            var response = await commonController.GetAllRMStatus() as JsonResult;

            Assert.IsNotNull(response);
            Assert.AreEqual(returnObject.Data, ((GeneralResponse<ICollection<RMStatusMaster>>)response.Value).Data);
            Assert.AreEqual(returnObject.Data.Count, ((GeneralResponse<ICollection<RMStatusMaster>>)response.Value).Data.Count);
            Assert.AreEqual(returnObject.Result, ((GeneralResponse<ICollection<RMStatusMaster>>)response.Value).Result);
        }

        [Test]
        public async Task Should_Return_All_StorageConditionMaster_Records()
        {
            var returnObject = GetStorageConditionMasterMockData();
            commonService = new Mock<ICommonService>();
            commonService.Setup(t => t.GetAllStorageCondition()).ReturnsAsync(returnObject);

            commonController = new CommonController(commonService.Object);
            var response = await commonController.GetAllStorageCondition() as JsonResult;

            Assert.IsNotNull(response);
            Assert.AreEqual(returnObject.Data, ((GeneralResponse<ICollection<StorageConditionMaster>>)response.Value).Data);
            Assert.AreEqual(returnObject.Data.Count, ((GeneralResponse<ICollection<StorageConditionMaster>>)response.Value).Data.Count);
            Assert.AreEqual(returnObject.Result, ((GeneralResponse<ICollection<StorageConditionMaster>>)response.Value).Result);
        }

        [Test]
        public async Task Should_Return_All_CountryMaster()
        {
            var returnObject = GetCountryMasterMockData();
            commonService = new Mock<ICommonService>();
            commonService.Setup(t => t.GetAllCountry()).ReturnsAsync(returnObject);

            commonController = new CommonController(commonService.Object);
            var response = await commonController.GetAllCountry() as JsonResult;

            Assert.IsNotNull(response);
            Assert.AreEqual(returnObject.Data, ((GeneralResponse<ICollection<CountryMaster>>)response.Value).Data);
            Assert.AreEqual(returnObject.Data.Count, ((GeneralResponse<ICollection<CountryMaster>>)response.Value).Data.Count);
            Assert.AreEqual(returnObject.Result, ((GeneralResponse<ICollection<CountryMaster>>)response.Value).Result);
        }

        [Test]
        public async Task Should_Return_All_Production_Line_By_Site()
        {
            var returnObject = GetSiteProductionLineMockObject();
            commonService = new Mock<ICommonService>();
            commonService.Setup(t => t.GetAllProductionLineBySite(1, 1)).ReturnsAsync(returnObject);

            commonController = new CommonController(commonService.Object);
            var response = await commonController.GetAllProductionLineBySite(1, 1) as JsonResult;

            Assert.IsNotNull(response);
            Assert.AreEqual(returnObject.Data, ((GeneralResponse<ICollection<ProductionLineMasterModel>>)response.Value).Data);
            Assert.AreEqual(returnObject.Data.Count, ((GeneralResponse<ICollection<ProductionLineMasterModel>>)response.Value).Data.Count);
            Assert.AreEqual(returnObject.Result, ((GeneralResponse<ICollection<ProductionLineMasterModel>>)response.Value).Result);
        }

        [Test]
        public async Task Should_Return_All_Production_Line_Mixer_By_Site()
        {
            var returnObject = GetProductionLineMixerMockObject();
            commonService = new Mock<ICommonService>();
            commonService.Setup(t => t.GetAllProductionLineMixerBySite(1)).ReturnsAsync(returnObject);

            commonController = new CommonController(commonService.Object);
            var response = await commonController.GetAllProductionLineMixerBySite(1) as JsonResult;

            Assert.IsNotNull(response);
            Assert.AreEqual(returnObject.Data, ((GeneralResponse<ICollection<ProductionLineMixerModel>>)response.Value).Data);
            Assert.AreEqual(returnObject.Data.Count, ((GeneralResponse<ICollection<ProductionLineMixerModel>>)response.Value).Data.Count);
            Assert.AreEqual(returnObject.Result, ((GeneralResponse<ICollection<ProductionLineMixerModel>>)response.Value).Result);
        }

        [Test]
        public async Task Should_Return_All_Release_Agent()
        {
            var returnObject = GetReleaseAgentMasterMockObject();
            commonService = new Mock<ICommonService>();
            commonService.Setup(t => t.GetAllReleaseAgent()).ReturnsAsync(returnObject);

            commonController = new CommonController(commonService.Object);
            var response = await commonController.GetAllReleaseAgent() as JsonResult;

            Assert.IsNotNull(response);
            Assert.AreEqual(returnObject.Data, ((GeneralResponse<ICollection<ReleaseAgentMaster>>)response.Value).Data);
            Assert.AreEqual(returnObject.Data.Count, ((GeneralResponse<ICollection<ReleaseAgentMaster>>)response.Value).Data.Count);
            Assert.AreEqual(returnObject.Result, ((GeneralResponse<ICollection<ReleaseAgentMaster>>)response.Value).Result);
        }

        [Test]
        public async Task Should_Return_All_PkoPercentage()
        {
            var returnObject = GetPkoPercentageMasterMockObject();
            commonService = new Mock<ICommonService>();
            commonService.Setup(t => t.GetAllPkoPercentage()).ReturnsAsync(returnObject);

            commonController = new CommonController(commonService.Object);
            var response = await commonController.GetAllPkoPercentage() as JsonResult;

            Assert.IsNotNull(response);
            Assert.AreEqual(returnObject.Data, ((GeneralResponse<ICollection<PkoPercentageMaster>>)response.Value).Data);
            Assert.AreEqual(returnObject.Data.Count, ((GeneralResponse<ICollection<PkoPercentageMaster>>)response.Value).Data.Count);
            Assert.AreEqual(returnObject.Result, ((GeneralResponse<ICollection<PkoPercentageMaster>>)response.Value).Result);
        }

        [Test]
        public async Task Should_Return_All_FormulaTypeMaster_Records()
        {
            var returnObject = GetFormulaTypeMasterMockData();
            commonService = new Mock<ICommonService>();
            commonService.Setup(t => t.GetAllFormulaTypeMaster()).ReturnsAsync(returnObject);

            commonController = new CommonController(commonService.Object);
            var response = await commonController.GetAllFormulaTypeMaster() as JsonResult;

            Assert.IsNotNull(response);
            Assert.AreEqual(returnObject.Data, ((GeneralResponse<ICollection<FormulaTypeMaster>>)response.Value).Data);
            Assert.AreEqual(returnObject.Data.Count, ((GeneralResponse<ICollection<FormulaTypeMaster>>)response.Value).Data.Count);
            Assert.AreEqual(returnObject.Result, ((GeneralResponse<ICollection<FormulaTypeMaster>>)response.Value).Result);
        }

        [Test]
        public async Task Should_Return_All_SitterWidthMaster_Records()
        {
            var returnObject = GetSitterWidthMasterMockData();
            commonService = new Mock<ICommonService>();
            commonService.Setup(t => t.GetAllSitterWidth(0)).ReturnsAsync(returnObject);

            commonController = new CommonController(commonService.Object);
            var response = await commonController.GetAllSitterWidth(0) as JsonResult;

            Assert.IsNotNull(response);
            Assert.AreEqual(returnObject.Data, ((GeneralResponse<ICollection<SitterWidthMaster>>)response.Value).Data);
            Assert.AreEqual(returnObject.Data.Count, ((GeneralResponse<ICollection<SitterWidthMaster>>)response.Value).Data.Count);
            Assert.AreEqual(returnObject.Result, ((GeneralResponse<ICollection<SitterWidthMaster>>)response.Value).Result);
        }

        [Test]
        public async Task Should_Return_All_ExtrusionDieMaster_Records()
        {
            var returnObject = GetExtrusionDieMasterMockData();
            commonService = new Mock<ICommonService>();
            commonService.Setup(t => t.GetAllExtrusionDie()).ReturnsAsync(returnObject);

            commonController = new CommonController(commonService.Object);
            var response = await commonController.GetAllExtrusionDie() as JsonResult;

            Assert.IsNotNull(response);
            Assert.AreEqual(returnObject.Data, ((GeneralResponse<ICollection<ExtrusionDieMaster>>)response.Value).Data);
            Assert.AreEqual(returnObject.Data.Count, ((GeneralResponse<ICollection<ExtrusionDieMaster>>)response.Value).Data.Count);
            Assert.AreEqual(returnObject.Result, ((GeneralResponse<ICollection<ExtrusionDieMaster>>)response.Value).Result);
        }
        
        [Test]
        public async Task Should_Return_All_BarFormatMaster_Records()
        {
            var returnObject = GetBarFormatMasterMockData();
            commonService = new Mock<ICommonService>();
            commonService.Setup(t => t.GetAllBarFormatMaster()).ReturnsAsync(returnObject);

            commonController = new CommonController(commonService.Object);
            var response = await commonController.GetAllBarFormatMaster() as JsonResult;

            Assert.IsNotNull(response);
            Assert.AreEqual(returnObject.Data, ((GeneralResponse<ICollection<BarFormatMaster>>)response.Value).Data);
            Assert.AreEqual(returnObject.Data.Count, ((GeneralResponse<ICollection<BarFormatMaster>>)response.Value).Data.Count);
            Assert.AreEqual(returnObject.Result, ((GeneralResponse<ICollection<BarFormatMaster>>)response.Value).Result);
        }

        [Test]
        public async Task Should_Return_All_BarFormatCodeMaster_Records()
        {
            var returnObject = GetBarFormatCodeMasterMockData();
            commonService = new Mock<ICommonService>();
            commonService.Setup(t => t.GetAllBarFormatCodeMaster()).ReturnsAsync(returnObject);

            commonController = new CommonController(commonService.Object);
            var response = await commonController.GetAllBarFormatCodeMaster() as JsonResult;

            Assert.IsNotNull(response);
            Assert.AreEqual(returnObject.Data, ((GeneralResponse<ICollection<BarFormatCodeMaster>>)response.Value).Data);
            Assert.AreEqual(returnObject.Data.Count, ((GeneralResponse<ICollection<BarFormatCodeMaster>>)response.Value).Data.Count);
            Assert.AreEqual(returnObject.Result, ((GeneralResponse<ICollection<BarFormatCodeMaster>>)response.Value).Result);
        }
        [Test]
        public async Task Should_Return_All_Internal_QC_MAV_LookUp_Records()
        {
            var returnObject = GetInternalQCMAVLookUpMasterMockData();
            commonService = new Mock<ICommonService>();
            commonService.Setup(t => t.GetAllInternalQCMAVLookUp()).ReturnsAsync(returnObject);

            commonController = new CommonController(commonService.Object);
            var response = await commonController.GetAllInternalQCMAVLookUp() as JsonResult;

            Assert.IsNotNull(response);
            Assert.AreEqual(returnObject.Data, ((GeneralResponse<ICollection<InternalQCMAVLookUpMaster>>)response.Value).Data);
            Assert.AreEqual(returnObject.Data.Count, ((GeneralResponse<ICollection<InternalQCMAVLookUpMaster>>)response.Value).Data.Count);
            Assert.AreEqual(returnObject.Result, ((GeneralResponse<ICollection<InternalQCMAVLookUpMaster>>)response.Value).Result);
        }

        #region Mock Data
        private GeneralResponse<ICollection<SiteMaster>> GetSiteMockData()
        {
            var response = new GeneralResponse<ICollection<SiteMaster>>();
            response.Data = new List<SiteMaster>();
            response.Data.Add(new SiteMaster() { SiteID = 1, SiteCode = "ONT" });
            response.Data.Add(new SiteMaster() { SiteID = 2, SiteCode = "LAC" });
            return response;
        }

        private GeneralResponse<ICollection<ProductTypeMaster>> GetProductTypeMockData()
        {
            var response = new GeneralResponse<ICollection<ProductTypeMaster>>();
            response.Data = new List<ProductTypeMaster>();
            response.Data.Add(new ProductTypeMaster() { ProductTypeID = 1, ProductType = "Bar" });
            response.Data.Add(new ProductTypeMaster() { ProductTypeID = 1, ProductType = "Powder" });
            return response;
        }

        private GeneralResponse<ICollection<InstructionCategoryMaster>> GetInstructionCategoryMockData()
        {
            var response = new GeneralResponse<ICollection<InstructionCategoryMaster>>();
            response.Data = new List<InstructionCategoryMaster>();
            response.Data.Add(new InstructionCategoryMaster() { InstructionCategoryID = 1, InstructionCategory = "Cat 1" });
            response.Data.Add(new InstructionCategoryMaster() { InstructionCategoryID = 2, InstructionCategory = "Cat 2" });
            return response;
        }

        private GeneralResponse<ICollection<BrokerMaster>> GetBrokerMasterMockData()
        {
            var response = new GeneralResponse<ICollection<BrokerMaster>>();
            response.Data = new List<BrokerMaster>();
            response.Data.Add(new BrokerMaster() { BrokerID = 1, BrokerName = "Broker 1" });
            response.Data.Add(new BrokerMaster() { BrokerID = 2, BrokerName = "Broker 2" });
            return response;
        }

        private GeneralResponse<ICollection<KosherCodeMaster>> GetKosherCodeMasterMockData()
        {
            var response = new GeneralResponse<ICollection<KosherCodeMaster>>();
            response.Data = new List<KosherCodeMaster>();
            response.Data.Add(new KosherCodeMaster() { KosherCodeID = 1, KosherCode = "K001" });
            response.Data.Add(new KosherCodeMaster() { KosherCodeID = 2, KosherCode = "K002" });
            return response;
        }

        private GeneralResponse<ICollection<HACCPMaster>> GetHACCPMasterMockData()
        {
            var response = new GeneralResponse<ICollection<HACCPMaster>>();
            response.Data = new List<HACCPMaster>();
            response.Data.Add(new HACCPMaster() { HACCPID = 1, HACCPType = "BIOLOGICAL", HACCPDescription = "cGMP" });
            response.Data.Add(new HACCPMaster() { HACCPID = 1, HACCPType = "BIOLOGICAL", HACCPDescription = "fill" });
            return response;
        }

        private GeneralResponse<ICollection<RMStatusMaster>> GetRMStatusMasterMockData()
        {
            var response = new GeneralResponse<ICollection<RMStatusMaster>>();
            response.Data = new List<RMStatusMaster>();
            response.Data.Add(new RMStatusMaster() { RMStatusMasterID = 1, RMStatusType = "STERILIZATION", RMStatus = "Steam" });
            response.Data.Add(new RMStatusMaster() { RMStatusMasterID = 2, RMStatusType = "REGULATORY", RMStatus = "GRAS" });
            return response;
        }

        private GeneralResponse<ICollection<StorageConditionMaster>> GetStorageConditionMasterMockData()
        {
            var response = new GeneralResponse<ICollection<StorageConditionMaster>>();
            response.Data = new List<StorageConditionMaster>();
            response.Data.Add(new StorageConditionMaster() { StorageConditionID = 1, StorageDescription = "Cold Room" });
            response.Data.Add(new StorageConditionMaster() { StorageConditionID = 2, StorageDescription = "Room Temp" });
            return response;
        }

        private GeneralResponse<ICollection<CountryMaster>> GetCountryMasterMockData()
        {
            var response = new GeneralResponse<ICollection<CountryMaster>>();
            response.Data = new List<CountryMaster>();
            var recordList = new List<CountryMaster>()
            {
                new CountryMaster(){ CountryID = 1, CountryName = "None" },
                new CountryMaster(){ CountryID = 2, CountryName = "USA" },
                new CountryMaster(){ CountryID = 3, CountryName = "Argentina" },
                new CountryMaster(){ CountryID = 4, CountryName = "Australia" },
            };
            response.Data = recordList;
            return response;
        }

        private GeneralResponse<ICollection<ProductionLineMasterModel>> GetSiteProductionLineMockObject()
        {
            var response = new GeneralResponse<ICollection<ProductionLineMasterModel>>();
            response.Data = new List<ProductionLineMasterModel>();
            response.Data.Add(new ProductionLineMasterModel() { ProductionLineID = 1, LineCode = "L1", LineDescription = "Line 1" });
            response.Data.Add(new ProductionLineMasterModel() { ProductionLineID = 2, LineCode = "L2", LineDescription = "Line 2" });
            return response;
        }

        private GeneralResponse<ICollection<ProductionLineMixerModel>> GetProductionLineMixerMockObject()
        {
            var response = new GeneralResponse<ICollection<ProductionLineMixerModel>>();
            response.Data = new List<ProductionLineMixerModel>();
            response.Data.Add(new ProductionLineMixerModel() { ProductionMixerID = 1, ProductionLineID = 1, MixerDescription = "Mixer 1" });
            response.Data.Add(new ProductionLineMixerModel() { ProductionMixerID = 2, ProductionLineID = 2, MixerDescription = "Mixer 2" });
            return response;
        }

        private GeneralResponse<ICollection<ReleaseAgentMaster>> GetReleaseAgentMasterMockObject()
        {
            var response = new GeneralResponse<ICollection<ReleaseAgentMaster>>();
            response.Data = new List<ReleaseAgentMaster>();
            response.Data.Add(new ReleaseAgentMaster() { ReleaseAgentID = 1, ReleaseAgentDescription = "Agent 1" });
            response.Data.Add(new ReleaseAgentMaster() { ReleaseAgentID = 2, ReleaseAgentDescription = "Agent 2" });
            return response;
        }

        private GeneralResponse<ICollection<PkoPercentageMaster>> GetPkoPercentageMasterMockObject()
        {
            var response = new GeneralResponse<ICollection<PkoPercentageMaster>>();
            response.Data = new List<PkoPercentageMaster>();
            response.Data.Add(new PkoPercentageMaster() { PkoPercentageMasterID = 1, PkoPercentageDescription = "%1" });
            response.Data.Add(new PkoPercentageMaster() { PkoPercentageMasterID = 2, PkoPercentageDescription = "%2" });
            return response;
        }

        private GeneralResponse<ICollection<FormulaTypeMaster>> GetFormulaTypeMasterMockData()
        {
            var response = new GeneralResponse<ICollection<FormulaTypeMaster>>();
            response.Data = new List<FormulaTypeMaster>();
            response.Data.Add(new FormulaTypeMaster() { FormulaTypeID = 1, FormulaTypeCode = "Type1", FormulaDescription = "S30" });
            response.Data.Add(new FormulaTypeMaster() { FormulaTypeID = 2, FormulaTypeCode = "Type2", FormulaDescription = "S70" });
            return response;
        }

        private GeneralResponse<ICollection<SitterWidthMaster>> GetSitterWidthMasterMockData()
        {
            var response = new GeneralResponse<ICollection<SitterWidthMaster>>();
            response.Data = new List<SitterWidthMaster>();
            response.Data.Add(new SitterWidthMaster() { SitterWidthID = 1, SiteID = 1, SitterWidth = "ONT: 22" });
            response.Data.Add(new SitterWidthMaster() { SitterWidthID = 2, SiteID = 1, SitterWidth = "ONT: 28" });
            return response;
        }

        private GeneralResponse<ICollection<ExtrusionDieMaster>> GetExtrusionDieMasterMockData()
        {
            var response = new GeneralResponse<ICollection<ExtrusionDieMaster>>();
            response.Data = new List<ExtrusionDieMaster>();
            response.Data.Add(new ExtrusionDieMaster() { ExtrusionDieID = 1, ExtrusionDie = "125" });
            response.Data.Add(new ExtrusionDieMaster() { ExtrusionDieID = 1, ExtrusionDie = "130" });
            return response;
        }

        private GeneralResponse<ICollection<BarFormatMaster>> GetBarFormatMasterMockData()
        {
            var response = new GeneralResponse<ICollection<BarFormatMaster>>();
            response.Data = new List<BarFormatMaster>();
            response.Data.Add(new BarFormatMaster() { BarFormatCode = "1", BarFormatDescription = "N/A", BarFormatID = 1, BarFormatType = "Equipment", DisplayOrder = 1 });
            response.Data.Add(new BarFormatMaster() { BarFormatCode = "2", BarFormatDescription = "WEB 1", BarFormatID = 1, BarFormatType = "Equipment", DisplayOrder = 2 });
            response.Data.Add(new BarFormatMaster() { BarFormatCode = "3", BarFormatDescription = "WEB 1 + Readco", BarFormatID = 1, BarFormatType = "Equipment", DisplayOrder = 2 });
            return response;
        }
        private GeneralResponse<ICollection<BarFormatCodeMaster>> GetBarFormatCodeMasterMockData()
        {
            var response = new GeneralResponse<ICollection<BarFormatCodeMaster>>();
            response.Data = new List<BarFormatCodeMaster>();
            response.Data.Add(new BarFormatCodeMaster() { BarFormatCode = "1111", BarFormatCodeID = 1, BarFormatDescription = "Unspecified Format", IsActive = true });
            response.Data.Add(new BarFormatCodeMaster() { BarFormatCode = "2212", BarFormatCodeID = 1, BarFormatDescription = "Single dough layer", IsActive = true });
            response.Data.Add(new BarFormatCodeMaster() { BarFormatCode = "2213", BarFormatCodeID = 1, BarFormatDescription = "Single cereal layer", IsActive = true });
            return response;
        }

        private GeneralResponse<ICollection<InternalQCMAVLookUpMaster>> GetInternalQCMAVLookUpMasterMockData()
        {
            var response = new GeneralResponse<ICollection<InternalQCMAVLookUpMaster>>();
            response.Data = new List<InternalQCMAVLookUpMaster>();
            response.Data.Add(new InternalQCMAVLookUpMaster() { Subtract=13, TotalBarWeight=125, CreatedBy=1, InternalQCMAVLookUpMasterID=1, IsActive =true });
            response.Data.Add(new InternalQCMAVLookUpMaster() { Subtract = 43, TotalBarWeight = 1200, CreatedBy = 1, InternalQCMAVLookUpMasterID = 2, IsActive = true });
            return response;
        }

        #endregion
    }
}