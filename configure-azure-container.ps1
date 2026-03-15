# Configure Azure Web App to use Docker Container from GitHub Container Registry
# This script configures the Azure Web App to pull the container image

$webAppName = "oneentrance"
$containerImage = "ghcr.io/adyrin-cloud/northendoneentrance:latest"
$username = "`$oneentrance"
$password = "rQhnE0Z94GlmrSeQLKZrcZTbNepaTmi6J5ENnfXqLesqTiW2pAycXyuxExgF"

Write-Host "Configuring Azure Web App: $webAppName" -ForegroundColor Cyan
Write-Host "Container Image: $containerImage" -ForegroundColor Cyan

# Create authentication header
$base64AuthInfo = [Convert]::ToBase64String([Text.Encoding]::ASCII.GetBytes("${username}:${password}"))
$headers = @{
    Authorization = "Basic $base64AuthInfo"
    "Content-Type" = "application/json"
}

# Configuration payload for Linux container
$config = @{
    properties = @{
        siteConfig = @{
            linuxFxVersion = "DOCKER|$containerImage"
            appSettings = @(
                @{
                    name = "DOCKER_REGISTRY_SERVER_URL"
                    value = "https://ghcr.io"
                }
                @{
                    name = "DOCKER_ENABLE_CI"
                    value = "true"
                }
                @{
                    name = "WEBSITES_PORT"
                    value = "80"
                }
            )
        }
    }
} | ConvertTo-Json -Depth 10

try {
    # Update web app configuration
    $response = Invoke-RestMethod `
        -Uri "https://$webAppName.scm.azurewebsites.net/api/settings" `
        -Method Post `
        -Headers $headers `
        -Body $config
    
    Write-Host "`nConfiguration updated successfully!" -ForegroundColor Green
    Write-Host "Restarting web app..." -ForegroundColor Yellow
    
    # Restart the app
    Invoke-RestMethod `
        -Uri "https://$webAppName.scm.azurewebsites.net/api/app/restart" `
        -Method Post `
        -Headers $headers
    
    Write-Host "`nWeb app restarted!" -ForegroundColor Green
    Write-Host "`nYour app should be available at: https://$webAppName.azurewebsites.net" -ForegroundColor Cyan
    Write-Host "Note: It may take 2-3 minutes for the container to pull and start." -ForegroundColor Yellow
}
catch {
    Write-Host "`nError configuring web app:" -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Red
    Write-Host "`nPlease configure manually via Azure Portal:" -ForegroundColor Yellow
    Write-Host "1. Go to https://portal.azure.com" -ForegroundColor White
    Write-Host "2. Navigate to your Web App: $webAppName" -ForegroundColor White
    Write-Host "3. Go to 'Deployment Center'" -ForegroundColor White
    Write-Host "4. Select 'Container' settings" -ForegroundColor White
    Write-Host "5. Choose 'Other Registry'" -ForegroundColor White
    Write-Host "6. Image source: $containerImage" -ForegroundColor White
    Write-Host "7. Registry server URL: https://ghcr.io" -ForegroundColor White
    Write-Host "8. Make the repository public or add registry credentials" -ForegroundColor White
    Write-Host "9. Save the configuration" -ForegroundColor White
}
