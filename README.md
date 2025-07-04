# NET-Test-2025

A .NET 8 Web API project for client management with CRUD operations, built with clean architecture principles and secured with Azure AD OAuth2 authentication.

## Features

- **Clean Architecture**: Separated concerns with Controllers, Services, and DTOs
- **Entity Framework Core**: Code-first approach with SQL Server
- **Input Validation**: Model validation with custom attributes
- **Error Handling**: ServiceResult pattern for consistent error responses
- **OAuth2 Authentication**: Secured with Azure AD Single Sign-On (SSO)
- **Swagger UI**: Interactive API documentation with OAuth2 authentication
- **Soft Delete**: Clients are marked as deleted rather than physically removed

## Technology Stack

- **.NET 8** - Web API framework
- **Entity Framework Core** - ORM for database operations
- **SQL Server LocalDB** - Database engine
- **Azure AD** - OAuth2 authentication provider
- **JWT Bearer** - Token-based authentication
- **Swagger/OpenAPI** - API documentation with OAuth2 integration

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server LocalDB](https://docs.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb) (included with Visual Studio)
- **Azure AD Tenant** - For OAuth2 authentication
- **Azure AD App Registration** - Required for API authentication

## Authentication

This API uses **OAuth2 with Azure AD** for authentication. All API endpoints require a valid JWT Bearer token.

### Authenticating via Swagger UI

1. **Run the application**:
   ```bash
   dotnet run
   ```

2. **Navigate to Swagger UI**:
   ```
   http://localhost:5217/swagger
   ```

3. **Authenticate using OAuth2**:
   - Click the **"Authorize"** button at the top right
   - In the OAuth2 section, click **"Authorize"**
   - You'll be redirected to Azure AD login page
   - Enter your Azure AD credentials
   - After successful login, you'll be redirected back to Swagger
   - The lock icon should now show as **closed**

4. **Test API endpoints**:
   - All endpoints now include the Authorization header automatically
   - You can test any endpoint with your authenticated session

### Manual Authentication (for other tools)

If you're using tools like Postman or curl, you need to:

1. **Get an access token** from Azure AD
2. **Include the token** in the Authorization header:
   ```
   Authorization: Bearer YOUR_ACCESS_TOKEN
   ```

### Token Information

- **Token Type**: JWT Bearer
- **Token Issuer**: Azure AD
- **Token Audience**: API Application ID
- **Token Expiration**: Typically 1 hour (configurable in Azure AD)

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

### Using Swagger UI with OAuth2

1. **Run the application**: `dotnet run`
2. **Navigate to Swagger UI**: `http://localhost:5217/swagger`
3. **Authenticate with OAuth2**:
   - Click the **"Authorize"** button at the top right
   - In the OAuth2 section, click **"Authorize"**
   - You'll be redirected to Azure AD login page
   - Enter your Azure AD credentials
   - After successful login, you'll be redirected back to Swagger
   - The lock icon should now show as **closed**
4. **Test API endpoints interactively** - All requests will include authentication automatically

### Using curl with OAuth2

**Note**: All API endpoints require authentication. You need to obtain an access token first.

#### Option 1: Get Token via Browser (Recommended for Testing)
1. Use Swagger UI to authenticate (steps above)
2. Open browser Developer Tools → Network tab
3. Make an API request in Swagger
4. Copy the `Authorization: Bearer <token>` header from the network request

#### Option 2: Get Token Programmatically
```bash
# Get access token (replace with your values)
curl -X POST "https://login.microsoftonline.com/YOUR_TENANT_ID/oauth2/v2.0/token" \
  -H "Content-Type: application/x-www-form-urlencoded" \
  -d "client_id=YOUR_CLIENT_ID&client_secret=YOUR_CLIENT_SECRET&scope=api://YOUR_CLIENT_ID/access_as_user&grant_type=client_credentials"
```

#### Making Authenticated API Calls
```bash
# Set your access token
TOKEN="YOUR_ACCESS_TOKEN_HERE"

# Get all clients (authenticated)
curl -X GET "http://localhost:5217/api/Client" \
  -H "Authorization: Bearer $TOKEN"

# Create a client (authenticated)
curl -X POST "http://localhost:5217/api/Client" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d '{
    "name": "Test User",
    "email": "test@example.com",
    "phone": "1234567890",
    "age": "25",
    "gender": "Male"
  }'

# Get specific client (authenticated)
curl -X GET "http://localhost:5217/api/Client/1" \
  -H "Authorization: Bearer $TOKEN"

# Update client (authenticated)
curl -X PUT "http://localhost:5217/api/Client/1" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d '{
    "name": "Updated User",
    "email": "updated@example.com",
    "phone": "0987654321",
    "age": "30",
    "gender": "Female"
  }'

# Delete client (authenticated)
curl -X DELETE "http://localhost:5217/api/Client/1" \
  -H "Authorization: Bearer $TOKEN"
```

### Expected Response for Unauthenticated Requests
```json
{
  "type": "https://tools.ietf.org/html/rfc7235#section-3.1",
  "title": "Unauthorized",
  "status": 401,
  "traceId": "00-..."
}
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
