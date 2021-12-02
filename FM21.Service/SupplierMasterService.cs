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
    public class SupplierMasterService : BaseService, ISupplierMasterService
    {
        private readonly IRepository<SupplierMaster> supplierMasterRepository;

        public SupplierMasterService(IServiceProvider provider, IExceptionHandler exceptionHandler, IMapper mapper, IUnitOfWork unitOfWork, ICacheProvider cacheProvider,
             IRepository<SupplierMaster> supplierMasterRepository)
            : base(provider, exceptionHandler, mapper, unitOfWork, cacheProvider)
        {
            this.supplierMasterRepository = supplierMasterRepository;
        }

        public async Task<GeneralResponse<bool>> Create(SupplierMasterModel supplierMasterModel)
        {
            var response = new GeneralResponse<bool>();
            try
            {
                var validator = new SupplierMasterValidator(localizer);
                var results = validator.Validate(supplierMasterModel, ruleSet: "New");

                if (results.IsValid)
                {
                    var isExists = supplierMasterRepository.Any(x => x.SupplierName.ToLower().Trim() == supplierMasterModel.SupplierName.ToLower().Trim());
                    if (isExists)
                    {
                        response.Result = ResultType.Warning;
                        response.Message = localizer["msgDuplicateRecord", new string[] { "Supplier" }];
                    }
                    else
                    {
                        SupplierMaster supplierMaster = mapper.Map<SupplierMaster>(supplierMasterModel);
                        supplierMaster.IsActive = true;
                        supplierMaster.IsDeleted = false;
                        supplierMaster.CreatedBy = RequestUserID;
                        supplierMaster.CreatedOn = DateTime.Now;
                        supplierMasterRepository.AddAsync(supplierMaster);
                        await Save();
                        response.Message = localizer["msgInsertSuccess", new string[] { "Supplier" }];
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
                var obj = await supplierMasterRepository.GetByIdAsync(id);
                if (obj != null)
                {
                    // check has reference 
                    if (!HasReference(id))
                    {
                        obj.UpdatedBy = RequestUserID;
                        obj.UpdatedOn = DateTime.Now;
                        obj.IsActive = false;
                        obj.IsDeleted = true;
                        supplierMasterRepository.UpdateAsync(obj);
                        await Save();
                        response.Message = localizer["msgDeleteSuccess", new string[] { "Supplier" }];
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
                    response.Message = localizer["msgRecordNotExist", new string[] { "Supplier" }];
                }
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }

        public async Task<GeneralResponse<SupplierMaster>> Get(int id)
        {
            var response = new GeneralResponse<SupplierMaster>();
            try
            {
                var obj = await supplierMasterRepository.GetByIdAsync(id);
                if (obj != null)
                {
                    response.Data = obj;
                }
                else
                {
                    response.Message = localizer["msgRecordNotExist", new string[] { "Supplier" }];
                }
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }

        public async Task<GeneralResponse<ICollection<SupplierMaster>>> GetAll()
        {
            var response = new GeneralResponse<ICollection<SupplierMaster>>();
            try
            {
                response.Data = (await supplierMasterRepository.GetManyAsync(o => o.IsDeleted == false && o.IsActive == true, true))
                                            .OrderBy(o => o.SupplierName).ToList();
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }

        public async Task<PagedEntityResponse<SupplierMaster>> GetPageWiseData(string filter, int pageIndex, int pageSize, string sortColumn, string sortDirection)
        {
            filter = filter?.Trim().ToLower();
            var response = new PagedEntityResponse<SupplierMaster>();
            try
            {
                var data = supplierMasterRepository.Query(true);
                //Filter
                if (!string.IsNullOrWhiteSpace(filter))
                {
                    data = data.Where(x => x.SupplierName.ToLower().Contains(filter) || (x.Address ?? string.Empty).ToLower().Contains(filter) ||
                    (x.PhoneNumber ?? string.Empty).Contains(filter) || (x.SupplierAbbreviation1 ?? string.Empty).ToLower().Contains(filter)
                    || (x.SupplierAbbreviation2 ?? string.Empty).ToLower().Contains(filter) || (x.Email ?? string.Empty).ToLower().Contains(filter));
                }
                //sort 
                var ascending = (sortDirection == "asc");
                if (!string.IsNullOrWhiteSpace(sortColumn))
                {
                    switch (sortColumn.Trim().ToLower())
                    {
                        case "suppliername":
                            data = data.OrderBy(p => p.SupplierName, ascending);
                            break;
                        case "address":
                            data = data.OrderBy(p => p.Address, ascending);
                            break;
                        case "email":
                            data = data.OrderBy(p => p.Email, ascending);
                            break;
                        case "phonenumber":
                            data = data.OrderBy(p => p.PhoneNumber, ascending);
                            break;
                        case "supplierabbreviation1":
                            data = data.OrderBy(p => p.SupplierAbbreviation1, ascending);
                            break;
                        case "supplierabbreviation2":
                            data = data.OrderBy(p => p.SupplierAbbreviation2, ascending);
                            break;
                        default:
                            data = data.OrderBy(p => p.SupplierID, ascending);
                            break;
                    }
                }
                else
                {
                    data = data.OrderBy(p => p.SupplierID, ascending);
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

        public async Task<GeneralResponse<bool>> Update(SupplierMasterModel supplierMasterModel)
        {
            var response = new GeneralResponse<bool>();
            try
            {
                var validator = new SupplierMasterValidator(localizer);
                var results = validator.Validate(supplierMasterModel, ruleSet: "Edit,New");
                if (results.IsValid)
                {
                    // Unique validation 
                    var isExists = supplierMasterRepository.Any(x => x.SupplierID != supplierMasterModel.SupplierId
                                                            && x.SupplierName.ToLower() == supplierMasterModel.SupplierName.ToLower().Trim());
                    if (isExists)
                    {
                        response.Result = ResultType.Warning;
                        response.Message = localizer["msgDuplicateRecord", new string[] { "Supplier" }];
                    }
                    else
                    {
                        var obj = await supplierMasterRepository.GetByIdAsync(supplierMasterModel.SupplierId);
                        if (obj != null)
                        {
                            mapper.Map<SupplierMasterModel, SupplierMaster>(supplierMasterModel, obj);
                            obj.UpdatedBy = RequestUserID;
                            obj.UpdatedOn = DateTime.Now;
                            obj.IsDeleted = false;
                            obj.IsActive = true;
                            supplierMasterRepository.UpdateAsync(obj);

                            await Save();
                            response.Message = localizer["msgUpdateSuccess", new string[] { "Supplier" }];
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

        public bool HasReference(int Id)
        {
            var refData = supplierMasterRepository.Query(true)
                                    .Where(o => o.SupplierID == Id)
                                    .Include(o => o.IngredientSupplierMapping);
            if (refData.FirstOrDefault().IngredientSupplierMapping.Any())
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