using FM21.API.Controllers;
using FM21.Core;
using FM21.Core.Model;
using FM21.Service;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FM21.Tests
{
    [TestFixture]
    public class RolePermissionMatrixTest
    {
        Mock<IPermissionMasterService> mydata;

        [Test]
        public async Task Test_GetRolePermissionMatrix_ReturnJsonObject()
        {
            var request = new GeneralResponse<ICollection<RolePermissionMatrix>>();
            request.Data = new List<RolePermissionMatrix>();
            RolePermissionMatrix rolePermissionMatrix = new RolePermissionMatrix();
            rolePermissionMatrix.name = "Manage formula instruction list";
            rolePermissionMatrix.permissionId = 2;
            rolePermissionMatrix.roleAccessList = new List<RolePermissionAccess>();
            RolePermissionAccess rolePermissionAccess = new RolePermissionAccess()
            {
                rolePermissionID = 2,
                roleID = 1031,
                permissionID = 2,
                roleName = "Site Admin",
                isAccess = false
            };
            rolePermissionMatrix.roleAccessList.Add(rolePermissionAccess);
            request.Data.Add(rolePermissionMatrix);

            mydata = new Mock<IPermissionMasterService>();
            mydata.Setup(t => t.GetRolePermissionMatrix()).ReturnsAsync(request);


            var permissionController = new PermissionController(mydata.Object);
            var getRolePermissionMatrixResult = await permissionController.GetRolePermissionMatrix() as JsonResult;
            Assert.IsNotNull(getRolePermissionMatrixResult);
            Assert.That(getRolePermissionMatrixResult, Is.TypeOf<JsonResult>());
            Assert.AreEqual(request, getRolePermissionMatrixResult.Value);
        }

        [Test]
        public async Task Test_UpdateRolePermissionMatrix_OnValidInput_ReturnSuccess()
        {
            var response = new GeneralResponse<bool>();
            response.Message = "Role Permission has been updated successfully.";

            List<RolePermissionAccess> roleAccessList = new List<RolePermissionAccess>();
            string msgUpdateSuccess_en = "Role Permission has been updated successfully.";

            RolePermissionAccess rolePermissionAccess = new RolePermissionAccess()
            {
                rolePermissionID = 2,
                roleID = 1031,
                permissionID = 2,
                roleName = "Site Admin",
                isAccess = false
            };
            roleAccessList.Add(rolePermissionAccess);


            mydata = new Mock<IPermissionMasterService>();
            mydata.Setup(t => t.UpdateRoleMatrix(roleAccessList)).ReturnsAsync(response);


            var permissionController = new PermissionController(mydata.Object);
            var getRolePermissionMatrixResult = await permissionController.PostRolePermissionMatrix(roleAccessList);
            var controllerResponse = getRolePermissionMatrixResult.Result as JsonResult;
            var convertGeneralResp = (GeneralResponse<bool>)controllerResponse.Value;
            Assert.IsNotNull(controllerResponse);
            Assert.That(controllerResponse, Is.TypeOf<JsonResult>());
            Assert.AreEqual(convertGeneralResp.Message, msgUpdateSuccess_en);
        }


        [Test]
        public async Task Test_SaveRolePermissionMatrix_OnValidInput_ReturnSuccess()
        {
            var response = new GeneralResponse<bool>();
            response.Message = "Role Permission has been updated successfully.";

            List<RolePermissionAccess> roleAccessList = new List<RolePermissionAccess>();
            string msgUpdateSuccess_en = "Role Permission has been updated successfully.";

            RolePermissionAccess rolePermissionAccess = new RolePermissionAccess()
            {
                rolePermissionID = 0,
                roleID = 1031,
                permissionID = 2,
                roleName = "Site Admin",
                isAccess = false
            };
            roleAccessList.Add(rolePermissionAccess);


            mydata = new Mock<IPermissionMasterService>();
            mydata.Setup(t => t.UpdateRoleMatrix(roleAccessList)).ReturnsAsync(response);


            var permissionController = new PermissionController(mydata.Object);
            var getRolePermissionMatrixResult = await permissionController.PostRolePermissionMatrix(roleAccessList);
            var controllerResponse = getRolePermissionMatrixResult.Result as JsonResult;
            var convertGeneralResp = (GeneralResponse<bool>)controllerResponse.Value;
            Assert.IsNotNull(controllerResponse);
            Assert.That(controllerResponse, Is.TypeOf<JsonResult>());
            Assert.AreEqual(convertGeneralResp.Message, msgUpdateSuccess_en);
        }

        [Test]

        public async Task Test_SaveUpdateRolePermissionMatrix_OnInvalidData()
        {
            var response = new GeneralResponse<bool>();
            response.Result = ResultType.Error;
            List<RolePermissionAccess> roleAccessList = new List<RolePermissionAccess>();
            RolePermissionAccess rolePermissionAccess = new RolePermissionAccess()
            {
                rolePermissionID = 0,
                roleID = 0,
                permissionID = 0
            };
            roleAccessList.Add(rolePermissionAccess);


            mydata = new Mock<IPermissionMasterService>();
            mydata.Setup(t => t.UpdateRoleMatrix(roleAccessList)).ReturnsAsync(response);

            var permissionController = new PermissionController(mydata.Object);
            var getRolePermissionMatrixResult = await permissionController.PostRolePermissionMatrix(roleAccessList);
            var controllerResponse = getRolePermissionMatrixResult.Result as JsonResult;

            var convertGeneralResp = (GeneralResponse<bool>)controllerResponse.Value;
            Assert.IsNotNull(controllerResponse);
            Assert.That(controllerResponse, Is.TypeOf<JsonResult>());
            Assert.AreEqual(convertGeneralResp.Result, ResultType.Error);
        }

    }

}