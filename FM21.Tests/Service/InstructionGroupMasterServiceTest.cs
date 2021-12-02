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
    public class InstructionGroupMasterServiceTest : TestBase
    {
        private IInstructionGroupMasterService instructionGroupMasterService;
        private Mock<IRepository<InstructionGroupMaster>> instructionGroupRepository;
        private Mock<IRepository<InstructionMaster>> instructionRepository;

        [SetUp]
        [SetUICulture("en-us")]
        public void SetUp()
        {
            instructionGroupRepository = SetupInstructionGroupRepository();
            instructionRepository = SetupInstructionRepository();
            instructionGroupMasterService = new InstructionGroupMasterService(serviceProvider.Object, exceptionHandler.Object, mapper, unitOfWork.Object, null, instructionGroupRepository.Object, instructionRepository.Object);
        }

        [Test]
        public async Task Service_Should_Return_All_ActiveAnd_NotDeleted_Data_By_SortColumn()
        {
            var response = await instructionGroupMasterService.GetAll();

            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
        }

        [Test]
        public async Task Service_Should_Not_Return_Data_When_Exception()
        {
            instructionGroupRepository.Setup(r => r.GetManyAsync(o => o.IsActive, true)).Throws(new Exception("something went wrong"));
            var response = await instructionGroupMasterService.GetAll();
            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_Return_Data_GetGroupBySiteAndCategory()
        {
            var response = await instructionGroupMasterService.GetGroupBySiteProductAndCategory(1, 1);

            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
            Assert.IsNotNull(response.Data);
        }

        [Test]
        public async Task Service_Should_Not_Return_Data_GroupBySiteAndCategory_When_Exception()
        {
            instructionRepository.Setup(r => r.Query(true)).Throws(new Exception("something went wrong"));
            var response = await instructionGroupMasterService.GetGroupBySiteProductAndCategory(1, 1);

            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_CreateNew_Data()
        {
            var model = GetModelObjectMock();

            var response = await instructionGroupMasterService.Create(model);

            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
            Assert.AreEqual(response.Message, localizer["msgInsertSuccess", new string[] { "Instruction Group" }].Value);
        }

        [Test]
        public async Task Service_Should_NotCreate_When_Data_InValid()
        {
            var model = GetModelObjectMock();
            model.InstructionGroupName = string.Empty;

            var response = await instructionGroupMasterService.Create(model);

            Assert.That(response.Result, Is.EqualTo(ResultType.Warning));
            Assert.Greater(response.ExtraData.Count, 0);
        }
        
        [TestCase("Create")]
        [TestCase("Update")]
        public async Task Service_Should_Not_Create_Or_Update_When_Instruction_Group_Exist_In_The_System(string actionType)
        {
            var model = GetModelObjectMock();
            GeneralResponse<bool> response=null;

            instructionGroupRepository.Setup(r => r.Any(It.IsAny<System.Linq.Expressions.Expression<Func<InstructionGroupMaster, bool>>>())).Returns(true);
            if (actionType == "Create")
            {
                 response = await instructionGroupMasterService.Create(model);
            }
            if (actionType == "Update")
            {
                     model.InstructionGroupID = 1;
                    response = await instructionGroupMasterService.Update(model);
            }

            Assert.That(response.Result, Is.EqualTo(ResultType.Warning));
            Assert.AreEqual(response.Message, localizer["msgDuplicateRecord", new string[] { "Instruction Group" }].Value);
        }

        [Test]
        public async Task Service_Should_Not_Create_New_InstructionGroup_When_Exception()
        {
            var model = GetModelObjectMock();
            instructionGroupRepository.Setup(r => r.Any(It.IsAny<Expression<Func<InstructionGroupMaster, bool>>>())).Throws(new Exception("something went wrong"));
            var response = await instructionGroupMasterService.Create(model);
            Assert.AreEqual(response.Result, ResultType.Error);
        }

        [Test]
        public async Task Service_Should_Update_Data()
        {
            var model = GetModelObjectMock();
            model.InstructionGroupID = 1;
            model.InstructionGroupName = "Instruction edited";

            var response = await instructionGroupMasterService.Update(model);

            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
        }

        [Test]
        public async Task Service_Should_Not_Update_When_PrimaryKey_InValid()
        {
            var model = GetModelObjectMock();

            var response = await instructionGroupMasterService.Update(model);

            Assert.That(response.Result, Is.EqualTo(ResultType.Warning));
            Assert.Greater(response.ExtraData.Count, 0);
            Assert.AreEqual("InstructionGroupID", response.ExtraData[0].Key);
            Assert.AreEqual(localizer["msgMustBeGreaterThenZero"].Value, response.ExtraData[0].Value);
        }

        [Test]
        public async Task Service_Should_Not_Update_Data_When_Values_Are_InValid()
        {
            var model = GetModelObjectMock();
            model.InstructionGroupID = 1;
            model.InstructionGroupName = string.Empty;

            var response = await instructionGroupMasterService.Update(model);

            Assert.That(response.Result, Is.EqualTo(ResultType.Warning));
            Assert.Greater(response.ExtraData.Count, 0);
            Assert.AreEqual("InstructionGroupName", response.ExtraData[0].Key);
            Assert.AreEqual(localizer["msgRequiredField"].Value, response.ExtraData[0].Value);
        }

        [Test]
        public async Task Service_Should_Not_Update_InstructionGroup_When_Exception()
        {
            var model = GetModelObjectMock();
            model.InstructionGroupID = 1;
            instructionGroupRepository.Setup(r => r.Any(It.IsAny<Expression<Func<InstructionGroupMaster, bool>>>())).Throws(new Exception("something went wrong"));
            var response = await instructionGroupMasterService.Update(model);
            Assert.AreEqual(response.Result, ResultType.Error);
        }

        [Test]
        public async Task Service_Should_Delete_Data_When_PrimaryKey_Valid()
        {
            var model = GetModelObjectMock();
            model.InstructionGroupID = 2;
            var objObject = GetObjectMock(1);
            var objResponse = new GeneralResponse<InstructionGroupMaster>() { Data = objObject };

            instructionGroupRepository.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(objResponse.Data);
            var response = await instructionGroupMasterService.Delete(model.InstructionGroupID);

            Assert.AreEqual(response.Result, ResultType.Success);
        }
        
        [Test]
        public async Task Service_Should_Not_Delete_Data_When_Instruction_Group_Has_A_Reference_On_Other_Place()
        {
            var model = GetModelObjectMock();
            model.InstructionGroupID = 1;
            var response = await instructionGroupMasterService.Delete(model.InstructionGroupID);

            Assert.AreEqual(response.Result, ResultType.Warning);
            Assert.AreEqual(localizer["msgDeleteFailAsUsedByOthers"].Value, response.Message);
        }

        [Test]
        public async Task Service_Should_Not_Delete_Data_When_PrimaryKey_InValid()
        {
            var model = GetModelObjectMock();
            model.InstructionGroupID = 5;

            var response = await instructionGroupMasterService.Delete(model.InstructionGroupID);

            Assert.AreEqual(response.Result, ResultType.Warning);
            Assert.AreEqual(response.Message, localizer["msgRecordNotExist", new string[] { "Instruction Group" }].Value);
        }

        [Test]
        public async Task Service_Should_Delete_When_Exception()
        {
            instructionGroupRepository.Setup(r => r.GetByIdAsync(1)).Throws(new Exception("something went wrong"));
            var response = await instructionGroupMasterService.Delete(1);
            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        #region Setup Dummy Data & Repository
        private Mock<IRepository<InstructionGroupMaster>> SetupInstructionGroupRepository()
        {
            var repo = new Mock<IRepository<InstructionGroupMaster>>();
            var objObject = GetObjectMock(1);
            var objResponse = new GeneralResponse<InstructionGroupMaster>() { Data = objObject };

            List<InstructionGroupMaster> recordList = new List<InstructionGroupMaster>();
            recordList.Add(objObject);

            var mockData = GetPagedDataMock();
            IQueryable<InstructionGroupMaster> queryableData = mockData.Data.AsQueryable();

            repo.Setup(r => r.GetManyAsync(o => o.IsActive)).ReturnsAsync(recordList);
            repo.Setup(r => r.GetManyAsync(o => o.IsActive, true)).ReturnsAsync(recordList);
            repo.Setup(r => r.UpdateAsync(It.IsAny<InstructionGroupMaster>()))
             .Callback(new Action<InstructionGroupMaster>(x =>
             {
             }));
            repo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(objResponse.Data);
            repo.Setup(r => r.Any(x => x.InstructionGroupID != objResponse.Data.InstructionGroupID && x.InstructionGroupName == string.Empty)).Returns(true);
            repo.Setup(r => r.Query(true)).Returns(queryableData);
            repo.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(() => { objResponse.Data = null; return objResponse.Data; }); // Get data from Invalid ID

            return repo;
        }

        private Mock<IRepository<InstructionMaster>> SetupInstructionRepository()
        {
            var repo = new Mock<IRepository<InstructionMaster>>();
            
            var mockData = new PagedEntityResponse<InstructionMaster>();
            mockData.Data = new List<InstructionMaster>();
            mockData.Data.Add(GetObjectMockInstructionMaster(1));
            mockData.Data.Add(GetObjectMockInstructionMaster(2));

            IQueryable<InstructionMaster> queryableData = mockData.Data.AsQueryable();

            repo.Setup(r => r.Query(true)).Returns(queryableData);
            int Id = 1;
            repo.Setup(r => r.Any(o => o.InstructionGroupID == Id)).Returns(true);
            return repo;
        }

        private InstructionMaster GetObjectMockInstructionMaster(int? id = null)
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
                IsDeleted = false,
                InstructionGroup = new InstructionGroupMaster() { InstructionGroupID = 1, InstructionGroupName = "Group 1" }
            };
            if (id != null)
            {
                model.InstructionMasterID = Convert.ToInt32(id);
            }
            return model;
        }

        private PagedEntityResponse<InstructionGroupMaster> GetPagedDataMock()
        {
            List<InstructionGroupMaster> lst = new List<InstructionGroupMaster>();
            var response = new PagedEntityResponse<InstructionGroupMaster>();
            lst.Add(GetObjectMock(1));
            lst.Add(GetObjectMock(2));
            response.Data = lst;
            return response;
        }

        private InstructionGroupMaster GetObjectMock(int? id = null)
        {
            InstructionGroupMaster model = new InstructionGroupMaster()
            {
                InstructionGroupName = "Test Group"
            };
            if (id != null)
            {
                model.InstructionGroupID = Convert.ToInt32(id);
            }
            return model;
        }

        private InstructionGroupMasterModel GetModelObjectMock(int? id = null)
        {
            InstructionGroupMasterModel model = new InstructionGroupMasterModel()
            {
                InstructionGroupName = "Test Group"
            };
            if(id != null)
            {
                model.InstructionGroupID = Convert.ToInt32(id);
            }
            return model;
        }
        #endregion
    }
}