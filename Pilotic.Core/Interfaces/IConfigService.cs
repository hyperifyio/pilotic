namespace Pilotic.Core.Interfaces;

public interface IConfigService : IInjectableSingletonModule
{
    T GetConfig<T>(string section) where T : new();
}
