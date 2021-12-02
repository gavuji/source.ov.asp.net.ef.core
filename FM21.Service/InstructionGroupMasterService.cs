using AutoMapper;
using FluentValidation;
using FM21.Core;
using FM21.Core.Model;
using FM21.Core.Validator;
using FM21.Data.Infrastructure;
using FM21.Entities;
using FM21.Service.Caching;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace FM21.Service
{
    public class InstructionGroupMasterService : BaseService, IInstructionGroupMasterService
    {
        private readonly IRepository<InstructionGroupMaster> instructionGroupMasterRepository;
        private readonly IRepository<InstructionMaster> instructionRepository;

        public InstructionGroupMasterService(IServiceProvider provider, IExceptionHandler exceptionHandler, IMapper mapper, IUnitOfWork unitOfWork, ICacheProvider cacheProvider,
            IRepository<InstructionGroupMaster> instructionGroupMasterRepository, IRepository<InstructionMaster> instructionRepository)
            : base(provider, exceptionHandler, mapper, unitOfWork, cacheProvider)
        {
            this.instructionGroupMasterRepository = instructionGroupMasterRepository;
            this.instructionRepository = instructionRepository;
        }

        public async Task<GeneralResponse<ICollection<InstructionGroupMaster>>> GetAll()
        {
            var response = new GeneralResponse<ICollection<InstructionGroupMaster>>();
            try
            {
                response.Data = (await instructionGroupMasterRepository.GetManyAsync(o => o.IsActive, true))
                                                .OrderBy(o => o.InstructionGroupName).ToList();
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }

        public async Task<GeneralResponse<ICollection<InstructionGroupMasterModel>>> GetGroupBySiteProductAndCategory(int siteProductMapID, int categoryID)
        {
            var response = new GeneralResponse<ICollection<InstructionGroupMasterModel>>();
            try
            {
                response.Data = new Collection<InstructionGroupMasterModel>();
                InstructionGroupMasterModel instructionGroup;
                int groupIndex = 0;

                var arrData = instructionRepository.Query(true)
                        .Where(o => o.IsActive && !o.IsDeleted && o.SiteProductMapID == siteProductMapID && o.InstructionCategoryID == categoryID)
                        .Include(o => o.InstructionGroup)
                        .Select(o => new
                        {
                            o.InstructionGroupID,
                            o.InstructionGroup.InstructionGroupName,
                            o.GroupDisplayOrder
                        })
                        .Distinct()
                        .OrderBy(o => o.GroupDisplayOrder);

                foreach (var item in arrData)
                {
                    groupIndex++;
                    instructionGroup = new InstructionGroupMasterModel();
                    instructionGroup.InstructionGroupID = item.InstructionGroupID;
                    instructionGroup.InstructionGroupName = item.InstructionGroupName;
                    instructionGroup.GroupDisplayOrder = groupIndex;
                    response.Data.Add(instructionGroup);
                }
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return await Task.FromResult(response);
        }

        public async Task<GeneralResponse<bool>> Create(InstructionGroupMasterModel instructionGroupModel)
        {
            var response = new GeneralResponse<bool>();
            try
            {
                var validator = new InstructionGroupMasterValidator(localizer);
                var results = validator.Validate(instructionGroupModel, ruleSet: "New");
                if (results.IsValid)
                {
                    // Unique validation 
                    var isExists = instructionGroupMasterRepository.Any(x => x.InstructionGroupName.ToLower().Trim() == instructionGroupModel.InstructionGroupName.ToLower().Trim());
                    if (isExists)
                    {
                        response.Result = ResultType.Warning;
                        response.Message = localizer["msgDuplicateRecord", new string[] { "Instruction Group" }];
                    }
                    else
                    {
                        InstructionGroupMaster instructionGroupMaster = new InstructionGroupMaster();
                        instructionGroupMaster.InstructionGroupName = instructionGroupModel.InstructionGroupName;
                        instructionGroupMaster.IsActive = true;
                        instructionGroupMaster.CreatedBy = RequestUserID;
                        instructionGroupMasterRepository.AddAsync(instructionGroupMaster);
                        await Save();
                        response.Message = localizer["msgInsertSuccess", new string[] { "Instruction Group" }];
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

        public async Task<GeneralResponse<bool>> Update(InstructionGroupMasterModel instructionGroupModel)
        {
            var response = new GeneralResponse<bool>();
            try
            {
                var validator = new InstructionGroupMasterValidator(localizer);
                var results = validator.Validate(instructionGroupModel, ruleSet: "Edit,New");
                if (results.IsValid)
                {
                    // Unique validation 
                    var isExists = instructionGroupMasterRepository.Any(x => x.InstructionGroupID != instructionGroupModel.InstructionGroupID
                                                            && x.InstructionGroupName.ToLower() == instructionGroupModel.InstructionGroupName.ToLower().Trim());
                    if (isExists)
                    {
                        response.Result = ResultType.Warning;
                        response.Message = localizer["msgDuplicateRecord", new string[] { "Instruction Group" }];
                    }
                    else
                    {
                        var obj = await instructionGroupMasterRepository.GetByIdAsync(instructionGroupModel.InstructionGroupID);
                        if (obj != null)
                        {
                            obj.InstructionGroupName = instructionGroupModel.InstructionGroupName;
                            instructionGroupMasterRepository.UpdateAsync(obj);
                            await Save();
                            response.Message = localizer["msgUpdateSuccess", new string[] { "Instruction Group" }];
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
                var obj = await instructionGroupMasterRepository.GetByIdAsync(id);
                if (obj != null)
                {
                    // check has reference 
                    if (!HasReference(id))
                    {
                        obj.IsActive = false;
                        instructionGroupMasterRepository.UpdateAsync(obj);
                        await Save();
                        response.Message = localizer["msgDeleteSuccess", new string[] { "Instruction Group" }];
                    }
                    else
                    {
                        response.Result = ResultType.Warning;
                        response.Message = localizer["msgDeleteFailAsUsedByOthers"];
                    }
                }
                else
                {
                    response.Result = ResultType.Warning;
                    response.Message = localizer["msgRecordNotExist", new string[] { "Instruction Group" }];
                }
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }

        public bool HasReference(int Id)
        {
            if (instructionRepository.Any(o => o.InstructionGroupID == Id))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}