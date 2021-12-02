using FM21.Core;
using FM21.Core.Model;
using FM21.Data.Infrastructure;
using FM21.Entities;
using FM21.Service;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace FM21.Tests
{
    [TestFixture]
    public class FormulaMasterServiceTest : TestBase
    {
        private IFormulaMasterService formulaMasterService;
        private Mock<IRepository<FormulaMaster>> formulaMasterRepository;
        private Mock<IRepository<FormulaCriteriaMapping>> formulaCriteriaMappingRepository;
        private Mock<IRepository<FormulaClaimMapping>> formulaClaimMappingRepository;
        private Mock<IRepository<ClaimMaster>> claimMasterRepository;
        private Mock<IRepository<CriteriaMaster>> criteriaMasterRepository;
        private Mock<IRepository<NutrientMaster>> nutrientMasterRepository;
        private Mock<IRepository<IngredientMaster>> ingredientMasterRepository;
        private Mock<IRepository<FormulaRegulatoryCategoryMaster>> formulaRegulatoryCategoryMasterRepository;
        private Mock<IRepository<DatasheetFormatMaster>> datasheetFormatMasterRepository;
        private Mock<IRepository<ReconstitutionMaster>> reconstitutionMasterRepository;
        private Mock<IRepository<PowderLiquidMaster>> powderLiquidMasterRepository;
        private Mock<IRepository<PowderBlenderSiteMapping>> powderBlenderSiteMappingRepository;
        private Mock<IRepository<UnitServingMaster>> unitServingMasterRepository;
        private Mock<IRepository<PowderUnitServingSiteMapping>> powderUnitServingSiteMappingRepository;
        private Mock<IRepository<FormulaSearchHistory>> formulaSearchHistoryRepository;
        private Mock<IRepository<FormulaChangeCode>> formulaChangeCodeRepository;
        private Mock<IRepository<FormulaRevision>> formulaRevisionRepository;
        private Mock<IRepository<FormulaTypeMaster>> formulaTypeMasterRepository;
        private Mock<IRepository<FormulaStatusMaster>> formulaStatusMasterRepository;
        private Mock<IRepository<UserMaster>> userMasterRepository;

        [SetUp]
        [SetUICulture("en-us")]
        public void SetUp()
        {
            formulaMasterRepository = SetupFormulaMasterRepository();
            claimMasterRepository = SetupClaimMasterRepository();
            criteriaMasterRepository = SetupCriteriaMasterRepository(); 
            formulaClaimMappingRepository = SetupFormulaClaimMappingRepository();
            formulaCriteriaMappingRepository = SetupFormulaCriteriaMappingRepository();
            nutrientMasterRepository = SetupNutrientMasterRepository();
            ingredientMasterRepository = SetupIngredientMasterRepository();
            formulaRegulatoryCategoryMasterRepository = SetupRegulatoryCategoryMasterRepository();
            datasheetFormatMasterRepository = SetupDatasheetFormatMasterRepository();
            reconstitutionMasterRepository = SetupReconstitutionMasterRepository();
            powderLiquidMasterRepository = SetupPowderLiquidMasterRepository();
            powderBlenderSiteMappingRepository = SetupPowderBlenderSiteMappingRepository();
            unitServingMasterRepository = SetupUnitServingMasterRepository();
            powderUnitServingSiteMappingRepository = SetupPowderUnitServingSiteMappingRepository();
            formulaSearchHistoryRepository = SetupFormulaSearchHistoryRepository();
            formulaChangeCodeRepository = SetupFormulaChangeCodeRepository();
            formulaRevisionRepository = SetupFormulaRevisionRepository();
            formulaTypeMasterRepository = SetupFormulaTypeMasterRepository();
            formulaStatusMasterRepository = SetupFormulaStatusMasterRepository();
            userMasterRepository = SetupUserMasterRepository();
            formulaMasterService = new FormulaMasterService(serviceProvider.Object, exceptionHandler.Object, mapper, unitOfWork.Object, null, formulaMasterRepository.Object,
                formulaClaimMappingRepository.Object, formulaCriteriaMappingRepository.Object, claimMasterRepository.Object, criteriaMasterRepository.Object,
                formulaRegulatoryCategoryMasterRepository.Object, datasheetFormatMasterRepository.Object, nutrientMasterRepository.Object, ingredientMasterRepository.Object,
                reconstitutionMasterRepository.Object, powderLiquidMasterRepository.Object, powderBlenderSiteMappingRepository.Object, unitServingMasterRepository.Object,
                powderUnitServingSiteMappingRepository.Object, formulaSearchHistoryRepository.Object, formulaChangeCodeRepository.Object, formulaRevisionRepository.Object, 
                formulaTypeMasterRepository.Object, formulaStatusMasterRepository.Object, userMasterRepository.Object,null);
        }

        [Test]
        [TestCase("", "")]
        [TestCase("ProductCode", "test")]
        public async Task Service_Should_Search_And_Return_Formula_Details(string displayCol, string searchText)
        {
            var searchFilter = new FormulaSearchFilter()
            {
                EnableSearchHistory = true,
                PageIndex = 1,
                PageSize = 10,
                SiteID = 1,
                FieldValue1 = "ProjectDescription",
                FieldValue2 = "Claims",
                FieldValue3 = "PowderLiquidAmount",
                FieldValue4 = displayCol,
                SearchText1 = searchText
            };
            ApplicationConstants.RequestUserID = 1;
            var response = await formulaMasterService.SearchFormula(searchFilter);
            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
            Assert.Greater(response.RowCount, 0);
        }

        [Test]
        public async Task Service_Should_Not_Return_Formula_Details_When_Exception()
        {
            formulaMasterRepository.Setup(r => r.GetFromStoredProcedureAsync("SearchFormula", It.IsAny<(string, object)[]>())).Throws(new Exception("something went wrong"));
            var response = await formulaMasterService.SearchFormula(new FormulaSearchFilter());
            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_Return_Formula_Details_By_Valid_PrimaryKey()
        {
            var response = await formulaMasterService.GetFormulaByFormulaID(1);

            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
            Assert.IsNotNull(response.Data);
        }

        [Test]
        public async Task Service_Should_Return_RecordNotExist_On_Invalid_PrimaryKey()
        {
            var response = await formulaMasterService.GetFormulaByFormulaID(5);

            Assert.IsNull(response.Data);
            Assert.AreEqual(response.Message, localizer["msgRecordNotExist", new string[] { "Formula" }].Value);
        }

        [Test]
        public async Task Service_Should_Return_Blank_Formula_When_FormulaID_Is_Zero()
        {
            var response = await formulaMasterService.GetFormulaByFormulaID(0);

            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
            Assert.IsNotNull(response.Data);
        }

        [Test]
        public async Task Service_Should_Return_Formula_When_Exception()
        {
            formulaMasterRepository.Setup(r => r.GetFromStoredProcedureAsync("GetFormulaDetailsByFormulaID", It.IsAny<(string, object)[]>())).Throws(new Exception("something went wrong"));
            var response = await formulaMasterService.GetFormulaByFormulaID(0);
            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_Update_Data()
        {
            FormulaModel model = new FormulaModel() { FormulaMaster = GetObjectMockFormula(1) };
            var response = await formulaMasterService.UpdateFormula(model);

            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
        }

        [Test]
        public async Task Service_Should_Not_Update_Data_When_Post_Formula_Data_Is_Null()
        {
            var response = await formulaMasterService.UpdateFormula(null);

            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_Not_Update_Formula_When_Exception_In_Claim_Save()
        {
            formulaClaimMappingRepository.Setup(r => r.DeleteAsync(It.IsAny<System.Linq.Expressions.Expression<Func<FormulaClaimMapping, bool>>>())).Throws(new Exception("something went wrong"));
            FormulaModel model = new FormulaModel() { FormulaMaster = GetObjectMockFormula(1) };
            var response = await formulaMasterService.UpdateFormula(model);

            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_Not_Update_When_PrimaryKey_InValid()
        {
            FormulaModel model = new FormulaModel() { FormulaMaster = GetObjectMockFormula() };
            var response = await formulaMasterService.UpdateFormula(model);

            Assert.That(response.Result, Is.EqualTo(ResultType.Warning));
            Assert.AreEqual(response.Message, localizer["msgRecordNotExist", new string[] { "Formula" }].Value);
        }

        [Test]
        public async Task Service_Should_Update_Claims_Mapping_Data()
        {
            var recordList = new List<ClaimModel>
            {
                new ClaimModel() { ClaimID = 1 },
                new ClaimModel() { ClaimID = 2, Description = "Test" }
            };
            formulaClaimMappingRepository.Setup(r => r.GetAsync(It.IsAny<System.Linq.Expressions.Expression<Func<FormulaClaimMapping, bool>>>())).Returns(GetFormulaClaimMappingAsync(false));
            var response = await formulaMasterService.SaveClaims(1, recordList);

            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
            Assert.AreEqual(response.Message, localizer["msgUpdateSuccess", new string[] { "Claims" }].Value);
        }

        [Test]
        public async Task Service_Should_Add_New_Claims_Mapping_Data_When_New_Claim_Selected_In_Edit()
        {
            var recordList = new List<ClaimModel> { new ClaimModel() { ClaimID = 1 } };
            formulaClaimMappingRepository.Setup(r => r.GetAsync(It.IsAny<System.Linq.Expressions.Expression<Func<FormulaClaimMapping, bool>>>())).Returns(GetFormulaClaimMappingAsync(true));
            var response = await formulaMasterService.SaveClaims(1, recordList);

            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
            Assert.AreEqual(response.Message, localizer["msgUpdateSuccess", new string[] { "Claims" }].Value);
        }

        [Test]
        public async Task Service_Should_Delete_Claims_When_No_Claim_Has_Selected_In_Edit()
        {
            var response = await formulaMasterService.SaveClaims(1, null);
            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
        }

        [Test]
        public async Task Service_Should_Update_Formula_When_Exception_In_Claim_Update()
        {
            var recordList = new List<ClaimModel> { new ClaimModel() { ClaimID = 1 } };
            formulaClaimMappingRepository.Setup(r => r.Delete(It.IsAny<System.Linq.Expressions.Expression<Func<FormulaClaimMapping, bool>>>())).Throws(new Exception("something went wrong"));
            var response = await formulaMasterService.SaveClaims(9, recordList);

            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public void Service_Should_Pass_Values_To_FormulaMaster_Entities()
        {
            FormulaMaster obj = new FormulaMaster()
            {
                FormulaChangeNote = "test",
                TextureAppearanceNote = "test",
                ProjectInfoNote = "test",
                PackagingInfoForPowderNote = "test",
                DatasheetNote = "test",
                IsActive = true,
                FormulaCriteriaMapping = new Collection<FormulaCriteriaMapping>() { new FormulaCriteriaMapping() { FormulaID = 1, CriteriaID = 1 } },
                FormulaClaimMapping = new Collection<FormulaClaimMapping>() { new FormulaClaimMapping() { FormulaID = 1, ClaimID = 1 } },
                CreatedBy = 8888,
                CreatedOn = DateTime.Now,
                FormulaID = 1,
                IsDeleted = false,
                UpdatedBy = 8888,
                UpdatedOn = DateTime.Now
            };
            Assert.IsNotNull(obj);
        }

        [Test]
        public async Task Service_Should_Return_All_ClaimMaster_Data()
        {
            var response = await formulaMasterService.GetClaimsByFormulaID(1);

            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
            Assert.IsNotNull(response.Data);
        }

        [Test]
        public async Task Service_Should_Not_Return_Mapping_Claim_Data_When_Invaild_FormulaID()
        {
            var response = await formulaMasterService.GetClaimsByFormulaID(-1);

            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
            Assert.That(response.Data.FirstOrDefault().FormulaClaimMapID, Is.EqualTo(0));
        }

        [Test]
        public async Task Service_Should_Not_Return_Mapping_Claim_Data_When_Exception()
        {
            claimMasterRepository.Setup(r => r.Query(true)).Throws(new Exception("something went wrong"));
            var response = await formulaMasterService.GetClaimsByFormulaID(99);

            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_Pass_Values_To_ClaimMaster_Entities()
        {
            ClaimMaster Data = new ClaimMaster()
            {
                ClaimCode = "test",
                ClaimDescription = "test",
                ClaimGroupType = "test",
                IsActive = true,
                IsDeleted = false,
                FormulaClaimMapping = new Collection<FormulaClaimMapping>()
                {
                    new FormulaClaimMapping() { ClaimID = 1, FormulaID = 1, CreatedBy = 8888,
                    CreatedOn = DateTime.Now, Description = "", UpdatedBy = 8888, UpdatedOn = DateTime.Now,
                    FormulaClaimMapID=1, ClaimMaster=new ClaimMaster(), FormulaMaster=new FormulaMaster()
                    }
                },
                HasImpact = false,
                CreatedBy = 8888,
                CreatedOn = DateTime.Now,
                ClaimID = 587,
                UpdatedBy = 8888,
                UpdatedOn = DateTime.Now
            };
            await Task.FromResult(Data);
            Assert.IsNotNull(Data);
        }

        [Test]
        public async Task Service_Should_Return_All_Criteria_Data()
        {
            var response = await formulaMasterService.GetCriteriaByFormulaID(1);

            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
            Assert.IsNotNull(response.Data);
        }

        [Test]
        public async Task Service_Should_Not_Return_Criteria_Mapping_Data_When_Invaild_FormulaID()
        {
            var response = await formulaMasterService.GetCriteriaByFormulaID(-1);

            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
            Assert.That(response.Data.FirstOrDefault().FormulaCriteriaMapID, Is.EqualTo(0));
        }

        [Test]
        public void Service_Should_Pass_Value_To_CriteriaMaster_Entities()
        {
            CriteriaMaster criteriaMaster = new CriteriaMaster()
            {
                CriteriaDescription = "test",
                ColorCode = "yellow",
                CriteriaOrder = "A",
                IsActive = true,
                IsDeleted = false,
                CriteriaID = 125,
                CreatedBy = 8888,
                CreatedOn = DateTime.Now,
                UpdatedBy = 8888,
                UpdatedOn = DateTime.Now,
                FormulaCriteriaMapping = new Collection<FormulaCriteriaMapping>()
                {
                    new FormulaCriteriaMapping()
                    {
                        CriteriaID = 1, FormulaID = 1, CreatedBy = 8888, CreatedOn = DateTime.Now,
                        CriteriaMaster=new CriteriaMaster(), FormulaCriteriaMapID = 1, FormulaMaster=new FormulaMaster(), UpdatedBy=8888, UpdatedOn=DateTime.Now
                    }
                }
            };
            Assert.IsNotNull(criteriaMaster);
        }

        [Test]
        public async Task Service_Should_Not_Return_Criteria_When_Exception()
        {
            criteriaMasterRepository.Setup(r => r.Query(true)).Throws(new Exception("something went wrong"));
            var response = await formulaMasterService.GetCriteriaByFormulaID(1);

            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_Fetch_And_Calculate_PDCAAS_Data()
        {
            FormulaIngredient[] arrIngredients = new FormulaIngredient[] 
            { 
                new FormulaIngredient() { IngredientID = 1, Amount = 3 },
                new FormulaIngredient() { IngredientID = 2, Amount = 2 }
            };
            var response = await formulaMasterService.GetPDCAASInfo(arrIngredients);
            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
            Assert.IsNotNull(response.Data);
        }

        [Test]
        public async Task Service_Should_Not_Return_PDCAAS_Info_When_Exception()
        {
            ingredientMasterRepository.Setup(r => r.Query(true)).Throws(new Exception("something went wrong"));
            var response = await formulaMasterService.GetPDCAASInfo(null);
            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_Fetch_And_Calculate_Amino_Acid_Data()
        {
            var data = GetIngredientMasterMockObject();
            decimal? nutrientValue = data.First(o => o.IngredientID == 1)
                                .IngredientNutrientMapping
                                .First(o => o.NutrientID == 1)
                                .NutrientValue;

            FormulaIngredient[] arrIngredients = new FormulaIngredient[] { new FormulaIngredient() { IngredientID = 1, Amount = 3 } };
            var response = await formulaMasterService.GetAminoAcidInfo(arrIngredients);

            var result = response.Data.First(o => o.NutrientId == 1).NutrientValue;
            var calculatedValue = Math.Round((Convert.ToDecimal(nutrientValue) / 100) * Convert.ToDecimal(arrIngredients[0].Amount), 2);
            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
            Assert.IsNotNull(response.Data);
            Assert.AreEqual(calculatedValue, result);
        }

        [Test]
        public async Task Service_Should_Return_Sum_Same_Ingredient_Amount_And_Return_Amino_Acid_Data()
        {
            FormulaIngredient[] arrIngredients = new FormulaIngredient[]
            {
                new FormulaIngredient() { IngredientID = 1, Amount = 3 },
                new FormulaIngredient() { IngredientID = 1, Amount = 6 }
            };
            var data = GetIngredientMasterMockObject();
            decimal? nutrientValue = data.First(o => o.IngredientID == 1)
                                .IngredientNutrientMapping
                                .First(o => o.NutrientID == 1)
                                .NutrientValue;

            var response = await formulaMasterService.GetAminoAcidInfo(arrIngredients);

            var result = response.Data.First(o => o.NutrientId == 1).NutrientValue;
            var calculatedValue = Math.Round((Convert.ToDecimal(nutrientValue) / 100) * Convert.ToDecimal(arrIngredients.Sum(o => o.Amount)), 2);
            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
            Assert.AreEqual(calculatedValue, result);
        }

        [Test]
        public async Task Service_Should_Not_Return_Amino_Acid_Info_When_Exception()
        {
            nutrientMasterRepository.Setup(r => r.GetManyAsync(o => o.IsActive && !o.IsDeleted && o.IsAminoAcid, true)).Throws(new Exception("something went wrong"));
            var response = await formulaMasterService.GetAminoAcidInfo(null);

            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_Fetch_And_Calculate_Carbohydrate_Data()
        {
            FormulaIngredient[] arrIngredients = new FormulaIngredient[]
            {
                new FormulaIngredient() { IngredientID = 1, Amount = 7 },
                new FormulaIngredient() { IngredientID = 2, Amount = 3 }
            };
            var response = await formulaMasterService.GetCarbohydrateInfo(arrIngredients);
            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
            Assert.IsNotNull(response.Data);
        }

        [Test]
        public async Task Service_Should_Return_Carbohydrate_Ingredient_Breakdown_Value_As_Zero_When_No_Data_Pass()
        {
            var response = await formulaMasterService.GetCarbohydrateInfo(null);
            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
            Assert.AreEqual(0, response.Data.TotalCarbs);
        }

        [Test]
        public async Task Service_Should_Not_Return_Carbohydrate_Data_When_Exception()
        {
            FormulaIngredient[] arrIngredients = new FormulaIngredient[]
            {
                new FormulaIngredient() { IngredientID = 5, Amount = 7 }
            };
            ingredientMasterRepository.Setup(r => r.Query(true)).Throws(new Exception("something went wrong"));
            var response = await formulaMasterService.GetCarbohydrateInfo(arrIngredients);
            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_Fetch_And_Calculate_DSActual_Data()
        {
            var arrIngredients = new FormulaIngredient[]
            {
                new FormulaIngredient() { IngredientID = 1, Amount = 5 }
            };
            var response = await formulaMasterService.GetDSActualInfo(arrIngredients);
            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
            Assert.IsNotNull(response.Data);
        }

        [Test]
        public async Task Service_Should_Not_Return_DSActual_Data_When_Exception()
        {
            var arrIngredients = new FormulaIngredient[]
            {
                new FormulaIngredient() { IngredientID = 99, Amount = 99 }
            };
            ingredientMasterRepository.Setup(r => r.Query(true)).Throws(new Exception("something went wrong"));
            var response = await formulaMasterService.GetDSActualInfo(arrIngredients);
            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_Fetch_And_Calculate_IngredientList_Data()
        {
            var arrIngredients = new FormulaIngredient[]
            {
                new FormulaIngredient() { IngredientID = 1, Amount = 6, Percent = 2 },
                new FormulaIngredient() { IngredientID = 2, Amount = 6, Percent = 2 }
            };
            var response = await formulaMasterService.GetIngredientListInfo(arrIngredients);
            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
            Assert.IsNotNull(response.Data);
        }

        [Test]
        public async Task Service_Should_Not_Return_IngredientList_Data_When_Exception()
        {
            var arrIngredients = new FormulaIngredient[]
            {
                new FormulaIngredient() { IngredientID = 99, Amount = 99, Percent = 9 }
            };
            ingredientMasterRepository.Setup(r => r.Query(true)).Throws(new Exception("something went wrong"));
            var response = await formulaMasterService.GetIngredientListInfo(arrIngredients);
            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_Update_Criteria_Mapping_Data()
        {
            formulaCriteriaMappingRepository.Setup(r => r.GetAsync(It.IsAny<System.Linq.Expressions.Expression<Func<FormulaCriteriaMapping, bool>>>())).Returns(GetFormulaCriteriaMappingAsync());
            var response = await formulaMasterService.SaveCriteria(1, new int[] { 1, 2 });

            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
            Assert.AreEqual(response.Message, localizer["msgUpdateSuccess", new string[] { "Criteria" }].Value);
        }

        [Test]
        public async Task Service_Should_Insert_New_Criteria_On_Save_Formula()
        {
            var response = await formulaMasterService.SaveCriteria(1, new int[] { 1, 2 });

            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
            Assert.AreEqual(response.Message, localizer["msgUpdateSuccess", new string[] { "Criteria" }].Value);
        }

        [Test]
        public async Task Service_Should_Delete_Criteria_Mapping_Data_When_Not_Selected()
        {
            var response = await formulaMasterService.SaveCriteria(1, null);

            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
            Assert.AreEqual(response.Message, localizer["msgUpdateSuccess", new string[] { "Criteria" }].Value);
        }

        [Test]
        public async Task Service_Should_Not_Update_Criteria_Mapping_Data_When_Exception()
        {
            formulaCriteriaMappingRepository.Setup(r => r.DeleteAsync(It.IsAny<System.Linq.Expressions.Expression<Func<FormulaCriteriaMapping, bool>>>())).Throws(new Exception("something went wrong"));
            var response = await formulaMasterService.SaveCriteria(1, new int[] { 1, 2 });

            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [TestCase("1_for_instruction")]
        [TestCase("2_for_ingredients")]
        [TestCase("3_for_subformula")]
        [TestCase("4_SaveFormulaCodeIncrements")]
        [TestCase("5_WhenFormulaCodeResultIsNull")]
        public async Task Service_Should_Save_Formula_Instructions_And_Igredients_When_Valid_Data(string formulaReferType)
        {
            var formulaModel = GetMockDataForFormulaSave();
            if (formulaReferType == "1_for_instruction")
            {
                formulaModel.FormulaDetails.FirstOrDefault().ReferenceType = 1;
            }
            if (formulaReferType == "2_for_ingredients")
            {
                formulaModel.FormulaDetails.FirstOrDefault().ReferenceType = 2;
                formulaModel.FormulaDetails.FirstOrDefault().HierarchyRowID = "1";
                formulaModel.FormulaDetails.FirstOrDefault().Description = "Syrup for cough";
                formulaModel.FormulaDetails.FirstOrDefault().OvgPercent = Convert.ToDecimal(2.12);
                formulaModel.FormulaDetails.FirstOrDefault().ParentRowID = 1;
                formulaModel.FormulaDetails.FirstOrDefault().RowNumber = 1;
                formulaModel.FormulaDetails.FirstOrDefault().SubgroupPercent = Convert.ToDecimal(2.12);
                formulaModel.FormulaDetails.FirstOrDefault().Description = string.Empty;
            }
            if (formulaReferType == "3_for_subformula")
            {
                formulaModel.FormulaDetails.FirstOrDefault().ReferenceType = 3;
                formulaModel.FormulaDetails.FirstOrDefault().Description = string.Empty;
            }
            if (formulaReferType == "4_SaveFormulaCodeIncrements")
            {
                formulaModel.FormulaMaster.FormulaCode = "S10600999.002a-085";
                formulaMasterRepository.Setup(r => r.GetAsync(x => x.FormulaCode == formulaModel.FormulaMaster.FormulaCode)).Returns(GetFormulaMasterAsync());
            }
            if (formulaReferType == "5_WhenFormulaCodeResultIsNull")
            {             
                IQueryable<FormulaChangeCode> queryableData = null;
                formulaChangeCodeRepository.Setup(r => r.Query(true)).Returns(queryableData);
                formulaModel.FormulaMaster.FormulaCode = "S10600999.002a-085";
                formulaMasterRepository.Setup(r => r.GetAsync(x => x.FormulaCode == formulaModel.FormulaMaster.FormulaCode)).Returns(GetFormulaMasterAsync());
            }
            var response = await formulaMasterService.SaveFormula(formulaModel);
            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
        }

        [Test]
        public async Task Service_Should_Save_Formula_Without_Child_Details()
        {
            string formulaCode = "S10600999.009a-099";
            string formulaTypeCode = formulaCode.Split(new char[] { '.' })[0].Substring(0, 3);
            var formulaModel = GetMockDataForFormulaSave();
            formulaModel.FormulaMaster.FormulaCode = formulaCode;
            formulaModel.FormulaDetails = null;
            formulaModel.ClaimInfo = null;
            formulaModel.CriteriaInfo = null;
            formulaModel.lstFormulaStatus = null;
            formulaModel.FormulaMaster.SiteID = 5;
            formulaMasterRepository.Setup(r => r.GetAsync(x => x.FormulaCode == formulaModel.FormulaMaster.FormulaCode)).Returns(GetFormulaMasterAsync());
            formulaTypeMasterRepository.Setup(r => r.GetAsync(x => x.FormulaTypeCode == formulaTypeCode)).Returns(GetFormulaTypeMasterMockObject());
            var response = await formulaMasterService.SaveFormula(formulaModel);
            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
        }

        [Test]
        public async Task Service_Should_Give_Warning_FormulaMaster_Object_Is_Null()
        {
            formulaMasterRepository.Setup(r => r.AddAsync(It.IsAny<FormulaMaster>())).Throws(new Exception("something went wrong"));
            var formulaModel = GetMockDataForFormulaSave();
            var response = await formulaMasterService.SaveFormula(formulaModel);
            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_Not_Save_Formula_Instructions_And_Igredients_When_InValid_Data()
        {
            var formulaModel = GetMockDataForFormulaSave();
            formulaModel.FormulaMaster.CanadaMavMin = "123456789098765431124534";
            var response = await formulaMasterService.SaveFormula(formulaModel);
            Assert.That(response.Result, Is.EqualTo(ResultType.Warning));
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        public async Task Service_Should_Save_Formula_Yield_Loss_Data(int formulaID)
        {
            var formulaModel = GetObjectMockFormulaYieldLoss();
            var response = await formulaMasterService.SaveYieldLoss(formulaID, formulaModel);
            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
        }

        [Test]
        public async Task Service_Should_Not_Save_Formula_Yield_Loss_Data_When_Exception()
        {
            var formulaModel = GetObjectMockFormulaYieldLoss();
            formulaMasterRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>())).Throws(new Exception("something went wrong"));
            var response = await formulaMasterService.SaveYieldLoss(9, formulaModel);
            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_Return_Formula_Regulatory_Category_Master_Data()
        {
            var response = await formulaMasterService.GetFormulaRegulatoryCategoryMaster();
            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
            Assert.Greater(response.Data.Count, 0);
        }

        [Test]
        public async Task Service_Should_Not_Return_Formula_Regulatory_Category_Master_Data_When_Exception()
        {
            formulaRegulatoryCategoryMasterRepository.Setup(r => r.Query(true)).Throws(new Exception("something went wrong"));
            var response = await formulaMasterService.GetFormulaRegulatoryCategoryMaster();
            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_Return_Datasheet_Format_Master_Data()
        {
            var response = await formulaMasterService.GetDatasheetFormatMaster();
            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
            Assert.Greater(response.Data.Count, 0);
        }

        [Test]
        public async Task Service_Should_Not_Return_Datasheet_Format_Master_Data_When_Exception()
        {
            datasheetFormatMasterRepository.Setup(r => r.Query(true)).Throws(new Exception("something went wrong"));
            var response = await formulaMasterService.GetDatasheetFormatMaster();
            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]


        public async Task Service_Should_Return_All_Reconstitution_Master_Data()
        {
            var response = await formulaMasterService.GetAllReconstitutionMaster();

            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
            Assert.IsNotNull(response.Data);
        }

        [Test]
        public async Task Service_Should_Not_Return_Reconstitution_Master_Data_When_Exception()
        {
            reconstitutionMasterRepository.Setup(r => r.GetManyAsync(o => o.IsActive && !o.IsDeleted, true)).Throws(new Exception("something went wrong"));
            var response = await formulaMasterService.GetAllReconstitutionMaster();

            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_Return_All_PowderLiquid_Master_Data()
        {
            var response = await formulaMasterService.GetAllPowderLiquidMaster();

            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
            Assert.IsNotNull(response.Data);
        }

        [Test]
        public async Task Service_Should_Not_Return_PowderLiquid_Master_Data_When_Exception()
        {
            powderLiquidMasterRepository.Setup(r => r.GetManyAsync(o => o.IsActive && !o.IsDeleted, true)).Throws(new Exception("something went wrong"));
            var response = await formulaMasterService.GetAllPowderLiquidMaster();

            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_Return_All_PowderBlender_Master_Data()
        {
            var response = await formulaMasterService.GetAllPowderBlenderMaster(0);

            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
            Assert.IsNotNull(response.Data);
        }

        [Test]
        public async Task Service_Should_Not_Return_PowderBlender_Master_Data_When_Exception()
        {
            powderBlenderSiteMappingRepository.Setup(r => r.Query(true)).Throws(new Exception("something went wrong"));
            var response = await formulaMasterService.GetAllPowderBlenderMaster(0);

            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_Return_All_PowderUnit_Master_Data()
        {
            var response = await formulaMasterService.GetAllPowderUnitMaster(0);

            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
            Assert.IsNotNull(response.Data);
        }

        [Test]
        public async Task Service_Should_Not_Return_PowderUnit_Master_Data_When_Exception()
        {
            powderUnitServingSiteMappingRepository.Setup(r => r.Query(true)).Throws(new Exception("something went wrong"));
            var response = await formulaMasterService.GetAllPowderUnitMaster(0);

            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_Return_All_PowderUnitServing_Master_Data()
        {
            var response = await formulaMasterService.GetAllPowderUnitServingMaster();

            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
            Assert.IsNotNull(response.Data);
        }

        [Test]
        public async Task Service_Should_Not_Return_PowderUnitServing_Master_Data_When_Exception()
        {
            unitServingMasterRepository.Setup(r => r.GetManyAsync(o => o.IsActive && !o.IsDeleted && o.UnitServingType.ToUpper() == "UNIT/SERVING", true)).Throws(new Exception("something went wrong"));
            var response = await formulaMasterService.GetAllPowderUnitServingMaster();

            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_Return_FormulaYieldLoss_Factor_DefaultValues()
        {
            var response = await formulaMasterService.GetFormulaYieldLossFactorDefaultValue(1, "planttrials");

            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
            Assert.IsNotNull(response.Data);
        }
        
        [Test]
        public async Task Service_Should_Return_Validation_Message_When_SiteId_OR_MixerType_IsInvalid()
        {
            var response = await formulaMasterService.GetFormulaYieldLossFactorDefaultValue(0, "");

            Assert.That(response.Message, Is.EqualTo(localizer["msgRecordNotExist", new string[] { "Formula" }].Value));
            Assert.IsNull(response.Data);
        }

        [Test]
        public async Task Service_Should_Not_Return_FormulaYieldLoss_Factor_DefaultValues_When_Exception()
        {
            formulaMasterRepository.Setup(r => r.GetFromStoredProcedureAsync("GetFormulaYieldLossFactorDefaultValue", It.IsAny<(string, object)[]>())).Throws(new Exception("something went wrong"));
            var response = await formulaMasterService.GetFormulaYieldLossFactorDefaultValue(9, "planttrials");

            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_Return_User_Formula_Search_History()
        {
            var response = await formulaMasterService.GetFormulaSearchHistory(1);
            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
            Assert.Greater(response.Data.Count, 0);
        }

        [Test]
        public async Task Service_Should_Not_Return_User_Formula_Search_History_When_Exception()
        {
            int userID = 9;
            formulaSearchHistoryRepository.Setup(r => r.GetManyAsync(o => o.UserID == userID, true)).Throws(new Exception("something went wrong"));
            var response = await formulaMasterService.GetFormulaSearchHistory(userID);
            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_Not_Update_User_Formula_Search_History_When_Exception()
        {
            int userID = 9;
            formulaSearchHistoryRepository.Setup(r => r.GetMany(o => o.UserID == userID, true)).Throws(new Exception("something went wrong"));
            var response = await formulaMasterService.UpdateFormulaSearchHistory(userID, string.Empty);
            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_Return_Formula_Header_Page_Information()
        {
            var response = await formulaMasterService.GetFormulaHomePageHeaderInfo(1);

            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
            Assert.IsNotNull(response.Data);
        }
        
        [Test]
        public async Task Service_Should_Return_Validation_Message_When_FormulaID_IsInvalid()
        {
            var response = await formulaMasterService.GetFormulaHomePageHeaderInfo(0);

            Assert.That(response.Message, Is.EqualTo(localizer["msgRecordNotExist", new string[] { "Formula" }].Value));
            Assert.IsNull(response.Data);
        }

        [Test]
        public async Task Service_Should_Not_Return_Formula_Header_Page_Information_When_Exception()
        {
            formulaMasterRepository.Setup(r => r.GetFromStoredProcedureAsync("GetFormulaHomePageHeaderInfo", It.IsAny<(string, object)[]>())).Throws(new Exception("something went wrong"));
            var response = await formulaMasterService.GetFormulaHomePageHeaderInfo(9);

            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        [TestCase(new int[] { 1 })]
        [TestCase(null)]
        public async Task Service_Should_Delete_Formula_By_FormulaID(int[] formulaIDs)
        {
            var response = await formulaMasterService.DeleteFormula(formulaIDs);

            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
            Assert.That(response.Message, Is.EqualTo(localizer["msgDeleteSuccess", new string[] { "Formula" }].Value));
            Assert.IsNotNull(response.Data);
        }

        [Test]
        public async Task Service_Should_Not_Delete_Formula_When_Exception()
        {
            formulaMasterRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>())).Throws(new Exception("something went wrong"));
            var response = await formulaMasterService.DeleteFormula(new int[] { 1 });

            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_Return_Formula_ChangeCodes()
        {
            var formulaChangeCodeParam = new FormulaChangeCodeParam()
            {
                FormulaTypeCode = "S10",
                CurrentSiteID = 1,
                ChangeSiteID = 0,
                ServingSize = "033",
                FormulaCode = "S10600115.022a-033",
                FormulaID = 1
            };
            var response = await formulaMasterService.GetFormulaChangeCodeList(formulaChangeCodeParam);
            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
            Assert.Greater(response.Data.Rows.Count, 0);
        }

        [Test]
        public async Task Service_Should_Not_Return_Formula_ChangeCodes_When_SiteID_Is_Zero()
        {
            var response = await formulaMasterService.GetFormulaChangeCodeList(null);
            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
            Assert.AreEqual(localizer["msgRecordNotExist", new string[] { "Formula" }].Value, response.Message);
        }

        [Test]
        public async Task Service_Should_Not_Return_Formula_ChangeCodes_When_Exception()
        {
            var formulaChangeCodeParam = new FormulaChangeCodeParam() { CurrentSiteID = 5 };
            formulaMasterRepository.Setup(r => r.GetFromStoredProcedureAsync("GetFormulaChangeCodes", It.IsAny<(string, object)[]>())).Throws(new Exception("something went wrong"));
            var response = await formulaMasterService.GetFormulaChangeCodeList(formulaChangeCodeParam);
            
            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_Return_Validation_Message_If_FormulaCode_Already_Exist()
        {
            string formulaCode = "S10600999.002a-085";
            formulaMasterRepository.Setup(r => r.Any(x => x.FormulaCode == formulaCode.Trim())).Returns(true);
            var response = await formulaMasterService.IsFormulaCodeExist(formulaCode);
            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
            Assert.That(response.Data, Is.EqualTo(true));
            Assert.That(response.Message, Is.EqualTo(localizer["msgDuplicateRecord", new string[] { "Formula Code" }].Value));
        }
        
        [Test]
        public async Task Service_Should_Return_Validation_Message_If_FormulaCode_Not_In_Correct_Format()
        {
            string formulaCode = string.Empty;
            formulaMasterRepository.Setup(r => r.Any(x => x.FormulaCode == formulaCode.Trim())).Returns(false);
            var response = await formulaMasterService.IsFormulaCodeExist(formulaCode);
            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
            Assert.That(response.Data, Is.EqualTo(false));
            Assert.That(response.Message, Is.EqualTo(localizer["msgInvalidCodeFormat"].Value));
        }

        [Test]
        public async Task Service_Should_Not_Return_Validation_Message_When_Exception()
        {
            string formulaCode = "S10600999.009a-099";
            formulaMasterRepository.Setup(r => r.Any(x => x.FormulaCode == formulaCode.Trim())).Throws(new Exception("something went wrong"));
            var response = await formulaMasterService.IsFormulaCodeExist(formulaCode);
            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_Return_All_Formula_Status()
        {
            var response = await formulaMasterService.GetFormulaStatusMaster();
            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
            Assert.Greater(response.Data.Count, 0);
        }

        [Test]
        public async Task Service_Should_Not_Return_Formula_Status_When_Exception()
        {
            formulaStatusMasterRepository.Setup(r => r.Query(true)).Throws(new Exception("something went wrong"));
            var response = await formulaMasterService.GetFormulaStatusMaster();
            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_Return_All_Formula_targetModel()
        {
            FormulaTargetInfoParam formulaTargetInfoParam = new FormulaTargetInfoParam() { DatasheetFormatID = 1, IngredientID = "717,1414" };
            var response = await formulaMasterService.GetTargetInfo(formulaTargetInfoParam);
            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
            Assert.Greater(response.Data.Count, 0);
        }

        #region Setup Dummy Data & Repository - FormulaMaster
        private Mock<IRepository<FormulaMaster>> SetupFormulaMasterRepository()
        {
            var repo = new Mock<IRepository<FormulaMaster>>();
            ICollection<FormulaMaster> recordList = GetFormulaMasterMockData();
            IQueryable<FormulaMaster> queryableData = recordList.AsQueryable();
            DataTable dtFormula = GetFormulaMockDataTable();
            DataTable dtYieldLoss = GetYieldLossMockDataTable();
            DataTable dtFormulaPageHeaderInfo = GetFormulaHomePageHeaderInfo();
            DataTable dtFormulaChangeCode = GetFormulaChangeCodesMockDataTable();
            DataTable dtFormulaTargetInfo = GetFormulaTargetInfoMockDataTable();
            var objResponse = new GeneralResponse<FormulaMaster>();

            repo.Setup(r => r.AddAsync(It.IsAny<FormulaMaster>()));
            repo.Setup(r => r.UpdateAsync(It.IsAny<FormulaMaster>()));
            repo.Setup(r => r.DeleteAsync(It.IsAny<FormulaMaster>()));
            repo.Setup(r => r.Query(true)).Returns(queryableData);
            repo.Setup(r => r.GetAsync(o => o.FormulaID == 1)).Returns(GetFormulaMasterAsync());
            repo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(() => { objResponse.Data = GetFormulaMasterMockObject(1); return objResponse.Data; });
            repo.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(() => { objResponse.Data = null; return objResponse.Data; });
            repo.Setup(r => r.GetFromStoredProcedureAsync("SearchFormula", It.IsAny<(string, object)[]>())).ReturnsAsync(dtFormula);
            repo.Setup(r => r.GetFromStoredProcedureAsync("GetFormulaYieldLossFactorDefaultValue",
                                new Tuple<string, object>("siteID", 1).ToValueTuple(),
                                new Tuple<string, object>("mixerType", "planttrials").ToValueTuple()
                )).ReturnsAsync(dtYieldLoss);
            repo.Setup(r => r.GetFromStoredProcedureAsync("GetFormulaHomePageHeaderInfo",
                               new Tuple<string, object>("formulaID", 1).ToValueTuple()
               )).ReturnsAsync(dtFormulaPageHeaderInfo);
            repo.Setup(r => r.GetFromStoredProcedureAsync("GetFormulaChangeCodes", It.IsAny<(string, object)[]>())).ReturnsAsync(dtFormulaChangeCode);
            repo.Setup(r => r.GetFromStoredProcedureAsync("GetTargetInfo", It.IsAny<(string, object)[]>())).ReturnsAsync(dtFormulaTargetInfo);
            Task.Run(() => recordList);
            return repo;
        }

        private DataTable GetFormulaMockDataTable()
        {
            DataTable tbl = new DataTable();
            tbl.Columns.Add("FormulaID", typeof(int));
            tbl.Columns.Add("FormulaCode", typeof(string));
            tbl.Columns.Add("ProductCode", typeof(string));
            tbl.Columns.Add("ProjectDescription", typeof(string));
            tbl.Columns.Add("Claims", typeof(string));
            tbl.Columns.Add("PowderLiquidAmount", typeof(string));
            tbl.Columns.Add("CustomerCode", typeof(string));
            tbl.Rows.Add(11, "F0001", "");
            tbl.Rows.Add(12, "F0002", "");
            tbl.Rows.Add(13, "F0003", "");
            return tbl;
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
        private DataTable GetFormulaTargetInfoMockDataTable()
        {
            DataTable tbl = new DataTable();
            tbl.Columns.Add("IngredientID", typeof(int));
            tbl.Columns.Add("NutrientID", typeof(int));
            tbl.Columns.Add("NutrientName", typeof(string));
            tbl.Columns.Add("NutrientValue", typeof(decimal));
            tbl.Columns.Add("RDIValue", typeof(int));
            tbl.Rows.Add(123,1, "Sodium", 2.500M,40);
            return tbl;
        }

        private ICollection<FormulaMaster> GetFormulaMasterMockData()
        {
            ICollection<FormulaMaster> recordList = new Collection<FormulaMaster>();
            recordList.Add(GetFormulaMasterMockObject(1));
            recordList.Add(GetFormulaMasterMockObject(2));
            return recordList;
        }

        private FormulaMaster GetFormulaMasterMockObject(int? id = null)
        {
            FormulaMaster obj = new FormulaMaster()
            {
                FormulaChangeNote = "test",
                TextureAppearanceNote = "test",
                ProjectInfoNote = "test",
                PackagingInfoForPowderNote = "test",
                DatasheetNote = "test",
                IsActive = true,
                UpdatedBy = 1, 
                FormulaCriteriaMapping = new Collection<FormulaCriteriaMapping>() { new FormulaCriteriaMapping() { FormulaID = 1, CriteriaID = 1 } },
                FormulaClaimMapping = new Collection<FormulaClaimMapping>() { new FormulaClaimMapping() { FormulaID = 1, ClaimID = 1, ClaimMaster = new ClaimMaster() } }
            };
            if (id != null)
            {
                obj.FormulaID = Convert.ToInt32(id);
            }
            return obj;
        }

        private Task<FormulaMaster> GetFormulaMasterAsync()
        {
            return Task.FromResult(GetFormulaMasterMockData().FirstOrDefault());
        }

        private FormulaMasterModel GetObjectMockFormula(int? id = null)
        {
            FormulaMasterModel obj = new FormulaMasterModel()
            {
                FormulaChangeNote = "test",
                TextureAppearanceNote = "test",
                ProjectInfoNote = "test",
                PackagingInfoForPowderNote = "test",
                DatasheetNote = "test"
            };
            if (id != null)
            {
                obj.FormulaID = Convert.ToInt32(id);
            }
            return obj;
        }

        private FormulaModel GetMockDataForFormulaSave()
        {
            FormulaModel formulaModel = new FormulaModel();
            formulaModel.FormulaMaster = new FormulaMasterModel() { FormulaID = 0, SiteID = 2, FormulaChangeNote = "Mixing time was changed to 200 Test of 14 sep 2020" };
            formulaModel.FormulaDetails = new List<FormulaDetailsModel>();
            formulaModel.FormulaDetails.Add(new FormulaDetailsModel() { FormulaDetailMapID = 0, FormulaID = 0, ReferenceID = 2, ReferenceType = 1, Description = "__USE Sollich to SLAB base and ADD FONDANT top layer.", RowNumber = 1 });
            formulaModel.FormulaDetails.Add(new FormulaDetailsModel() { FormulaDetailMapID = 0, FormulaID = 0, ReferenceID = 2, ReferenceType = 1, Description = "__USE Sollich to SLAB base and ADD FONDANT top layer.", RowNumber = 1 });
            formulaModel.ClaimInfo = new List<ClaimModel>();
            formulaModel.ClaimInfo.Add(new ClaimModel() { ClaimID = 1, Description = "Contains only Kosher ingredients" });
            formulaModel.ClaimInfo.Add(new ClaimModel() { ClaimID = 2, Description = "Certified OU Dairy" });
            formulaModel.CriteriaInfo = new int[] { 1 };
            formulaModel.lstFormulaStatus = new List<FormulaStatusUpdate>();
            formulaModel.lstFormulaStatus.Add(new FormulaStatusUpdate() { FormulaID = 1, FormulaStatus = "P1" });
            formulaModel.lstFormulaStatus.Add(new FormulaStatusUpdate() { FormulaID = 2, FormulaStatus = "P1" });
            return formulaModel;
        }

        private Mock<IRepository<FormulaClaimMapping>> SetupFormulaClaimMappingRepository()
        {
            var repo = new Mock<IRepository<FormulaClaimMapping>>();
            ICollection<FormulaClaimMapping> recordList = GetFormulaClaimMappingMockData();
            IQueryable<FormulaClaimMapping> queryableData = recordList.AsQueryable();

            repo.Setup(r => r.AddAsync(It.IsAny<FormulaClaimMapping>()));
            repo.Setup(r => r.UpdateAsync(It.IsAny<FormulaClaimMapping>()));
            repo.Setup(r => r.DeleteAsync(It.IsAny<FormulaClaimMapping>()));
            repo.Setup(r => r.Query(true)).Returns(queryableData);
            repo.Setup(r => r.GetAsync(o => o.ClaimID == 1 && o.FormulaID == 1)).Returns(GetFormulaClaimMappingAsync(false));
            Task.Run(() => recordList);
            return repo;
        }

        private Task<FormulaClaimMapping> GetFormulaClaimMappingAsync(bool returnBlank)
        {
            if(!returnBlank)
                return Task.FromResult(GetFormulaClaimMappingMockData().FirstOrDefault());
            else
                return Task.FromResult(GetFormulaClaimMappingMockData().FirstOrDefault(o => o.ClaimID == -1));
        }

        private ICollection<FormulaClaimMapping> GetFormulaClaimMappingMockData()
        {
            ICollection<FormulaClaimMapping> recordList = new Collection<FormulaClaimMapping>();
            recordList.Add(new FormulaClaimMapping() { ClaimID = 1, FormulaID = 1, FormulaClaimMapID = 1 });
            recordList.Add(new FormulaClaimMapping() { ClaimID = 2, FormulaID = 1, FormulaClaimMapID = 2, Description = "Test" });
            return recordList;
        }
        
        private FormulaMasterModel GetObjectMockFormulaYieldLoss()
        {
            FormulaMasterModel formulaMasterModel = new FormulaMasterModel();
            formulaMasterModel.S10CoreDoughYield = 0;
            formulaMasterModel.S65SyrupYield = 0;
            formulaMasterModel.S68_1LiquidBlendYield = Convert.ToDecimal(1.23);
            formulaMasterModel.S68_2LiquidBlendYield = Convert.ToDecimal(4.23);
            formulaMasterModel.S40CoreIngredientYield = 0;
            formulaMasterModel.S40ToppingIngredientYield = 0;
            formulaMasterModel.S60DryBlendYield = 0;
            formulaMasterModel.S63DryBlendYield = Convert.ToDecimal(2.10);
            formulaMasterModel.S20LayerDoughYield = 0;
            formulaMasterModel.S30CoatingYield = 0;
            formulaMasterModel.S10CoreDoughYieldLoss = 0;
            formulaMasterModel.S65SyrupYieldLoss = 0;
            formulaMasterModel.S68_1LiquidBlendYieldLoss = 0;
            formulaMasterModel.S68_2LiquidBlendYieldLoss = 0;
            formulaMasterModel.S40CoreIngredientYieldLoss = 0;
            formulaMasterModel.S40ToppingIngredientYieldLoss = 0;
            formulaMasterModel.S60DryBlendYieldLoss = 0;
            formulaMasterModel.S63DryBlendYieldLoss = 0;
            formulaMasterModel.S20LayerDoughYieldLoss = 0;
            formulaMasterModel.S30CoatingYieldLoss = 0;

            return formulaMasterModel;
        }
        #endregion

        #region Setup Dummy Data & Repository - ClaimMaster
        private Mock<IRepository<ClaimMaster>> SetupClaimMasterRepository()
        {
            var repo = new Mock<IRepository<ClaimMaster>>();

            var mockData = GetGeneralClaimsMockData();
            IQueryable<ClaimMaster> queryableData = mockData.Result.AsQueryable();

            repo.Setup(r => r.Query(true)).Returns(queryableData);
            return repo;
        }

        private async Task<ICollection<ClaimMaster>> GetGeneralClaimsMockData()
        {
            List<ClaimMaster> recordList = new List<ClaimMaster>();
            recordList.Add(GetClaimMockObject(1));
            recordList.Add(GetClaimMockObject(2));
            return await Task.FromResult(recordList);
        }

        private ClaimMaster GetClaimMockObject(int? id = null)
        {
            ClaimMaster obj = new ClaimMaster()
            {
                ClaimCode = "test",
                ClaimDescription = "test",
                ClaimGroupType = "test",
                IsActive = true,
                IsDeleted = false,
                FormulaClaimMapping = new Collection<FormulaClaimMapping>() { new FormulaClaimMapping() { ClaimID = 1, FormulaID = 1 } }
            };
            if (id != null)
            {
                obj.ClaimID = Convert.ToInt32(id);
            }
            return obj;
        }
        #endregion

        #region Setup Dummy Data & Repository - CriteriaMaster
        private Mock<IRepository<CriteriaMaster>> SetupCriteriaMasterRepository()
        {
            var repo = new Mock<IRepository<CriteriaMaster>>();

            var mockData = GetGeneralCriteriaMockData();
            IQueryable<CriteriaMaster> queryableData = mockData.Result.AsQueryable();

            repo.Setup(r => r.Query(true)).Returns(queryableData);
            return repo;
        }

        private async Task<ICollection<CriteriaMaster>> GetGeneralCriteriaMockData()
        {
            List<CriteriaMaster> recordList = new List<CriteriaMaster>();
            recordList.Add(GetCriteriaMockObject(1));
            recordList.Add(GetCriteriaMockObject(2));
            return await Task.FromResult(recordList);
        }

        private CriteriaMaster GetCriteriaMockObject(int? id = null)
        {
            CriteriaMaster obj = new CriteriaMaster()
            {
                CriteriaDescription = "test",
                ColorCode = "yellow",
                CriteriaOrder = "A",
                IsActive = true,
                IsDeleted = false,
                FormulaCriteriaMapping = new Collection<FormulaCriteriaMapping>() { new FormulaCriteriaMapping() { CriteriaID = 1, FormulaID = 1 } }
            };
            if (id != null)
            {
                obj.CriteriaID = Convert.ToInt32(id);
            }
            return obj;
        }

        private Mock<IRepository<FormulaCriteriaMapping>> SetupFormulaCriteriaMappingRepository()
        {
            var repo = new Mock<IRepository<FormulaCriteriaMapping>>();

            ICollection<FormulaCriteriaMapping> recordList = GetFormulaCriteriaMappingMockData();

            IQueryable<FormulaCriteriaMapping> queryableData = recordList.AsQueryable();

            repo.Setup(r => r.AddAsync(It.IsAny<FormulaCriteriaMapping>()));
            repo.Setup(r => r.UpdateAsync(It.IsAny<FormulaCriteriaMapping>()));
            repo.Setup(r => r.DeleteAsync(It.IsAny<FormulaCriteriaMapping>()));
            repo.Setup(r => r.Query(true)).Returns(queryableData);
            repo.Setup(r => r.GetAsync(o => o.CriteriaID == 1 && o.FormulaID == 1)).Returns(GetFormulaCriteriaMappingAsync());
            Task.Run(() => recordList);
            return repo;
        }

        private Task<FormulaCriteriaMapping> GetFormulaCriteriaMappingAsync()
        {
            return Task.FromResult(GetFormulaCriteriaMappingMockData().FirstOrDefault());
        }

        private ICollection<FormulaCriteriaMapping> GetFormulaCriteriaMappingMockData()
        {
            ICollection<FormulaCriteriaMapping> recordList = new Collection<FormulaCriteriaMapping>();
            recordList.Add(new FormulaCriteriaMapping() { CriteriaID = 1, FormulaID = 1, FormulaCriteriaMapID = 1 });
            recordList.Add(new FormulaCriteriaMapping() { CriteriaID = 2, FormulaID = 1, FormulaCriteriaMapID = 2 });
            return recordList;
        }

        #endregion

        #region Setup Dummy Data & Repository - NutrientMaster
        private Mock<IRepository<NutrientMaster>> SetupNutrientMasterRepository()
        {
            var repo = new Mock<IRepository<NutrientMaster>>();
            var mockData = GetMockNutrienListAsync();
            var recordList = GetMockNutrienList();
            repo.Setup(r => r.GetManyAsync(o => o.IsActive && !o.IsDeleted && o.IsAminoAcid, true)).Returns(mockData);
            repo.Setup(r => r.Query(true)).Returns(recordList.AsQueryable());
            return repo;
        }

        private async Task<ICollection<NutrientMaster>> GetMockNutrienListAsync()
        {
            ICollection<NutrientMaster> recordList = GetMockNutrienList();
            return await Task.FromResult(recordList);
        }

        private ICollection<NutrientMaster> GetMockNutrienList()
        {
            ICollection<NutrientMaster> recordList = new Collection<NutrientMaster>();
            recordList.Add(GetNutrientMasterMockObject(1, NutrientName.Protein, 2));
            recordList.Add(GetNutrientMasterMockObject(5, NutrientName.ProteinDigestibility, 3));
            recordList.Add(GetNutrientMasterMockObject(2, NutrientName.TotalFat, 2));
            recordList.Add(GetNutrientMasterMockObject(4, NutrientName.TotalFiber, 2));
            recordList.Add(GetNutrientMasterMockObject(6, NutrientName.TotalSugars, 2));
            recordList.Add(GetNutrientMasterMockObject(7, NutrientName.TotalCarbohydrate, 2));
            recordList.Add(GetNutrientMasterMockObject(8, NutrientName.FATkcalFactor, 2));
            recordList.Add(GetNutrientMasterMockObject(9, NutrientName.CarbkcalFactor, 2));
            recordList.Add(GetNutrientMasterMockObject(10, NutrientName.PROkcalFactor, 2));
            recordList.Add(GetNutrientMasterMockObject(11, NutrientName.NPNkcalFactor, 2));
            recordList.Add(GetNutrientMasterMockObject(12, NutrientName.Polyunsaturated, 2));
            recordList.Add(GetNutrientMasterMockObject(13, NutrientName.NonProximateNutrient, 2));
            recordList.Add(GetNutrientMasterMockObject(14, NutrientName.L_Lysine_Lys, 2));
            recordList.Add(GetNutrientMasterMockObject(15, NutrientName.L_Threonine_Thr, 2));
            recordList.Add(GetNutrientMasterMockObject(16, NutrientName.L_Leucine_Leu, 2));
            recordList.Add(GetNutrientMasterMockObject(17, NutrientName.L_Tryptophan_Trp, 2));
            recordList.Add(GetNutrientMasterMockObject(18, NutrientName.L_Phenylalanine_Phe, 2));
            recordList.Add(GetNutrientMasterMockObject(19, NutrientName.L_Tyrosine_Tyr, 2));
            recordList.Add(GetNutrientMasterMockObject(20, NutrientName.L_Histidine_His, 2));
            recordList.Add(GetNutrientMasterMockObject(21, NutrientName.L_Methionine_Met, 2));
            recordList.Add(GetNutrientMasterMockObject(3, NutrientName.L_Cystine_Cys, 4));
            recordList.Add(GetNutrientMasterMockObject(22, NutrientName.L_Valine_Val, 2));
            recordList.Add(GetNutrientMasterMockObject(23, NutrientName.L_Isoleucine_Ile, 2));
            recordList.Add(GetNutrientMasterMockObject(23, NutrientName.Moisture, 2));
            recordList.Add(GetNutrientMasterMockObject(23, NutrientName.Ash, 2));
            return recordList;
        }

        private NutrientMaster GetNutrientMasterMockObject(int id, string name, decimal defaultValue)
        {
            var obj = new NutrientMaster()
            {
                NutrientID = id,
                Name = name,
                DefaultValue = defaultValue,
                IsActive = true,
                IsDeleted = false,
                UnitOfMeasurement = new UnitOfMeasurementMaster() { MeasurementUnit = "gram" }
            };
            return obj;
        }
        #endregion

        #region Setup Dummy Data & Repository - IngredientMaster
        private Mock<IRepository<IngredientMaster>> SetupIngredientMasterRepository()
        {
            var repo = new Mock<IRepository<IngredientMaster>>();
            var recordList = GetIngredientMasterMockObject();
            repo.Setup(r => r.Query(true)).Returns(recordList.AsQueryable());
            return repo;
        }

        private ICollection<IngredientMaster> GetIngredientMasterMockObject()
        {
            ICollection<IngredientMaster> recordList = new Collection<IngredientMaster>();
            recordList.Add(new IngredientMaster()
            {
                IngredientID = 1,
                RMDescription = "Cookie Pieces",
                IngredientList = "maltodextrin,\\water",
                IngredientBreakDown = "5.2\\test",
                IngredientNutrientMapping = new Collection<IngredientNutrientMapping>()
                {
                    new IngredientNutrientMapping() { NutrientID = 1, NutrientValue = 1, Nutrient = new NutrientMaster() { Name = NutrientName.Protein } },
                    new IngredientNutrientMapping() { NutrientID = 3, NutrientValue = 2, Nutrient = new NutrientMaster() { Name = NutrientName.L_Lysine_Lys } },
                    new IngredientNutrientMapping() { NutrientID = 7, NutrientValue = 7, Nutrient = new NutrientMaster() { Name = NutrientName.TotalCarbohydrate } },
                    new IngredientNutrientMapping() { NutrientID = 4, NutrientValue = 8, Nutrient = new NutrientMaster() { Name = NutrientName.TotalFiber } },
                    new IngredientNutrientMapping() { NutrientID = 2, NutrientValue = 8, Nutrient = new NutrientMaster() { Name = NutrientName.TotalFat } },
                    new IngredientNutrientMapping() { NutrientID = 6, NutrientValue = 9, Nutrient = new NutrientMaster() { Name = NutrientName.TotalSugars } },
                    new IngredientNutrientMapping() { NutrientID = 9, NutrientValue = 6, Nutrient = new NutrientMaster() { Name = NutrientName.CarbkcalFactor } },
                    new IngredientNutrientMapping() { NutrientID = 10, NutrientValue = 8, Nutrient = new NutrientMaster() { Name = NutrientName.PROkcalFactor } },
                    new IngredientNutrientMapping() { NutrientID = 8, NutrientValue = 9, Nutrient = new NutrientMaster() { Name = NutrientName.FATkcalFactor } },
                    new IngredientNutrientMapping() { NutrientID = 11, NutrientValue = 9, Nutrient = new NutrientMaster() { Name = NutrientName.NPNkcalFactor } },
                }
            });
            recordList.Add(new IngredientMaster()
            {
                IngredientID = 2,
                RMDescription = "Caramel",
                IngredientNutrientMapping = new Collection<IngredientNutrientMapping>()
                {
                    new IngredientNutrientMapping() { NutrientID = 3, NutrientValue = 3, Nutrient = new NutrientMaster() { Name = "Protein Digestibility" } },
                }
            });
            return recordList;
        }
        #endregion

        #region Setup Dumme Data & Repository - RegulatoryCategoryMaster
        private Mock<IRepository<FormulaRegulatoryCategoryMaster>> SetupRegulatoryCategoryMasterRepository()
        {
            var repo = new Mock<IRepository<FormulaRegulatoryCategoryMaster>>();
            var recordList = GetFormulaRegulatoryCategoryMaster();
            repo.Setup(r => r.Query(true)).Returns(recordList.AsQueryable());
            return repo;
        }
        private ICollection<FormulaRegulatoryCategoryMaster> GetFormulaRegulatoryCategoryMaster()
        {
            ICollection<FormulaRegulatoryCategoryMaster> recordList = new Collection<FormulaRegulatoryCategoryMaster>();
            recordList.Add(new FormulaRegulatoryCategoryMaster()
            {
                FormulaRegulatoryCateoryID = 1,
                FormulaRegulatoryCategoryDescription = "Bar",
                CreatedBy = 8,
                CreatedOn = DateTime.Now,
                IsActive = true,
                IsDeleted = false,
                UpdatedBy = 8,
                UpdatedOn = DateTime.Now
            });
            recordList.Add(new FormulaRegulatoryCategoryMaster()
            {
                FormulaRegulatoryCateoryID = 2,
                FormulaRegulatoryCategoryDescription = "Drink Mix",
                CreatedBy = 8,
                CreatedOn = DateTime.Now,
                IsActive = true,
                IsDeleted = false,
                UpdatedBy = 8,
                UpdatedOn = DateTime.Now
            });
            return recordList;
        }

        #endregion

        #region Setup Dumme Data & Repository - DatasheetFormatMaster
        private Mock<IRepository<DatasheetFormatMaster>> SetupDatasheetFormatMasterRepository()
        {
            var repo = new Mock<IRepository<DatasheetFormatMaster>>();
            var recordList = GetDatasheetFormatMaster();
            repo.Setup(r => r.Query(true)).Returns(recordList.AsQueryable());
            return repo;
        }
        private ICollection<DatasheetFormatMaster> GetDatasheetFormatMaster()
        {
            ICollection<DatasheetFormatMaster> recordList = new Collection<DatasheetFormatMaster>();
            recordList.Add(new DatasheetFormatMaster()
            {
                DatasheetCode = "Bar",
                DatasheetDescription = "Nutrition Facts - New US",
                DatasheetFormatID = 1,
                FormulaDatasheetMapping = new Collection<FormulaDatasheetMapping>() { new FormulaDatasheetMapping() { FormulaDatasheetMapID = 1, FormulaID = 1 } },
                CreatedBy = 8,
                CreatedOn = DateTime.Now,
                IsActive = true,
                IsDeleted = false,
                UpdatedBy = 8,
                UpdatedOn = DateTime.Now
            });
            recordList.Add(new DatasheetFormatMaster()
            {
                DatasheetCode = "Bar",
                DatasheetDescription = "Supplement Facts - New US",
                DatasheetFormatID = 1,
                FormulaDatasheetMapping = new Collection<FormulaDatasheetMapping>() { new FormulaDatasheetMapping() { FormulaDatasheetMapID = 1, FormulaID = 1 } },
                CreatedBy = 8,
                CreatedOn = DateTime.Now,
                IsActive = true,
                IsDeleted = false,
                UpdatedBy = 8,
                UpdatedOn = DateTime.Now
            });
            return recordList;
        }

        #endregion

        #region Setup Dummy Data & Repository - ReconstitutionMaster
        private Mock<IRepository<ReconstitutionMaster>> SetupReconstitutionMasterRepository()
        {
            var repo = new Mock<IRepository<ReconstitutionMaster>>();
            var recordList = GetReconstitutionMasterMockObjectAsync();
            repo.Setup(r => r.GetManyAsync(o => o.IsActive && !o.IsDeleted, true)).Returns(recordList);
            return repo;
        }

        private async Task<ICollection<ReconstitutionMaster>> GetReconstitutionMasterMockObjectAsync()
        {
            var recordList = GetReconstitutionMasterMockObject();
            return await Task.FromResult(recordList);
        }

        private ICollection<ReconstitutionMaster> GetReconstitutionMasterMockObject()
        {
            ICollection<ReconstitutionMaster> recordList = new Collection<ReconstitutionMaster>()
            {
                new ReconstitutionMaster(){ ReconstitutionID = 1, ReconstitutionDescription = "USA nonfat milk" },
                new ReconstitutionMaster(){ ReconstitutionID = 2, ReconstitutionDescription = "CAN skim milk" }
            };
            return recordList;
        }
        #endregion

        #region Setup Dummy Data & Repository - PowderLiquidMaster
        private Mock<IRepository<PowderLiquidMaster>> SetupPowderLiquidMasterRepository()
        {
            var repo = new Mock<IRepository<PowderLiquidMaster>>();
            var recordList = GetPowderLiquidMasterMockObjectAsync();
            repo.Setup(r => r.GetManyAsync(o => o.IsActive && !o.IsDeleted, true)).Returns(recordList);
            return repo;
        }

        private async Task<ICollection<PowderLiquidMaster>> GetPowderLiquidMasterMockObjectAsync()
        {
            var recordList = GetPowderLiquidMasterMockObject();
            return await Task.FromResult(recordList);
        }

        private ICollection<PowderLiquidMaster> GetPowderLiquidMasterMockObject()
        {
            ICollection<PowderLiquidMaster> recordList = new Collection<PowderLiquidMaster>()
            {
                new PowderLiquidMaster(){ PowderLiquidID = 1, LiquidDescription = "1 Tbs" },
                new PowderLiquidMaster(){ PowderLiquidID = 2, LiquidDescription = "2 Tbs" }
            };
            return recordList;
        }
        #endregion

        #region Setup Dummy Data & Repository - PowderBlenderSiteMapping
        private Mock<IRepository<PowderBlenderSiteMapping>> SetupPowderBlenderSiteMappingRepository()
        {
            ICollection<PowderBlenderSiteMapping> recordList = GetPowderBlenderSiteMappingMockObject();
            IQueryable<PowderBlenderSiteMapping> queryableData = recordList.AsQueryable();

            var repo = new Mock<IRepository<PowderBlenderSiteMapping>>();
            repo.Setup(r => r.Query(true)).Returns(queryableData);
            return repo;
        }

        private ICollection<PowderBlenderSiteMapping> GetPowderBlenderSiteMappingMockObject()
        {
            ICollection<PowderBlenderSiteMapping> recordList = new Collection<PowderBlenderSiteMapping>()
            {
                new PowderBlenderSiteMapping(){ PowderBlenderID = 1, SiteID = 1, PowderBlender = new PowderBlenderMaster() { BlenderDescription = "Blander A" } },
                new PowderBlenderSiteMapping(){ PowderBlenderID = 2, SiteID = 1, PowderBlender = new PowderBlenderMaster() { BlenderDescription = "Blander G" } }
            };
            return recordList;
        }
        #endregion

        #region Setup Dummy Data & Repository - PowderUnitServingSiteMapping
        private Mock<IRepository<PowderUnitServingSiteMapping>> SetupPowderUnitServingSiteMappingRepository()
        {
            ICollection<PowderUnitServingSiteMapping> recordList = GetPowderUnitServingSiteMappingMockObject();
            IQueryable<PowderUnitServingSiteMapping> queryableData = recordList.AsQueryable();

            var repo = new Mock<IRepository<PowderUnitServingSiteMapping>>();
            repo.Setup(r => r.Query(true)).Returns(queryableData);
            return repo;
        }

        private ICollection<PowderUnitServingSiteMapping> GetPowderUnitServingSiteMappingMockObject()
        {
            ICollection<PowderUnitServingSiteMapping> recordList = new Collection<PowderUnitServingSiteMapping>()
            {
                new PowderUnitServingSiteMapping(){ UnitServingMaster = new UnitServingMaster(){ UnitServingID = 1, UnitServingType = "Unit", UnitDescription = "Blender A" }, SiteProductMapID = 1, SiteProductMap = new SiteProductTypeMapping() { SiteID = 1 } },
                new PowderUnitServingSiteMapping(){ UnitServingMaster = new UnitServingMaster(){ UnitServingID = 2, UnitServingType = "Unit", UnitDescription = "Blender G" }, SiteProductMapID = 1, SiteProductMap = new SiteProductTypeMapping() { SiteID = 1 } }
            };
            return recordList;
        }
        #endregion

        #region Setup Dummy Data & Repository - UnitServingMaster
        private Mock<IRepository<UnitServingMaster>> SetupUnitServingMasterRepository()
        {
            var repo = new Mock<IRepository<UnitServingMaster>>();
            var recordList = GetUnitServingMasterMockObjectAsync();
            repo.Setup(r => r.GetManyAsync(o => o.IsActive && !o.IsDeleted && o.UnitServingType.ToUpper() == "UNIT/SERVING", true)).Returns(recordList);
            return repo;
        }

        private async Task<ICollection<UnitServingMaster>> GetUnitServingMasterMockObjectAsync()
        {
            var recordList = GetUnitServingMasterMockObject();
            return await Task.FromResult(recordList);
        }

        private ICollection<UnitServingMaster> GetUnitServingMasterMockObject()
        {
            ICollection<UnitServingMaster> recordList = new Collection<UnitServingMaster>()
            {
                new UnitServingMaster(){ UnitServingID = 1, UnitDescription = "Cup" },
                new UnitServingMaster(){ UnitServingID = 2, UnitDescription = "Packet" }
            };
            return recordList;
        }
        #endregion

        #region Setup Dummy Data & Repository - Yield Loss
        private DataTable GetYieldLossMockDataTable()
        {
            DataTable tbl = new DataTable();
            tbl.Columns.Add("FormulaYieldLossFactorID", typeof(int));
            tbl.Columns.Add("YieldFactor", typeof(string));
            tbl.Columns.Add("DefaultValue", typeof(double));
            tbl.Columns.Add("SiteID", typeof(int));
            tbl.Columns.Add("MixerType", typeof(string));
            tbl.Rows.Add(1, "S10CoreDoughYield", 20.0000, 1, "planttrials");
            tbl.Rows.Add(2, "S65SyrupYield", 20.0000, 1, "planttrials");
            tbl.Rows.Add(2, "S68_1LiquidBlendYield", 0, 1, "planttrials");
            return tbl;
        }
        #endregion

        #region Setup Dummy Data & Repository - FormulaSearchHistory
        private Mock<IRepository<FormulaSearchHistory>> SetupFormulaSearchHistoryRepository()
        {
            var repo = new Mock<IRepository<FormulaSearchHistory>>();
            List<FormulaSearchHistory> recordList = GetFormulaSearchHistoryMockData();
            int userID = 1;
            string searchData = ", , , , , , , ";
            repo.Setup(r => r.GetManyAsync(o => o.UserID == userID, true)).ReturnsAsync(recordList);
            repo.Setup(r => r.GetMany(o => o.UserID == userID, true)).Returns(recordList);
            repo.Setup(r => r.GetMany(o => o.UserID == userID && o.SearchData == searchData)).Returns(recordList);
            repo.Setup(r => r.DeleteAsync(It.IsAny<System.Linq.Expressions.Expression<Func<FormulaSearchHistory, bool>>>()));
            repo.Setup(r => r.AddAsync(It.IsAny<FormulaSearchHistory>()));
            repo.Setup(r => r.UpdateAsync(It.IsAny<FormulaSearchHistory>()));
            return repo;
        }

        private List<FormulaSearchHistory> GetFormulaSearchHistoryMockData()
        {
            List<FormulaSearchHistory> recordList = new List<FormulaSearchHistory>();
            recordList.Add(new FormulaSearchHistory() { FormulaSearchID = 1, UserID = 1, SearchData = "test1", SearchDate = DateTime.Now });
            recordList.Add(new FormulaSearchHistory() { FormulaSearchID = 2, UserID = 1, SearchData = "test2", SearchDate = DateTime.Now.AddMinutes(-1) });
            recordList.Add(new FormulaSearchHistory() { FormulaSearchID = 3, UserID = 1, SearchData = "test3", SearchDate = DateTime.Now.AddMinutes(-2) });
            recordList.Add(new FormulaSearchHistory() { FormulaSearchID = 4, UserID = 1, SearchData = "test4", SearchDate = DateTime.Now.AddMinutes(-3) });
            recordList.Add(new FormulaSearchHistory() { FormulaSearchID = 5, UserID = 1, SearchData = "test5", SearchDate = DateTime.Now.AddMinutes(-4) });
            recordList.Add(new FormulaSearchHistory() { FormulaSearchID = 6, UserID = 1, SearchData = "test6", SearchDate = DateTime.Now.AddMinutes(-4) });
            return recordList;
        }
        #endregion

        #region Setup Dummy Data & Repository - Formula Home Page Header Information
        private DataTable GetFormulaHomePageHeaderInfo()
        {
            DataTable tbl = new DataTable();
            tbl.Columns.Add("FormulaCode", typeof(string));
            tbl.Columns.Add("ProductName", typeof(string));
            tbl.Columns.Add("ProjectCode", typeof(string));
            tbl.Columns.Add("FormulaReference", typeof(string));
            tbl.Columns.Add("FormulaStatusCode", typeof(string));
            tbl.Columns.Add("DieNumber", typeof(string));
            tbl.Columns.Add("ActualWaterPercentage", typeof(string));
            tbl.Rows.Add("F0001", "S10CoreDoughYield", 100005, "S10600115.022a-033", "M");
            return tbl;
        }
        #endregion

        #region Setup Dummy Data & Repository - Formula Revision
        private Mock<IRepository<FormulaRevision>> SetupFormulaRevisionRepository()
        {
            var repo = new Mock<IRepository<FormulaRevision>>();
            repo.Setup(r => r.AddAsync(It.IsAny<FormulaRevision>()));
            return repo;
        }
        #endregion

        #region Setup Dummy Data & Repository - Formula Change Code
        private Mock<IRepository<FormulaChangeCode>> SetupFormulaChangeCodeRepository()
        {
            var repo = new Mock<IRepository<FormulaChangeCode>>();

            var mockData = GetFormulaChangeCodeMockObject();
            IQueryable<FormulaChangeCode> queryableData = mockData.Result.AsQueryable();

            repo.Setup(r => r.Query(true)).Returns(queryableData);
            repo.Setup(r => r.AddAsync(It.IsAny<FormulaChangeCode>()));
            return repo;
        }
        
        private async Task<ICollection<FormulaChangeCode>> GetFormulaChangeCodeMockObject()
        {
            ICollection<FormulaChangeCode> recordList = new Collection<FormulaChangeCode>();
            recordList.Add(FormulaChangeCodeMockObject());
            return await Task.FromResult(recordList);
        }
        
        private FormulaChangeCode FormulaChangeCodeMockObject()
        {
            FormulaChangeCode formulaChangeCode = new FormulaChangeCode()
            {
                FormulaChangeCodeID = 4,
                FormulaTypeID = 1,
                IncrementNumber = 999,
                CreatedBy = 2,
                CreatedOn = DateTime.Now,
                FormulaTypeMaster = new FormulaTypeMaster() { FormulaTypeID = 1, FormulaTypeCode = "S10", FormulaDescription = "Complete S10_", IsActive = true, IsDeleted = false },
                Site = new SiteMaster() { S30CodePrefix="6",SiteCode="ONT",SiteID=1,IsActive=true,CreatedBy=2,CreatedOn=DateTime.Now }
            };            
            return formulaChangeCode;
        }
        #endregion
        
        #region Setup Dummy Data & Repository - Formula Type Master
        private Mock<IRepository<FormulaTypeMaster>> SetupFormulaTypeMasterRepository()
        {
            var repo = new Mock<IRepository<FormulaTypeMaster>>();
            repo.Setup(r => r.GetAsync(o=>o.FormulaTypeCode=="S10")).Returns(GetFormulaTypeMasterMockObject());
            repo.Setup(r => r.AddAsync(It.IsAny<FormulaTypeMaster>()));
            return repo;
        }

        private async Task<FormulaTypeMaster> GetFormulaTypeMasterMockObject()
        {
            ICollection<FormulaChangeCode> recordList = new Collection<FormulaChangeCode>();
            recordList.Add(FormulaChangeCodeMockObject());
            FormulaTypeMaster formulaTypeMaster = new FormulaTypeMaster()
            {
                FormulaTypeID = 1,
                FormulaTypeCode = "S10",
                IsActive = true,
                IsDeleted = false,
                CreatedBy = 2,
                CreatedOn = DateTime.Now,
                FormulaChangeCode = recordList,
                FormulaDescription = "Complete S10_"
            };
            return await Task.FromResult(formulaTypeMaster);
        }
        #endregion

        #region Setup Dummy Data & Repository - Formula Status Master
        private Mock<IRepository<FormulaStatusMaster>> SetupFormulaStatusMasterRepository()
        {
            var repo = new Mock<IRepository<FormulaStatusMaster>>();
            var recordList = GetFormulaStatusMasterMockObject();
            repo.Setup(r => r.Query(true)).Returns(recordList.AsQueryable());
            return repo;
        }

        private ICollection<FormulaStatusMaster> GetFormulaStatusMasterMockObject()
        {
            var recordList = new Collection<FormulaStatusMaster>();
            recordList.Add(new FormulaStatusMaster()
            {
                FormulaStatusID = 1,
                FormulaStatusCode = "S10",
                FormulaStatusCodeDescription = "Complete S10_",
                IsActive = true,
                IsDeleted = false
            });
            return recordList;
        }
        #endregion

        #region Setup Dummy Data & Repository - User Master
        private Mock<IRepository<UserMaster>> SetupUserMasterRepository()
        {
            int? userID = 1;
            var repo = new Mock<IRepository<UserMaster>>();
            var recordList = GetUserMasterMockObject();
            repo.Setup(r => r.Get(x => x.UserID == userID.Value)).Returns(recordList.First());
            return repo;
        }

        private ICollection<UserMaster> GetUserMasterMockObject()
        {
            var recordList = new Collection<UserMaster>();
            recordList.Add(new UserMaster()
            {
                UserID = 1, DisplayName = "Administrator"
            });
            return recordList;
        }
        #endregion
    }
}