using FM21.Core;
using FM21.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;

namespace FM21.Tests
{
    [TestFixture]
    public class StartUpUnitTest
    {
        private IWebHost webHost;

        [SetUp]
        public void SetUp()
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
            webHost = Microsoft.AspNetCore.WebHost.CreateDefaultBuilder()
                .UseStartup<Startup>()
                .UseDefaultServiceProvider(options => options.ValidateScopes = false)
                .Build();
        }

        [Test]
        public void API_Should_Return_Valid_WebHost_On_StartUp_Build()
        {
            Assert.IsNotNull(webHost);
        }

        [Test]
        public void Can_load_ConnectionString_From_ConfigurationSetting()
        {
            var connectionString = SecurityProvider.Encrypt(ApplicationConstants.DbConnectionString);
            Assert.IsNotNull(connectionString);
        }

        [Test]
        public void Web_Host_Should_Return_Injected_Service_Object_Of_Type()
        {
            Assert.IsNotNull(webHost.Services.GetRequiredService<ICustomerService>());
            Assert.IsNotNull(webHost.Services.GetRequiredService<IRegulatoryMasterService>());
        }

        [Test]
        public void Service_API_Should_Return_Encrypted_ConnectionString_From_Configuration()
        {
            var encryptedText = SecurityProvider.Encrypt(ApplicationConstants.DbConnectionString);
            var decryptedText = SecurityProvider.Decrypt(encryptedText);
            Assert.IsNotNull(encryptedText);
            Assert.IsNotNull(decryptedText);
            Assert.AreEqual(ApplicationConstants.DbConnectionString, decryptedText);
        }
    }

    public class Startup : FM21.API.Startup
    {
        public Startup(IConfiguration config) : base(config) { }
    }
}