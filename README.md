# StoreManagement

A Store and Inventory Management System built with ASP.NET Core Web API and Entity Framework Core.

The project manages products, categories, customers, inventory, and orders, with authentication and role-based authorization using ASP.NET Core Identity and JWT.

## Technologies Used

- C#
- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- ASP.NET Core Identity
- JWT Bearer Authentication
- Role-Based Authorization
- Git and GitHub

## Features

- Product management (CRUD operations)
- Category management
- Customer management
- Inventory management
- Order creation and management
- Automatic inventory deduction when creating orders
- Inventory restoration during eligible order deletion or cancellation operations
- User registration and login
- JWT-based authentication
- Role-based authorization (Admin and Customer)
- Order validation and stock availability checks
- Customer-specific order access and authorization
- Transactional operations to help maintain order and inventory consistency

## Project Structure

The solution is organized into four projects:

- **StoreManagement.API** — Handles HTTP requests, controllers, authentication, and API configuration.
- **StoreManagement.Application** — Contains application logic, use cases, DTOs, and service contracts.
- **StoreManagement.Domain** — Contains domain entities and business-related types.
- **StoreManagement.Infrastructure** — Handles database access and Entity Framework Core implementations.

## Getting Started

### Prerequisites

Before running the project, make sure you have installed:

- .NET 8 SDK
- SQL Server
- Visual Studio 2022 or another compatible .NET IDE

### 1. Clone the Repository

```bash
git clone https://github.com/Baraa999X/StoreManagement.git
cd StoreManagement
```

Open `StoreManagement.sln` in Visual Studio.

### 2. Configure User Secrets

The project uses ASP.NET Core User Secrets to store local configuration outside the source-controlled files.

Open the Package Manager Console in Visual Studio and configure the following settings. Replace the example values with your own:

```powershell
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "YOUR_CONNECTION_STRING" --project StoreManagement.API

dotnet user-secrets set "Jwt:Key" "YOUR_SECURE_SECRET_KEY" --project StoreManagement.API

dotnet user-secrets set "AdminSeed:Email" "YOUR_ADMIN_EMAIL" --project StoreManagement.API

dotnet user-secrets set "AdminSeed:Password" "YOUR_ADMIN_PASSWORD" --project StoreManagement.API
```

Use a valid SQL Server connection string for your local environment. Generate a strong, private JWT signing key and choose a secure password for the initial admin account.

The `Jwt:Issuer`, `Jwt:Audience`, and `Jwt:ExpirationMinutes` settings are configured separately in the application configuration.

**Security:** Never commit real passwords, JWT signing keys, or machine-specific connection strings to source control.

### 3. Apply Database Migrations

The project uses Entity Framework Core migrations to manage database schema changes.

In Visual Studio, open the Package Manager Console and run:

```powershell
Update-Database -Project StoreManagement.Infrastructure -StartupProject StoreManagement.API
```

Ensure that SQL Server is running and the connection string is configured correctly before applying the migrations.

### 4. Run the Application

1. Open `StoreManagement.sln` in Visual Studio.
2. Set `StoreManagement.API` as the startup project.
3. Confirm that the required User Secrets are configured.
4. Make sure SQL Server is running and accessible.
5. Run the application.

The local application URLs are configured in `StoreManagement.API/Properties/launchSettings.json`.

## Authentication and Authorization

The project uses ASP.NET Core Identity and JWT Bearer authentication.

- Users can register and log in to receive a JWT.
- Protected endpoints require a valid JWT.
- Role-based authorization separates administrative operations from customer operations.
- The application supports two roles: `Admin` and `Customer`.
- Customer-specific order operations use the authenticated user's identity to restrict access to their own data.

## Order and Inventory Management

- Orders are validated before creation.
- Product prices are retrieved from the database rather than trusted from client input.
- Inventory availability is checked before order creation.
- Stock quantities are updated as part of order operations.
- Database transactions help maintain consistency during operations that affect orders and inventory.

## Security Notes

- Never commit real passwords, JWT signing keys, or other sensitive configuration to GitHub.
- Keep machine-specific SQL Server connection strings in User Secrets or another local configuration source excluded from source control.
- Use strong secret values and configure production secrets through an appropriate secure configuration provider.
- Administrative operations are protected through role-based authorization.

## Future Improvements

- JavaScript frontend using the Fetch API
- Additional automated tests
- Improved API documentation and deployment configuration