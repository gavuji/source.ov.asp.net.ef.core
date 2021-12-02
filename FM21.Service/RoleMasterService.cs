using AutoMapper;
using FluentValidation;
using FM21.Core;
using FM21.Core.Model;
using FM21.Core.Validator;
using FM21.Data;
using FM21.Data.Infrastructure;
using FM21.Entities;
using FM21.Service.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FM21.Service
{
    public class RoleMasterService : BaseService, IRoleMasterService
    {
        private readonly IRepository<RoleMaster> roleMasterRepository;
        private readonly IRepository<RolePermissionMapping> rolePermissionRepository;

        public RoleMasterService(IServiceProvider provider, IExceptionHandler exceptionHandler, IMapper mapper, IUnitOfWork unitOfWork, ICacheProvider cacheProvider,
            IRepository<RoleMaster> roleMasterRepository, IRepository<RolePermissionMapping> rolePermissionRepository)
            : base(provider, exceptionHandler, mapper, unitOfWork, cacheProvider)
        {
            this.roleMasterRepository = roleMasterRepository;
            this.rolePermissionRepository = rolePermissionRepository;
        }

        public async Task<GeneralResponse<ICollection<RoleMaster>>> GetAll()
        {
            var response = new GeneralResponse<ICollection<RoleMaster>>();
            try
            {
                response.Data = (await roleMasterRepository.GetManyAsync(o => o.IsActive && !o.IsDeleted, true))
                                            .OrderBy(o => o.RoleName).ToList();
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }

        public async Task<PagedEntityResponse<RoleMaster>> GetPageWiseData(SearchFilter searchFilter)
        {
            searchFilter.Search = searchFilter.Search?.Trim().ToLower();
            var response = new PagedEntityResponse<RoleMaster>();
            try
            {
                var data = roleMasterRepository.Query(true).Where(o => o.IsActive && !o.IsDeleted);
                //Filter
                if (!string.IsNullOrWhiteSpace(searchFilter.Search))
                {
                    data = data.Where(x => x.RoleName.ToLower().Contains(searchFilter.Search) || x.RoleDescription.ToLower().Contains(searchFilter.Search));
                }
                //sort 
                var ascending = searchFilter.SortDirection == "asc";
                if (!string.IsNullOrWhiteSpace(searchFilter.SortColumn))
                {
                    switch (searchFilter.SortColumn.Trim().ToLower())
                    {
                        case "rolename":
                            data = data.OrderBy(p => p.RoleName, ascending);
                            break;
                        case "roledescription":
                            data = data.OrderBy(p => p.RoleDescription, ascending);
                            break;
                        default:
                            data = data.OrderBy(p => p.RoleName, ascending);
                            break;
                    }
                }
                //Page wise
                response = await data.GetPaged(searchFilter.PageIndex, searchFilter.PageSize);
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }

        public async Task<GeneralResponse<RoleMaster>> Get(int id)
        {
            var response = new GeneralResponse<RoleMaster>();
            try
            {
                var obj = await roleMasterRepository.GetByIdAsync(id);
                if (obj != null)
                {
                    response.Data = obj;
                }
                else
                {
                    response.Message = localizer["msgRecordNotExist", new string[] { "Role" }];
                }
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }

        public async Task<GeneralResponse<bool>> Create(RoleMasterModel roleMasterModel)
        {
            var response = new GeneralResponse<bool>();
            try
            {
                var validator = new RoleMasterValidator(localizer);
                var results = validator.Validate(roleMasterModel, ruleSet: "New");
                if (results.IsValid)
                {
                    // Unique validation 
                    var isExists = roleMasterRepository.Any(x => x.RoleName.ToLower().Trim() == roleMasterModel.RoleName.ToLower().Trim());
                    if (isExists)
                    {
                        response.Result = ResultType.Warning;
                        response.Message = localizer["msgDuplicateRecord", new string[] { "Role" }];
                    }
                    else
                    {
                        RoleMaster roleMaster = mapper.Map<RoleMaster>(roleMasterModel);
                        roleMaster.CreatedBy = RequestUserID;
                        roleMaster.IsActive = true;
                        roleMaster.IsDeleted = false;
                        roleMasterRepository.AddAsync(roleMaster);
                        await Save();
                        response.Message = localizer["msgInsertSuccess", new string[] { "Role" }];
                    }
                }
                else
                {
                    response.Result = ResultType.Warning;
                    response.SetInfo(results);
                }
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }

        public async Task<GeneralResponse<bool>> Update(RoleMasterModel roleMasterModel)
        {
            var response = new GeneralResponse<bool>();
            try
            {
                var validator = new RoleMasterValidator(localizer);
                var results = validator.Validate(roleMasterModel, ruleSet: "Edit,New");
                if (results.IsValid)
                {
                    // Unique validation 
                    var isExists = roleMasterRepository.Any(x => x.RoleID != roleMasterModel.RoleID
                                                            && x.RoleName.ToLower() == roleMasterModel.RoleName.ToLower().Trim());
                    if (isExists)
                    {
                        response.Result = ResultType.Warning;
                        response.Message = localizer["msgDuplicateRecord", new string[] { "Role" }];
                    }
                    else
                    {
                        var obj = await roleMasterRepository.GetByIdAsync(roleMasterModel.RoleID);
                        if (obj != null)
                        {
                            mapper.Map<RoleMasterModel, RoleMaster>(roleMasterModel, obj);
                            obj.UpdatedBy = RequestUserID;
                            obj.UpdatedOn = DateTime.Now;
                            obj.IsActive = true;
                            obj.IsDeleted = false;
                            roleMasterRepository.UpdateAsync(obj);
                            await Save();
                            response.Message = localizer["msgUpdateSuccess", new string[] { "Role" }];
                        }
                    }
                }
                else
                {
                    response.Result = ResultType.Warning;
                    response.SetInfo(results);
                }
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }

        public async Task<GeneralResponse<bool>> Delete(int id)
        {
            var response = new GeneralResponse<bool>();
            try
            {
                var obj = await roleMasterRepository.GetByIdAsync(id);
                if (obj != null)
                {
                    if (!HasReference(id))
                    {
                        obj.UpdatedBy = RequestUserID;
                        obj.UpdatedOn = DateTime.Now;
                        obj.IsDeleted = true;
                        roleMasterRepository.UpdateAsync(obj);
                        //Delete all the permission which belongs to role
                        DeleteRolePermission(id);
                        await Save();
                        response.Message = localizer["msgDeleteSuccess", new string[] { "Role" }];
                    }
                    else
                    {
                        response.Result = ResultType.Warning;
                        response.Message = localizer["msgRoleDeleteFail"];
                    }
                }
                else
                {
                    response.Result = ResultType.Warning;
                    response.Message = localizer["msgRecordNotExist", new string[] { "Role" }];
                }
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }

        public bool HasReference(int id)
        {
            return roleMasterRepository.Any(x => x.RoleID == id &&
                    (
                        x.UserRole.Any()
                    ));
        }

        private void DeleteRolePermission(int roleID)
        {
            rolePermissionRepository.Delete(o => o.RoleID == roleID);
        }
    }
}