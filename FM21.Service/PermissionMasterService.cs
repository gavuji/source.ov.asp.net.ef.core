using AutoMapper;
using FM21.Core;
using FM21.Core.Model;
using FM21.Data.Infrastructure;
using FM21.Entities;
using FM21.Service.Caching;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FM21.Service
{
    public class PermissionMasterService : BaseService, IPermissionMasterService
    {
        private readonly IRepository<PermissionMaster> permissionMasterRepository;
        private readonly IRepository<RoleMaster> roleRepository;
        private readonly IRepository<RolePermissionMapping> rolePermissionMapRepo;

        public PermissionMasterService(IServiceProvider provider, IExceptionHandler exceptionHandler, IMapper mapper, IUnitOfWork unitOfWork, ICacheProvider cacheProvider,
            IRepository<PermissionMaster> permissionMasterRepository, IRepository<RoleMaster> roleRepository, IRepository<RolePermissionMapping> rolePermissionMapRepo)
            : base(provider, exceptionHandler, mapper, unitOfWork, cacheProvider)
        {
            this.permissionMasterRepository = permissionMasterRepository;
            this.roleRepository = roleRepository;
            this.rolePermissionMapRepo = rolePermissionMapRepo;
        }

        public async Task<GeneralResponse<ICollection<RolePermissionMatrix>>> GetRolePermissionMatrix()
        {
            var response = new GeneralResponse<ICollection<RolePermissionMatrix>>();
            try
            {
                await Task.Run(() =>
                {
                    response.Data = permissionMasterRepository.Query(true)
                         .Where(o => o.PermissionID != Convert.ToInt32(PermissionName.ViewPrintformulareports)
                          && o.PermissionID != Convert.ToInt32(PermissionName.ViewPrintingredientreports)
                          && o.IsDeleted == false)
                        .Include(o => o.RolePermissionMapping)
                        .ThenInclude(o => o.Role)
                        .OrderBy(o => o.PermissionFor)
                   .Select(x => new RolePermissionMatrix
                   {
                       permissionId = x.PermissionID,
                       name = x.PermissionFor,
                       roleAccessList = (
                                    from rm in roleRepository.Query(true).
                                    Where(o => o.IsActive && !o.IsDeleted
                                    && o.RoleName != RoleName.Admin.ToString())
                                    join rmmapping in x.RolePermissionMapping
                                    on rm.RoleID equals rmmapping.RoleID into joinRoleMasterRolePermissionMap
                                    from dataResult in joinRoleMasterRolePermissionMap.DefaultIfEmpty()
                                    orderby rm.RoleName ascending
                                    select new RolePermissionAccess
                                    {
                                        roleID = rm.RoleID,
                                        roleName = rm.RoleName,
                                        isAccess = dataResult != null && dataResult.PermissionType != 0,
                                        permissionID = dataResult.PermissionID,
                                        rolePermissionID = dataResult.RolePermissionID,
                                    }).ToList()
                   })
                   .AsQueryable().ToList();
                });

                
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }

        public async Task<GeneralResponse<bool>> UpdateRoleMatrix(List<RolePermissionAccess> roleAccessList)
        {
            var response = new GeneralResponse<bool>();
            try
            {
                foreach (var item in roleAccessList)
                {
                    if (item.rolePermissionID > 0)
                    {

                        var obj = await rolePermissionMapRepo.GetByIdAsync(item.rolePermissionID);
                        if (obj != null)
                        {
                            obj.PermissionType = Convert.ToByte(item.isAccess.GetHashCode());
                            obj.UpdatedBy = RequestUserID;
                            obj.UpdatedOn = DateTime.Now;
                            rolePermissionMapRepo.UpdateAsync(obj);
                        }
                    }
                    else
                    {
                        RolePermissionMapping rpMapping = mapper.Map<RolePermissionMapping>(item);
                        rolePermissionMapRepo.AddAsync(rpMapping);
                    }
                }
                await Save();
                response.Message = localizer["msgUpdateSuccess", new string[] { "Role Permission" }];
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }
    }
}