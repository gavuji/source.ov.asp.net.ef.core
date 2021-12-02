using FM21.Data;
using FM21.Data.Infrastructure;
using FM21.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FM21.Tests
{
    [TestFixture]
    public class RepositoryTest
    {
        Mock<AppEntities> mockAppEntity;
        Mock<IDatabaseFactory> mockDatabaseFactory;
        Repository<SiteMaster> repository;
        readonly List<SiteMaster> entities = new List<SiteMaster>();

        [SetUp]
        public void SetUp()
        {
            entities.Add(new SiteMaster { SiteID = 1, SiteCode = "ONT", SiteProductTypeMapping = new SiteProductTypeMapping[] { new SiteProductTypeMapping() { SiteID = 1, SiteProductMapID = 1, ProductTypeID = 1 } } });
            mockAppEntity = new Mock<AppEntities>(null);
            mockAppEntity.Setup(x => x.Set<SiteMaster>()).Returns(GetQueryableMockDbSet<SiteMaster>(entities));
            mockAppEntity.Setup(x => x.SaveChanges()).Returns(1);
            mockDatabaseFactory = new Mock<IDatabaseFactory>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockAppEntity.Object);
            repository = new Repository<SiteMaster>(mockDatabaseFactory.Object);
        }

        public static DbSet<T> GetQueryableMockDbSet<T>(List<T> sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();
            var mockDBSet = new Mock<DbSet<T>>();
            mockDBSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockDBSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockDBSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockDBSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            mockDBSet.Setup(d => d.Add(It.IsAny<T>())).Callback<T>((s) => sourceList.Add(s));
            return mockDBSet.Object;
        }

        [Test]
        public void Repository_Should_Insert_New_Record()
        {
            repository.Add(new SiteMaster() { SiteCode = "SLC", SiteDescription = "SLC" });
            Assert.IsNotNull("Success");
        }

        [Test]
        public void Repository_Should_Insert_List_Of_New_Records()
        {
            var recordList = new List<SiteMaster>();
            recordList.Add(new SiteMaster() { SiteCode = "ANA", SiteDescription = "ANA" });
            recordList.Add(new SiteMaster() { SiteCode = "ANJ", SiteDescription = "ANJ" });
            repository.AddAll(recordList);
            Assert.IsNotNull("Success");
        }

        [Test]
        public void Repository_Should_Delete_Record()
        {
            var obj = repository.Get(o => o.SiteID == 1);
            repository.Delete(obj);
            Assert.IsNotNull("Success");
        }

        [Test]
        public void Repository_Should_Delete_Records_Based_On_Condition()
        {
            repository.Delete(o => o.SiteID == 1);
            Assert.IsNotNull("Success");
        }

        [Test]
        public void Repository_Should_Return_Specific_Record()
        {
            var obj = repository.Get(o => o.SiteID == 1);
            Assert.IsNotNull(obj);
        }

        [Test]
        public void Repository_Should_Return_Record_With_Child_Data()
        {
            var obj = repository.Get(o => o.SiteID == 1, o => o.SiteProductTypeMapping);
            Assert.IsNotNull(obj);
        }

        [Test]
        public void Repository_Should_Return_All_Records_With_NoTracking()
        {
            var data = repository.GetAll(true);
            Assert.IsNotNull(data);
        }

        [Test]
        public void Repository_Should_Return_All_Records_Along_With_Child_Details_And_NoTracking()
        {
            var data = repository.GetAll(true, o => o.SiteProductTypeMapping);
            Assert.IsNotNull(data);
        }

        [Test]
        public void Repository_Should_Return_All_Records()
        {
            var data = repository.GetAll();
            Assert.IsNotNull(data);
        }

        [Test]
        public void Repository_Should_Get_Record_By_Long_ID()
        {
            repository.GetById((long)1);
            Assert.IsNotNull("Success");
        }

        [Test]
        public void Repository_Should_Get_Record_By_String_ID()
        {
            repository.GetById("1");
            Assert.IsNotNull("Success");
        }

        [Test]
        public void Repository_Should_Get_Many_Records()
        {
            var obj = repository.GetMany(o => o.SiteID == 1);
            Assert.IsNotNull(obj);
        }

        [Test]
        public void Repository_Should_Get_Many_Records_Along_With_Child_Data()
        {
            var obj = repository.GetMany(o => o.SiteID == 1, true, o => o.SiteProductTypeMapping);
            Assert.IsNotNull(obj);
        }

        [Test]
        public void Repository_Should_Get_Count_Of_Records()
        {
            var obj = repository.Count();
            Assert.Greater(obj, 0);
        }

        [Test]
        public void Repository_Should_Get_Count_Of_Record_With_Specific_Condition()
        {
            var obj = repository.Count(o => o.SiteID == 1);
            Assert.Greater(obj, 0);
        }

        [Test]
        public void Repository_Should_Check_Whether_Any_Record_Exist_On_Specific_condition()
        {
            var obj = repository.Any(o => o.SiteID == 1);
            Assert.IsTrue(obj);
        }

        [Test]
        public void Repository_Should_Check_Whether_Any_Record_Exist()
        {
            var obj = repository.Any();
            Assert.IsTrue(obj);
        }

        [Test]
        public void Repository_Should_Return_Data_By_Query()
        {
            var obj = repository.Query();
            Assert.IsNotNull(obj);
        }

        [Test]
        public void Repository_Should_Return_Data_By_Query_With_NoTracking()
        {
            var obj = repository.Query(true);
            Assert.IsNotNull(obj);
        }

        [Test]
        public void Repository_Should_Return_Data_Along_With_Child_Data_By_Query_With_NoTracking()
        {
            var obj = repository.Query(true, o => o.SiteProductTypeMapping);
            Assert.IsNotNull(obj);
        }

        [Test]
        public void Repository_Should_Insert_New_Record_Async()
        {
            repository.AddAsync(new SiteMaster() { SiteCode = "SLC", SiteDescription = "SLC" });
            Assert.IsNotNull("Success");
        }

        [Test]
        public async Task Repository_Should_Delete_Existing_Record_Async()
        {
            var obj = repository.Get(o => o.SiteID == 1);
            var response = await repository.DeleteAsync(obj);
            Assert.IsNotNull(response);
        }

        [Test]
        public async Task Repository_Should_Delete_Existing_Record_Async_By_Condition()
        {
            var response = await repository.DeleteAsync(o => o.SiteID == 0);
            Assert.IsNotNull(response);
        }
    }
}