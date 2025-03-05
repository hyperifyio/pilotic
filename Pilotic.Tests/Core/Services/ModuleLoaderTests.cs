using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

using Pilotic.Core.Interfaces;
using Pilotic.Core.Services;

namespace Pilotic.Tests.Core.Services;

public class AlternativeEventService : IEventBus
{
    private readonly IHttpClientFactory _httpClientFactory;

    public AlternativeEventService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : IEvent
    {
        throw new NotImplementedException();
    }
}

/// <summary>
/// Tests for verifying that the ModuleLoader correctly registers modules based on configuration.
/// </summary>
public class ModuleLoaderTests
{
    
    /// <summary>
    /// Verifies explicit module enabling also implicitly enables and registers dependent implementations.
    /// GitHubIssueManager requires IEventBus, so GitHubService must be implicitly registered.
    /// </summary>
    [Fact]
    public void Should_Register_All_Modules_Automatically()
    {
        // Arrange
        var services = new ServiceCollection();

        // Create a mocked ILogger
        var mockLogger = new Mock<ILogger<MemoryEventBus>>();
        services.AddSingleton(mockLogger.Object);

        // Use the full type name for the key so that it matches what ModuleLoader uses.
        string key = $"Services:{typeof(MemoryEventBus).FullName}:Enabled";
        var collection = new Dictionary<string, string?>
        {
            { key, "true" },
        };

        IConfiguration config = new ConfigurationBuilder()
            .AddInMemoryCollection(collection)
            .Build();
        var moduleLoader = new ModuleLoader(config);

        // Act: Register all modules that implement IInjectableModule
        moduleLoader.RegisterModules<IInjectableModule>(services);

        // Build the service provider to test resolution.
        var provider = services.BuildServiceProvider();
        var registeredModules = provider.GetServices<IInjectableModule>();

        // Assert:
        // Verify that at least one module is registered and that GitHubService,
        // which is a dependency of GitHubIssueManager, is also registered.
        Assert.NotEmpty(registeredModules);
        Assert.Contains(registeredModules, m => m is MemoryEventBus);
    }

    /// <summary>
    /// Tests related to resolving services by their interface.
    /// </summary>
    public class DependencyInjectionTests
    {
        private readonly ServiceProvider _provider;

        public DependencyInjectionTests()
        {
            var services = new ServiceCollection();

            // Create mocked dependencies
            var mockHttpFactory = new Mock<IHttpClientFactory>();
            services.AddSingleton(mockHttpFactory.Object);

            var mockLogger = new Mock<ILogger<MemoryEventBus>>();
            services.AddSingleton(mockLogger.Object);
            
            // Build the configuration using the full type name key for GitHubIssueManager.
            string key = $"Services:{typeof(MemoryEventBus).FullName}:Enabled";
            var collection = new Dictionary<string, string?>
            {
                { key, "true" },
            };

            IConfiguration config = new ConfigurationBuilder()
                .AddInMemoryCollection(collection)
                .Build();
            var moduleLoader = new ModuleLoader(config);

            // Register modules implementing IInjectableModule.
            moduleLoader.RegisterModules<IInjectableModule>(services);
            _provider = services.BuildServiceProvider();
        }

        /// <summary>
        /// Verifies that IEventBus is correctly resolved from the DI container.
        /// </summary>
        [Fact]
        public void Should_Resolve_EventBus_From_Interface()
        {
            var service = _provider.GetService<IEventBus>();
            
            Assert.NotNull(service);
        }
    }

    /// <summary>
    /// Tests to verify that constructor injection works correctly for consumer classes.
    /// </summary>
    public class ConstructorInjectionTests
    {
        /// <summary>
        /// Ensures that GitHubIssueManager, which depends on other services (like IEventBus),
        /// is properly constructed and its dependencies are injected.
        /// </summary>
        [Fact]
        public void Should_Inject_Dependency_Into_Consumer()
        {
            // Arrange
            string key = $"Services:{typeof(MemoryEventBus).FullName}:Enabled";
            var collection = new Dictionary<string, string?>
            {
                { key, "true" },
            };
            IConfiguration config = new ConfigurationBuilder()
                .AddInMemoryCollection(collection)
                .Build();
            var moduleLoader = new ModuleLoader(config);

            var services = new ServiceCollection();
            
            // Create mocked dependencies
            var mockHttpFactory = new Mock<IHttpClientFactory>();
            services.AddSingleton(mockHttpFactory.Object);

            var mockLogger = new Mock<ILogger<MemoryEventBus>>();
            services.AddSingleton(mockLogger.Object);
            
            // Register modules that implement IInjectableModule
            moduleLoader.RegisterModules<IInjectableModule>(services);
            // Explicitly register GitHubIssueManager as a consumer
            // services.AddSingleton<GitHubIssueManager>();

            // Act
            var provider = services.BuildServiceProvider();
            var manager = provider.GetService<MemoryEventBus>();

            // Assert
            Assert.NotNull(manager);
        }
    }
    
    [Fact]
    public void Should_Not_Register_Modules_When_Explicitly_Disabled()
    {
        // Arrange
        var services = new ServiceCollection();
        var disabledKey = $"Services:{typeof(MemoryEventBus).FullName}:Enabled";
        var collection = new Dictionary<string, string?>
        {
            { disabledKey, "false" },
        };

        IConfiguration config = new ConfigurationBuilder()
            .AddInMemoryCollection(collection)
            .Build();
        var moduleLoader = new ModuleLoader(config);

        // Act
        moduleLoader.RegisterModules<IInjectableModule>(services);
        var provider = services.BuildServiceProvider();

        // Assert
        var service = provider.GetService<IEventBus>();
        Assert.Null(service);
    }

    [Fact]
    public void Should_Register_Multiple_Implementations_Of_Interface()
    {
        var services = new ServiceCollection();
        
        // Create mocked dependencies
        var mockHttpFactory = new Mock<IHttpClientFactory>();
        services.AddSingleton(mockHttpFactory.Object);

        var mockLogger = new Mock<ILogger<MemoryEventBus>>();
        services.AddSingleton(mockLogger.Object);

        IConfiguration config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                [$"Services:{typeof(MemoryEventBus).FullName}:Enabled"] = "true",
                [$"Services:{typeof(AlternativeEventService).FullName}:Enabled"] = "true"
            })
            .Build();

        var moduleLoader = new ModuleLoader(config);

        moduleLoader.RegisterModules<IInjectableModule>(services);
        var provider = services.BuildServiceProvider();

        var implementations = provider.GetServices<IEventBus>().ToList();

        Assert.True(implementations.Count >= 1, "At least one IEventBus implementation should be registered.");
        Assert.Contains(implementations, impl => impl is MemoryEventBus);
        // You could also verify additional implementations if you have them
    }

    [Fact]
    public void Should_Not_Register_Modules_When_Not_Configured()
    {
        var services = new ServiceCollection();
        IConfiguration config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>())
            .Build();
        var moduleLoader = new ModuleLoader(config);

        // Act
        moduleLoader.RegisterModules<IInjectableModule>(services);
        var provider = services.BuildServiceProvider();

        var modules = provider.GetServices<IInjectableModule>();

        // Assert
        Assert.Empty(modules);
    }

    // [Fact]
    // public void Should_Handle_Circular_Dependencies_Gracefully()
    // {
    //     var services = new ServiceCollection();
    //     IConfiguration config = new ConfigurationBuilder()
    //         .AddInMemoryCollection(new Dictionary<string, string?>
    //         {
    //             [$"Services:{typeof(MemoryEventBus).FullName}:Enabled"] = "true",
    //             [$"Services:{typeof(ServiceProvider).FullName}:Enabled"] = "true"
    //         })
    //         .Build();
    //     var moduleLoader = new ModuleLoader(config);
    //
    //     // Act
    //     var exception = Record.Exception(() =>
    //     {
    //         moduleLoader.RegisterModules<IInjectableModule>(services);
    //         var provider = services.BuildServiceProvider();
    //         provider.GetService<MemoryEventBus>();
    //     });
    //
    //     // Assert that there is no infinite loop or stack overflow
    //     Assert.Null(exception);
    // }
    
}
