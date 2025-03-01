using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Pilotic.API.Modules.GitHub.Interfaces;
using Pilotic.API.Modules.GitHub.Services;
using Pilotic.Core.Interfaces;

namespace Pilotic.Tests.Core.Services;

public class ModuleLoaderTests
{
    [Fact]
    public void Should_Register_All_Modules_Automatically()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        ModuleLoader.RegisterModules<IInjectableModule>(services);

        // Build service provider to test resolution
        var provider = services.BuildServiceProvider();
        var registeredModules = provider.GetServices<IInjectableModule>();

        // Assert
        Assert.NotEmpty(registeredModules); // Ensure at least one module is registered
        Assert.Contains(registeredModules, m => m is GitHubService);
    }
    
    
    public class DependencyInjectionTests
    {
        private readonly ServiceProvider _provider;

        public DependencyInjectionTests()
        {
            var services = new ServiceCollection();
            ModuleLoader.RegisterModules<IInjectableModule>(services);
            _provider = services.BuildServiceProvider();
        }

        [Fact]
        public void Should_Resolve_GitHubService_From_Interface()
        {
            var service = _provider.GetService<IGitHubService>();
            Assert.NotNull(service);
        }
    }

    public class ConstructorInjectionTests
    {
        [Fact]
        public void Should_Inject_Dependency_Into_Consumer()
        {
            var services = new ServiceCollection();
            ModuleLoader.RegisterModules<IInjectableModule>(services);
            services.AddSingleton<GitHubIssueManager>(); // Register a dependent service

            var provider = services.BuildServiceProvider();
            var manager = provider.GetService<GitHubIssueManager>();

            Assert.NotNull(manager);
        }
    }

    
}
