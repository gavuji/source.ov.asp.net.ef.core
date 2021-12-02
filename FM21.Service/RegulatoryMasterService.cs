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
    public class RegulatoryMasterService : BaseService, IRegulatoryMasterService
    {
        private readonly IRepository<RegulatoryMaster> regulatoryMasterRepository;
        private readonly IRepository<NutrientMaster> nutrientRepository;
        private readonly IRepository<UnitOfMeasurementMaster> unitOfMeasurementRepository;

        public RegulatoryMasterService(IServiceProvider provider, IExceptionHandler exceptionHandler, IMapper mapper, IUnitOfWork unitOfWork, ICacheProvider cacheProvider,
            IRepository<RegulatoryMaster> regulatoryMasterRepository, IRepository<NutrientMaster> nutrientRepository,
            IRepository<UnitOfMeasurementMaster> unitOfMeasurementRepository)
            : base(provider, exceptionHandler, mapper, unitOfWork, cacheProvider)
        {
            this.regulatoryMasterRepository = regulatoryMasterRepository;
            this.nutrientRepository = nutrientRepository;
            this.unitOfMeasurementRepository = unitOfMeasurementRepository;
        }

        public async Task<GeneralResponse<ICollection<RegulatoryMaster>>> GetAll()
        {
            var response = new GeneralResponse<ICollection<RegulatoryMaster>>();
            try
            {
                response.Data = await regulatoryMasterRepository.GetManyAsync(o => o.IsActive && !o.IsDeleted, true);
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }

        public async Task<PagedEntityResponse<RegulatoryModel>> GetPageWiseData(string filter, int pageIndex, int pageSize, string sortColumn, string sortDirection)
        {
            var response = new PagedEntityResponse<RegulatoryModel>();
            try
            {
                var result = regulatoryMasterRepository.Query(true).Where(o => o.IsActive && !o.IsDeleted)
               .Include(x => x.NutrientMaster.UnitOfMeasurement).ToList();
          
                var data = mapper.Map<IList<RegulatoryMaster>, IList<RegulatoryModel>>(result).AsQueryable<RegulatoryModel>();

                var ascending = sortDirection == "asc";
                if (!string.IsNullOrWhiteSpace(sortColumn))
                {
                    switch (sortColumn.Trim().ToLower())
                    {
                        case "nutrient":
                            data = data.OrderBy(p => p.Nutrient, ascending);
                            break;
                        case "oldusa":
                            data = data.OrderBy(p => p.OldUsa, ascending);
                            break;
                        case "canadani":
                            data = data.OrderBy(p => p.CanadaNi, ascending);
                            break;
                        case "canadanf":
                            data = data.OrderBy(p => p.CanadaNf, ascending);
                            break;
                        case "newusrdi":
                            data = data.OrderBy(p => p.NewUsRdi, ascending);
                            break;
                        case "eu":
                            data = data.OrderBy(p => p.EU, ascending);
                            break;
                        case "unit":
                            data = data.OrderBy(p => p.Unit, ascending);
                            break;
                        case "unitpermg":
                            data = data.OrderBy(p => p.UnitPerMg, ascending);
                            break;

                        default:
                            data = data.OrderBy(p => p.Nutrient, ascending);
                            break;
                    }
                }
                else
                {
                    data = data.OrderBy(p => p.RegulatoryId, ascending);
                }

                response = await data.GetPaged(pageIndex, pageSize);
                response.ExtraData = GetAllNutrientMaster(response);
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }

        public async Task<GeneralResponse<RegulatoryMaster>> Get(int id)
        {
            var response = new GeneralResponse<RegulatoryMaster>();
            try
            {
                var obj = await regulatoryMasterRepository.GetByIdAsync(id);
                if (obj != null)
                {
                    response.Data = obj;
                }
                else
                {
                    response.Message = localizer["msgRecordNotExist", new string[] { "Regulatory" }];
                }
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }

        public async Task<GeneralResponse<bool>> Create(RegulatoryModel entity)
        {
            var response = new GeneralResponse<bool>();
            try
            {
                var validator = new RegulatoryMasterValidator(localizer);
                var results = validator.Validate(entity, ruleSet: "New");
                if (results.IsValid)
                {
                    // Unique validation 
                    if (IsRegulatoryNutrientExist(entity))
                    {
                        response.Result = ResultType.Warning;
                        response.Message = localizer["msgDuplicateRecord", new string[] { "Regulatory" }];
                    }
                    else
                    {
                        RegulatoryMaster regulatoryMaster = mapper.Map<RegulatoryMaster>(entity);
                        regulatoryMaster.IsActive = true;
                        regulatoryMaster.IsDeleted = false;
                        regulatoryMaster.CreatedBy = RequestUserID;
                        regulatoryMasterRepository.AddAsync(regulatoryMaster);
                        await Save();
                        response.Message = localizer["msgInsertSuccess", new string[] { "Regulatory" }];
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

        public async Task<GeneralResponse<bool>> Update(RegulatoryModel entity)
        {
            var response = new GeneralResponse<bool>();
            try
            {
                var validator = new RegulatoryMasterValidator(localizer);
                var results = validator.Validate(entity, ruleSet: "Edit,New");
                if (results.IsValid)
                {

                    var obj = await regulatoryMasterRepository.GetByIdAsync(entity.RegulatoryId);
                    if (obj != null && obj.IsActive && !obj.IsDeleted)
                    {
                        if (entity.NutrientId == obj.NutrientId || !IsRegulatoryNutrientExist(entity))
                        {
                            mapper.Map<RegulatoryModel, RegulatoryMaster>(entity, obj);
                            obj.UpdatedOn = DateTime.Now;
                            obj.UpdatedBy = RequestUserID;
                            obj.IsDeleted = false;
                            obj.IsActive = true;

                            regulatoryMasterRepository.UpdateAsync(obj);
                            await Save();
                            response.Message = localizer["msgUpdateSuccess", new string[] { "Regulatory" }];
                        }
                        else
                        {
                            response.Result = ResultType.Warning;
                            response.Message = localizer["msgDuplicateRecord", new string[] { "Regulatory" }];
                        }
                    }
                    else
                    {
                        response.Result = ResultType.Warning;
                        response.Message = localizer["msgRecordNotExist", new string[] { "Regulatory" }];
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
                var obj = await regulatoryMasterRepository.GetByIdAsync(id);
                if (obj != null)
                {
                    obj.UpdatedBy = RequestUserID;
                    obj.UpdatedOn = DateTime.Now;
                    obj.IsActive = false;
                    obj.IsDeleted = true;
                    regulatoryMasterRepository.UpdateAsync(obj);
                    await Save();
                    response.Message = localizer["msgDeleteSuccess", new string[] { "Regulatory" }];
                }
                else
                {
                    response.Result = ResultType.Warning;
                    response.Message = localizer["msgRecordNotExist", new string[] { "Regulatory" }];
                }
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }
        public async Task<GeneralResponse<ICollection<UnitOfMeasurementMaster>>> GetUnitOfMeasurement()
        {
            var response = new GeneralResponse<ICollection<UnitOfMeasurementMaster>>();
            try
            {
                response.Data = await unitOfMeasurementRepository.GetManyAsync(o => o.IsActive && !o.IsDeleted, true);
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }

        private List<KeyValuePair<string, string>> GetAllNutrientMaster(PagedEntityResponse<RegulatoryModel> response)
        {
            response.ExtraData = new List<KeyValuePair<string, string>>();
            var data = nutrientRepository.Query(true).Where(x => x.IsActive && !x.IsDeleted ).OrderBy(o => o.Name).ToList();
            foreach (var item in data)
            {
                response.ExtraData.Add(new KeyValuePair<string, string>(item.NutrientID.ToString(), item.Name));
            }

            return response.ExtraData;
        }

        private bool IsRegulatoryNutrientExist(RegulatoryModel entity)
        {
            var isExists = regulatoryMasterRepository.Any(x => x.NutrientId == entity.NutrientId && x.IsActive && !x.IsDeleted);
            return isExists;
        }
    }
}