using AutoMapper;
using FluentValidation;
using FM21.Core;
using FM21.Core.Model;
using FM21.Core.Validator;
using FM21.Data;
using FM21.Data.Infrastructure;
using FM21.Entities;
using FM21.Service.Caching;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Threading.Tasks;

namespace FM21.Service
{
    public class UserMasterService : BaseService, IUserMasterService
    {
        private readonly IRepository<UserMaster> userMasterRepository;
        private readonly IRepository<UserRole> userRoleRepository;

        public UserMasterService(IServiceProvider provider, IExceptionHandler exceptionHandler, IMapper mapper, IUnitOfWork unitOfWork, ICacheProvider cacheProvider,
              IRepository<UserMaster> userMasterRepository, IRepository<UserRole> userRoleRepository)
            : base(provider, exceptionHandler, mapper, unitOfWork, cacheProvider)
        {
            this.userMasterRepository = userMasterRepository;
            this.userRoleRepository = userRoleRepository;
        }

        public async Task<GeneralResponse<UserMasterModel>> GetCurrentUser(string currentUserName)
        {
            var response = new GeneralResponse<UserMasterModel>();
            try
            {
                currentUserName = ApplicationConstants.Domain.ToLower() + "/" + currentUserName.Split('\\')[1].ToLower();
                var user = userMasterRepository.Query(true)
                                .Where(o => o.DomainFullName.ToLower() == currentUserName)
                                .Include(o => o.UserRole)
                                .ThenInclude(o => o.Role)
                                .ThenInclude(o => o.RolePermissionMapping)
                                .FirstOrDefault();

                if (user != null)
                {
                    response.Data = new UserMasterModel()
                    {
                        UserID = user.UserID,
                        DomainFullName = user.DomainFullName,
                        DisplayName = user.DisplayName,
                        AssignedRoleList = user.UserRole.Select(x => x.RoleID).ToArray(),
                        AssignedRoles = user.UserRole.Select(x => x.Role.RoleName).ToArray(),
                        AssignedPermissionList = new List<int>()
                    };
                    user.UserRole.ToList().ForEach(x =>
                    {
                        response.Data.AssignedPermissionList.AddRange(x.Role.RolePermissionMapping.Where(o => o.PermissionType != 0)
                                                                            .Select(o => o.PermissionID).ToArray());
                    });
                    response.Data.AssignedPermissionList = response.Data.AssignedPermissionList.Distinct().ToList();
                }
                else
                {
                    response.Message = localizer["msgRecordNotExist", new string[] { "User" }];
                }
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return await Task.FromResult(response);
        }

        public async Task<PagedEntityResponse<UserMasterModel>> GetAllADUsersAlongWithRoles(SearchFilter searchFilter)
        {
            var response = new PagedEntityResponse<UserMasterModel>();
            try
            {
                await Task.Run(() =>
                {
                    var data = GetAllADUsersWithRoles();

                    //Filter
                    if (!string.IsNullOrWhiteSpace(searchFilter.Search))
                    {
                        data = data.Where(x => (x.DisplayName ?? string.Empty).ToLower().Contains(searchFilter.Search.ToLower())
                                            || x.DomainFullName.ToLower().Contains(searchFilter.Search.ToLower())
                                            || (x.AssignedRoles != null && x.AssignedRoleNames.ToLower().Contains(searchFilter.Search.ToLower())))
                                        .ToList();
                    }

                    //sort
                    var ascending = searchFilter.SortDirection == "asc";
                    if (!string.IsNullOrWhiteSpace(searchFilter.SortColumn))
                    {
                        switch (searchFilter.SortColumn.Trim().ToLower())
                        {
                            case "displayname":
                                data = data.AsQueryable().OrderBy(o => o.DisplayName, ascending).ToList();
                                break;
                            case "domainfullname":
                                data = data.AsQueryable().OrderBy(o => o.DomainFullName, ascending).ToList();
                                break;
                            case "assignedroles":
                                data = data.AsQueryable().OrderBy(o => o.AssignedRoleNames, ascending).ToList();
                                break;
                            default:
                                data = data.AsQueryable().OrderBy(o => o.DisplayName, ascending).ToList();
                                break;
                        }
                    }
                    else
                    {
                        data = data.AsQueryable().OrderBy(o => o.DisplayName, true).ToList();
                    }

                    //Page wise
                    response = data.GetPaged(searchFilter.PageIndex, searchFilter.PageSize);
                });
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }

        private List<Principal> GetADUserList()
        {
            List<Principal> userList = new List<Principal>();
            try
            {
                using (var context = new PrincipalContext(ContextType.Domain, ApplicationConstants.Domain))
                {
                    using (var group = GroupPrincipal.FindByIdentity(context, ApplicationConstants.ADUserGroup))
                    {
                        if (group != null)
                        {
                            userList = group.GetMembers(true).ToList();
                        }
                        else
                        {
                            throw new Exception(localizer["msgDomainADGroupNotFound"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return userList;
        }

        private List<UserMasterModel> GetAllADUsersWithRoles()
        {
            var data = new List<UserMasterModel>();
            try
            {
                string domainName = ApplicationConstants.Domain;
                var savedUsers = userMasterRepository.Query(true)
                                .Include(o => o.UserRole)
                                .ThenInclude(o => o.Role)
                                .ToList();

                var userList = GetADUserList();
                userList.ToList()
                    .ForEach(o =>
                    {
                        var obj = new UserMasterModel();
                        obj.DomainFullName = string.Format("{0}/{1}", domainName, o.SamAccountName);
                        obj.DisplayName = (string.IsNullOrEmpty(o.DisplayName) ? o.SamAccountName : o.DisplayName);
                        var user = savedUsers.FirstOrDefault(x => x.DomainFullName == obj.DomainFullName);
                        if (user != null)
                        {
                            obj.UserID = user.UserID;
                            obj.AssignedRoleList = user.UserRole.Select(x => x.RoleID).ToArray();
                            obj.AssignedRoles = user.UserRole.Select(x => x.Role.RoleName).ToArray();
                            obj.AssignedRoleNames = string.Join(", ", user.UserRole.Select(x => x.Role.RoleName));
                        }
                        data.Add(obj);
                    });
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }

        public async Task<GeneralResponse<bool>> UpdateUserRolePermission(UserMasterModel userMasterModel)
        {
            var response = new GeneralResponse<bool>();
            try
            {
                var validator = new UserMasterValidator(localizer);
                var results = validator.Validate(userMasterModel, ruleSet: "Edit,New");
                if (results.IsValid)
                {
                    var user = await userMasterRepository.GetAsync(x => x.DomainFullName.ToLower() == userMasterModel.DomainFullName.ToLower());
                    if (user == null)
                    {
                        UserMaster userMaster = mapper.Map<UserMaster>(userMasterModel);
                        userMaster.CreatedBy = RequestUserID;
                        userMaster.IsDeleted = false;
                        
                        if (userMasterModel.AssignedRoleList != null && userMasterModel.AssignedRoleList.Any())
                        {
                            userMasterModel.AssignedRoleList.ToList().ForEach(roleID =>
                            {
                                UserRole userRole = new UserRole();
                                userRole.RoleID = roleID;
                                userRole.CreatedBy = RequestUserID;
                                userMaster.UserRole.Add(userRole);
                            });
                        }

                        userMasterRepository.AddAsync(userMaster);
                        await Save();
                        response.Message = localizer["msgUpdateSuccess", new string[] { "User" }];
                    }
                    else
                    {
                        user.UpdatedBy = RequestUserID;
                        user.UpdatedOn = DateTime.Now;
                        userMasterRepository.UpdateAsync(user);

                        if(userMasterModel.AssignedRoleList == null)
                        {
                            userMasterModel.AssignedRoleList = new int[] { };
                        }

                        userRoleRepository.Delete(o => o.UserID == user.UserID && !userMasterModel.AssignedRoleList.Contains(o.RoleID));

                        if (userMasterModel.AssignedRoleList.Any())
                        {
                            userMasterModel.AssignedRoleList.ToList().ForEach(roleID =>
                            {
                                var role = userRoleRepository.Get(x => x.UserID == user.UserID && x.RoleID == roleID);
                                if (role != null)
                                {
                                    role.UpdatedBy = RequestUserID;
                                    role.UpdatedOn = DateTime.Now;
                                    userRoleRepository.UpdateAsync(role);
                                }
                                else
                                {
                                    UserRole userRole = new UserRole();
                                    userRole.UserID = user.UserID;
                                    userRole.RoleID = roleID;
                                    userRole.CreatedBy = RequestUserID;
                                    userRoleRepository.AddAsync(userRole);
                                }
                            });
                        }
                        await Save();
                        response.Message = localizer["msgUpdateSuccess", new string[] { "User" }];
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
    }
}