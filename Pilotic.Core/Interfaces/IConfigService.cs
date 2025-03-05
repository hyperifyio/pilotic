namespace Pilotic.Core.Interfaces;

public interface IConfigService : IInjectableModule
{
    T GetConfig<T>(string section) where T : new();
}
