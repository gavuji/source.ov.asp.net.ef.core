using FM21.API.Controllers;
using FM21.Core;
using FM21.Core.Model;
using FM21.Entities;
using FM21.Service;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Threading.Tasks;

namespace FM21.Tests
{
    [TestFixture]
    public class FormulaMasterTest
    {
        private FormulaMasterController formulaMasterController;
        private Mock<IFormulaMasterService> formulaMasterService;
        private DataTable dtFormulaMockData;
        private DataTable dtFormulaYieldLossFactorDefaultValue;

        [SetUp]
        public void SetUp()
        {
            dtFormulaMockData = GetFormulaMockDataTable();
            formulaMasterService = new Mock<IFormulaMasterService>();
            dtFormulaYieldLossFactorDefaultValue = GetFormulaYieldLossFactorDefaultValueMockDataTable();
        }

        [Test]
        public async Task Should_Return_Data_When_Get_Record_By_Valid_PrimaryKey()
        {
            FormulaModel data = new FormulaModel() { FormulaMaster = GetFormulaModelMockObject(1) };
            GeneralResponse<FormulaModel> returnObject = new GeneralResponse<FormulaModel>()
            {
                Data = data
            };

            formulaMasterService.Setup(t => t.GetFormulaByFormulaID(returnObject.Data.FormulaMaster.FormulaID)).ReturnsAsync(returnObject);

            formulaMasterController = new FormulaMasterController(formulaMasterService.Object);
            var response = await formulaMasterController.GetFormulaByFormulaID(returnObject.Data.FormulaMaster.FormulaID) as JsonResult;
            var responseResult = response.Value as GeneralResponse<FormulaModel>;

            formulaMasterService.Verify(c => c.GetFormulaByFormulaID(It.IsAny<int>()), Times.Once);
            Assert.AreEqual(returnObject.Data.FormulaMaster.FormulaChangeNote, responseResult.Data.FormulaMaster.FormulaChangeNote);
            Assert.AreEqual(returnObject.Data.FormulaMaster.DatasheetNote, responseResult.Data.FormulaMaster.DatasheetNote);
        }

        [Test]
        public async Task Should_Return_Null_When_Get_Record_With_InValid_PrimaryKey()
        {
            FormulaModel data = new FormulaModel() { FormulaMaster = GetFormulaModelMockObject(1) };
            GeneralResponse<FormulaModel> returnObject = new GeneralResponse<FormulaModel>()
            {
                Data = data
            };

            formulaMasterService = new Mock<IFormulaMasterService>();
            formulaMasterService.Setup(t => t.GetFormulaByFormulaID(returnObject.Data.FormulaMaster.FormulaID)).ReturnsAsync(returnObject);

            formulaMasterController = new FormulaMasterController(formulaMasterService.Object);

            var response = await formulaMasterController.GetFormulaByFormulaID(9999) as JsonResult;

            formulaMasterService.Verify(c => c.GetFormulaByFormulaID(It.IsAny<int>()), Times.Once);
            Assert.IsNull(response.Value);
        }

        [Test]
        public async Task Should_Update_When_Data_Is_Valid()
        {
            GeneralResponse<bool> returnObject = new GeneralResponse<bool>() { Data = true };
            int recordID = 1;
            FormulaModel data = new FormulaModel() { FormulaMaster = GetFormulaModelMockObject(recordID) };
            formulaMasterService.Setup(t => t.UpdateFormula(data)).ReturnsAsync(returnObject);

            formulaMasterController = new FormulaMasterController(formulaMasterService.Object);

            var response = await formulaMasterController.PutFormula(recordID, data);

            Assert.IsNotNull(response);
            Assert.AreEqual(((GeneralResponse<bool>)((JsonResult)response).Value).Result, ResultType.Success);
        }

        [Test]
        public async Task Should_Search_And_Return_Formula()
        {
            PagedTableResponse<DataTable> returnObject = new PagedTableResponse<DataTable>() { Data = dtFormulaMockData };
            var searchFilter = new FormulaSearchFilter();

            formulaMasterService = new Mock<IFormulaMasterService>();
            formulaMasterService.Setup(t => t.SearchFormula(searchFilter)).ReturnsAsync(returnObject);

            formulaMasterController = new FormulaMasterController(formulaMasterService.Object);
            var response = await formulaMasterController.SearchFormula(searchFilter) as JsonResult;

            Assert.IsNotNull(response);
            Assert.That((response.Value as ResponseBase).Result, Is.EqualTo(ResultType.Success));
            Assert.Greater(((PagedTableResponse<DataTable>)response.Value).Data.Rows.Count, 0);
        }

        [Test]
        public async Task Should_Return_All_Claims_Records()
        {
            var returnObject = GetGeneralClaimsMockData();
            formulaMasterService.Setup(t => t.GetClaimsByFormulaID(1)).ReturnsAsync(returnObject);

            formulaMasterController = new FormulaMasterController(formulaMasterService.Object);
            var response = await formulaMasterController.GetClaimsByFormulaID(1) as JsonResult;
            Assert.IsNotNull(response);
            Assert.AreEqual(returnObject.Data, ((GeneralResponse<ICollection<FormulaClaimsModel>>)response.Value).Data);
            Assert.AreEqual(returnObject.Data.Count, ((GeneralResponse<ICollection<FormulaClaimsModel>>)response.Value).Data.Count);
            Assert.AreEqual(returnObject.Result, ((GeneralResponse<ICollection<FormulaClaimsModel>>)response.Value).Result);
        }

        [Test]
        public async Task Should_Return_All_Criteria_Records()
        {
            var returnObject = GetGeneralCriteriaMockData();
            formulaMasterService.Setup(t => t.GetCriteriaByFormulaID(1)).ReturnsAsync(returnObject);

            formulaMasterController = new FormulaMasterController(formulaMasterService.Object);
            var response = await formulaMasterController.GetCriteriaByFormulaID(1) as JsonResult;
            Assert.IsNotNull(response);
            Assert.AreEqual(returnObject.Data, ((GeneralResponse<ICollection<FormulaCriteriaModel>>)response.Value).Data);
            Assert.AreEqual(returnObject.Data.Count, ((GeneralResponse<ICollection<FormulaCriteriaModel>>)response.Value).Data.Count);
            Assert.AreEqual(returnObject.Result, ((GeneralResponse<ICollection<FormulaCriteriaModel>>)response.Value).Result);
        }

        [Test]
        public async Task Should_Update_Criteria_When_Data_Is_Valid()
        {
            GeneralResponse<bool> returnObject = new GeneralResponse<bool>() { Data = true };
            int formulaID = 1;
            var data = new int[] { 1, 2, 3 };
            formulaMasterService.Setup(t => t.SaveCriteria(formulaID, data)).ReturnsAsync(returnObject);

            formulaMasterController = new FormulaMasterController(formulaMasterService.Object);

            var response = await formulaMasterController.PutCriteria(formulaID, data);

            Assert.IsNotNull(response);
            Assert.AreEqual(((GeneralResponse<bool>)((JsonResult)response).Value).Result, ResultType.Success);
        }

        [Test]
        public async Task Should_Return_All_Amino_Acid_Records()
        {
            FormulaIngredient[] arrIngredients = new FormulaIngredient[] { new FormulaIngredient() { IngredientID = 1, Amount = 3 } };
            var returnObject = GetAminoAcidMockData();
            formulaMasterService.Setup(t => t.GetAminoAcidInfo(arrIngredients)).ReturnsAsync(returnObject);

            formulaMasterController = new FormulaMasterController(formulaMasterService.Object);
            var response = await formulaMasterController.GetAminoAcidInfo(arrIngredients) as JsonResult;
            Assert.IsNotNull(response);
            Assert.AreEqual(returnObject.Data, ((GeneralResponse<ICollection<NutrientModel>>)response.Value).Data);
            Assert.AreEqual(returnObject.Data.Count, ((GeneralResponse<ICollection<NutrientModel>>)response.Value).Data.Count);
            Assert.AreEqual(returnObject.Result, ((GeneralResponse<ICollection<NutrientModel>>)response.Value).Result);
        }

        [Test]
        public async Task Should_Return_All_PDCAAS_Info()
        {
            FormulaIngredient[] arrIngredients = new FormulaIngredient[] { new FormulaIngredient() { IngredientID = 1, Amount = 3 } };
            GeneralResponse<PDCAASInfo> returnObject = new GeneralResponse<PDCAASInfo>();
            formulaMasterService.Setup(t => t.GetPDCAASInfo(arrIngredients)).ReturnsAsync(returnObject);

            formulaMasterController = new FormulaMasterController(formulaMasterService.Object);
            var response = await formulaMasterController.GetPDCAASInfo(arrIngredients) as JsonResult;
            Assert.IsNotNull(response);
            Assert.AreEqual(ResultType.Success, ((GeneralResponse<PDCAASInfo>)response.Value).Result);
        }

        [Test]
        public async Task Should_Return_All_Carbohydrate_Info()
        {
            FormulaIngredient[] arrIngredients = new FormulaIngredient[] { new FormulaIngredient() { IngredientID = 1, Amount = 4 } };
            GeneralResponse<CarbohydrateInfo> returnObject = new GeneralResponse<CarbohydrateInfo>();
            formulaMasterService.Setup(t => t.GetCarbohydrateInfo(arrIngredients)).ReturnsAsync(returnObject);

            formulaMasterController = new FormulaMasterController(formulaMasterService.Object);
            var response = await formulaMasterController.GetCarbohydrateInfo(arrIngredients) as JsonResult;
            Assert.IsNotNull(response);
            Assert.AreEqual(ResultType.Success, ((GeneralResponse<CarbohydrateInfo>)response.Value).Result);
        }

        [Test]
        public async Task Should_Return_All_DSActual_Info()
        {
            FormulaIngredient[] arrIngredients = new FormulaIngredient[] { new FormulaIngredient() { IngredientID = 1, Amount = 4 } };
            GeneralResponse<ICollection<DSActualInfo>> returnObject = new GeneralResponse<ICollection<DSActualInfo>>();
            formulaMasterService.Setup(t => t.GetDSActualInfo(arrIngredients)).ReturnsAsync(returnObject);

            formulaMasterController = new FormulaMasterController(formulaMasterService.Object);
            var response = await formulaMasterController.GetDSActualInfo(arrIngredients) as JsonResult;
            Assert.IsNotNull(response);
            Assert.AreEqual(ResultType.Success, ((GeneralResponse<ICollection<DSActualInfo>>)response.Value).Result);
        }

        [Test]
        public async Task Should_Return_All_IngredientList_Info()
        {
            FormulaIngredient[] arrIngredients = new FormulaIngredient[] { new FormulaIngredient() { IngredientID = 1, Amount = 6 } };
            GeneralResponse<ICollection<IngredientListInfo>> returnObject = new GeneralResponse<ICollection<IngredientListInfo>>();
            formulaMasterService.Setup(t => t.GetIngredientListInfo(arrIngredients)).ReturnsAsync(returnObject);

            formulaMasterController = new FormulaMasterController(formulaMasterService.Object);
            var response = await formulaMasterController.GetIngredientListInfo(arrIngredients) as JsonResult;
            Assert.IsNotNull(response);
            Assert.AreEqual(ResultType.Success, ((GeneralResponse<ICollection<IngredientListInfo>>)response.Value).Result);
        }

        [Test]
        public async Task Should_Save_Formula_Data()
        {
            GeneralResponse<bool> returnObject = new GeneralResponse<bool>() { Data = true };
            FormulaModel data = GetMockDataForFormulaSave();
            formulaMasterService.Setup(t => t.SaveFormula(data)).ReturnsAsync(returnObject);
            formulaMasterController = new FormulaMasterController(formulaMasterService.Object);
            var response = await formulaMasterController.SaveFormula(data);

            Assert.IsNotNull(response);
            Assert.AreEqual(((GeneralResponse<bool>)((JsonResult)response).Value).Result, ResultType.Success);
        }

        [Test]
        public async Task Should_Return_All_Reconstitution_Master_Records()
        {
            var returnObject = GetReconstitutionMasterMockObject();
            formulaMasterService = new Mock<IFormulaMasterService>();
            formulaMasterService.Setup(t => t.GetAllReconstitutionMaster()).ReturnsAsync(returnObject);

            formulaMasterController = new FormulaMasterController(formulaMasterService.Object);
            var response = await formulaMasterController.GetAllReconstitutionMaster() as JsonResult;

            Assert.IsNotNull(response);
            Assert.AreEqual(returnObject.Data, ((GeneralResponse<ICollection<ReconstitutionMaster>>)response.Value).Data);
            Assert.AreEqual(returnObject.Data.Count, ((GeneralResponse<ICollection<ReconstitutionMaster>>)response.Value).Data.Count);
            Assert.AreEqual(returnObject.Result, ((GeneralResponse<ICollection<ReconstitutionMaster>>)response.Value).Result);
        }

        [Test]
        public async Task Should_Return_All_PowderLiquid_Master_Records()
        {
            var returnObject = GetPowderLiquidMasterMockObject();
            formulaMasterService = new Mock<IFormulaMasterService>();
            formulaMasterService.Setup(t => t.GetAllPowderLiquidMaster()).ReturnsAsync(returnObject);

            formulaMasterController = new FormulaMasterController(formulaMasterService.Object);
            var response = await formulaMasterController.GetAllPowderLiquidMaster() as JsonResult;

            Assert.IsNotNull(response);
            Assert.AreEqual(returnObject.Data, ((GeneralResponse<ICollection<PowderLiquidMaster>>)response.Value).Data);
            Assert.AreEqual(returnObject.Data.Count, ((GeneralResponse<ICollection<PowderLiquidMaster>>)response.Value).Data.Count);
            Assert.AreEqual(returnObject.Result, ((GeneralResponse<ICollection<PowderLiquidMaster>>)response.Value).Result);
        }

        [Test]
        public async Task Should_Return_All_PowderBlender_Master_Records()
        {
            var returnObject = GetPowderBlenderSiteMappingMockObject();
            formulaMasterService = new Mock<IFormulaMasterService>();
            formulaMasterService.Setup(t => t.GetAllPowderBlenderMaster(0)).ReturnsAsync(returnObject);

            formulaMasterController = new FormulaMasterController(formulaMasterService.Object);
            var response = await formulaMasterController.GetAllPowderBlenderMaster(0) as JsonResult;

            Assert.IsNotNull(response);
            Assert.AreEqual(returnObject.Data, ((GeneralResponse<ICollection<PowderBlenderMasterModel>>)response.Value).Data);
            Assert.AreEqual(returnObject.Data.Count, ((GeneralResponse<ICollection<PowderBlenderMasterModel>>)response.Value).Data.Count);
            Assert.AreEqual(returnObject.Result, ((GeneralResponse<ICollection<PowderBlenderMasterModel>>)response.Value).Result);
        }

        [Test]
        public async Task Should_Return_All_PowderUnit_Master_Records()
        {
            var returnObject = GetUnitServingMasterMockObject();
            formulaMasterService = new Mock<IFormulaMasterService>();
            formulaMasterService.Setup(t => t.GetAllPowderUnitMaster(0)).ReturnsAsync(returnObject);

            formulaMasterController = new FormulaMasterController(formulaMasterService.Object);
            var response = await formulaMasterController.GetAllPowderUnitMaster(0) as JsonResult;

            Assert.IsNotNull(response);
            Assert.AreEqual(returnObject.Data, ((GeneralResponse<ICollection<UnitServingMasterModel>>)response.Value).Data);
            Assert.AreEqual(returnObject.Data.Count, ((GeneralResponse<ICollection<UnitServingMasterModel>>)response.Value).Data.Count);
            Assert.AreEqual(returnObject.Result, ((GeneralResponse<ICollection<UnitServingMasterModel>>)response.Value).Result);
        }

        [Test]
        public async Task Should_Return_All_PowderUnitServing_Master_Records()
        {
            var returnObject = GetUnitServingMasterMockObject();
            formulaMasterService = new Mock<IFormulaMasterService>();
            formulaMasterService.Setup(t => t.GetAllPowderUnitServingMaster()).ReturnsAsync(returnObject);

            formulaMasterController = new FormulaMasterController(formulaMasterService.Object);
            var response = await formulaMasterController.GetAllPowderUnitServingMaster() as JsonResult;

            Assert.IsNotNull(response);
            Assert.AreEqual(returnObject.Data, ((GeneralResponse<ICollection<UnitServingMasterModel>>)response.Value).Data);
            Assert.AreEqual(returnObject.Data.Count, ((GeneralResponse<ICollection<UnitServingMasterModel>>)response.Value).Data.Count);
            Assert.AreEqual(returnObject.Result, ((GeneralResponse<ICollection<UnitServingMasterModel>>)response.Value).Result);
        }

        [Test]
        public async Task Should_Update_Formula_Search_History()
        {
            formulaMasterService = new Mock<IFormulaMasterService>();
            formulaMasterService.Setup(t => t.UpdateFormulaSearchHistory(1, "test")).ReturnsAsync(new GeneralResponse<bool>());

            formulaMasterController = new FormulaMasterController(formulaMasterService.Object);
            var response = await formulaMasterController.UpdateFormulaSearchHistory(1, "test") as JsonResult;

            Assert.IsNotNull(response);
            Assert.AreEqual(((GeneralResponse<bool>)response.Value).Result, ResultType.Success);
        }

        [Test]
        public async Task Should_Return_Formula_Search_History()
        {
            var formulaSearchHistoryData = new GeneralResponse<ICollection<FormulaSearchHistory>>();
            formulaSearchHistoryData.Data = GetFormulaSearchHistoryMockData();

            formulaMasterService = new Mock<IFormulaMasterService>();
            formulaMasterService.Setup(t => t.GetFormulaSearchHistory(1)).ReturnsAsync(formulaSearchHistoryData);

            formulaMasterController = new FormulaMasterController(formulaMasterService.Object);
            var response = await formulaMasterController.GetFormulaSearchHistory(1) as JsonResult;

            Assert.IsNotNull(response);
            Assert.Greater(((GeneralResponse<ICollection<FormulaSearchHistory>>)response.Value).Data.Count, 0);
            Assert.AreEqual(((GeneralResponse<ICollection<FormulaSearchHistory>>)response.Value).Result, ResultType.Success);
        }

        [Test]
        public async Task Should_Return_Formula_Regulatory_CategoryMaster_Records()
        {
            var returnObject = GetFormulaRegulatoryCategoryMasterMockObject();
            formulaMasterService = new Mock<IFormulaMasterService>();
            formulaMasterService.Setup(t => t.GetFormulaRegulatoryCategoryMaster()).ReturnsAsync(returnObject);

            formulaMasterController = new FormulaMasterController(formulaMasterService.Object);
            var response = await formulaMasterController.GetFormulaRegulatoryCategoryMaster() as JsonResult;

            Assert.IsNotNull(response);
            Assert.AreEqual(returnObject.Data, ((GeneralResponse<ICollection<FormulaRegulatoryCategoryMaster>>)response.Value).Data);
            Assert.AreEqual(returnObject.Data.Count, ((GeneralResponse<ICollection<FormulaRegulatoryCategoryMaster>>)response.Value).Data.Count);
            Assert.AreEqual(returnObject.Result, ((GeneralResponse<ICollection<FormulaRegulatoryCategoryMaster>>)response.Value).Result);
        }

        [Test]
        public async Task Should_Return_Datasheet_FormatMaster_Records()
        {
            var returnObject = GetDatasheetFormatMasterMockObject();
            formulaMasterService = new Mock<IFormulaMasterService>();
            formulaMasterService.Setup(t => t.GetDatasheetFormatMaster()).ReturnsAsync(returnObject);

            formulaMasterController = new FormulaMasterController(formulaMasterService.Object);
            var response = await formulaMasterController.GetDatasheetFormatMaster() as JsonResult;

            Assert.IsNotNull(response);
            Assert.AreEqual(returnObject.Data, ((GeneralResponse<ICollection<DatasheetFormatMaster>>)response.Value).Data);
            Assert.AreEqual(returnObject.Data.Count, ((GeneralResponse<ICollection<DatasheetFormatMaster>>)response.Value).Data.Count);
            Assert.AreEqual(returnObject.Result, ((GeneralResponse<ICollection<DatasheetFormatMaster>>)response.Value).Result);
        }

        [Test]
        public async Task Should_Save_Yield_Loss_data()
        {
            GeneralResponse<bool> returnObject = new GeneralResponse<bool>() { Data = true };
            int recordID = 1;
            FormulaMasterModel obj = GetFormulaModelMockObject(recordID);
            formulaMasterService.Setup(t => t.SaveYieldLoss(recordID, obj)).ReturnsAsync(returnObject);

            formulaMasterController = new FormulaMasterController(formulaMasterService.Object);

            var response = await formulaMasterController.UpdateYieldLoss(recordID, obj);

            Assert.IsNotNull(response);
            Assert.AreEqual(((GeneralResponse<bool>)((JsonResult)response).Value).Result, ResultType.Success);
        }

        [Test]
        public async Task Should_Return_Formula_YieldLoss_Factor_DefaultValue()
        {
            GeneralResponse<DataTable> returnObject = new GeneralResponse<DataTable>() { Data = dtFormulaYieldLossFactorDefaultValue };
            int siteID = 1;
            string mixerType = "planttrials";

            formulaMasterService = new Mock<IFormulaMasterService>();
            formulaMasterService.Setup(t => t.GetFormulaYieldLossFactorDefaultValue(siteID, mixerType)).ReturnsAsync(returnObject);

            formulaMasterController = new FormulaMasterController(formulaMasterService.Object);
            var response = await formulaMasterController.GetFormulaYieldLossFactorDefaultValue(siteID, mixerType) as JsonResult;

            Assert.IsNotNull(response);
            Assert.That((response.Value as ResponseBase).Result, Is.EqualTo(ResultType.Success));
            Assert.Greater(((GeneralResponse<DataTable>)response.Value).Data.Rows.Count, 0);
        }

        [Test]
        public async Task Should_Return_Formula_Home_Page_Header_Info()
        {
            GeneralResponse<DataTable> returnObject = new GeneralResponse<DataTable>() { Data = GetFormulaHomePageHeaderInfoMockDataTable() };
            int formulaID = 1;

            formulaMasterService = new Mock<IFormulaMasterService>();
            formulaMasterService.Setup(t => t.GetFormulaHomePageHeaderInfo(formulaID)).ReturnsAsync(returnObject);

            formulaMasterController = new FormulaMasterController(formulaMasterService.Object);
            var response = await formulaMasterController.GetFormulaHomePageHeaderInfo(formulaID) as JsonResult;

            Assert.IsNotNull(response);
            Assert.That((response.Value as ResponseBase).Result, Is.EqualTo(ResultType.Success));
            Assert.Greater(((GeneralResponse<DataTable>)response.Value).Data.Rows.Count, 0);
        }

        [Test]
        public async Task Should_Delete_Formula()
        {
            GeneralResponse<bool> returnObject = new GeneralResponse<bool>() { Data = true };
            int[] formulaID = new int[] { 1, 2 };

            formulaMasterService = new Mock<IFormulaMasterService>();
            formulaMasterService.Setup(t => t.DeleteFormula(formulaID)).ReturnsAsync(returnObject);

            formulaMasterController = new FormulaMasterController(formulaMasterService.Object);
            var response = await formulaMasterController.DeleteFormula(formulaID) as JsonResult;

            Assert.IsNotNull(response);
            Assert.That((response.Value as ResponseBase).Result, Is.EqualTo(ResultType.Success));

        }

        [Test]
        public async Task Should_Return_Formula_Change_Code_List()
        {
            GeneralResponse<DataTable> returnObject = new GeneralResponse<DataTable>() { Data = GetFormulaChangeCodesMockDataTable() };
            FormulaChangeCodeParam formulaChangeCodeParam = new FormulaChangeCodeParam() { CurrentSiteID = 1, FormulaCode = "S10600115.022b-033", FormulaID = 15, FormulaTypeCode = "S10", ServingSize = "033", ChangeSiteID = 2 };

            formulaMasterService = new Mock<IFormulaMasterService>();
            formulaMasterService.Setup(t => t.GetFormulaChangeCodeList(formulaChangeCodeParam)).ReturnsAsync(returnObject);

            formulaMasterController = new FormulaMasterController(formulaMasterService.Object);
            var response = await formulaMasterController.GetFormulaChangeCodeList(formulaChangeCodeParam) as JsonResult;

            Assert.IsNotNull(response);
            Assert.That((response.Value as ResponseBase).Result, Is.EqualTo(ResultType.Success));
            Assert.Greater(((GeneralResponse<DataTable>)response.Value).Data.Rows.Count, 0);
        }

        [Test]
        public async Task Should_Return_True_Formula_Change_Code_Already_Exist()
        {
            GeneralResponse<bool> returnObject = new GeneralResponse<bool>() { Data = true };
            string formulaCode = "S10600115.022b-033";

            formulaMasterService = new Mock<IFormulaMasterService>();
            formulaMasterService.Setup(t => t.IsFormulaCodeExist(formulaCode)).ReturnsAsync(returnObject);

            formulaMasterController = new FormulaMasterController(formulaMasterService.Object);
            var response = await formulaMasterController.IsFormulaCodeExist(formulaCode) as JsonResult;

            Assert.IsNotNull(response);
            Assert.That((response.Value as ResponseBase).Result, Is.EqualTo(ResultType.Success));
            Assert.That(((GeneralResponse<bool>)response.Value).Data, Is.EqualTo(true));
        }

        [Test]
        public async Task Should_Return_False_Formula_Change_Code_Already_Exist()
        {
            GeneralResponse<bool> returnObject = new GeneralResponse<bool>() { Data = false };
            string formulaCode = "S10500115.022b-033";

            formulaMasterService = new Mock<IFormulaMasterService>();
            formulaMasterService.Setup(t => t.IsFormulaCodeExist(formulaCode)).ReturnsAsync(returnObject);

            formulaMasterController = new FormulaMasterController(formulaMasterService.Object);
            var response = await formulaMasterController.IsFormulaCodeExist(formulaCode) as JsonResult;

            Assert.IsNotNull(response);
            Assert.That((response.Value as ResponseBase).Result, Is.EqualTo(ResultType.Success));
            Assert.That(((GeneralResponse<bool>)response.Value).Data, Is.EqualTo(false));
        }

        [Test]
        public async Task Should_Return_Formula_Status_Master_List()
        {
            var returnObject = GetFormulaStatusMasterMockDataTable();

            formulaMasterService = new Mock<IFormulaMasterService>();
            formulaMasterService.Setup(t => t.GetFormulaStatusMaster()).ReturnsAsync(returnObject);

            formulaMasterController = new FormulaMasterController(formulaMasterService.Object);
            var response = await formulaMasterController.GetFormulaStatusMaster() as JsonResult;

            Assert.IsNotNull(response);
            Assert.That((response.Value as ResponseBase).Result, Is.EqualTo(ResultType.Success));
            Assert.AreEqual(returnObject.Data, ((GeneralResponse<ICollection<FormulaStatusMaster>>)response.Value).Data);
            Assert.AreEqual(returnObject.Data.Count, ((GeneralResponse<ICollection<FormulaStatusMaster>>)response.Value).Data.Count);
            Assert.AreEqual(returnObject.Result, ((GeneralResponse<ICollection<FormulaStatusMaster>>)response.Value).Result);
        }
        [Test]
        public async Task Should_Import_InternalQCInfo_By_FormulaCode()
        {
            GeneralResponse<FormulaMasterModel> returnObject = new GeneralResponse<FormulaMasterModel>() { Data = GetFormulaModelMockObject() };
            string formulaCode = "S10500115";
            formulaMasterService = new Mock<IFormulaMasterService>();
            formulaMasterService.Setup(t => t.ImportInternalQCInfoByFormulaCode(formulaCode)).ReturnsAsync(returnObject);

            formulaMasterController = new FormulaMasterController(formulaMasterService.Object);
            var response = await formulaMasterController.ImportInternalQCInfoByFormulaCode(formulaCode) as JsonResult;

            Assert.IsNotNull(response);
            Assert.That((response.Value as ResponseBase).Result, Is.EqualTo(ResultType.Success));

        }

        [Test]
        public async Task Should_Return_Formula_Target_Model_List()
        {
            var returnObject = GetFormulaTargetModelMockDataTable();
            FormulaTargetInfoParam formulaTargetInfoParam = new FormulaTargetInfoParam() { DatasheetFormatID = 1, IngredientID = "717,1414" };
            formulaMasterService = new Mock<IFormulaMasterService>();
            formulaMasterService.Setup(t => t.GetTargetInfo(formulaTargetInfoParam)).ReturnsAsync(returnObject);

            formulaMasterController = new FormulaMasterController(formulaMasterService.Object);
            var response = await formulaMasterController.GetTargetInfo(formulaTargetInfoParam) as JsonResult;

            Assert.IsNotNull(response);
            Assert.That((response.Value as ResponseBase).Result, Is.EqualTo(ResultType.Success));
            Assert.AreEqual(returnObject.Data, ((GeneralResponse<List<FormulaTargetModel>>)response.Value).Data);
            Assert.AreEqual(returnObject.Data.Count, ((GeneralResponse<List<FormulaTargetModel>>)response.Value).Data.Count);
            Assert.AreEqual(returnObject.Result, ((GeneralResponse<List<FormulaTargetModel>>)response.Value).Result);
        }
        #region Mock Data
        private DataTable GetFormulaMockDataTable()
        {
            DataTable tbl = new DataTable();
            tbl.Columns.Add("FormulaID", typeof(int));
            tbl.Columns.Add("FormulaCode", typeof(string));
            tbl.Columns.Add("ProductCode", typeof(string));
            tbl.Rows.Add(11, "F0001", "");
            tbl.Rows.Add(12, "F0002", "");
            tbl.Rows.Add(13, "F0003", "");
            return tbl;
        }

        private DataTable GetFormulaYieldLossFactorDefaultValueMockDataTable()
        {
            DataTable tbl = new DataTable();
            tbl.Columns.Add("YieldFactor", typeof(string));
            tbl.Columns.Add("DefaultValue", typeof(double));
            tbl.Columns.Add("SiteID", typeof(int));
            tbl.Columns.Add("MixerType", typeof(string));

            tbl.Rows.Add("S10CoreDoughYield", 20.0000, 1, "planttrials");
            tbl.Rows.Add("S65SyrupYield", 20.0000, 1, "planttrials");
            tbl.Rows.Add("S68_1LiquidBlendYield", 0.000, 1, "planttrials");
            return tbl;
        }

        private DataTable GetFormulaHomePageHeaderInfoMockDataTable()
        {
            DataTable tbl = new DataTable();
            tbl.Columns.Add("FormulaCode", typeof(string));
            tbl.Columns.Add("ProductName", typeof(string));
            tbl.Columns.Add("ProjectCode", typeof(string));
            tbl.Columns.Add("FormulaReference", typeof(string));
            tbl.Columns.Add("FormulaStatusCode", typeof(string));
            tbl.Columns.Add("DieNumber", typeof(string));
            tbl.Columns.Add("ActualWaterPercentage", typeof(string));

            tbl.Rows.Add("F0001", "Creamy Penuts butter", "100005", "S10600115.022a-033", "M", null, null);
            return tbl;
        }

        private ICollection<FormulaSearchHistory> GetFormulaSearchHistoryMockData()
        {
            ICollection<FormulaSearchHistory> recordList = new Collection<FormulaSearchHistory>();
            recordList.Add(new FormulaSearchHistory() { FormulaSearchID = 1, UserID = 1, SearchData = "test1", SearchDate = DateTime.Now });
            recordList.Add(new FormulaSearchHistory() { FormulaSearchID = 2, UserID = 1, SearchData = "test2", SearchDate = DateTime.Now.AddMinutes(-1) });
            recordList.Add(new FormulaSearchHistory() { FormulaSearchID = 3, UserID = 1, SearchData = "test3", SearchDate = DateTime.Now.AddMinutes(-2) });
            recordList.Add(new FormulaSearchHistory() { FormulaSearchID = 4, UserID = 1, SearchData = "test4", SearchDate = DateTime.Now.AddMinutes(-3) });
            recordList.Add(new FormulaSearchHistory() { FormulaSearchID = 5, UserID = 1, SearchData = "test5", SearchDate = DateTime.Now.AddMinutes(-4) });
            return recordList;
        }

        private FormulaMasterModel GetFormulaModelMockObject(int? id = null)
        {
            FormulaMasterModel obj = new FormulaMasterModel()
            {
                FormulaChangeNote = "test",
                TextureAppearanceNote = "test",
                ProjectInfoNote = "test",
                PackagingInfoForPowderNote = "test",
                DatasheetNote = "test",
                S10CoreDoughYield = 10,
                S10CoreDoughYieldLoss = 0,
                S30CoatingYield = 200
            };
            if (id != null)
            {
                obj.FormulaID = Convert.ToInt32(id);
            }
            return obj;
        }

        private GeneralResponse<ICollection<FormulaClaimsModel>> GetGeneralClaimsMockData()
        {
            var response = new GeneralResponse<ICollection<FormulaClaimsModel>>();
            List<FormulaClaimsModel> recordList = new List<FormulaClaimsModel>();
            recordList.Add(GetFormulaClaimMockObject(1, 1, 1));
            recordList.Add(GetFormulaClaimMockObject(2, 1, 2));
            response.Data = recordList;
            return response;
        }

        private GeneralResponse<ICollection<FormulaCriteriaModel>> GetGeneralCriteriaMockData()
        {
            var response = new GeneralResponse<ICollection<FormulaCriteriaModel>>();
            List<FormulaCriteriaModel> recordList = new List<FormulaCriteriaModel>();
            recordList.Add(GetFormulaCriteriaMockObject(1, 1, 1));
            recordList.Add(GetFormulaCriteriaMockObject(2, 1, 2));
            response.Data = recordList;
            return response;
        }

        private FormulaModel GetMockDataForFormulaSave()
        {
            FormulaModel formulaModel = new FormulaModel();
            formulaModel.FormulaMaster = new FormulaMasterModel() { FormulaID = 0, FormulaChangeNote = "Mixing time was changed to 200 Test of 14 sep 2020" };
            formulaModel.FormulaDetails = new List<FormulaDetailsModel>();
            formulaModel.FormulaDetails.Add(new FormulaDetailsModel() { FormulaDetailMapID = 0, FormulaID = 0, ReferenceID = 2, ReferenceType = 1, Description = "__USE Sollich to SLAB base and ADD FONDANT top layer.", RowNumber = 1 });
            formulaModel.FormulaDetails.Add(new FormulaDetailsModel() { FormulaDetailMapID = 0, FormulaID = 0, ReferenceID = 2, ReferenceType = 1, Description = "__USE Sollich to SLAB base and ADD FONDANT top layer.", RowNumber = 1 });
            formulaModel.ClaimInfo = new List<ClaimModel>();
            formulaModel.ClaimInfo.Add(new ClaimModel() { ClaimID = 1, Description = "Contains only Kosher ingredients" });
            formulaModel.ClaimInfo.Add(new ClaimModel() { ClaimID = 2, Description = "Certified OU Dairy" });
            formulaModel.CriteriaInfo = new int[] { 1 };
            return formulaModel;
        }

        private FormulaClaimsModel GetFormulaClaimMockObject(int claimID, int formulaID, int formulaClaimMapID)
        {
            FormulaClaimsModel obj = new FormulaClaimsModel()
            {
                ClaimID = claimID,
                FormulaID = formulaID,
                ClaimCode = "ABC",
                ClaimDescription = "test",
                ClaimGroupType = "Kesor",
                HasImpact = true,
                Description = "test",
                FormulaClaimMapID = formulaClaimMapID
            };
            return obj;
        }

        private FormulaCriteriaModel GetFormulaCriteriaMockObject(int criteriaID, int formulaID, int formulaCriteriaMapID)
        {
            FormulaCriteriaModel obj = new FormulaCriteriaModel()
            {
                CriteriaID = criteriaID,
                FormulaID = formulaID,
                CriteriaDescription = "ABC",
                ColorCode = "red",
                CriteriaOrder = "A",
                FormulaCriteriaMapID = formulaCriteriaMapID
            };
            return obj;
        }

        private GeneralResponse<ICollection<NutrientModel>> GetAminoAcidMockData()
        {
            var response = new GeneralResponse<ICollection<NutrientModel>>();
            List<NutrientModel> recordList = new List<NutrientModel>();
            recordList.Add(new NutrientModel()
            {
                NutrientId = 1,
                Name = "Protin"
            });
            response.Data = recordList;
            return response;
        }

        private GeneralResponse<ICollection<ReconstitutionMaster>> GetReconstitutionMasterMockObject()
        {
            GeneralResponse<ICollection<ReconstitutionMaster>> recordList = new GeneralResponse<ICollection<ReconstitutionMaster>>();
            recordList.Data = new Collection<ReconstitutionMaster>()
            {
                new ReconstitutionMaster(){ ReconstitutionID = 1, ReconstitutionDescription = "USA nonfat milk" },
                new ReconstitutionMaster(){ ReconstitutionID = 2, ReconstitutionDescription = "CAN skim milk" }
            };
            return recordList;
        }

        private GeneralResponse<ICollection<PowderLiquidMaster>> GetPowderLiquidMasterMockObject()
        {
            GeneralResponse<ICollection<PowderLiquidMaster>> recordList = new GeneralResponse<ICollection<PowderLiquidMaster>>();
            recordList.Data = new Collection<PowderLiquidMaster>()
            {
                new PowderLiquidMaster(){ PowderLiquidID = 1, LiquidDescription = "1 Tbs" },
                new PowderLiquidMaster(){ PowderLiquidID = 2, LiquidDescription = "2 Tbs" }
            };
            return recordList;
        }

        private GeneralResponse<ICollection<PowderBlenderMasterModel>> GetPowderBlenderSiteMappingMockObject()
        {
            GeneralResponse<ICollection<PowderBlenderMasterModel>> recordList = new GeneralResponse<ICollection<PowderBlenderMasterModel>>();
            recordList.Data = new Collection<PowderBlenderMasterModel>()
            {
                new PowderBlenderMasterModel(){ PowderBlenderID = 1, SiteID = 1, BlenderDescription = "Blander A" },
                new PowderBlenderMasterModel(){ PowderBlenderID = 2, SiteID = 1, BlenderDescription = "Blander G" }
            };
            return recordList;
        }

        private GeneralResponse<ICollection<UnitServingMasterModel>> GetUnitServingMasterMockObject()
        {
            GeneralResponse<ICollection<UnitServingMasterModel>> recordList = new GeneralResponse<ICollection<UnitServingMasterModel>>();
            recordList.Data = new Collection<UnitServingMasterModel>()
            {
                new UnitServingMasterModel(){ UnitServingID = 1, UnitDescription = "1 Tbs" },
                new UnitServingMasterModel(){ UnitServingID = 2, UnitDescription = "2 Tbs" }
            };
            return recordList;
        }

        private GeneralResponse<ICollection<FormulaRegulatoryCategoryMaster>> GetFormulaRegulatoryCategoryMasterMockObject()
        {
            GeneralResponse<ICollection<FormulaRegulatoryCategoryMaster>> recordList = new GeneralResponse<ICollection<FormulaRegulatoryCategoryMaster>>();
            recordList.Data = new Collection<FormulaRegulatoryCategoryMaster>()
            {
                new FormulaRegulatoryCategoryMaster(){  FormulaRegulatoryCateoryID = 1,  FormulaRegulatoryCategoryDescription = "Bar"},
                new FormulaRegulatoryCategoryMaster(){  FormulaRegulatoryCateoryID = 2,  FormulaRegulatoryCategoryDescription = "Drink Mix"}
            };
            return recordList;
        }

        private GeneralResponse<ICollection<DatasheetFormatMaster>> GetDatasheetFormatMasterMockObject()
        {
            GeneralResponse<ICollection<DatasheetFormatMaster>> recordList = new GeneralResponse<ICollection<DatasheetFormatMaster>>();
            recordList.Data = new Collection<DatasheetFormatMaster>()
            {
                new DatasheetFormatMaster(){ DatasheetFormatID=1, DatasheetCode="Bar", DatasheetDescription="Nutrition Facts - New US"},
                new DatasheetFormatMaster(){ DatasheetFormatID=2, DatasheetCode="Bar", DatasheetDescription="Supplement Facts - New US"},
            };
            return recordList;
        }

        private DataTable GetFormulaChangeCodesMockDataTable()
        {
            DataTable tbl = new DataTable();
            tbl.Columns.Add("FormulaCode", typeof(string));
            tbl.Columns.Add("Description", typeof(string));
            tbl.Rows.Add("S10600115.022b-033", " is next Process Change_");
            tbl.Rows.Add("S10600115.023a-033", " is next Formula Revision_");
            tbl.Rows.Add("S10600118.001a-033", " is next New Project_");
            return tbl;
        }

        private GeneralResponse<ICollection<FormulaStatusMaster>> GetFormulaStatusMasterMockDataTable()
        {
            GeneralResponse<ICollection<FormulaStatusMaster>> recordList = new GeneralResponse<ICollection<FormulaStatusMaster>>();
            recordList.Data = new Collection<FormulaStatusMaster>()
            {
                new FormulaStatusMaster()
                {
                    FormulaStatusID = 1,
                    FormulaStatusCode = "M1",
                    CreatedBy = 8,
                    CreatedOn = DateTime.Now,
                    FormulaStatusCodeDescription = "Production Stage"
                }
            };
            return recordList;
        }

        private GeneralResponse<List<FormulaTargetModel>> GetFormulaTargetModelMockDataTable()
        {
            GeneralResponse<List<FormulaTargetModel>> recordList = new GeneralResponse<List<FormulaTargetModel>>();
            recordList.Data = new List<FormulaTargetModel>()
            {
                new FormulaTargetModel()
                {
                     IngredientID=123,
                      NutrientID=1,
                       NutrientName="Sodium",
                        NutrientValue = 2.500M,
                         RDIValue =40               }
            };
            return recordList;
        }
        #endregion
    }
}