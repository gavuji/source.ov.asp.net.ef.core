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
using System.Linq;
using System.Threading.Tasks;

namespace FM21.Service
{
    public class ProjectMasterService : BaseService, IProjectMasterService
    {
        private readonly IRepository<ProjectMaster> projectMasterRepository;

        public ProjectMasterService(IServiceProvider provider, IExceptionHandler exceptionHandler, IMapper mapper, IUnitOfWork unitOfWork, ICacheProvider cacheProvider,
            IRepository<ProjectMaster> projectMasterRepository)
            : base(provider, exceptionHandler, mapper, unitOfWork, cacheProvider)
        {
            this.projectMasterRepository = projectMasterRepository;
        }

        public async Task<GeneralResponse<ICollection<ProjectMasterModel>>> GetAll()
        {
            var response = new GeneralResponse<ICollection<ProjectMasterModel>>();
            try
            {
                await Task.Run(() =>
                {
                    var resultList = projectMasterRepository.Query(true).Where(o => o.IsActive && !o.IsDeleted && o.ProjectCode != 100000)
                                    .Include(x => x.CustomerMaster).ToList();
                    response.Data = mapper.Map<ICollection<ProjectMaster>, ICollection<ProjectMasterModel>>(resultList).AsQueryable<ProjectMasterModel>().OrderBy(o => o.ProjectCode).ToList();
                });
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }

        public async Task<GeneralResponse<int>> GetNextProjectCode()
        {
            var response = new GeneralResponse<int>();
            try
            {
                response.Data = (await projectMasterRepository.GetAllAsync()).Select(o => o.ProjectCode).DefaultIfEmpty().Max() + 1;
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }

        public async Task<PagedEntityResponse<ProjectMasterModel>> GetPageWiseData(SearchFilter searchFilter)
        {
            var response = new PagedEntityResponse<ProjectMasterModel>();
            try
            {
                var result = projectMasterRepository.Query(true).Where(o => o.IsActive && !o.IsDeleted && o.ProjectCode != 100000)
               .Include(x => x.CustomerMaster).ToList();
                if (!string.IsNullOrWhiteSpace(searchFilter.Search))
                {
                    result = result.Where(x => x.ProjectCode.ToString().Contains(searchFilter.Search) || x.ProjectDescription.ToLower().Contains(searchFilter.Search.ToLower())
                     || (x.NPICode ?? string.Empty).ToLower().Contains(searchFilter.Search.ToLower()) || x.CustomerMaster.Name.ToLower().Contains(searchFilter.Search.ToLower())).ToList();

                }
                var data = mapper.Map<IList<ProjectMaster>, IList<ProjectMasterModel>>(result).AsQueryable<ProjectMasterModel>();

                var ascending = searchFilter.SortDirection == "asc";
                if (!string.IsNullOrWhiteSpace(searchFilter.SortColumn))
                {
                    switch (searchFilter.SortColumn.Trim().ToLower())
                    {
                        case "projectcode":
                            data = data.OrderBy(p => p.ProjectCode, ascending);
                            break;
                        case "npicode":
                            data = data.OrderBy(p => p.NPICode, ascending);
                            break;
                        case "customername":
                            data = data.OrderBy(p => p.CustomerName, ascending);
                            break;
                        case "projectdescription":
                            data = data.OrderBy(p => p.ProjectDescription.Trim(), ascending);
                            break;

                        default:
                            data = data.OrderBy(p => p.ProjectCode, ascending);
                            break;
                    }
                }
                else
                {
                    data = data.OrderBy(p => p.ProjectId, ascending);
                }
                response = await data.GetPaged(searchFilter.PageIndex, searchFilter.PageSize);
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }

        public async Task<GeneralResponse<ProjectMaster>> Get(int id)
        {
            var response = new GeneralResponse<ProjectMaster>();
            try
            {
                var obj = await projectMasterRepository.GetByIdAsync(id);
                if (obj != null)
                {
                    response.Data = obj;
                }
                else
                {
                    response.Message = localizer["msgRecordNotExist", new string[] { "Project" }];
                }
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }

        public async Task<GeneralResponse<bool>> Create(ProjectMasterModel entity)
        {
            var response = new GeneralResponse<bool>();
            try
            {
                var validator = new ProjectMasterValidator(localizer);
                var results = validator.Validate(entity, ruleSet: "New");
                if (results.IsValid)
                {
                    if (entity.ProjectCode == 100000)
                    {
                        response.Result = ResultType.Warning;
                        response.Message = localizer["msgReserveProjectCode100000"];
                    }
                    else
                    {
                        var isExists = projectMasterRepository.Any(o => o.ProjectCode == entity.ProjectCode);
                        if (!isExists)
                        {
                            ProjectMaster projectMaster = mapper.Map<ProjectMaster>(entity);
                            projectMaster.IsActive = true;
                            projectMaster.IsDeleted = false;
                            projectMaster.CreatedBy = RequestUserID;
                            projectMaster.CreatedOn = DateTime.Now;
                            projectMasterRepository.AddAsync(projectMaster);
                            await Save();
                            response.Message = localizer["msgInsertSuccess", new string[] { "Project" }];
                        }
                        else
                        {
                            response.Result = ResultType.Warning;
                            response.Message = localizer["msgDuplicateRecord", new string[] { "Project" }];
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

        public async Task<GeneralResponse<bool>> Update(ProjectMasterModel entity)
        {
            var response = new GeneralResponse<bool>();
            try
            {
                var validator = new ProjectMasterValidator(localizer);
                var results = validator.Validate(entity, ruleSet: "Edit,New");
                if (results.IsValid)
                {
                    if (entity.ProjectCode != 100000)
                    {
                        var obj = await projectMasterRepository.GetByIdAsync(entity.ProjectId);
                        if (obj != null && obj.IsActive && !obj.IsDeleted)
                        {
                            if (entity.ProjectCode == obj.ProjectCode || !IsProdcutCodeExist(entity))
                            {
                                mapper.Map<ProjectMasterModel, ProjectMaster>(entity, obj);
                                obj.UpdatedBy = RequestUserID;
                                obj.UpdatedOn = DateTime.Now;
                                obj.IsDeleted = false;
                                obj.IsActive = true;
                                projectMasterRepository.UpdateAsync(obj);
                                await Save();
                                response.Message = localizer["msgUpdateSuccess", new string[] { "Project" }];
                            }
                            else
                            {
                                response.Result = ResultType.Warning;
                                response.Message = localizer["msgDuplicateRecord", new string[] { "Project" }];
                            }
                        }
                        else
                        {
                            response.Result = ResultType.Warning;
                            response.Message = localizer["msgRecordNotExist", new string[] { "Project" }];
                        }
                    }
                    else
                    {
                        response.Result = ResultType.Warning;
                        response.Message = localizer["msgReserveProjectCode100000"];
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
                var obj = await projectMasterRepository.GetByIdAsync(id);
                if (obj != null)
                {
                    obj.UpdatedBy = RequestUserID;
                    obj.UpdatedOn = DateTime.Now;
                    obj.IsActive = false;
                    obj.IsDeleted = true;
                    projectMasterRepository.UpdateAsync(obj);
                    await Save();
                    response.Message = localizer["msgDeleteSuccess", new string[] { "Project" }];
                }
                else
                {
                    response.Result = ResultType.Warning;
                    response.Message = localizer["msgRecordNotExist", new string[] { "Project" }];
                }
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }

        private bool IsProdcutCodeExist(ProjectMasterModel entity)
        {
            return projectMasterRepository.Any(o => o.ProjectCode == entity.ProjectCode);
        }
    }
}