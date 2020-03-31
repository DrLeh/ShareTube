using System;
using System.Collections.Generic;
using System.Text;

namespace ShareTube.Core.Configuration
{
    public interface IConfiguration
    {
        string? EnvironmentName { get; }
        string ConnectionString { get; }
    }
}
