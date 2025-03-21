# Pilotic.API Modular Development Guidelines

## Project Structure
Each module in `Pilotic.API/Modules` should follow this structure:
```
ModuleName/
├── Controllers/         # API Controllers
├── Services/           # Implementation of services
├── Events/            # Event definitions and handlers
└── Interfaces/        # Service interfaces
```

## Interface Guidelines
1. All service interfaces should inherit from either:
   - `IInjectableSingletonModule` for singleton services
   - `IInjectableScopedModule` for scoped services

2. Interface naming convention:
   - Use `I` prefix for interfaces
   - Use descriptive names that indicate the service's purpose
   - Group related functionality in a single interface

3. Interface location:
   - Place interfaces in the module's `Interfaces` folder
   - One interface per file
   - Use namespace `Pilotic.API.Modules.{ModuleName}.Interfaces`

## Service Implementation Guidelines
1. Service classes should:
   - Implement their corresponding interface
   - Be placed in the module's `Services` folder
   - Use namespace `Pilotic.API.Modules.{ModuleName}.Services`

2. Dependencies:
   - Inject dependencies through constructor
   - Use interfaces for all dependencies
   - Document dependencies in XML comments

## Controller Guidelines
1. Controllers should:
   - Be placed in the module's `Controllers` folder
   - Use namespace `Pilotic.API.Modules.{ModuleName}.Controllers`
   - Inherit from `PiloticApiController`
   - Use dependency injection for services

2. Route naming:
   - Use plural nouns for resource endpoints
   - Use kebab-case for route segments
   - Include version prefix (e.g., `api/v1`)

## Event Guidelines
1. Event definitions should:
   - Be placed in the module's `Events` folder
   - Implement `IEvent` interface
   - Use namespace `Pilotic.API.Modules.{ModuleName}.Events`

2. Event handlers should:
   - Implement `IEventHandler<T>` interface
   - Be placed in the module's `Events` folder
   - Use namespace `Pilotic.API.Modules.{ModuleName}.Events`

## Dependency Injection
1. Use marker interfaces for DI:
   - `IInjectableSingletonModule` for singleton services
   - `IInjectableScopedModule` for scoped services
   - No need to register in Program.cs when using these interfaces

2. Service lifetime:
   - Use singleton for stateless services
   - Use scoped for request-scoped services
   - Avoid transient lifetime unless absolutely necessary

## Code Organization
1. Keep modules self-contained:
   - Minimize cross-module dependencies
   - Use events for cross-module communication
   - Share common code through Core project

2. Follow SOLID principles:
   - Single Responsibility Principle
   - Open/Closed Principle
   - Liskov Substitution Principle
   - Interface Segregation Principle
   - Dependency Inversion Principle

## Testing Guidelines
1. Unit tests should:
   - Test service implementations
   - Mock dependencies
   - Use xUnit framework
   - Follow AAA pattern (Arrange, Act, Assert)

2. Integration tests should:
   - Test API endpoints
   - Use TestServer
   - Clean up test data
   - Be independent of each other 