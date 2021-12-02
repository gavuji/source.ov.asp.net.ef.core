using AutoMapper;
using FM21.Core;
using FM21.Core.Localization;
using FM21.Data.Infrastructure;
using FM21.Service.Caching;
using Microsoft.Extensions.Localization;
using Moq;
using NUnit.Framework;
using System;

namespace FM21.Tests
{
    [TestFixture]
    public class TestBase
    {
        public Mock<IServiceProvider> serviceProvider;
        public Mock<IExceptionHandler> exceptionHandler;
        public IStringLocalizer localizer;
        public IMapper mapper;
        public Mock<IUnitOfWork> unitOfWork;
        public Mock<ICacheProvider> cacheProvider;

        public TestBase()
        {
            serviceProvider = new Mock<IServiceProvider>();
            exceptionHandler = new Mock<IExceptionHandler>();
            localizer = new JsonStringLocalizer();
            mapper = new MapperConfiguration(cfg => { cfg.AddProfile(new MappingProfile()); }).CreateMapper();
            unitOfWork = new Mock<IUnitOfWork>();
            cacheProvider = new Mock<ICacheProvider>();
            ApplicationConstants.RequestUserID = 999;
            ApplicationConstants.RequestLanguage = "en-us";
        }
    }
}