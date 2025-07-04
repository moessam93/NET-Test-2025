# NET-Test-2025

A .NET 8 Web API project for client management with CRUD operations, built with clean architecture principles.

## Features

- **Clean Architecture**: Separated concerns with Controllers, Services, and DTOs
- **Entity Framework Core**: Code-first approach with SQL Server
- **Input Validation**: Model validation with custom attributes
- **Error Handling**: ServiceResult pattern for consistent error responses
- **Swagger UI**: Interactive API documentation
- **Soft Delete**: Clients are marked as deleted rather than physically removed

## Technology Stack

- **.NET 8** - Web API framework
- **Entity Framework Core** - ORM for database operations
- **SQL Server LocalDB** - Database engine
- **Swagger/OpenAPI** - API documentation

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server LocalDB](https://docs.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb) (included with Visual Studio)

## Installation

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd NET-Test-2025
   ```

2. **Navigate to the project directory**
   ```bash
   cd Net-Test-2025
   ```

3. **Restore NuGet packages**
   ```bash
   dotnet restore
   ```

4. **Build the project**
   ```bash
   dotnet build
   ```

## Database Configuration

### Connection String

Update the connection string in `appsettings.json` or `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=NetTest2025;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

### Database Migration

1. **Create a new migration** (if needed):
   ```bash
   dotnet ef migrations add CreateClientsTable
   ```

2. **Update the database**:
   ```bash
   dotnet ef database update
   ```

3. **Remove last migration** (if needed):
   ```bash
   dotnet ef migrations remove
   ```

##  Running the Application

### Development Mode

```bash
dotnet run
```

The application will start on:
- **HTTP**: `http://localhost:5217`
- **Swagger UI**: `http://localhost:5217/swagger`

### Production Mode

```bash
dotnet run --environment Production
```

### Watch Mode (Auto-reload)

```bash
dotnet watch run
```

## 📚 API Endpoints

### Client Management

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/Client` | Get all clients with optional filtering |
| GET | `/api/Client/{id}` | Get client by ID |
| POST | `/api/Client` | Create new client |
| PUT | `/api/Client/{id}` | Update existing client |
| DELETE | `/api/Client/{id}` | Soft delete client |

### Query Parameters (GET /api/Client)

- `searchTerm`: Filter by client name
- `gender`: Filter by gender (Male/Female)
- `page`: Page number (default: 1)
- `pageSize`: Items per page (default: 10)

### Example Requests

**Create Client:**
```json
POST /api/Client
{
  "name": "John Doe",
  "email": "john.doe@example.com",
  "phone": "1234567890",
  "age": "30",
  "gender": "Male"
}
```

**Update Client:**
```json
PUT /api/Client/1
{
  "name": "Jane Doe",
  "email": "jane.doe@example.com",
  "phone": "0987654321",
  "age": "28",
  "gender": "Female"
}
```

## Validation Rules (On Create)

- **Name**: Required
- **Email**: Required, must be valid email format
- **Phone**: Required
- **Gender**: Required, must be "Male" or "Female"
- **Age**: Optional
- **Email & Phone**: Must be unique across clients

## Project Structure

```
Net-Test-2025/
├── Controllers/           # API Controllers
│   └── ClientController.cs
├── Data/                 # Database Context
│   ├── AppDbContext.cs
│   └── Migrations/
├── Domains/              # Domain Models
│   └── Client.cs
├── Helpers/              # Helper Classes
│   └── Enums.cs
├── Services/             # Business Logic
│   └── ClientService.cs
├── Services.Contracts/   # Interfaces and DTOs
│   ├── DTOs/
│   │   ├── CreateClientRequest.cs
│   │   ├── UpdateClientDto.cs
│   │   ├── GetClientDto.cs
│   │   ├── ClientsListingRequest.cs
│   │   └── ValidGenderAttribute.cs
│   └── Interfaces/
│       ├── IClientService.cs
│       └── IValidatable.cs
├── Properties/
│   └── launchSettings.json
├── appsettings.json
└── Program.cs
```

## Testing

### Using Swagger UI

1. Run the application: `dotnet run`
2. Navigate to `http://localhost:5217/swagger`
3. Test API endpoints interactively

### Using curl

```bash
# Get all clients
curl -X GET "http://localhost:5217/api/Client"

# Create a client
curl -X POST "http://localhost:5217/api/Client" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Test User",
    "email": "test@example.com",
    "phone": "1234567890",
    "age": "25",
    "gender": "Male"
  }'
```

## Common Commands

```bash
# Build project
dotnet build

# Run project
dotnet run

# Run with watch (auto-reload)
dotnet watch run

# Create migration
dotnet ef migrations add <MigrationName>

# Update database
dotnet ef database update

# Remove last migration
dotnet ef migrations remove

# Restore packages
dotnet restore

# Clean build artifacts
dotnet clean
```

## Troubleshooting

### Port Already in Use

If you get "address already in use" error:
```bash
# Kill process on port 5217
lsof -ti:5217 | xargs kill -9
```

### Database Connection Issues

1. Ensure SQL Server LocalDB is installed
2. Check connection string in `appsettings.json`
3. Run database update: `dotnet ef database update`

### Build Errors

```bash
# Clean and rebuild
dotnet clean
dotnet build
```

## Notes

- The project uses **soft delete** - clients are marked as deleted rather than removed
- All API responses follow the **ServiceResult pattern** for consistent error handling
- **Model validation** is automatically applied using Data Annotations
- **Swagger UI** launches automatically in development mode (if not launching just navigate to http://localhost:5217/swagger/index.html)
