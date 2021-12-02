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
    public class InstructionMasterServiceTest : TestBase
    {
        private Mock<IRepository<InstructionMaster>> instructionMasterRepository;
        private IInstructionMasterService instructionMasterService;

        [SetUp]
        [SetUICulture("en-us")]
        public void SetUp()
        {
            instructionMasterRepository = SetupInstructionRepository();
            instructionMasterService = new InstructionMasterService(serviceProvider.Object, exceptionHandler.Object, mapper, unitOfWork.Object, null, instructionMasterRepository.Object);
        }

        [Test]
        public async Task Service_Should_Return_All_ActiveAnd_Not_Deleted_InstructionMaster_Data()
        {
            var response = await instructionMasterService.GetAll();
            Assert.That(response.Result.ToString(), Is.EqualTo("Success"));
        }

        [Test]
        public async Task Service_InstructionMaster_ThrowsException_On_Delete()
        {
            instructionMasterRepository.Setup(r => r.Query(true)).Throws(new Exception("something went wrong"));
            var response = await instructionMasterService.GetAll();
            Assert.AreEqual(response.Result, ResultType.Error);
        }

        [Test]
        public async Task Service_Should_Return_All_Active_Data()
        {
            var returnObject = GetPagedDataMock();

            var response = await instructionMasterService.GetPageWiseData(new SearchFilter() { PageIndex = 1, PageSize = 10 });

            Assert.That(response.Result, Is.EqualTo(returnObject.Result));
            Assert.IsNotNull(response.Data);
        }

        [TestCase("DescriptionEn")]
        [TestCase("siteproduct")]
        [TestCase("instructioncategory")]
        [TestCase("instructiongroup")]
        [TestCase("descriptionen")]
        [TestCase("descriptionfr")]
        [TestCase("descriptiones")]
        [TestCase("createdby")]
        public async Task Service_Should_Return_All_Active_Data_By_SortColumn(string sortColumn)
        {
            var returnObject = GetPagedDataMock();
            var response = await instructionMasterService.GetPageWiseData(new SearchFilter() { PageIndex = 1, PageSize = 10, SortColumn = sortColumn, Search= "mix items1" });
            Assert.That(response.Result, Is.EqualTo(returnObject.Result));
        }
        
        [Test]
        public async Task Service_InstructionMaster_GetPageWiseData_ThrowsException()
        {
           
            instructionMasterRepository.Setup(r => r.Query(true)).Throws(new Exception("something went wrong"));
            var response = await instructionMasterService.GetPageWiseData(new SearchFilter() { PageIndex = 1, PageSize = 10, SortColumn = "createdby", Search = "mix items1" });
            Assert.AreEqual(response.Result, ResultType.Error);
        }

        [Test]
        public async Task Service_Should_Return_Data_Get_Search_List_With_Filter()
        {
            var returnObject = GetPagedDataMock();

            var response = await instructionMasterService.GetSearchListWithFilter(new SearchFilter() { PageIndex = 1, PageSize = 10, Search = "mix items1" }, 1, 1);

            Assert.That(response.Result, Is.EqualTo(returnObject.Result));
            Assert.IsNotNull(response.Data);
        }

        [Test]
        public async Task Service_InstructionMaster_Get_Search_List_With_Filter_ThrowsException()
        {

            instructionMasterRepository.Setup(r => r.Query(true)).Throws(new Exception("something went wrong"));
            var response = await instructionMasterService.GetSearchListWithFilter(new SearchFilter() { PageIndex = 1, PageSize = 10, Search = "mix items1" }, 1, 1);
            Assert.AreEqual(response.Result, ResultType.Error);
        }

        [Test]
        public async Task Service_Should_Return_Data_GetInstructionBySiteCategoryAndGroup()
        {
            var returnObject = GetPagedDataMock();

            var response = await instructionMasterService.GetBySiteProductCategoryAndGroup(1, 1, 1);

            Assert.That(response.Result, Is.EqualTo(returnObject.Result));
            Assert.IsNotNull(response.Data);
        }

        [Test]
        public async Task Service_InstructionMaster_Get_By_Site_Product_Category_And_Group_ThrowsException()
        {

            instructionMasterRepository.Setup(r => r.Query(true)).Throws(new Exception("something went wrong"));
            var response = await instructionMasterService.GetBySiteProductCategoryAndGroup(1, 1, 1);
            Assert.AreEqual(response.Result, ResultType.Error);
        }

        [Test]
        public async Task Service_Should_Get_Record_By_Valid_PrimaryKey()
        {
            var returnObject = new GeneralResponse<InstructionMaster>();
            returnObject.Data = GetObjectMock(1);

            var response = await instructionMasterService.Get(1);

            Assert.That(response.Result, Is.EqualTo(returnObject.Result));
            Assert.AreEqual(response.Data.InstructionMasterID, returnObject.Data.InstructionMasterID);
        }

        [Test]
        public async Task Service_Should_Return_RecordNotExist_On_Invalid_PrimaryKey()
        {
            var returnObject = new GeneralResponse<InstructionMaster>();
            returnObject.Data = null;
            returnObject.Message = localizer["msgRecordNotExist", new string[] { "Instruction" }].Value;

            var response = await instructionMasterService.Get(5);

            Assert.IsNull(response.Data);
            Assert.AreEqual(response.Message, returnObject.Message);
        }

        [Test]
        public async Task Service_InstructionMaster_Get_By_Id_ThrowsException()
        {
            instructionMasterRepository.Setup(r => r.GetByIdAsync(1)).Throws(new Exception("something went wrong"));
            var response = await instructionMasterService.Get(1);
            Assert.AreEqual(response.Result, ResultType.Error);
        }

        [Test]
        public async Task Service_Should_CreateNew_Data()
        {
            var model = GetModelObjectMock();

            var response = await instructionMasterService.Create(model);

            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
            Assert.AreEqual(response.Message, localizer["msgInsertSuccess", new string[] { "Instruction" }].Value);
        }

        [Test]
        public async Task Service_Should_CreateNew_Instruction_In_Existing_Site_Category_Group()
        {
            var model = GetModelObjectMock();
            List<InstructionMaster> recordList = new List<InstructionMaster>();
            recordList.Add(GetObjectMock(2));
            instructionMasterRepository.Setup(r => r.GetManyAsync(It.IsAny<System.Linq.Expressions.Expression<Func<InstructionMaster, bool>>>(), true)).ReturnsAsync(recordList);

            var response = await instructionMasterService.Create(model);

            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
            Assert.AreEqual(response.Message, localizer["msgInsertSuccess", new string[] { "Instruction" }].Value);
        }

        [Test]
        public async Task Service_Should_Not_Create_New_Data_When_Instruction_Already_Exist()
        {
            var model = GetModelObjectMock();
            instructionMasterRepository.Setup(r => r.Any(It.IsAny<System.Linq.Expressions.Expression<Func<InstructionMaster, bool>>>())).Returns(true);
            var response = await instructionMasterService.Create(model);

            Assert.That(response.Result, Is.EqualTo(ResultType.Warning));
            Assert.AreEqual(response.Message, localizer["msgDuplicateRecord", new string[] { "Instruction" }].Value);
        }

        [Test]
        public async Task Service_Should_NotCreate_When_Data_InValid()
        {
            var model = GetModelObjectMock();
            model.DescriptionEn = string.Empty;

            var response = await instructionMasterService.Create(model);

            Assert.That(response.Result, Is.EqualTo(ResultType.Warning));
            Assert.Greater(response.ExtraData.Count, 0);
        }

        [Test]
        public async Task Service_Should_Not_Create_New_When_Exception()
        {
            var model = GetModelObjectMock();
            instructionMasterRepository.Setup(r => r.Any(It.IsAny<Expression<Func<InstructionMaster, bool>>>())).Throws(new Exception("something went wrong"));
            var response = await instructionMasterService.Create(model);
            Assert.AreEqual(response.Result, ResultType.Error);
        }

        [Test]
        public async Task Service_Should_Update_Data()
        {
            var model = GetModelObjectMock();
            model.InstructionMasterID = 1;
            model.InstructionCategoryID = 9;
            model.DescriptionEn = "Instruction edited";

            var response = await instructionMasterService.Update(model);

            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
        }

        [Test]
        public async Task Service_Should_Update_Instruction_In_Existing_Site_Category_Group()
        {
            var model = GetModelObjectMock();
            model.InstructionMasterID = 1;
            model.InstructionCategoryID = 9;
            model.DescriptionEn = "Instruction edited";
            List<InstructionMaster> recordList = new List<InstructionMaster>();
            recordList.Add(GetObjectMock(2));
            instructionMasterRepository.Setup(r => r.GetManyAsync(It.IsAny<System.Linq.Expressions.Expression<Func<InstructionMaster, bool>>>(), true)).ReturnsAsync(recordList);

            var response = await instructionMasterService.Update(model);

            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
        }

        [Test]
        public async Task Service_Should_Not_Update_When_PrimaryKey_InValid()
        {
            var model = GetModelObjectMock();

            var response = await instructionMasterService.Update(model);

            Assert.That(response.Result, Is.EqualTo(ResultType.Warning));
            Assert.Greater(response.ExtraData.Count, 0);
            Assert.AreEqual("InstructionMasterID", response.ExtraData[0].Key);
            Assert.AreEqual(localizer["msgMustBeGreaterThenZero"].Value, response.ExtraData[0].Value);
        }

        [Test]
        public async Task Service_Should_Not_Update_Data_When_Values_Are_InValid()
        {
            var model = GetModelObjectMock();
            model.InstructionMasterID = 1;
            model.DescriptionEn = string.Empty;

            var response = await instructionMasterService.Update(model);

            Assert.That(response.Result, Is.EqualTo(ResultType.Warning));
            Assert.Greater(response.ExtraData.Count, 0);
            Assert.AreEqual("DescriptionEn", response.ExtraData[0].Key);
            Assert.AreEqual(localizer["msgRequiredField"].Value, response.ExtraData[0].Value);
        }

        [Test]
        public async Task Service_Should_Not_Update_Data_When_Instruction_Already_Exist()
        {
            var model = GetModelObjectMock();
            model.InstructionMasterID = 1;
            instructionMasterRepository.Setup(r => r.Any(It.IsAny<System.Linq.Expressions.Expression<Func<InstructionMaster, bool>>>())).Returns(true);
            var response = await instructionMasterService.Update(model);

            Assert.That(response.Result, Is.EqualTo(ResultType.Warning));
            Assert.AreEqual(response.Message, localizer["msgDuplicateRecord", new string[] { "Instruction" }].Value);
        }

        [Test]
        public async Task Service_Should_Not_Update_Instruction_When_Exception()
        {
            var model = GetModelObjectMock();
            model.InstructionMasterID = 1;
            instructionMasterRepository.Setup(r => r.Any(It.IsAny<Expression<Func<InstructionMaster, bool>>>())).Throws(new Exception("something went wrong"));
            var response = await instructionMasterService.Update(model);
            Assert.AreEqual(response.Result, ResultType.Error);
        }

        [Test]
        public async Task Service_Should_Delete_Data_When_PrimaryKey_Valid()
        {
            var model = GetModelObjectMock();
            model.InstructionMasterID = 1;

            var response = await instructionMasterService.Delete(model.InstructionMasterID);

            Assert.AreEqual(response.Result, ResultType.Success);
        }

        [Test]
        public async Task Service_Should_Not_Delete_Data_When_PrimaryKey_InValid()
        {
            var model = GetModelObjectMock();
            model.InstructionMasterID = 5;

            var response = await instructionMasterService.Delete(model.InstructionMasterID);

            Assert.AreEqual(response.Result, ResultType.Warning);
            Assert.AreEqual(response.Message, localizer["msgRecordNotExist", new string[] { "Instruction" }].Value);
        }

        [Test]
        public async Task Service_Should_Delete_When_Exception()
        {
            instructionMasterRepository.Setup(r => r.GetByIdAsync(1)).Throws(new Exception("something went wrong"));
            var response = await instructionMasterService.Delete(1);
            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_Update_InstructionGroupOrder()
        {
            var recordList = new List<InstructionGroupMasterModel>();
            recordList.Add(new InstructionGroupMasterModel() { GroupDisplayOrder = 1, InstructionGroupID = 1 });
            recordList.Add(new InstructionGroupMasterModel() { GroupDisplayOrder = 1, InstructionGroupID = 2 });

            var response = await instructionMasterService.UpdateInstructionGroupOrder(1, 1, recordList);

            Assert.AreEqual(response.Result, ResultType.Success);
        }

        [Test]
        public async Task Service_Should_Not_Update_InstructionGroupOrder_When_Exception()
        {
            var recordList = new List<InstructionGroupMasterModel>();
            recordList.Add(new InstructionGroupMasterModel() { GroupDisplayOrder = 1, InstructionGroupID = 1 });
            instructionMasterRepository.Setup(o => o.Query(true)).Throws(new Exception("something went wrong"));
            var response = await instructionMasterService.UpdateInstructionGroupOrder(1, 1, recordList);

            Assert.AreEqual(response.Result, ResultType.Error);
        }

        [Test]
        public async Task Service_Should_Update_InstructionOrder()
        {
            var recordList = new List<InstructionMasterModel>();
            recordList.Add(GetModelObjectMock(1));
            recordList[0].GroupItemDisplayOrder = 2;
            recordList.Add(GetModelObjectMock(2));
            recordList[1].GroupItemDisplayOrder = 1;

            var response = await instructionMasterService.UpdateInstructionOrder(recordList);

            Assert.AreEqual(response.Result, ResultType.Success);
        }

        [Test]
        public async Task Service_Should_Not_Update_InstructionOrder_When_Exception()
        {
            var recordList = new List<InstructionMasterModel>();
            recordList.Add(GetModelObjectMock(1));
            instructionMasterRepository.Setup(o => o.Query(true)).Throws(new Exception("something went wrong"));
            var response = await instructionMasterService.UpdateInstructionOrder(recordList);

            Assert.AreEqual(response.Result, ResultType.Error);
        }

        #region Setup Dummy Data & Repository
        private Mock<IRepository<InstructionMaster>> SetupInstructionRepository()
        {
            var repo = new Mock<IRepository<InstructionMaster>>();
            var objObject = GetObjectMock(1);
            var objResponse = new GeneralResponse<InstructionMaster>() { Data = objObject };

            List<InstructionMaster> recordList = new List<InstructionMaster>();
            recordList.Add(objObject);
            recordList.Add(GetObjectMock(2));

            var mockData = GetPagedDataMock();
            IQueryable<InstructionMaster> queryableData = mockData.Data.AsQueryable();
            repo.Setup(r => r.GetAllAsync()).ReturnsAsync(recordList);
            //repo.Setup(r => r.GetManyAsync(It.IsAny<System.Linq.Expressions.Expression<Func<InstructionMaster, bool>>>())).ReturnsAsync(recordList);
            //repo.Setup(r => r.GetManyAsync(It.IsAny<System.Linq.Expressions.Expression<Func<InstructionMaster, bool>>>(), true)).ReturnsAsync(recordList);
            repo.Setup(r => r.UpdateAsync(It.IsAny<InstructionMaster>()))
             .Callback(new Action<InstructionMaster>(x =>
             {
                 var oldInstructionMaster = recordList.Find(a => a.InstructionMasterID == x.InstructionMasterID);
                 oldInstructionMaster.UpdatedOn = DateTime.Now;
                 oldInstructionMaster = x;

             }));
            repo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(objResponse.Data);
            repo.Setup(r => r.Any(x => x.InstructionMasterID != objResponse.Data.InstructionMasterID && x.DescriptionEn == string.Empty)).Returns(true);
            repo.Setup(r => r.Query(true)).Returns(queryableData);
            repo.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(() => { objResponse.Data = null; return objResponse.Data; }); // Get data from Invalid ID

            return repo;
        }

        private PagedEntityResponse<InstructionMaster> GetPagedDataMock()
        {
            var response = new PagedEntityResponse<InstructionMaster>();
            response.Data = new List<InstructionMaster>();
            response.Data.Add(GetObjectMock(1));
            response.Data.Add(GetObjectMock(2));
            response.Data[0].GroupItemDisplayOrder = 2;
            return response;
        }

        private InstructionMaster GetObjectMock(int? id = null)
        {
            InstructionMaster model = new InstructionMaster()
            {
                SiteProductMapID = 1,
                InstructionCategoryID = 1,
                InstructionGroupID = 1,
                DescriptionEn = "mix items1",
                DescriptionFr = "mix items_fr1",
                DescriptionEs = "mix items_es1",
                GroupDisplayOrder = 1,
                GroupItemDisplayOrder = 1,
                IsActive = true,
                IsDeleted = false
            };
            if (id != null)
            {
                model.InstructionMasterID = Convert.ToInt32(id);
            }
            return model;
        }

        private InstructionMasterModel GetModelObjectMock(int? id = null)
        {
            InstructionMasterModel model = new InstructionMasterModel()
            {
                SiteProductMapID = 1,
                InstructionCategoryID = 1,
                InstructionGroupID = 1,
                DescriptionEn = "mix items1",
                DescriptionFr = "mix items_fr1",
                DescriptionEs = "mix items_es1",
                GroupDisplayOrder = 1,
                GroupItemDisplayOrder = 1
            };
            if(id != null)
            {
                model.InstructionMasterID = Convert.ToInt32(id);
            }
            return model;
        }
        #endregion
    }
}