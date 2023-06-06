using Microsoft.Extensions.Configuration;
using System;
using Xunit;

namespace DaXiong.Tools.Xunit
{
    public class Configuration
    {
        IConfiguration configuration;
        public Configuration()
        {
            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddConfigurationFolder();
            configuration = configurationBuilder.Build();
        }

        [Fact]
        public void GetFolderConfiguration()
        {
  
            var appSecret = configuration.GetSection("AppConfig")["AppSecret"];
            Assert.NotNull(appSecret);
        }
    }
}
