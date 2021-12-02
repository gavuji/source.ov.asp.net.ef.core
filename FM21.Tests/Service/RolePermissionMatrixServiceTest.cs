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
using System.Threading.Tasks;

namespace FM21.Tests
{
    [TestFixture]
    public class RolePermissionMatrixServiceTest : TestBase
    {
        private Mock<IRepository<PermissionMaster>> permissionMasterRepository;
        private Mock<IRepository<RoleMaster>> roleMasterRepository;
        private Mock<IRepository<RolePermissionMapping>> rolePermissionMapRepository;
        private IPermissionMasterService permissionMasterService;

        [SetUp]
        public void SetUp()
        {
            permissionMasterRepository = SetupPermissionRepository();
            roleMasterRepository = SetupRoleRepository();
            rolePermissionMapRepository = SetupRolePermissionMapRepository();
            permissionMasterService = new PermissionMasterService(serviceProvider.Object, exceptionHandler.Object, mapper, unitOfWork.Object, null, permissionMasterRepository.Object, roleMasterRepository.Object, rolePermissionMapRepository.Object);
        }

        [Test]
        public async Task Service_Should_Return_All_RolePermisssionMatrix_When_PermissionType_Access_True()
        {
            var response = await permissionMasterService.GetRolePermissionMatrix();
            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
            Assert.Greater(response.Data.FirstOrDefault().roleAccessList.Count, 0);
            Assert.AreEqual(true, response.Data.FirstOrDefault().roleAccessList.FirstOrDefault().isAccess);
        }

        [Test]
        public async Task Service_Should_Return_All_RolePermisssionMatrix_When_PermissionType_Access_False()
        {
            var response = await permissionMasterService.GetRolePermissionMatrix();

            Assert.Greater(response.Data.FirstOrDefault().roleAccessList.Count, 0);
            Assert.AreEqual(false, response.Data.FirstOrDefault().roleAccessList.FirstOrDefault(x => !x.isAccess).isAccess);
        }

        [Test]
        public async Task Service_Should_Not_Return_RolePermisssionMatrix_When_Exception()
        {
            permissionMasterRepository.Setup(r => r.Query(true)).Throws(new Exception("something went wrong"));
            var response = await permissionMasterService.GetRolePermissionMatrix();

            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        [Test]
        public async Task Service_Should_Update_RolePermisssionMatrix_When_RolePermissionId_Greator_Than_Zero()
        {
            var setRolePermissionMatrixMockData = GetRolePermissionAccessMockList().Where(x => x.rolePermissionID == 1).ToList();
           
            var response = await permissionMasterService.UpdateRoleMatrix(setRolePermissionMatrixMockData);
            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
        }

        [Test]
        public async Task Service_Should_Save_New_RolePermisssionMatrix_When_RolePermissionId_Is_Zero()
        {
            var setRolePermissionMatrixMockData = GetRolePermissionAccessMockList().Where(x => x.rolePermissionID == 0).ToList();

            var response = await permissionMasterService.UpdateRoleMatrix(setRolePermissionMatrixMockData);
            Assert.That(response.Result, Is.EqualTo(ResultType.Success));
        }

        [Test]
        public async Task Service_Should_Not_Save_New_RolePermisssionMatrix_When_Exception()
        {
            var setRolePermissionMatrixMockData = GetRolePermissionAccessMockList().Where(x => x.rolePermissionID > 0).ToList();
            rolePermissionMapRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>())).Throws(new Exception("something went wrong"));
            var response = await permissionMasterService.UpdateRoleMatrix(setRolePermissionMatrixMockData);
            Assert.That(response.Result, Is.EqualTo(ResultType.Error));
        }

        #region Setup Dummy Data & Repository
        private Mock<IRepository<PermissionMaster>> SetupPermissionRepository()
        {
            var repo = new Mock<IRepository<PermissionMaster>>();
            var getRegMockData = GetGeneralRegulatoryMockPagedData();
         
            IQueryable<PermissionMaster> queryableRegulatory = getRegMockData.Data.AsQueryable();

            repo.Setup(r => r.Query(true)).Returns(queryableRegulatory);
            
            return repo;
        }

        private Mock<IRepository<RolePermissionMapping>> SetupRolePermissionMapRepository()
        {
            var roleAccess = GetRolePermissionAccessMockList();
            var repo = new Mock<IRepository<RolePermissionMapping>>();
            var lstGetRolePermission = SetRolePermissionMapping();
            var getSingleRolePermissionObject = lstGetRolePermission.FirstOrDefault(); 
            repo.Setup(r => r.GetByIdAsync(getSingleRolePermissionObject.RolePermissionID)).ReturnsAsync(getSingleRolePermissionObject);

            repo.Setup(r => r.UpdateAsync(It.IsAny<RolePermissionMapping>()))
            .Callback(new Action<RolePermissionMapping>(x => {
                var oldRegulatory = lstGetRolePermission.Find(a => a.RolePermissionID == x.RolePermissionID);
                oldRegulatory.UpdatedOn = DateTime.Now;
                oldRegulatory.PermissionType = Convert.ToByte(true);
                oldRegulatory = x;

            }));
            repo.Setup(r => r.AddAsync(It.IsAny<RolePermissionMapping>()))
             .Callback(new Action<RolePermissionMapping>(newRegulatory => {
                 dynamic maxRegularID = roleAccess.Last(x => x.rolePermissionID!= 0).rolePermissionID;
                 dynamic nextRegularID = maxRegularID + 1;
                 var getSignleObj = roleAccess.FirstOrDefault(x => x.rolePermissionID == 0);
                 getSignleObj.rolePermissionID = newRegulatory.RolePermissionID=nextRegularID;
                 roleAccess.Add(getSignleObj);
             }));
            return repo;
        }

        private Mock<IRepository<RoleMaster>> SetupRoleRepository()
        {
            var repo = new Mock<IRepository<RoleMaster>>();
            var getRegMockData = GetRoleMasterMockPagedData();
            IQueryable<RoleMaster> queryableCountries = getRegMockData.Data.AsQueryable();
            repo.Setup(r => r.Query(true)).Returns(queryableCountries);
            return repo;
        }

        private PagedEntityResponse<PermissionMaster> GetGeneralRegulatoryMockPagedData()
        {
            List<PermissionMaster> lst = new List<PermissionMaster>();
            var response = new PagedEntityResponse<PermissionMaster>();
            lst.Add(new PermissionMaster()
            {
                PermissionID = 1,
                PermissionFor = "Things that no one else can do",
                IsDeleted = false,
                RolePermissionMapping = new HashSet<RolePermissionMapping>() {
                   new RolePermissionMapping(){
                    RolePermissionID=1,
                    RoleID = 1031,
                    PermissionID=1,
                    Role=new RoleMaster(){
                    RoleID =1031,
                    RoleName="Site Admin",
                    IsActive=true,
                    IsDeleted=false
                    },
                    PermissionType=1
                   },
                    new RolePermissionMapping(){
                    RolePermissionID=2,
                    RoleID = 1031,
                    PermissionID=0,
                    Role=new RoleMaster(){
                    RoleID =1031,
                    RoleName="Site Admin",
                    IsActive=true,
                    IsDeleted=false
                    },
                    PermissionType=0
                   }
                }
            });
            response.Data = lst;
            return response;
        }

        private PagedEntityResponse<RoleMaster> GetRoleMasterMockPagedData()
        {
            List<RoleMaster> lst = new List<RoleMaster>();
            var response = new PagedEntityResponse<RoleMaster>();
            lst.Add(new RoleMaster()
            {
                RoleID = 1031,
                RoleName = "Site Admin",
                IsActive = true,
                IsDeleted = false,
                RolePermissionMapping = new HashSet<RolePermissionMapping>() {
                   new RolePermissionMapping(){
                    RolePermissionID=1,
                    RoleID = 1031,
                    PermissionID=1,
                    Role=new RoleMaster(){
                    RoleID =1031,
                    RoleName="Site Admin",
                    IsActive=true,
                    IsDeleted=false
                    },
                    PermissionType=1
                   }
                }
            });
            response.Data = lst;
            return response;
        }

        private List<RolePermissionAccess> GetRolePermissionAccessMockList()
        {
            List<RolePermissionAccess> roleAccessList = new List<RolePermissionAccess>();
            roleAccessList.Add(
                new RolePermissionAccess()
                {
                    isAccess = true,
                    permissionID = 1,
                    roleID = 1031,
                    roleName = "Admin",
                    rolePermissionID = 1
                });
            roleAccessList.Add(
               new RolePermissionAccess()
               {
                   isAccess = true,
                   permissionID = 3,
                   roleID = 1031,
                   roleName = "Admin",
                   rolePermissionID = 0
               });

            return roleAccessList;
        }
        private List<RolePermissionMapping> SetRolePermissionMapping()
        {
            List<RolePermissionMapping> lstRolePermi = new List<RolePermissionMapping>();
            lstRolePermi.Add(new RolePermissionMapping()
            {
                RolePermissionID = 1,
                RoleID = 1031,
                PermissionID = 1,
                PermissionType = 1
            });
            lstRolePermi.Add(new RolePermissionMapping()
            {
                RolePermissionID = 2,
                RoleID = 1031,
                PermissionID = 2,
                PermissionType = 1
            });

            lstRolePermi.Add(new RolePermissionMapping()
            {
                RolePermissionID = 0,
                RoleID = 1031,
                PermissionID = 3,
                PermissionType = 1
            });
            return lstRolePermi;
        }
        #endregion
    }
}