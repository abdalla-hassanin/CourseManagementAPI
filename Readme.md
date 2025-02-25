# 🛍️ Course Management API

## 📋 Overview
A modern, scalable Course Management API built with .NET Core 9, implementing best practices.

## ⚙️ Technology Stack & Features

### Core Technologies
- [x] .NET Core 9 / C# 13
- [x] Entity Framework Core
- [x] CQRS with MediatR Pattern
- [x] Unit of Work & Generic Repository Pattern
- [x] JWT Authentication
- [x] ASP.NET Core Identity
- [x] Role-based Authorization
- [x] Fluent API
- [x] Fluent Validation
- [x] AutoMapper

### Development Features
- [x] Email Service Integration
- [x] Swagger Documentation
- [x] Pagination Implementation
- [x] Filter Implementation
- [x] Specification Pattern
- [x] Unified API Response Structure
- [x] Global Error Handling
- [x] Development/Production Environment Configurations
- [x] Seq Logging (Development)
- [x] SQL Server (Development Database)

## 🏗️ Project Structure

```
CourseManagementAPI/
├── CourseManagementAPI.Api/          # API Layer - Main application entry point
│   ├── Controllers/                  # API endpoints controllers
│   ├── View/                        # View models and DTOs specific to API
│   ├── ResponseExample/             # Example responses for documentation
│   ├── Properties/                  # Project properties
│   ├── appsettings.json            # Main configuration file
│   ├── appsettings.Development.json # Development environment settings
│   ├── appsettings.Production.json  # Production environment settings
│   ├── Program.cs                   # Application startup configuration
│   └── CourseManagementAPI.Api.csproj      # API project file
│
├── CourseManagementAPI.Core/               # Core Layer - Business logic and domain
│   ├── Base/                        # Base classes and shared components
│   │   ├── Middleware/              # Custom middleware components
│   │   │   ├── ErrorHandlerMiddleware.cs  # Global error handling
│   │   │   └── ValidationBehavior.cs      # Request validation behavior
│   │   └── Response/
│   │       └── Response.cs          # Unified response structure
│   │
│   └── MediatrHandlers/             # CQRS handlers using MediatR
│       ├── Course/                 # Course feature module
│       │   ├── CourseDto.cs        # Course data transfer objects
│       │   ├── CourseMappingProfile.cs   # AutoMapper profile for Course
│       │   ├── Commands/            # Command handlers for Course
│       │   │   ├── CreateCourseHandler.cs
│       │   │   ├── DeleteCourseHandler.cs
│       │   │   └── UpdateCourseHandler.cs
│       │   └── Queries/             # Query handlers for Course
│       │       ├── GetCourseByIdHandler.cs
│       │       └── GetCourseesByCustomerIdHandler.cs
│
├── CourseManagementAPI.Data/        # Data Layer - Domain entities and data models
│   ├── Entities/                    # Domain entities
│   │   ├── Course.cs
│   │   ├── Admin.cs
│   │   ├── ApplicationUser.cs
│   │   ├── Trainer.cs
│   │   └── Payment.cs
│   └── Options/                     # Configuration options classes
│       └── SecretOptions.cs
│
├── CourseManagementAPI.Infrastructure/      # Infrastructure Layer
│   ├── Base/                        # Base infrastructure components
│   │   ├── Specifications/          # Specification pattern implementation
│   │   │   ├── BaseSpecification.cs
│   │   │   ├── ISpecification.cs
│   │   │   └── SpecificationEvaluator.cs
│   │   ├── GenericRepository.cs     # Generic repository implementation
│   │   ├── IGenericRepository.cs    # Generic repository interface
│   │   ├── IUnitOfWork.cs          # Unit of work interface
│   │   └── UnitOfWork.cs           # Unit of work implementation
│   ├── Context/                     # Database context
│   │   └── ApplicationDbContext.cs
│   ├── Configurations/              # Entity configurations
│   ├── Migrations/                  # Database migrations
│   └── ModuleInfrastructureDependencies.cs  # Infrastructure DI setup
│
├── CourseManagementAPI.Service/            # Service Layer
│   ├── Base/                        # Base service components
│   │   ├── AuthResult.cs           # Authentication result model
│   │   └── PaginationList.cs       # Pagination implementation
│   ├── IService/                    # Service interfaces
│   ├── Service/                     # Service implementations
│   ├── Specification/               # Business specifications
│   └── ModuleServiceDependencies.cs # Service DI setup
└── 
```

## 🚀 Getting Started

### Prerequisites
- [.NET Core SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
- [Gmail Account](https://gmail.com) (for email service)

### Local Development Setup

1. **Clone the repository**
```bash
git clone https://github.com/yourusername/CourseManagementAPI.git
cd CourseManagementAPI
```

2**Set up the database**
```bash
cd CourseManagementAPI.Api
dotnet ef database update
```

4. **Run the application**
```bash
dotnet run
```

## 📚 API Documentation

### Authentication

The API uses JWT Bearer token authentication. To access protected endpoints:

1. Register a new user at `POST /api/auth/register`
2. Login at `POST /api/auth/login` to receive a JWT token
3. Include the token in the Authorization header: `Bearer {token}`

### Available Endpoints
Detailed API documentation is available through Swagger UI at:

http://course-management-api.runasp.net/index.html

## 🔒 Security

- JWT token-based authentication
- Role-based access control
- Password hashing with ASP.NET Core Identity
- HTTPS enforcement
- Cross-Origin Resource Sharing (CORS) configuration
- Input validation using Fluent Validation

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 📞 Support

For support, email abdalla.hassanin.2000@gmail.com or create an issue in the GitHub repository.

## ✨ Acknowledgements

- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core)
- All contributors who have helped this project grow

---
Made with ❤️ by [Abdalla Hassanin]


