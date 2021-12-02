using AutoMapper;
using FM21.Core;
using FM21.Core.Localization;
using FM21.Data.Infrastructure;
using FM21.Service.Caching;
using Microsoft.Extensions.Localization;
using System;
using System.Threading.Tasks;

namespace FM21.Service
{
    public abstract class BaseService : IBaseService
    {
        public readonly IServiceProvider provider;
        public readonly IExceptionHandler exceptionHandler;
        public readonly IStringLocalizer localizer;
        public readonly IMapper mapper;
        public readonly IUnitOfWork unitOfWork;
        public readonly ICacheProvider cacheProvider;

        protected BaseService(IServiceProvider provider, IExceptionHandler exceptionHandler, IMapper mapper, IUnitOfWork unitOfWork, ICacheProvider cacheProvider)
        {
            this.provider = provider;
            this.exceptionHandler = exceptionHandler;
            this.localizer = new JsonStringLocalizer();
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.cacheProvider = cacheProvider;
        }

        public async Task<int> Save()
        {
            return await unitOfWork.CommitAsync();
        }

        public int? RequestUserID { get { return ApplicationConstants.RequestUserID; } }

        public string RequestLanguage { get { return ApplicationConstants.RequestLanguage; } }
    }
}