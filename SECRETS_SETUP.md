# Secret Management Setup Guide

## Overview
This application uses Azure AD authentication which requires sensitive configuration values. **Never commit secrets to source control!**

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

## How It Works

- **User Secrets** stores sensitive data outside your project directory
- **Location**: `~/.microsoft/usersecrets/{user-secrets-id}/secrets.json`
- **Security**: Only accessible to your local user account
- **Source Control**: Never committed to git

## Production Setup

### Option 1: Environment Variables
```bash
export AzureAd__ClientSecret="YOUR_PRODUCTION_SECRET"
```

### Option 2: Azure Key Vault
```csharp
builder.Configuration.AddAzureKeyVault(
    new Uri("https://your-keyvault.vault.azure.net/"),
    new DefaultAzureCredential());
```

### Option 3: Application Settings (Azure App Service)
Set in Azure Portal → App Service → Configuration:
- Key: `AzureAd:ClientSecret`
- Value: `YOUR_PRODUCTION_SECRET`

## Required Secrets

| Secret | Description | Example |
|--------|-------------|---------|
| `AzureAd:ClientSecret` | Azure AD App Registration Client Secret | `somesecretkey` |

## Troubleshooting

### Secret Not Found
```bash
# Check if secret exists
dotnet user-secrets list

# Re-add if missing
dotnet user-secrets set "AzureAd:ClientSecret" "YOUR_SECRET"
```

### Permission Issues
```bash
# Check user secrets location
echo $HOME/.microsoft/usersecrets/

# Verify permissions
ls -la ~/.microsoft/usersecrets/*/
```

## Additional Resources

- [ASP.NET Core User Secrets](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets)
- [Azure Key Vault Configuration](https://docs.microsoft.com/en-us/aspnet/core/security/key-vault-configuration)
- [Environment Variables in .NET](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/)

## Important Notes

1. **NEVER** commit `appsettings.Development.json` with real secrets
2. **ALWAYS** use empty strings or placeholders in config files
3. **SHARE** this setup guide with team members
4. **ROTATE** secrets regularly for security

## For New Team Members

1. Clone the repository
2. Follow steps 1-3 above
3. Get the actual client secret from team lead/Azure portal
4. Set it using `dotnet user-secrets set`
5. Run the application with `dotnet run`

---

*This setup ensures your secrets are secure and never accidentally committed to source control!* 