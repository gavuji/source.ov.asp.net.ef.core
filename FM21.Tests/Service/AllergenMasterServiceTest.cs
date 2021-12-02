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
using System.Threading.Tasks;

namespace FM21.Tests
{
    [TestFixture]
    public class AllergenMasterServiceTest : TestBase
    {
        private Mock<IRepository<AllergenMaster>> allergenMasterRepository;
        private IAllergenMasterService allergenMasterService;
        List<AllergenMaster> allergenMaster;

        [SetUp]
        public void SetUp()
        {
            allergenMaster = GetMockDataForCreateAllergenData();
            allergenMasterRepository = SetupRegulatoryRepository();
            allergenMasterService = new AllergenMasterService(serviceProvider.Object, exceptionHandler.Object, mapper, unitOfWork.Object, null, allergenMasterRepository.Object);
        }

        [Test]
        public async Task Service_Should_Return_All_ActiveAnd_NotDeleted_AllergenData_By_SortColumn()
        {
            var response = await allergenMasterService.GetAll();
            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
        }
        
        [Test]
        public async Task Service_Should_Not_Return_Data_When_Exception()
        {
            allergenMasterRepository.Setup(r => r.GetManyAsync(o => o.IsActive == true && o.IsDeleted == false, true)).Throws(new Exception("something went wrong"));
            var response = await allergenMasterService.GetAll();
            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_Return_All_Active_AllergenData()
        {
            string filterText = GetSignleObject().AllergenName;
            var response = await allergenMasterService.GetPageWiseData(filter: filterText, pageIndex: 1, pageSize: 10, sortColumn: null, sortDirection: null);

            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
        }

        [Test]
        public async Task Service_Should_Not_Return_All_Active_AllergenData_When_Exception_On_Query()
        {
            allergenMasterRepository.Setup(r => r.Query(true)).Throws(new Exception("something went wrong"));
            var response = await allergenMasterService.GetPageWiseData(filter: null, pageIndex: 1, pageSize: 10, sortColumn: null, sortDirection: null);

            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [TestCase("allergencode")]
        [TestCase("allergendescription_en")]
        [TestCase("allergendescription_fr")]
        [TestCase("allergendescription_es")]
        [TestCase("allergenname")]
        [TestCase("createdby")]
        public async Task Service_Should_Return_All_Active_AllergenData_By_SortColumn(string sortColumn)
        {
            var response = await allergenMasterService.GetPageWiseData(filter: null, pageIndex: 1, pageSize: 10, sortColumn: sortColumn, sortDirection: null);
            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
        }

        [Test]
        public async Task Service_Should_Return_Allergen_By_Valid_AllergenId()
        {
            var returnObject = GetSingleAllergenObjectMockData();
            var response = await allergenMasterService.Get(1);
            Assert.That(response.Result, Is.EqualTo(returnObject.Result));
            Assert.AreEqual(response.Data.AllergenID, returnObject.Data.AllergenID);
        }

        [Test]
        public async Task Service_Should_Return_RecordNotExist_On_Invalid_AllergenId()
        {
            var response = await allergenMasterService.Get(5);
            
            Assert.IsNull(response.Data);
            Assert.AreEqual(response.Message, localizer["msgRecordNotExist", new string[] { "Allergen" }].Value);
        }

        [Test]
        public async Task Service_Should_Not_Return_Allergen_If_Exception()
        {
            allergenMasterRepository.Setup(r => r.GetByIdAsync(1)).Throws(new Exception("something went wrong"));
            var response = await allergenMasterService.Get(1);

            Assert.AreEqual(response.Result, ResultType.Error);
        }

        [Test] 
        public async Task Service_Should_CreateNew_AllergenData()
        {
            AllergenMasterModel allergenMasterModel = GetSignleObject();
            int _maxRegIDBeforeAdd = allergenMaster.Max(a => a.AllergenID);
            var response = await allergenMasterService.Create(allergenMasterModel);

            Assert.That(_maxRegIDBeforeAdd + 1, Is.EqualTo(allergenMaster.Last().AllergenID));
            Assert.AreEqual(response.Result, ResultType.Success);
        }

        [Test]
        public async Task Service_Should_Not_Create_New_AllergenData_When_DataExist()
        {
            AllergenMasterModel allergenMasterModel = GetSignleObject();
            allergenMasterRepository.Setup(r => r.Any(x => x.AllergenName.ToLower().Trim() == allergenMasterModel.AllergenName.ToLower().Trim())).Returns(true);
            var response = await allergenMasterService.Create(allergenMasterModel);
            Assert.AreEqual(response.Result, ResultType.Warning);
        }

        [Test]
        public async Task Service_Should_Not_Create_New_AllergenData_When_Exception()
        {
            AllergenMasterModel allergenMasterModel = GetSignleObject();
            allergenMasterRepository.Setup(r => r.Any(x => x.AllergenName.ToLower().Trim() == allergenMasterModel.AllergenName.ToLower().Trim())).Throws(new Exception("something went wrong"));
            var response = await allergenMasterService.Create(allergenMasterModel);
            Assert.AreEqual(response.Result, ResultType.Error);
        }
        
        [Test]
        public async Task Service_Should_NotCreate_When_AllergenData_InValid()
        {
            AllergenMasterModel allergenMasterModel = GetSignleObject();
            allergenMasterModel.AllergenCode = null;

            var response = await allergenMasterService.Create(allergenMasterModel);

            Assert.That(response.Result, Is.EqualTo(ResultType.Warning));
            Assert.Greater(response.ExtraData.Count, 0);
        }

        [Test]
        public async Task Service_Should_Update_AllergenData()
        {
            var _firstAllergen = GetSignleObject();
            string oldCode = _firstAllergen.AllergenCode;
            _firstAllergen.AllergenCode = "E";
            _firstAllergen.AllergenID = 1;

            var response = await allergenMasterService.Update(_firstAllergen);

            Assert.That(_firstAllergen.AllergenCode, Is.Not.EqualTo(oldCode));
            Assert.That(_firstAllergen.AllergenID, Is.EqualTo(1));
            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
        }

        [Test]
        public async Task Service_Should_Not_Update_When_AllergenId_InValid()
        {
            var _firstAllergen = GetSignleObject();

            var response = await allergenMasterService.Update(_firstAllergen);
            Assert.AreEqual(response.Result, ResultType.Warning);
            Assert.Greater(response.ExtraData.Count, 0);
        }

        [Test]
        public async Task Service_Should_Not_Update_When_AllergenId_Not_Exist()
        {
            var _firstAllergen = GetSignleObject();
            _firstAllergen.AllergenCode = "E";
            _firstAllergen.AllergenID = 1;
            allergenMasterRepository.Setup(r => r.Any(It.IsAny<System.Linq.Expressions.Expression<Func<AllergenMaster, bool>>>())).Returns(true);
            var response = await allergenMasterService.Update(_firstAllergen);
            Assert.AreEqual(response.Result, ResultType.Warning);
            Assert.That(response.Message, Is.EqualTo(localizer["msgDuplicateRecord", new string[] { "Allergen" }].Value));
        }

        [Test]
        public async Task Service_Should_Update_AllergenData_When_Expecption_Generate()
        {
            var obj = GetSignleObject();
            obj.AllergenID = 1;
            allergenMasterRepository.Setup(r => r.GetByIdAsync(1)).Throws(new Exception("something went wrong"));
            var response = await allergenMasterService.Update(obj);
            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_Delete_AllergenData_When_AllergenId_Valid()
        {
            var response = await allergenMasterService.Delete(1);
            Assert.AreEqual(response.Result, ResultType.Success);
        }

        [Test]
        public async Task Service_Should_Delete_AllergenData_When_AllergenId_InValid()
        {
            var _firstAllergen = GetSignleObject();
            _firstAllergen.AllergenID = 10;

            var response = await allergenMasterService.Delete(_firstAllergen.AllergenID);
            Assert.AreEqual(response.Result, ResultType.Warning);
        }

        [Test]
        public async Task Service_Should_Not_Delete_AllergenData_When_Used_At_Other_Place()
        {
            var response = await allergenMasterService.Delete(2);

            Assert.AreEqual(response.Result, ResultType.Warning);
            Assert.AreEqual(response.Message, localizer["msgDeleteFailAsUsedByOthers"].Value);
        }

        [Test]
        public async Task Service_Should_Delete_AllergenData_When_Exception()
        {
            allergenMasterRepository.Setup(r => r.GetByIdAsync(1)).Throws(new Exception("something went wrong"));
            var response = await allergenMasterService.Delete(1);
            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        #region Setup Dummy Data & Repository
        private Mock<IRepository<AllergenMaster>> SetupRegulatoryRepository()
        {
            var repo = new Mock<IRepository<AllergenMaster>>();
            var responseSingle = GetSingleAllergenObjectMockData();
            var response = GetGeneralAllergenMockData();

            var getAllergenMockData = GetAllergenMasterPagedMockData();
            IQueryable<AllergenMaster> queryableRegulatory = getAllergenMockData.Data.AsQueryable();

            repo.Setup(r => r.GetManyAsync(o => o.IsActive == true && o.IsDeleted == false)).Returns(response);
            repo.Setup(r => r.GetManyAsync(o => o.IsActive == true && o.IsDeleted == false, true)).Returns(response);
            repo.Setup(r => r.AddAsync(It.IsAny<AllergenMaster>()))
             .Callback(new Action<AllergenMaster>(newAllergen =>
             {
                 dynamic maxRegularID = allergenMaster.Last().AllergenID;
                 dynamic nextRegularID = maxRegularID + 1;
                 newAllergen.AllergenID = nextRegularID;
                 newAllergen.CreatedOn = DateTime.Now;
                 allergenMaster.Add(newAllergen);
             }));
            repo.Setup(r => r.UpdateAsync(It.IsAny<AllergenMaster>()))
             .Callback(new Action<AllergenMaster>(x =>
             {
                 var oldRegulatory = allergenMaster.Find(a => a.AllergenID == x.AllergenID);
                 oldRegulatory.UpdatedOn = DateTime.Now;
                 oldRegulatory = x;

             }));
            repo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(responseSingle.Data);
            repo.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(responseSingle.Data);
            repo.Setup(r => r.Any(x => x.AllergenID != responseSingle.Data.AllergenID)).Returns(true);
            repo.Setup(r => r.Query(true)).Returns(queryableRegulatory);
            repo.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(() => { responseSingle.Data = null; return responseSingle.Data; }); // Get data from Invalid ID
            return repo;
        }

        private AllergenMasterModel GetSignleObject()
        {
            AllergenMasterModel allergenMasterModel = new AllergenMasterModel();

            allergenMasterModel.AllergenName = "Shrimp";
            allergenMasterModel.AllergenCode = "L";
            allergenMasterModel.AllergenDescription_En = "Crustacean";
            allergenMasterModel.AllergenDescription_Fr = "Crustacés";
            allergenMasterModel.IsActive = true;
            allergenMasterModel.IsDeleted = false;
            return allergenMasterModel;
        }

        private List<AllergenMaster> GetMockDataForCreateAllergenData()
        {
            List<AllergenMaster> allergenMasters = new List<AllergenMaster>
            {
                new AllergenMaster()
                {
                    AllergenCode = "Code",
                    AllergenID = 1,
                    AllergenName = "se",
                    AllergenDescription_En = "English Desc",
                    AllergenDescription_Fr = "French Desc",
                    AllergenDescription_Es = "Spanish Desc",
                    IsActive = true,
                    IsDeleted = false
                }
            };
            return allergenMasters;
        }

        private async Task<ICollection<AllergenMaster>> GetGeneralAllergenMockData()
        {
            List<AllergenMaster> allergenMasters = new List<AllergenMaster>();
            allergenMasters.Add(new AllergenMaster()
            {
                AllergenCode = "Code",
                AllergenID = 1,
                AllergenName = "se",
                AllergenDescription_En = "English Desc",
                AllergenDescription_Fr = "French Desc",
                AllergenDescription_Es = "Spanish Desc",
                IsActive = true,
                IsDeleted = false,
                CreatedBy = 25412,
                CreatedOn = DateTime.Now,
                UpdatedBy = null,
                UpdatedOn = DateTime.Now,
            });
            allergenMasters.Add(new AllergenMaster()
            {
                AllergenCode = "L",
                AllergenID = 2,
                AllergenName = "Crab",
                AllergenDescription_En = "English Desc",
                AllergenDescription_Fr = "French Desc",
                AllergenDescription_Es = "Spanish Desc",
                IsActive = true,
                IsDeleted = false,
                CreatedBy = 25412,
                CreatedOn = DateTime.Now,
                UpdatedBy = null,
                UpdatedOn = DateTime.Now,

            });
            return await Task.FromResult(allergenMasters);
        }

        private PagedEntityResponse<AllergenMaster> GetAllergenMasterPagedMockData()
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
                IsDeleted = false,
                IngredientAllergenMapping = new IngredientAllergenMapping[] { new IngredientAllergenMapping() { IngredientID = 1, AllergenID = 1 } }
            });
            response.Data = allergenMasterModels;
            return response;
        }

        private GeneralResponse<AllergenMaster> GetSingleAllergenObjectMockData()
        {
            var response = new GeneralResponse<AllergenMaster>();
            response.Data = new AllergenMaster()
            {
                AllergenCode = "Code",
                AllergenID = 1,
                AllergenName = "se",
                AllergenDescription_En = "English Desc",
                AllergenDescription_Fr = "French Desc",
                AllergenDescription_Es = "Spanish Desc",
                IsActive = true,
                IsDeleted = false
            };
            return response;
        }
        #endregion
    }
}
