# Documentation for Developers

## Pilotic API

### Interfaces

1. **`IConfigService`**
   - **Namespace**: `Pilotic.Core.Interfaces`
   - **Description**: Provides configuration services.
   - **Methods**:
     - `T GetConfig<T>(string section) where T : new();` - Retrieves configuration section as a strongly-typed object.

2. **`IEvent`**
   - **Namespace**: `Pilotic.Core.Interfaces`
   - **Description**: Marker interface for events.

3. **`IEventBus`**
   - **Namespace**: `Pilotic.Core.Interfaces`
   - **Description**: Event bus for publishing events.
   - **Methods**:
     - `Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : IEvent;` - Publishes an event asynchronously.

4. **`IEventHandler<TEvent>`**
   - **Namespace**: `Pilotic.Core.Interfaces`
   - **Description**: Handles events of type `TEvent`.
   - **Methods**:
     - `Task HandleAsync(TEvent @event, CancellationToken cancellationToken = default);` - Handles the event asynchronously.

5. **`IInjectableModule`**
   - **Namespace**: `Pilotic.Core.Interfaces`
   - **Description**: Marker interface for injectable modules.

6. **`IIssueService`**
   - **Namespace**: `Pilotic.API.Modules.Issues.Interfaces`
   - **Description**: Provides issue-related services.
   - **Methods**:
     - `Task<Issue> RefineIssue(string issueId);` - Refines an issue by its ID.

7. **`IIssueRepository`**
   - **Namespace**: `Pilotic.API.Modules.TicketRepository.Interfaces`
   - **Description**: Repository for managing issues.
   - **Methods**:
     - `Task<Issue?> GetById(string id, CancellationToken cancellationToken = default);` - Retrieves an issue by its ID.
     - `Task Update(Issue issue, CancellationToken cancellationToken = default);` - Updates an issue.
     - `Task Add(Issue issue, CancellationToken cancellationToken = default);` - Adds a new issue.
     - `Task Delete(string id, CancellationToken cancellationToken = default);` - Deletes an issue by its ID.

8. **`IGitHubIssueRepository`**
   - **Namespace**: `Pilotic.API.Modules.GitHub.Interfaces`
   - **Description**: Repository for managing GitHub issues.
   - **Inherits**: `IIssueRepository`

9. **`IGitHubService`**
   - **Namespace**: `Pilotic.API.Modules.GitHub.Interfaces`
   - **Description**: Service for interacting with GitHub.

### Implementations

1. **`ConfigService`**
   - **Namespace**: `Pilotic.Core.Services`
   - **Implements**: `IConfigService`
   - **Description**: Provides configuration services using `IConfiguration`.
   - **Methods**:
     - `T GetConfig<T>(string section) where T : new();` - Retrieves configuration section as a strongly-typed object.

2. **`MemoryEventBus`**
   - **Namespace**: `Pilotic.Core.Services`
   - **Implements**: `IEventBus`
   - **Description**: In-memory event bus for publishing events.
   - **Methods**:
     - `Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : IEvent;` - Publishes an event asynchronously.

3. **`ModuleLoader`**
   - **Namespace**: `Pilotic.Core.Services`
   - **Description**: Loads and registers injectable modules.
   - **Methods**:
     - `void RegisterModules<TInjectableClass>(IServiceCollection services) where TInjectableClass : class;` - Registers modules implementing `TInjectableClass`.

4. **`IssueService`**
   - **Namespace**: `Pilotic.API.Modules.Issues.Services`
   - **Implements**: `IIssueService`
   - **Description**: Provides issue-related services.
   - **Methods**:
     - `Task<Issue> RefineIssue(string ticketId);` - Refines an issue by its ID.

5. **`MarkdownRefineIssueHandler`**
   - **Namespace**: `Pilotic.API.Events.RefineIssue`
   - **Implements**: `IEventHandler<RefineIssueEvent>`
   - **Description**: Handles `RefineIssueEvent` to refine issue descriptions.
   - **Methods**:
     - `Task HandleAsync(RefineIssueEvent @event, CancellationToken cancellationToken = default);` - Handles the event asynchronously.

6. **`GitHubIssueRepository`**
   - **Namespace**: `Pilotic.API.Modules.GitHub.Services`
   - **Implements**: `IGitHubIssueRepository`
   - **Description**: Repository for managing GitHub issues.
   - **Methods**:
     - `Task<Issue?> GetById(string id, CancellationToken cancellationToken = default);` - Retrieves an issue by its ID.
     - `Task Update(Issue issue, CancellationToken cancellationToken = default);` - Updates an issue.
     - `Task Add(Issue issue, CancellationToken cancellationToken = default);` - Adds a new issue.
     - `Task Delete(string id, CancellationToken cancellationToken = default);` - Deletes an issue by its ID.

7. **`GitHubService`**
   - **Namespace**: `Pilotic.API.Modules.GitHub.Services`
   - **Implements**: `IGitHubService`
   - **Description**: Service for interacting with GitHub.
   - **Methods**:
     - `void SyncIssues();` - Syncs issues from GitHub.

8. **`IssueRepository`**
   - **Namespace**: `Pilotic.API.Modules.IssueRepository.Services`
   - **Implements**: `IIssueRepository`
   - **Description**: Repository for managing issues using `IssueDbContext`.
   - **Methods**:
     - `Task<Issue?> GetById(string id, CancellationToken cancellationToken = default);` - Retrieves an issue by its ID.
     - `Task Update(Issue issue, CancellationToken cancellationToken = default);` - Updates an issue.
     - `Task Add(Issue issue, CancellationToken cancellationToken = default);` - Adds a new issue.
     - `Task Delete(string id, CancellationToken cancellationToken = default);` - Deletes an issue by its ID.


## Common Microsoft Interfaces and Implementations

### Interfaces

1. **`IServiceCollection`**
    - **Namespace**: `Microsoft.Extensions.DependencyInjection`
    - **Description**: Defines a contract for a collection of service descriptors.
    - **Methods**:
        - `IServiceCollection AddSingleton<TService>(Func<IServiceProvider, TService> implementationFactory) where TService : class;` - Adds a singleton service to the collection.
        - `IServiceCollection AddScoped<TService>(Func<IServiceProvider, TService> implementationFactory) where TService : class;` - Adds a scoped service to the collection.
        - `IServiceCollection AddTransient<TService>(Func<IServiceProvider, TService> implementationFactory) where TService : class;` - Adds a transient service to the collection.

2. **`IConfiguration`**
    - **Namespace**: `Microsoft.Extensions.Configuration`
    - **Description**: Represents a set of key/value application configuration properties.
    - **Methods**:
        - `string this[string key] { get; set; }` - Gets or sets a configuration value.
        - `IConfigurationSection GetSection(string key);` - Gets a configuration sub-section with the specified key.
        - `IEnumerable<IConfigurationSection> GetChildren();` - Gets the immediate descendant configuration sub-sections.

3. **`ILogger`**
    - **Namespace**: `Microsoft.Extensions.Logging`
    - **Description**: Defines a contract for a type used to perform logging.
    - **Methods**:
        - `void LogInformation(string message, params object[] args);` - Logs an informational message.
        - `void LogWarning(string message, params object[] args);` - Logs a warning message.
        - `void LogError(string message, params object[] args);` - Logs an error message.

4. **`IDbContext`**
    - **Namespace**: `Microsoft.EntityFrameworkCore`
    - **Description**: Represents a session with the database and can be used to query and save instances of your entities.
    - **Methods**:
        - `DbSet<TEntity> Set<TEntity>() where TEntity : class;` - Gets a `DbSet` that can be used to query and save instances of `TEntity`.
        - `int SaveChanges();` - Saves all changes made in this context to the database.
        - `Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);` - Asynchronously saves all changes made in this context to the database.

5. **`IHostedService`**
    - **Namespace**: `Microsoft.Extensions.Hosting`
    - **Description**: Defines methods for objects that are managed by the host.
    - **Methods**:
        - `Task StartAsync(CancellationToken cancellationToken);` - Triggered when the application host is ready to start the service.
        - `Task StopAsync(CancellationToken cancellationToken);` - Triggered when the application host is performing a graceful shutdown.

### Implementations

1. **`ServiceCollection`**
    - **Namespace**: `Microsoft.Extensions.DependencyInjection`
    - **Implements**: `IServiceCollection`
    - **Description**: Default implementation of `IServiceCollection`.

2. **`ConfigurationBuilder`**
    - **Namespace**: `Microsoft.Extensions.Configuration`
    - **Implements**: `IConfigurationBuilder`
    - **Description**: Provides a mechanism to build application configuration.

3. **`Logger<T>`**
    - **Namespace**: `Microsoft.Extensions.Logging`
    - **Implements**: `ILogger<T>`
    - **Description**: Default implementation of `ILogger<T>`.

4. **`DbContext`**
    - **Namespace**: `Microsoft.EntityFrameworkCore`
    - **Implements**: `IDbContext`
    - **Description**: Default implementation of `IDbContext`.

5. **`BackgroundService`**
    - **Namespace**: `Microsoft.Extensions.Hosting`
    - **Implements**: `IHostedService`
    - **Description**: Base class for implementing a long-running `IHostedService`.

For detailed documentation, please refer to the official Microsoft documentation at [docs.microsoft.com](https://docs.microsoft.com/en-us/dotnet/).
