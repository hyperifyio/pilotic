using Microsoft.Extensions.DependencyInjection;

public static class ModuleLoader
{
    public static void RegisterModules<TInjectableClass>(IServiceCollection services)
        where TInjectableClass : class
    {
        services.Scan(scan => scan
            .FromApplicationDependencies()
            .AddClasses(classes => classes.AssignableTo<TInjectableClass>())
            .AsImplementedInterfaces()
            .WithSingletonLifetime());
    }
}
