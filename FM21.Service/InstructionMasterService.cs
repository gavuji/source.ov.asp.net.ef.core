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
    public class InstructionMasterService : BaseService, IInstructionMasterService
    {
        private readonly IRepository<InstructionMaster> instructionRepository;

        public InstructionMasterService(IServiceProvider provider, IExceptionHandler exceptionHandler, IMapper mapper, IUnitOfWork unitOfWork, ICacheProvider cacheProvider,
            IRepository<InstructionMaster> instructionRepository)
            : base(provider, exceptionHandler, mapper, unitOfWork, cacheProvider)
        {
            this.instructionRepository = instructionRepository;
        }

        public async Task<GeneralResponse<ICollection<InstructionMasterModel>>> GetAll()
        {
            var response = new GeneralResponse<ICollection<InstructionMasterModel>>();
            try
            {
                await Task.Run(() =>
                {
                    var arrList = instructionRepository.Query(true)
                                    .Where(o => o.IsActive && !o.IsDeleted)
                                    .Include(o => o.SiteProductMap).ThenInclude(o => o.ProductType)
                                    .Include(o => o.SiteProductMap).ThenInclude(o => o.Site)
                                    .Include(o => o.InstructionCategory)
                                    .Include(o => o.InstructionGroup)
                                    .ToList();

                    response.Data = mapper.Map<IList<InstructionMaster>, IList<InstructionMasterModel>>(arrList);
                });
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }

        public async Task<PagedEntityResponse<InstructionMasterModel>> GetPageWiseData(SearchFilter searchFilter)
        {
            searchFilter.Search = searchFilter.Search?.Trim().ToLower();
            var response = new PagedEntityResponse<InstructionMasterModel>();
            try
            {
                var result = instructionRepository.Query(true)
                                .Where(o => o.IsActive && !o.IsDeleted)
                                .Include(o => o.SiteProductMap).ThenInclude(o => o.ProductType)
                                .Include(o => o.SiteProductMap).ThenInclude(o => o.Site)
                                .Include(o => o.InstructionCategory)
                                .Include(o => o.InstructionGroup)
                                .ToList();

                //Filter
                if (!string.IsNullOrWhiteSpace(searchFilter.Search))
                {
                    result = result.Where(x => x.DescriptionEn.ToLower().Contains(searchFilter.Search)
                                        || (x.DescriptionFr ?? string.Empty).ToLower().Contains(searchFilter.Search)
                                        || (x.DescriptionEs ?? string.Empty).Contains(searchFilter.Search)
                                        || x.InstructionCategory.InstructionCategory.ToLower().Contains(searchFilter.Search)
                                        || x.InstructionGroup.InstructionGroupName.ToLower().Contains(searchFilter.Search))
                                    .ToList();
                }

                var data = mapper.Map<IList<InstructionMaster>, IList<InstructionMasterModel>>(result).AsQueryable<InstructionMasterModel>();

                //sort 
                var ascending = searchFilter.SortDirection == "asc";
                if (!string.IsNullOrWhiteSpace(searchFilter.SortColumn))
                {
                    switch (searchFilter.SortColumn.Trim().ToLower())
                    {
                        case "siteproduct":
                            data = data.OrderBy(p => p.SiteProductMap, ascending);
                            break;
                        case "instructioncategory":
                            data = data.OrderBy(p => p.InstructionCategory, ascending);
                            break;
                        case "instructiongroup":
                            data = data.OrderBy(p => p.InstructionGroup, ascending);
                            break;
                        case "descriptionen":
                            data = data.OrderBy(p => p.DescriptionEn, ascending);
                            break;
                        case "descriptionfr":
                            data = data.OrderBy(p => p.DescriptionFr, ascending);
                            break;
                        case "descriptiones":
                            data = data.OrderBy(p => p.DescriptionEs, ascending);
                            break;
                        default:
                            data = data.OrderBy(p => p.DescriptionEn, ascending);
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

        public async Task<PagedEntityResponse<InstructionMasterModel>> GetSearchListWithFilter(SearchFilter searchFilter, int siteProductMapID, int instructionCategoryID)
        {
            searchFilter.Search = searchFilter.Search?.Trim().ToLower();
            var response = new PagedEntityResponse<InstructionMasterModel>();
            try
            {
                var result = instructionRepository.Query(true)
                                .Where(o => o.IsActive && !o.IsDeleted
                                && (siteProductMapID == 0 || o.SiteProductMapID == siteProductMapID)
                                && (instructionCategoryID == 0 || o.InstructionCategoryID == instructionCategoryID))
                                .Include(o => o.SiteProductMap).ThenInclude(o => o.ProductType)
                                .Include(o => o.SiteProductMap).ThenInclude(o => o.Site)
                                .Include(o => o.InstructionCategory)
                                .Include(o => o.InstructionGroup)
                                .ToList();

                //Filter
                if (!string.IsNullOrWhiteSpace(searchFilter.Search))
                {
                    result = result.Where(x => x.DescriptionEn.ToLower().Contains(searchFilter.Search)
                                        || (x.DescriptionFr ?? string.Empty).ToLower().Contains(searchFilter.Search)
                                        || (x.DescriptionEs ?? string.Empty).Contains(searchFilter.Search)
                                        || x.InstructionGroup.InstructionGroupName.ToLower().Contains(searchFilter.Search))
                                    .ToList();
                }

                var data = mapper.Map<IList<InstructionMaster>, IList<InstructionMasterModel>>(result).AsQueryable<InstructionMasterModel>();

                data = data.OrderBy(p => p.SiteProductMapID, true)
                            .ThenBy(p => p.InstructionCategoryID)
                            .ThenBy(p => p.GroupDisplayOrder)
                            .ThenBy(p => p.GroupItemDisplayOrder);

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

        public async Task<GeneralResponse<InstructionMaster>> Get(int id)
        {
            var response = new GeneralResponse<InstructionMaster>();
            try
            {
                var obj = await instructionRepository.GetByIdAsync(id);
                if (obj != null)
                {
                    response.Data = obj;
                }
                else
                {
                    response.Message = localizer["msgRecordNotExist", new string[] { "Instruction" }];
                }
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }

        public async Task<GeneralResponse<ICollection<InstructionMasterModel>>> GetBySiteProductCategoryAndGroup(int siteProductMapID, int categoryID, int groupID)
        {
            var response = new GeneralResponse<ICollection<InstructionMasterModel>>();
            try
            {
                await Task.Run(() =>
                {
                    int index = 0;
                    var arrList = instructionRepository.Query(true)
                            .Where(o => o.IsActive && !o.IsDeleted && o.SiteProductMapID == siteProductMapID && o.InstructionCategoryID == categoryID && o.InstructionGroupID == groupID)
                            .OrderBy(o => o.GroupItemDisplayOrder)
                            .ToList();

                    arrList.ForEach(o =>
                    {
                        index++;
                        o.GroupItemDisplayOrder = index;
                    });

                    response.Data = mapper.Map<IList<InstructionMaster>, IList<InstructionMasterModel>>(arrList);
                });
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }

        public async Task<GeneralResponse<bool>> Create(InstructionMasterModel instructionModel)
        {
            var response = new GeneralResponse<bool>();
            try
            {
                var validator = new InstructionMasterValidator(localizer);
                var results = validator.Validate(instructionModel, ruleSet: "New");
                if (results.IsValid)
                {
                    // Unique validation 
                    var isExists = instructionRepository.Any(o => o.SiteProductMapID == instructionModel.SiteProductMapID
                                                                && o.InstructionCategoryID == instructionModel.InstructionCategoryID
                                                                && o.DescriptionEn.ToLower().Trim() == instructionModel.DescriptionEn.ToLower().Trim());
                    if (isExists)
                    {
                        response.Result = ResultType.Warning;
                        response.Message = localizer["msgDuplicateRecord", new string[] { "Instruction" }];
                    }
                    else
                    {
                        InstructionMaster instructionMaster = mapper.Map<InstructionMaster>(instructionModel);
                        instructionMaster.IsActive = true;
                        instructionMaster.CreatedBy = RequestUserID;

                        var groupData = await instructionRepository.GetManyAsync(o => o.SiteProductMapID == instructionMaster.SiteProductMapID 
                                                                        && o.InstructionCategoryID == instructionMaster.InstructionCategoryID
                                                                        && o.InstructionGroupID == instructionMaster.InstructionGroupID, true);
                        if (groupData == null || groupData.FirstOrDefault() == null)
                        {
                            int groupDisplayOrder = (await instructionRepository.GetAllAsync())
                                                        .Where(o => o.SiteProductMapID == instructionMaster.SiteProductMapID && o.InstructionCategoryID == instructionMaster.InstructionCategoryID)
                                                        .Select(o => o.GroupDisplayOrder)
                                                        .DefaultIfEmpty()
                                                        .Max() + 1;
                            instructionMaster.GroupDisplayOrder = groupDisplayOrder;
                            instructionMaster.GroupItemDisplayOrder = 1;
                        }
                        else
                        {
                            instructionMaster.GroupDisplayOrder = groupData.FirstOrDefault().GroupDisplayOrder;
                            instructionMaster.GroupItemDisplayOrder = groupData.Select(o => o.GroupItemDisplayOrder).DefaultIfEmpty().Max() + 1;
                        }
                        
                        instructionRepository.AddAsync(instructionMaster);
                        await Save();
                        response.Message = localizer["msgInsertSuccess", new string[] { "Instruction" }];
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

        public async Task<GeneralResponse<bool>> Update(InstructionMasterModel instructionModel)
        {
            var response = new GeneralResponse<bool>();
            try
            {
                var validator = new InstructionMasterValidator(localizer);
                var results = validator.Validate(instructionModel, ruleSet: "Edit,New");
                if (results.IsValid)
                {
                    // Unique validation 
                    var isExists = instructionRepository.Any(o => o.SiteProductMapID == instructionModel.SiteProductMapID
                                                                && o.InstructionCategoryID == instructionModel.InstructionCategoryID
                                                                && o.InstructionMasterID != instructionModel.InstructionMasterID
                                                                && o.DescriptionEn.ToLower() == instructionModel.DescriptionEn.ToLower().Trim());
                    if (isExists)
                    {
                        response.Result = ResultType.Warning;
                        response.Message = localizer["msgDuplicateRecord", new string[] { "Instruction" }];
                    }
                    else
                    {
                        var obj = await instructionRepository.GetByIdAsync(instructionModel.InstructionMasterID);
                        if (obj != null)
                        {
                            if (instructionModel.SiteProductMapID != obj.SiteProductMapID || instructionModel.InstructionCategoryID != obj.InstructionCategoryID || instructionModel.InstructionGroupID != obj.InstructionGroupID)
                            {
                                var groupData = await instructionRepository.GetManyAsync(o => o.SiteProductMapID == instructionModel.SiteProductMapID
                                                                                           && o.InstructionCategoryID == instructionModel.InstructionCategoryID
                                                                                           && o.InstructionGroupID == instructionModel.InstructionGroupID, true);
                                if (groupData == null || groupData.FirstOrDefault() == null)
                                {
                                    int groupDisplayOrder = (await instructionRepository.GetAllAsync())
                                                                .Where(o => o.SiteProductMapID == instructionModel.SiteProductMapID && o.InstructionCategoryID == instructionModel.InstructionCategoryID)
                                                                .Select(o => o.GroupDisplayOrder)
                                                                .DefaultIfEmpty()
                                                                .Max() + 1;
                                    obj.GroupDisplayOrder = groupDisplayOrder;
                                    obj.GroupItemDisplayOrder = 1;
                                }
                                else
                                {
                                    obj.GroupDisplayOrder = groupData.FirstOrDefault().GroupDisplayOrder;
                                    obj.GroupItemDisplayOrder = groupData.Where(o => o.InstructionMasterID != instructionModel.InstructionMasterID)
                                                                    .Select(o => o.GroupItemDisplayOrder)
                                                                    .DefaultIfEmpty()
                                                                    .Max() + 1;
                                }
                            }

                            mapper.Map<InstructionMasterModel, InstructionMaster>(instructionModel, obj);
                            obj.UpdatedBy = RequestUserID;
                            obj.UpdatedOn = DateTime.Now;
                            instructionRepository.UpdateAsync(obj);
                            await Save();
                            response.Message = localizer["msgUpdateSuccess", new string[] { "Instruction" }];
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
                var obj = await instructionRepository.GetByIdAsync(id);
                if (obj != null)
                {
                    instructionRepository.Delete(obj);
                    await Save();
                    response.Message = localizer["msgDeleteSuccess", new string[] { "Instruction" }];
                }
                else
                {
                    response.Result = ResultType.Warning;
                    response.Message = localizer["msgRecordNotExist", new string[] { "Instruction" }];
                }
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }

        public async Task<GeneralResponse<bool>> UpdateInstructionGroupOrder(int siteProductMapID, int categoryID, ICollection<InstructionGroupMasterModel> lstInstructionGroup)
        {
            var response = new GeneralResponse<bool>();
            try
            {
                foreach (var group in lstInstructionGroup)
                {
                    var obj = instructionRepository.Query(true)
                           .Where(o => o.IsActive && !o.IsDeleted && o.SiteProductMapID == siteProductMapID && o.InstructionCategoryID == categoryID && o.InstructionGroupID == group.InstructionGroupID)
                           .ToList();

                    foreach (var objInstruction in obj)
                    {
                        objInstruction.GroupDisplayOrder = group.GroupDisplayOrder;
                        objInstruction.UpdatedBy = RequestUserID;
                        objInstruction.UpdatedOn = DateTime.Now;
                        instructionRepository.UpdateAsync(objInstruction);
                    }
                }
                await Save();
                response.Message = localizer["msgUpdateSuccess", new string[] { "Instruction" }];
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }

        public async Task<GeneralResponse<bool>> UpdateInstructionOrder(ICollection<InstructionMasterModel> lstInstruction)
        {
            var response = new GeneralResponse<bool>();
            try
            {
                foreach (var instruction in lstInstruction)
                {
                    var objInstruction = instructionRepository.Query(true)
                            .Where(o => o.InstructionMasterID == instruction.InstructionMasterID)
                            .FirstOrDefault();

                    if (objInstruction != null)
                    {
                        objInstruction.GroupItemDisplayOrder = instruction.GroupItemDisplayOrder;
                        objInstruction.UpdatedBy = RequestUserID;
                        objInstruction.UpdatedOn = DateTime.Now;
                        instructionRepository.UpdateAsync(objInstruction);
                    }
                }
                await Save();
                response.Message = localizer["msgUpdateSuccess", new string[] { "Instruction" }];
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