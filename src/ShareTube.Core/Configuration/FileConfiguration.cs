using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ShareTube.Core.Configuration
{
    public class FileConfiguration : IConfiguration
    {
        protected IConfigurationRoot config;

        public FileConfiguration()
        {
            config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        }

        public string? EnvironmentName => config[nameof(EnvironmentName)];
        public string ConnectionString => config.GetConnectionString("ShareTubeConnection");
    }
}
