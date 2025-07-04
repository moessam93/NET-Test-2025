# Secret Management & OpenID Connect SSO Setup Guide

## Overview
This application uses **Azure AD OpenID Connect** for Single Sign-On (SSO) authentication, which requires sensitive configuration values. **Never commit secrets to source control!**

## Azure AD App Registration Setup

### Step 1: Create Azure AD App Registration
1. Go to [Azure Portal](https://portal.azure.com) → Azure Active Directory → App registrations
2. Click **"New registration"**
3. **Name**: `NET-Test-2025-API`
4. **Supported account types**: Accounts in this organizational directory only
5. **Redirect URI**: 
   - Platform: **Web**
   - URL: `http://localhost:5217/signin-oidc`
6. Click **"Register"**

### Step 2: Configure Authentication
1. Go to **Authentication** section
2. **Redirect URIs**: Ensure `http://localhost:5217/signin-oidc` is configured
3. **Front-channel logout URL**: `https://localhost:5217/signout-oidc`
4. **Implicit grant and hybrid flows**: 
   - ✅ **ID tokens** (used for OpenID Connect)
   - ✅ **Access tokens** (optional, for API access)
5. Click **"Save"**

### Step 3: Create Client Secret
1. Go to **Certificates & secrets** → **Client secrets**
2. Click **"New client secret"**
3. **Description**: `NET-Test-2025-Development`
4. **Expires**: 24 months (recommended)
5. Click **"Add"**
6. **⚠️ IMPORTANT**: Copy the **Value** immediately (you won't see it again)

### Step 4: Configure API Permissions (Optional)
1. Go to **API permissions**
2. **Default permissions** already include:
   - `User.Read` (Microsoft Graph)
   - `openid`, `profile`, `email` (OpenID Connect)
3. Click **"Grant admin consent"** if prompted

## Development Setup (User Secrets)

### Step 1: Initialize User Secrets
```bash
cd Net-Test-2025
dotnet user-secrets init
```

### Step 2: Add Azure AD Client Secret
```bash
dotnet user-secrets set "AzureAd:ClientSecret" "YOUR_ACTUAL_CLIENT_SECRET_HERE"
```

### Step 3: Verify Secret is Set
```bash
dotnet user-secrets list
```

### Step 4: Verify Configuration
Check that your `appsettings.Development.json` has the correct values:
```json
{
  "AzureAd": {
    "Instance": "https://login.microsoftonline.com/",
    "Domain": "your-tenant.onmicrosoft.com",
    "TenantId": "your-tenant-id",
    "ClientId": "your-client-id",
    "ClientSecret": "",
    "CallbackPath": "/signin-oidc"
  }
}
```

## How OpenID Connect SSO Works

1. **User accesses protected resource** → `/api/Client`
2. **System redirects to Azure AD** → `https://login.microsoftonline.com/...`
3. **User authenticates with Azure AD** → Enters credentials
4. **Azure AD redirects back** → `/signin-oidc` with authentication token
5. **System creates session** → HTTP-only cookie
6. **User accesses resources** → Session maintained automatically

## Required Configuration

| Setting | Description | Example |
|---------|-------------|---------|
| `AzureAd:TenantId` | Azure AD Tenant ID | `635f8cf6-...` |
| `AzureAd:ClientId` | App Registration Client ID | `a3f08093-...` |
| `AzureAd:ClientSecret` | App Registration Client Secret | `whk8Q~...` (in User Secrets) |
| `AzureAd:CallbackPath` | OpenID Connect callback path | `/signin-oidc` |

## Production Setup

### Option 1: Environment Variables
```bash
export AzureAd__ClientSecret="YOUR_PRODUCTION_SECRET"
export AzureAd__TenantId="YOUR_PRODUCTION_TENANT_ID"
export AzureAd__ClientId="YOUR_PRODUCTION_CLIENT_ID"
```

### Option 2: Azure Key Vault
```csharp
builder.Configuration.AddAzureKeyVault(
    new Uri("https://your-keyvault.vault.azure.net/"),
    new DefaultAzureCredential());
```

### Option 3: Application Settings (Azure App Service)
Set in Azure Portal → App Service → Configuration:
- Key: `AzureAd:ClientSecret` | Value: `YOUR_PRODUCTION_SECRET`
- Key: `AzureAd:TenantId` | Value: `YOUR_PRODUCTION_TENANT_ID`
- Key: `AzureAd:ClientId` | Value: `YOUR_PRODUCTION_CLIENT_ID`

### Production Azure AD Configuration
For production deployment, create a separate app registration:
1. **Redirect URI**: `https://your-domain.com/signin-oidc`
2. **Front-channel logout URL**: `https://your-domain.com/signout-oidc`
3. **New client secret** for production environment

## Testing the SSO Setup

### Step 1: Run the Application
```bash
dotnet run
```

### Step 2: Test Authentication
1. Navigate to: `http://localhost:5217/api/Client`
2. You should be redirected to Azure AD login
3. Enter your Azure AD credentials
4. You should be redirected back and see the API response

### Step 3: Test User Info
```bash
# After authentication, check user info
curl http://localhost:5217/api/Auth/user
```

## Troubleshooting

### "invalid_client" Error
```bash
# Check client secret is set
dotnet user-secrets list

# Re-add if missing
dotnet user-secrets set "AzureAd:ClientSecret" "YOUR_SECRET"
```

### Redirect URI Mismatch
```bash
# Error: "AADSTS50011: The reply URL specified in the request does not match..."
# Solution: Ensure Azure AD app registration has exact redirect URI:
# http://localhost:5217/signin-oidc
```

### Authentication Loop
```bash
# Check callback path configuration
# Ensure appsettings.json has: "CallbackPath": "/signin-oidc"
# Verify Azure AD redirect URI matches exactly
```



## Security Best Practices

1. **NEVER** commit `appsettings.Development.json` with real secrets
2. **ALWAYS** use empty strings or placeholders in config files
3. **ROTATE** client secrets regularly (every 6-12 months)
4. **USE** different app registrations for dev/staging/production
5. **ENABLE** conditional access policies in Azure AD for production
6. **MONITOR** sign-in logs in Azure AD for security

## For New Team Members

1. Clone the repository
2. Follow Azure AD App Registration steps (or get existing app details)
3. Follow Development Setup steps 1-4
4. Get the actual client secret from team lead/Azure portal
5. Set it using `dotnet user-secrets set`
6. Run the application with `dotnet run`
7. Test by navigating to `http://localhost:5217/api/Client`

## Additional Resources

- [ASP.NET Core OpenID Connect](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/openid-connect)
- [Azure AD OpenID Connect](https://docs.microsoft.com/en-us/azure/active-directory/develop/v2-protocols-oidc)
- [ASP.NET Core User Secrets](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets)
- [Azure Key Vault Configuration](https://docs.microsoft.com/en-us/aspnet/core/security/key-vault-configuration)

---

*This setup provides true Single Sign-On authentication with Azure AD using OpenID Connect!* 