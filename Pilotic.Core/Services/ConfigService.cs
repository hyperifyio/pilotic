using Pilotic.Core.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Pilotic.Core.Services;

public class ConfigService : IConfigService
{
    private readonly IConfiguration _configuration;

    public ConfigService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public T GetConfig<T>(string section) where T : new()
    {
        var config = new T();
        _configuration.GetSection(section).Bind(config);
        return config;
    }
}
