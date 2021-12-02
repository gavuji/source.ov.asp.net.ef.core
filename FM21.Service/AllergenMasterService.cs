using AutoMapper;
using FluentValidation;
using FM21.Core;
using FM21.Core.Model;
using FM21.Core.Validator;
using FM21.Data;
using FM21.Data.Infrastructure;
using FM21.Entities;
using FM21.Service.Caching;
using FM21.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FM21.Service
{
    public class AllergenMasterService : BaseService, IAllergenMasterService
    {
        private readonly IRepository<AllergenMaster> allergenRepository;

        public AllergenMasterService(IServiceProvider provider, IExceptionHandler exceptionHandler, IMapper mapper, IUnitOfWork unitOfWork, ICacheProvider cacheProvider,
            IRepository<AllergenMaster> allergenRepository)
            : base(provider, exceptionHandler, mapper, unitOfWork, cacheProvider)
        {
            this.allergenRepository = allergenRepository;
        }

        public async Task<GeneralResponse<ICollection<AllergenMaster>>> GetAll()
        {
            var response = new GeneralResponse<ICollection<AllergenMaster>>();
            try
            {
                response.Data = (await allergenRepository.GetManyAsync(o => o.IsActive == true && o.IsDeleted == false, true))
                                        .OrderBy(o => o.AllergenName).ToList();
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }

        public async Task<PagedEntityResponse<AllergenMaster>> GetPageWiseData(string filter, int pageIndex, int pageSize, string sortColumn, string sortDirection)
        {
            filter = filter?.Trim().ToLower();
            var response = new PagedEntityResponse<AllergenMaster>();
            try
            {
                var data = allergenRepository.Query(true);
                //Filter
                if (!string.IsNullOrWhiteSpace(filter))
                {
                    data = data.Where(x => x.AllergenName.ToLower().Contains(filter) || x.AllergenCode.ToLower().Contains(filter)
                                      || (x.AllergenDescription_En ?? string.Empty).ToLower().Contains(filter) || (x.AllergenDescription_Fr ?? string.Empty).ToLower().Contains(filter)
                                      || (x.AllergenDescription_Es ?? string.Empty).ToLower().Contains(filter));
                }
                //sort 
                var ascending = (sortDirection == "asc");
                if (!string.IsNullOrWhiteSpace(sortColumn))
                {
                    switch (sortColumn.Trim().ToLower())
                    {
                        case "allergendescription_en":
                            data = data.OrderBy(p => p.AllergenDescription_En, ascending);
                            break;
                        case "allergendescription_fr":
                            data = data.OrderBy(p => p.AllergenDescription_Fr, ascending);
                            break;
                        case "allergendescription_es":
                            data = data.OrderBy(p => p.AllergenDescription_Es, ascending);
                            break;
                        case "allergencode":
                            data = data.OrderBy(p => p.AllergenCode, ascending);
                            break;
                        case "allergenname":
                            data = data.OrderBy(p => p.AllergenName, ascending);
                            break;
                        default:
                            data = data.OrderBy(p => p.AllergenID, ascending);
                            break;
                    }
                }
                else
                {
                    data = data.OrderBy(p => p.AllergenID, ascending);
                }
                //Page wise
                data = data.Where(i => i.IsActive == true && i.IsDeleted == false);
                response = await data.GetPaged(pageIndex, pageSize);
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }

        public async Task<GeneralResponse<bool>> Create(AllergenMasterModel allergenMasterModel)
        {
            var response = new GeneralResponse<bool>();
            try
            {
                var validator = new AllergenMasterValidator(localizer);
                var results = validator.Validate(allergenMasterModel, ruleSet: "New");

                if (results.IsValid)
                {
                    var isExists = allergenRepository.Any(x => x.AllergenName.ToLower().Trim() == allergenMasterModel.AllergenName.ToLower().Trim());
                    if (isExists)
                    {
                        response.Result = ResultType.Warning;
                        response.Message = localizer["msgDuplicateRecord", new string[] { "Allergen" }];
                    }
                    else
                    {
                        AllergenMaster allergenMaster = mapper.Map<AllergenMaster>(allergenMasterModel);
                        allergenMaster.IsActive = true;
                        allergenMaster.IsDeleted = false;
                        allergenMaster.CreatedBy = RequestUserID;
                        allergenMaster.CreatedOn = DateTime.Now;
                        allergenRepository.AddAsync(allergenMaster);
                        await Save();
                        response.Message = localizer["msgInsertSuccess", new string[] { "Allergen" }];
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

        public async Task<GeneralResponse<bool>> Update(AllergenMasterModel allergenMasterModel)
        {
            var response = new GeneralResponse<bool>();
            try
            {
                var validator = new AllergenMasterValidator(localizer);
                var results = validator.Validate(allergenMasterModel, ruleSet: "Edit,New");
                if (results.IsValid)
                {
                    // Unique validation 
                    var isExists = allergenRepository.Any(x => x.AllergenID != allergenMasterModel.AllergenID
                                                            && x.AllergenName.ToLower() == allergenMasterModel.AllergenName.ToLower().Trim());
                    if (isExists)
                    {
                        response.Result = ResultType.Warning;
                        response.Message = localizer["msgDuplicateRecord", new string[] { "Allergen" }];
                    }
                    else
                    {
                        var obj = await allergenRepository.GetByIdAsync(allergenMasterModel.AllergenID);
                        if (obj != null)
                        {
                            mapper.Map<AllergenMasterModel, AllergenMaster>(allergenMasterModel, obj);
                            obj.UpdatedBy = RequestUserID;
                            obj.UpdatedOn = DateTime.Now;
                            obj.IsDeleted = false;
                            obj.IsActive = true;
                            allergenRepository.UpdateAsync(obj);
                            await Save();
                            response.Message = localizer["msgUpdateSuccess", new string[] { "Allergen" }];
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
                var obj = await allergenRepository.GetByIdAsync(id);
                if (obj != null)
                {
                    if (!HasReference(id))
                    {
                        obj.UpdatedBy = RequestUserID;
                        obj.UpdatedOn = DateTime.Now;
                        obj.IsActive = false;
                        obj.IsDeleted = true;
                        allergenRepository.UpdateAsync(obj);
                        await Save();
                        response.Message = localizer["msgDeleteSuccess", new string[] { "Allergen" }];
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
                    response.Message = localizer["msgRecordNotExist", new string[] { "Allergen" }];
                }
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }

        public async Task<GeneralResponse<AllergenMaster>> Get(int id)
        {
            var response = new GeneralResponse<AllergenMaster>();
            try
            {
                var obj = await allergenRepository.GetByIdAsync(id);
                if (obj != null)
                {
                    response.Data = obj;
                }
                else
                {
                    response.Message = localizer["msgRecordNotExist", new string[] { "Allergen" }];
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
            var refData = allergenRepository.Query(true)
                                    .Where(o => o.AllergenID == Id)
                                    .Include(o => o.IngredientAllergenMapping);
            if (refData.FirstOrDefault().IngredientAllergenMapping.Any())
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
