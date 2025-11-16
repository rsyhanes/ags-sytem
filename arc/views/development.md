# AGS Windows & Doors - .NET Solution Architecture

## Overview

This document outlines the recommended folder structure for a .NET solution implementing Domain-Driven Design (DDD) and Ports & Adapters (Hexagonal) architecture for the AGS Windows & Doors system.

## Solution Structure

```
AGS.WindowsAndDoors/
├── src/
│   ├── SharedKernel/
│   │   └── AGS.WindowsAndDoors.SharedKernel/
│   │       ├── Domain/
│   │       │   ├── ValueObjects/
│   │       │   │   ├── Color.cs
│   │       │   │   ├── Measure.cs
│   │       │   │   ├── Category.cs
│   │       │   │   └── Angle.cs
│   │       │   ├── Exceptions/
│   │       │   │   ├── DomainException.cs
│   │       │   │   └── BusinessRuleViolationException.cs
│   │       │   └── Interfaces/
│   │       │       ├── IEntity.cs
│   │       │       ├── IAggregateRoot.cs
│   │       │       └── IDomainEvent.cs
│   │       └── Infrastructure/
│   │           ├── Extensions/
│   │           └── Utilities/
│   │
│   ├── BoundedContexts/
│   │   ├── ProductCatalog/
│   │   │   ├── AGS.WindowsAndDoors.ProductCatalog.Domain/
│   │   │   │   ├── Entities/
│   │   │   │   │   └── Item.cs
│   │   │   │   ├── ValueObjects/
│   │   │   │   ├── Exceptions/
│   │   │   │   │   └── DuplicateItemCodeException.cs
│   │   │   │   ├── Ports/
│   │   │   │   │   └── IItemRepository.cs
│   │   │   │   └── Services/
│   │   │   │       └── ItemDomainService.cs
│   │   │   │
│   │   │   ├── AGS.WindowsAndDoors.ProductCatalog.Application/
│   │   │   │   ├── UseCases/
│   │   │   │   │   ├── CreateItem/
│   │   │   │   │   │   ├── CreateItemCommand.cs
│   │   │   │   │   │   ├── CreateItemHandler.cs
│   │   │   │   │   │   └── CreateItemValidator.cs
│   │   │   │   │   ├── UpdateItem/
│   │   │   │   │   ├── GetItem/
│   │   │   │   │   └── ListItems/
│   │   │   │   ├── DTOs/
│   │   │   │   │   ├── ItemDto.cs
│   │   │   │   │   ├── CreateItemDto.cs
│   │   │   │   │   └── UpdateItemDto.cs
│   │   │   │   └── Mappings/
│   │   │   │       └── ItemMappingProfile.cs
│   │   │   │
│   │   │   └── AGS.WindowsAndDoors.ProductCatalog.Infrastructure/
│   │   │       ├── Adapters/
│   │   │       │   └── Persistence/
│   │   │       │       ├── ItemRepository.cs
│   │   │       │       ├── ProductCatalogDbContext.cs
│   │   │       │       └── Configurations/
│   │   │       │           └── ItemConfiguration.cs
│   │   │       └── DependencyInjection.cs
│   │   │
│   │   ├── ProductDesign/
│   │   │   ├── AGS.WindowsAndDoors.ProductDesign.Domain/
│   │   │   │   ├── Entities/
│   │   │   │   │   ├── System.cs
│   │   │   │   │   └── SystemComponent.cs
│   │   │   │   ├── ValueObjects/
│   │   │   │   │   ├── SizeConstraints.cs
│   │   │   │   │   ├── LengthCalculation.cs
│   │   │   │   │   ├── NoopLength.cs
│   │   │   │   │   ├── FixedLength.cs
│   │   │   │   │   └── DelegatedLength.cs
│   │   │   │   ├── Aggregates/
│   │   │   │   │   └── SystemAggregate.cs
│   │   │   │   ├── Ports/
│   │   │   │   │   ├── ISystemRepository.cs
│   │   │   │   │   ├── IComponentRepository.cs
│   │   │   │   │   ├── IFormulaEngine.cs
│   │   │   │   │   └── IItemCatalogService.cs
│   │   │   │   ├── Services/
│   │   │   │   │   └── SystemDesignService.cs
│   │   │   │   └── Exceptions/
│   │   │   │       ├── InvalidFormulaException.cs
│   │   │   │       └── SystemConstraintViolationException.cs
│   │   │   │
│   │   │   ├── AGS.WindowsAndDoors.ProductDesign.Application/
│   │   │   │   ├── UseCases/
│   │   │   │   │   ├── CreateSystem/
│   │   │   │   │   │   ├── CreateSystemCommand.cs
│   │   │   │   │   │   ├── CreateSystemHandler.cs
│   │   │   │   │   │   └── CreateSystemValidator.cs
│   │   │   │   │   ├── ConfigureComponent/
│   │   │   │   │   ├── TestCalculation/
│   │   │   │   │   └── ListSystemComponents/
│   │   │   │   ├── DTOs/
│   │   │   │   │   ├── SystemDto.cs
│   │   │   │   │   ├── SystemComponentDto.cs
│   │   │   │   │   └── TestCalculationDto.cs
│   │   │   │   └── Services/
│   │   │   │       └── ComponentCalculationService.cs
│   │   │   │
│   │   │   └── AGS.WindowsAndDoors.ProductDesign.Infrastructure/
│   │   │       ├── Adapters/
│   │   │       │   ├── Persistence/
│   │   │       │   │   ├── SystemRepository.cs
│   │   │       │   │   ├── ComponentRepository.cs
│   │   │       │   │   ├── ProductDesignDbContext.cs
│   │   │       │   │   └── Configurations/
│   │   │       │   ├── FormulaEngine/
│   │   │       │   │   └── ExpressionFormulaEngine.cs
│   │   │       │   └── ExternalServices/
│   │   │       │       └── ItemCatalogServiceAdapter.cs
│   │   │       └── DependencyInjection.cs
│   │   │
│   │   └── OrderProcessing/
│   │       ├── AGS.WindowsAndDoors.OrderProcessing.Domain/
│   │       │   ├── Entities/
│   │       │   │   └── Order.cs
│   │       │   ├── ValueObjects/
│   │       │   │   ├── CustomerInfo.cs
│   │       │   │   ├── Address.cs
│   │       │   │   ├── Frame.cs
│   │       │   │   ├── BOM.cs
│   │       │   │   ├── BOMLine.cs
│   │       │   │   └── OrderStatus.cs
│   │       │   ├── Aggregates/
│   │       │   │   └── OrderAggregate.cs
│   │       │   ├── Events/
│   │       │   │   ├── OrderSubmitted.cs
│   │       │   │   └── OrderStatusChanged.cs
│   │       │   ├── Ports/
│   │       │   │   ├── IOrderRepository.cs
│   │       │   │   ├── IBOMCalculationService.cs
│   │       │   │   ├── IValidationService.cs
│   │       │   │   ├── INotificationService.cs
│   │       │   │   └── IEventPublisher.cs
│   │       │   ├── Services/
│   │       │   │   └── OrderProcessingService.cs
│   │       │   └── Exceptions/
│   │       │       ├── InvalidFrameSpecificationException.cs
│   │       │       └── BOMCalculationException.cs
│   │       │
│   │       ├── AGS.WindowsAndDoors.OrderProcessing.Application/
│   │       │   ├── UseCases/
│   │       │   │   ├── SubmitOrder/
│   │       │   │   │   ├── SubmitOrderCommand.cs
│   │       │   │   │   ├── SubmitOrderHandler.cs
│   │       │   │   │   └── SubmitOrderValidator.cs
│   │       │   │   ├── CalculateBOM/
│   │       │   │   ├── ValidateFrame/
│   │       │   │   ├── UpdateOrderStatus/
│   │       │   │   └── ListOrders/
│   │       │   ├── DTOs/
│   │       │   │   ├── OrderDto.cs
│   │       │   │   ├── FrameDto.cs
│   │       │   │   ├── BOMDto.cs
│   │       │   │   └── CustomerInfoDto.cs
│   │       │   └── Services/
│   │       │       └── OrderApplicationService.cs
│   │       │
│   │       └── AGS.WindowsAndDoors.OrderProcessing.Infrastructure/
│   │           ├── Adapters/
│   │           │   ├── Persistence/
│   │           │   │   ├── OrderRepository.cs
│   │           │   │   ├── OrderProcessingDbContext.cs
│   │           │   │   └── Configurations/
│   │           │   ├── ExternalServices/
│   │           │   │   ├── ProductDesignServiceAdapter.cs
│   │           │   │   └── BOMCalculationServiceAdapter.cs
│   │           │   ├── Messaging/
│   │           │   │   ├── EventPublisher.cs
│   │           │   │   └── NotificationService.cs
│   │           │   └── Validation/
│   │           │       └── FrameValidationService.cs
│   │           └── DependencyInjection.cs
│   │
│   └── Presentation/
│       ├── Backend/
│       │   └── AGS.WindowsAndDoors.WebAPI/
│       │       ├── Endpoints/
│       │       │   ├── ItemsEndpoints.cs
│       │       │   ├── SystemsEndpoints.cs
│       │       │   ├── ComponentsEndpoints.cs
│       │       │   ├── OrdersEndpoints.cs
│       │       │   └── CalculationsEndpoints.cs
│       │       ├── Middleware/
│       │       │   ├── ErrorHandlingMiddleware.cs
│       │       │   └── ValidationMiddleware.cs
│       │       ├── Models/
│       │       │   ├── Requests/
│       │       │   └── Responses/
│       │       ├── Configuration/
│       │       │   ├── SwaggerConfiguration.cs
│       │       │   └── CorsConfiguration.cs
│       │       ├── Program.cs
│       │       └── appsettings.json
│       │
│       └── Frontend/
│           ├── AGS.WindowsAndDoors.AdminPortal/
│           │   ├── src/
│           │   │   ├── app/
│           │   │   │   ├── core/
│           │   │   │   │   ├── services/
│           │   │   │   │   │   ├── api.service.ts
│           │   │   │   │   │   ├── auth.service.ts
│           │   │   │   │   │   └── validation.service.ts
│           │   │   │   │   └── models/
│           │   │   │   │       ├── item.model.ts
│           │   │   │   │       ├── system.model.ts
│           │   │   │   │       └── order.model.ts
│           │   │   │   ├── features/
│           │   │   │   │   ├── items/
│           │   │   │   │   │   ├── item-list/
│           │   │   │   │   │   ├── item-create/
│           │   │   │   │   │   └── item-edit/
│           │   │   │   │   ├── systems/
│           │   │   │   │   │   ├── system-list/
│           │   │   │   │   │   ├── system-designer/
│           │   │   │   │   │   └── component-configurator/
│           │   │   │   │   └── orders/
│           │   │   │   │       ├── order-list/
│           │   │   │   │       └── order-details/
│           │   │   │   ├── shared/
│           │   │   │   │   ├── components/
│           │   │   │   │   │   ├── calculation-tester/
│           │   │   │   │   │   └── formula-builder/
│           │   │   │   │   └── pipes/
│           │   │   │   └── app.component.ts
│           │   │   ├── assets/
│           │   │   ├── environments/
│           │   │   └── main.ts
│           │   ├── angular.json
│           │   ├── package.json
│           │   ├── tsconfig.json
│           │   └── README.md
│           │
│           └── AGS.WindowsAndDoors.CustomerPortal/
│               ├── src/
│               │   ├── app/
│               │   │   ├── core/
│               │   │   │   ├── services/
│               │   │   │   │   ├── api.service.ts
│               │   │   │   │   ├── calculation.service.ts
│               │   │   │   │   └── order.service.ts
│               │   │   │   └── models/
│               │   │   │       ├── system.model.ts
│               │   │   │       ├── frame.model.ts
│               │   │   │       ├── bom.model.ts
│               │   │   │       └── customer.model.ts
│               │   │   ├── features/
│               │   │   │   ├── home/
│               │   │   │   ├── systems/
│               │   │   │   │   ├── system-browser/
│               │   │   │   │   └── system-details/
│               │   │   │   ├── configure/
│               │   │   │   │   ├── frame-configurator/
│               │   │   │   │   └── bom-preview/
│               │   │   │   ├── order/
│               │   │   │   │   ├── order-review/
│               │   │   │   │   ├── order-submit/
│               │   │   │   │   └── order-confirmation/
│               │   │   │   └── account/
│               │   │   │       ├── profile/
│               │   │   │       └── order-history/
│               │   │   ├── shared/
│               │   │   │   ├── components/
│               │   │   │   │   ├── system-card/
│               │   │   │   │   └── order-tracker/
│               │   │   │   └── pipes/
│               │   │   └── app.component.ts
│               │   ├── assets/
│               │   ├── environments/
│               │   └── main.ts
│               ├── angular.json
│               ├── package.json
│               ├── tsconfig.json
│               └── README.md
│
├── tests/
│   ├── UnitTests/
│   │   ├── AGS.WindowsAndDoors.ProductCatalog.Domain.Tests/
│   │   ├── AGS.WindowsAndDoors.ProductDesign.Domain.Tests/
│   │   ├── AGS.WindowsAndDoors.OrderProcessing.Domain.Tests/
│   │   └── AGS.WindowsAndDoors.SharedKernel.Tests/
│   │
│   ├── IntegrationTests/
│   │   ├── AGS.WindowsAndDoors.ProductCatalog.Infrastructure.Tests/
│   │   ├── AGS.WindowsAndDoors.ProductDesign.Infrastructure.Tests/
│   │   ├── AGS.WindowsAndDoors.OrderProcessing.Infrastructure.Tests/
│   │   └── AGS.WindowsAndDoors.WebAPI.Tests/
│   │
│   └── AcceptanceTests/
│       └── AGS.WindowsAndDoors.AcceptanceTests/
│           ├── Features/
│           │   ├── ItemManagement.feature
│           │   ├── SystemDesign.feature
│           │   └── OrderProcessing.feature
│           └── StepDefinitions/
│
├── docs/
│   ├── architecture/
│   ├── api-docs/
│   └── deployment/
│
├── scripts/
│   ├── build.ps1
│   ├── deploy.ps1
│   └── database-migrations.ps1
│
├── AGS.WindowsAndDoors.sln
└── Directory.Build.props
```

## Key Architecture Principles

### 1. Bounded Context Isolation
- Each bounded context is a separate project with its own Domain, Application, and Infrastructure layers
- No direct references between bounded contexts
- Communication via ports and adapters

### 2. Dependency Direction (Hexagonal Architecture)
```
Presentation → Application → Domain ← Infrastructure
                    ↑               ↗
                    └── Ports ←──────┘
```

- **Domain**: Contains business logic, entities, value objects, and port interfaces
- **Application**: Orchestrates use cases, contains DTOs and application services
- **Infrastructure**: Implements ports, contains adapters for external concerns
- **Presentation**: Web API controllers, web apps, console apps

### 3. Domain Layer Structure
```csharp
// Example: ProductDesign Domain
namespace AGS.WindowsAndDoors.ProductDesign.Domain
{
    // Entities - Rich domain objects with behavior
    public class System : AggregateRoot<string>
    
    // Value Objects - Immutable objects with no identity
    public record SizeConstraints(decimal MinHeight, decimal MaxHeight, decimal MinWidth, decimal MaxWidth)
    
    // Ports - Interfaces the domain needs (to be implemented by infrastructure)
    public interface ISystemRepository
    
    // Domain Services - Pure business logic that doesn't belong to a single entity
    public class SystemDesignService
}
```

### 4. Application Layer Structure
```csharp
// Example: Use Case Handler
[Handler]
public class CreateSystemHandler : IRequestHandler<CreateSystemCommand, SystemDto>
{
    private readonly ISystemRepository _systemRepository;
    private readonly IMapper _mapper;
    
    public async Task<SystemDto> Handle(CreateSystemCommand request, CancellationToken cancellationToken)
    {
        // Validate
        // Create domain entity
        // Save via repository
        // Return DTO
    }
}
```

### 5. Infrastructure Layer Structure
```csharp
// Example: Repository Adapter
public class SystemRepository : ISystemRepository
{
    private readonly ProductDesignDbContext _context;
    
    public async Task<System> FindByCodeAsync(string code)
    {
        // EF Core implementation
    }
}

// Dependency Injection Configuration
public static class DependencyInjection
{
    public static IServiceCollection AddProductDesignInfrastructure(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddDbContext<ProductDesignDbContext>();
        services.AddScoped<ISystemRepository, SystemRepository>();
        return services;
    }
}
```

## Package Dependencies

### Domain Projects
- **No external dependencies** (pure .NET)
- Reference only SharedKernel

### Application Projects
- MassTransit.Abstractions Mediator (Command/Query handling)
- LanguageExt.Core (for some level of functional programming)

### Infrastructure Projects
- Entity Framework Core (Persistence)
- External service clients
- Message queues, caching, etc.

### Backend Projects (.NET)
- ASP.NET Core Web API
- Swagger/OpenAPI
- Authentication libraries (JWT, Identity)
- CORS configuration for Angular SPAs

### Frontend Projects (Angular)
- Angular Framework
- TypeScript
- Zoneless (no zone.js)
- Standalone components
- Components with ChangeStrategy.OnPush
- Signals for components reactivity
- Signal Store for state management
- RxJS (Reactive programming)
- Angular Material (UI components)
- Angular HTTP Client
- Angular Router
- Angular Forms (Reactive forms)

## Presentation Layer Details

### Web API (Backend Service)
- **Purpose**: RESTful API backend serving both Angular applications
- **Technology**: ASP.NET Core Minimal API (.NET 9.0)
- **Responsibilities**:
    - Authentication and authorization (JWT tokens)
    - Request validation and error handling
    - CORS configuration for Angular SPAs
    - Orchestrating application use cases via MassTransit Mediator
    - OpenAPI documentation for API endpoints

### Admin Portal (Angular SPA - Engineer/Staff Interface)
- **Purpose**: Single-page application for system management and order processing
- **Technology**: Angular v20 with TypeScript
- **Key Features**:
    - Item catalog management (create, edit, delete items)
    - System design with interactive components
    - Formula testing and validation tools
    - Order management dashboard with real-time updates
    - Component configurator with drag-and-drop interface
- **Architecture**:
    - Feature-based module organization
    - Reactive forms with validation
    - HTTP interceptors for API communication
    - State management with services
- **Users**: Engineers, administrators, manufacturing staff

### Customer Portal (Angular SPA - Client Interface)
- **Purpose**: Customer-facing single-page application for product selection and ordering
- **Technology**: Angular v20 with TypeScript
- **Key Features**:
    - System browser with search and filtering
    - Interactive frame configurator with real-time preview
    - Live BOM calculation and pricing updates
    - Streamlined order submission workflow
    - Account management and order history tracking
- **Architecture**:
    - Responsive design for mobile and desktop
    - Reactive programming with RxJS observables
    - Real-time calculation updates
    - Optimistic UI updates for better UX
- **Users**: External customers placing orders

## Project Count Summary

**Total Projects**: 22 projects
- **1** SharedKernel (Class Library)
- **9** Bounded Context projects (3 contexts × 3 layers each)
- **1** Backend API project (ASP.NET Core Web API)
- **2** Frontend projects (Angular SPAs)
- **9** Test projects (4 Unit + 4 Integration + 1 Acceptance)

## Benefits of This Structure

1. **Clear Separation of Concerns**: Each layer has distinct responsibilities
2. **Testability**: Domain logic is isolated and easily unit testable
3. **Flexibility**: Infrastructure can be swapped without affecting business logic
4. **Bounded Context Isolation**: Each context can evolve independently
5. **Scalability**: Contexts can be deployed as separate microservices later
6. **Maintainability**: Clear folder structure makes navigation intuitive
7. **User Experience Separation**: Different interfaces optimized for different user types
8. **Independent Deployment**: Customer and admin interfaces can be deployed separately

## Implementation Notes

- Use **Aggregate patterns** for transactional consistency boundaries
- Implement **Domain Events** for cross-context communication
- Apply **CQRS** pattern where read/write models differ significantly
- Use **Specifications pattern** for complex domain queries
- Implement **Unit of Work** pattern for transaction management

This structure provides a solid foundation for implementing the AGS Windows & Doors system using modern .NET practices and DDD principles.
