using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Pilotic.Core.Services;

public class ModuleLoader
{
    private readonly IConfiguration _configuration;

    public ModuleLoader(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void RegisterModules<TInjectableClass>(IServiceCollection services)
        where TInjectableClass : class
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => !a.IsDynamic && !string.IsNullOrWhiteSpace(a.Location))
            .ToArray();

        var injectableTypes = assemblies
            .SelectMany(asm => asm.GetTypes())
            .Where(t => typeof(TInjectableClass).IsAssignableFrom(t)
                        && !t.IsAbstract && !t.IsInterface)
            .ToList();

        var dependencyMap = CreateDependencyMap(injectableTypes);

        var enabledSet = new HashSet<Type>();

        foreach (var type in injectableTypes)
        {
            if (IsExplicitlyEnabled(type))
                EnableTypeRecursively(type, dependencyMap, enabledSet);
        }

        // Register all enabled types explicitly with their interfaces.
        foreach (var implementationType in enabledSet)
        {
            var interfaces = implementationType.GetInterfaces()
                .Where(i => typeof(TInjectableClass).IsAssignableFrom(i));

            foreach (var serviceType in interfaces)
            {
                services.AddSingleton(serviceType, implementationType);
            }

            // Also register the class itself in case direct injections are needed
            services.AddSingleton(implementationType);
        }
    }

    private Dictionary<Type, List<Type>> CreateDependencyMap(IList<Type> allTypes)
    {
        var allTypeSet = new HashSet<Type>(allTypes);

        var dependencyMap = new Dictionary<Type, List<Type>>();

        foreach (var type in allTypes)
        {
            var dependencies = type.GetConstructors()
                .OrderByDescending(ctor => ctor.GetParameters().Length)
                .FirstOrDefault()?
                .GetParameters()
                .Select(p => p.ParameterType)
                .Where(t => t.IsInterface || allTypeSet.Contains(t))
                .ToList() ?? new List<Type>();

            dependencyMap[type] = dependencies;
        }

        return dependencyMap;
    }

    private void EnableTypeRecursively(Type type, Dictionary<Type, List<Type>> dependencyMap, HashSet<Type> enabledSet)
    {
        if (enabledSet.Contains(type) || IsExplicitlyDisabled(type))
            return;

        enabledSet.Add(type);

        if (!dependencyMap.TryGetValue(type, out var dependencies))
            return;

        foreach (var dep in dependencies)
        {
            // Find concrete implementations of interface dependencies
            var implementations = dependencyMap.Keys
                .Where(t => dep.IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface)
                .ToList();

            foreach (var implementation in implementations)
            {
                EnableTypeRecursively(implementation, dependencyMap, enabledSet);
            }
        }
    }

    private bool IsExplicitlyEnabled(Type type)
    {
        return _configuration.GetValue<bool?>($"Services:{type.FullName}:Enabled") == true;
    }

    private bool IsExplicitlyDisabled(Type type)
    {
        return _configuration.GetValue<bool?>($"Services:{type.FullName}:Enabled") == false;
    }
}
