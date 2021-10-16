<#
    .SYNOPSIS
        Create az resources
    .DESCRIPTION

        Sets up
        1) App Service plan        
#>

param(
    [Parameter(Mandatory = $true)]
    [ValidateNotNullOrEmpty()]
    [String]$appServicePlan,
    [Parameter(Mandatory = $true)]
    [ValidateNotNullOrEmpty()]
    [String]$appServiceName,
    [Parameter(Mandatory = $true)]
    [ValidateNotNullOrEmpty()]
    [String]$resourceGroup
)

# Create resrouce group
az group create `
    --location eastus `
    --name $resourceGroup

# Create app service plan
az appservice plan create `
    --resource-group $resourceGroup `
    --name $appServicePlan 

# Create app service
az webapp create `
    --name $appServiceName `
    --plan $appServicePlan `
    --resource-group $resourceGroup `
    --runtime 'DOTNETCORE:3.1'