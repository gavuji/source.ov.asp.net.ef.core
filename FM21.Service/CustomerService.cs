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
    public class CustomerService : BaseService, ICustomerService
    {
        private readonly IRepository<Customer> customerRepository;
        
        public CustomerService(IServiceProvider provider, IExceptionHandler exceptionHandler, IMapper mapper, IUnitOfWork unitOfWork, ICacheProvider cacheProvider,
            IRepository<Customer> customerRepository)
            : base(provider, exceptionHandler, mapper, unitOfWork, cacheProvider)
        {
            this.customerRepository = customerRepository;
        }

      
        public async Task<GeneralResponse<ICollection<Customer>>> GetAll()
        {
            var response = new GeneralResponse<ICollection<Customer>>();
            try
            {
                response.Data = (await customerRepository.GetManyAsync(o => o.IsActive == true && o.IsDeleted == false, true)).OrderBy(o => o.Name).ToList();
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }

        public async Task<PagedEntityResponse<Customer>> GetPageWiseData(string filter, int pageIndex, int pageSize, string sortColumn, string sortDirection)
        {
            var response = new PagedEntityResponse<Customer>();
            try
            {
                var data = customerRepository.Query(true).Where(o => o.IsActive == true && o.IsDeleted == false);
                //Filter
                if (!string.IsNullOrWhiteSpace(filter))
                {
                    data = data.Where(x => x.Name.ToLower().Contains(filter) || x.Address.ToLower().Contains(filter)
                                      || x.PhoneNumber.Contains(filter) || x.CustomerAbbreviation1.Contains(filter) || x.CustomerAbbreviation2.Contains(filter)
                                      || x.Email.Contains(filter));
                }
                //sort 
                var ascending = sortDirection == "asc";
                if (!string.IsNullOrWhiteSpace(sortColumn))
                {
                    switch (sortColumn.Trim().ToLower())
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          {
                        case "name":
                            data = data.OrderBy(p => p.Name, ascending);
                            break;
                        case "address":
                            data = data.OrderBy(p => p.Address, ascending);
                            break;
                        case "email":
                            data = data.OrderBy(p => p.Email, ascending);
                            break;
                        case "customerabbreviation1":
                            data = data.OrderBy(p => p.CustomerAbbreviation1, ascending);
                            break;
                        case "customerabbreviation2":
                            data = data.OrderBy(p => p.CustomerAbbreviation2, ascending);
                            break;
                        case "phonenumber":
                            data = data.OrderBy(p => p.PhoneNumber, ascending);
                            break;
                        default:
                            data = data.OrderBy(p => p.Name, ascending);
                            break;
                    }
                }
                else
                {
                    data = data.OrderBy(p => p.CustomerId, ascending);
                }
                //Page wise
                response = await data.GetPaged(pageIndex, pageSize);
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }

        public async Task<GeneralResponse<Customer>> Get(int id)
        {
            var response = new GeneralResponse<Customer>();
            try
            {
                var obj = await customerRepository.GetByIdAsync(id);
                if (obj != null)
                {
                    response.Data = obj;
                }
                else
                {
                    response.Message = localizer["msgRecordNotExist", new string[] { "Customer" }];
                }
            }
            catch (Exception ex)
            {
                response.Exception = ex;
                exceptionHandler.LogError(ex);
            }
            return response;
        }

        public async Task<GeneralResponse<bool>> Create(CustomerModel entity)
        {
            var response = new GeneralResponse<bool>();
            try
            {
                var validator = new CustomerValidator(localizer);
                var results = validator.Validate(entity, ruleSet: "New");

                if (results.IsValid)
                {
                    // Unique validation 
                    var isExists = customerRepository.Any(x => x.Name.ToLower().Trim() == entity.Name.ToLower().Trim()
                                                         || x.Email.ToLower().Trim() == entity.Email.ToLower().Trim()
                                                         || x.PhoneNumber.Trim() == entity.PhoneNumber.Trim());
                    if (isExists)
                    {
                        response.Result = ResultType.Warning;
                        response.Message = localizer["msgDuplicateRecord", new string[] { "Customer" }];
                    }
                    else
                    {
                        Customer objcustomer = mapper.Map<Customer>(entity);
                        objcustomer.IsActive = true;
                        objcustomer.IsDeleted = false;
                        objcustomer.CreatedBy = RequestUserID;
                        customerRepository.AddAsync(objcustomer);
                        await Save();
                        response.Message = localizer["msgInsertSuccess", new string[] { "Customer" }];
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

        public async Task<GeneralResponse<bool>> Update(CustomerModel entity)
        {
            var response = new GeneralResponse<bool>();
            try
            {
                var validator = new CustomerValidator(localizer);
                var results = validator.Validate(entity, ruleSet: "Edit,New");

                if (results.IsValid)
                {
                    // Unique validation 
                    var isExists = customerRepository.Any(x => x.CustomerId != entity.CustomerId
                                                            && (x.Name.ToLower() == entity.Name.ToLower().Trim() || x.Email.ToLower().Trim() == entity.Email.ToLower().Trim()
                                                            || x.PhoneNumber.Trim() == entity.PhoneNumber.Trim()));
                    if (isExists)
                    {
                        response.Result = ResultType.Warning;
                        response.Message = localizer["msgDuplicateRecord", new string[] { "Customer" }];
                    }
                    else
                    {
                        var obj = await customerRepository.GetByIdAsync(entity.CustomerId);
                        if (obj != null)
                        {
                            mapper.Map<CustomerModel, Customer>(entity, obj);
                            obj.UpdatedBy = RequestUserID;
                            obj.UpdatedOn = DateTime.Now;
                            obj.IsDeleted = false;
                            obj.IsActive = true;
                            customerRepository.UpdateAsync(obj);
                            await Save();
                            response.Message = localizer["msgUpdateSuccess", new string[] { "Customer" }];
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
                var obj = await customerRepository.GetByIdAsync(id);
                if (obj != null)
                {
                    obj.UpdatedBy = RequestUserID;
                    obj.UpdatedOn = DateTime.Now;
                    obj.IsActive = false;
                    obj.IsDeleted = true;
                    customerRepository.UpdateAsync(obj);
                    await Save();
                    response.Message = localizer["msgDeleteSuccess", new string[] { "Customer" }];
                }
                else
                {
                    response.Result = ResultType.Warning;
                    response.Message = localizer["msgRecordNotExist", new string[] { "Customer" }];
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