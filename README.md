# NET-Test-2025

A .NET 8 Web API project for client management with CRUD operations, built with clean architecture principles and secured with Azure AD OpenID Connect Single Sign-On (SSO).

## Features

- **Clean Architecture**: Separated concerns with Controllers, Services, and DTOs
- **Entity Framework Core**: Code-first approach with SQL Server
- **Input Validation**: Model validation with custom attributes
- **Error Handling**: ServiceResult pattern for consistent error responses
- **OpenID Connect SSO**: Secured with Azure AD Single Sign-On authentication
- **Cookie Authentication**: Session-based authentication with OpenID Connect
- **Swagger UI**: Interactive API documentation with OpenID Connect authentication
- **Soft Delete**: Clients are marked as deleted rather than physically removed

## Technology Stack

- **.NET 8** - Web API framework
- **Entity Framework Core** - ORM for database operations
- **SQL Server LocalDB** - Database engine
- **Azure AD (Entra ID)** - OpenID Connect identity provider
- **OpenID Connect** - Authentication protocol for SSO
- **Cookie Authentication** - Session management
- **Swagger/OpenAPI** - API documentation with OpenID Connect integration

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server LocalDB](https://docs.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb) (included with Visual Studio)
- **Azure AD Tenant** - For OpenID Connect authentication
- **Azure AD App Registration** - Required for SSO authentication

## Authentication

This API uses **OpenID Connect with Azure AD** for Single Sign-On (SSO) authentication. All API endpoints require users to be authenticated through Azure AD.

### How SSO Works

1. **User attempts to access protected resource**
2. **System redirects to Azure AD login page**
3. **User authenticates with Azure AD credentials**
4. **Azure AD redirects back with authentication token**
5. **System creates authenticated session (cookie)**
6. **User can access all protected resources without re-authentication**

### Authentication Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/Auth/login` | Initiates SSO login process |
| POST | `/api/Auth/logout` | Logs out user and terminates session |
| GET | `/api/Auth/user` | Get current authenticated user info |

### Authenticating via Browser

1. **Run the application**:
   ```bash
   dotnet run
   ```

2. **Navigate to any protected endpoint**:
   ```
   http://localhost:5217/api/Client
   ```

3. **Automatic SSO Login**:
   - You'll be automatically redirected to Azure AD login page
   - Enter your Azure AD credentials
   - After successful login, you'll be redirected back to the API
   - Your session will be maintained via secure cookies

4. **Access Swagger UI**:
   ```
   http://localhost:5217/swagger
   ```
   - Click "Authorize" to authenticate if needed
   - All API calls will use your authenticated session

### Manual Login (Optional)

If you need to manually trigger the login process:
```
GET http://localhost:5217/api/Auth/login
```

### Session Management

- **Session Duration**: Determined by Azure AD configuration
- **Session Storage**: Secure HTTP-only cookies
- **Session Renewal**: Automatic when token expires
- **Logout**: Clears both local session and Azure AD session

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

5. **Configure Azure AD settings** (see SECRETS_SETUP.md)

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

## API Endpoints

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
│   ├── ClientController.cs
│   └── AuthController.cs
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

### Using Swagger UI with OpenID Connect

1. **Run the application**: `dotnet run`
2. **Navigate to Swagger UI**: `http://localhost:5217/swagger`
3. **Authenticate with OpenID Connect**:
   - If not already authenticated, you'll be redirected to Azure AD login
   - Enter your Azure AD credentials
   - After successful login, you'll be redirected back to Swagger
   - Click **"Authorize"** button if needed to enable authentication for API calls
4. **Test API endpoints interactively** - All requests will use your authenticated session

### Using Browser for Direct API Access

```bash
# Open browser and navigate to:
http://localhost:5217/api/Client

# You'll be redirected to Azure AD for authentication
# After login, you'll see the API response
```

### Using curl with Session Authentication

**Note**: For programmatic access, you'll need to handle the OpenID Connect flow or use the API after browser authentication.

#### Option 1: Browser-based Authentication (Recommended)
1. Open browser and navigate to: `http://localhost:5217/api/Auth/login`
2. Complete Azure AD authentication
3. Copy session cookies from browser developer tools
4. Use cookies in curl requests

#### Option 2: Session-based API Calls
```bash
# Get current user info (requires authentication)
curl -X GET "http://localhost:5217/api/Auth/user" \
  -H "Cookie: YOUR_SESSION_COOKIES"

# Get all clients (requires authentication)
curl -X GET "http://localhost:5217/api/Client" \
  -H "Cookie: YOUR_SESSION_COOKIES"
```

### Expected Response for Unauthenticated Requests
```
HTTP/1.1 302 Found
Location: https://login.microsoftonline.com/[tenant-id]/oauth2/v2.0/authorize?...
```

Users are redirected to Azure AD login page instead of receiving a 401 error.

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

### Authentication Issues

**Problem**: Redirected to Azure AD but authentication fails
**Solution**: 
- Check Azure AD app registration settings
- Ensure redirect URI is configured: `http://localhost:5217/signin-oidc`
- Verify client secret is configured (see SECRETS_SETUP.md)

**Problem**: "invalid_client" error
**Solution**: 
- Verify ClientId in appsettings.json matches Azure AD app registration
- Check client secret is properly configured in User Secrets

**Problem**: Infinite redirect loop
**Solution**: 
- Check callback path configuration: `/signin-oidc`
- Ensure Azure AD app registration has correct redirect URI

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

- The project uses **OpenID Connect** for true Single Sign-On authentication
- **Session-based authentication** with secure HTTP-only cookies
- **Azure AD integration** for enterprise-grade security
- The project uses **soft delete** - clients are marked as deleted rather than removed
- All API responses follow the **ServiceResult pattern** for consistent error handling
- **Model validation** is automatically applied using Data Annotations
- **Swagger UI** launches automatically in development mode
