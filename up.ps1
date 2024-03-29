$ErrorActionPreference = "Stop";

$stopwatch = New-Object System.Diagnostics.Stopwatch
$stopwatch.Start()

# Build all containers in the Sitecore instance, forcing a pull of latest base containers
Write-Host "Building containers..." -ForegroundColor Green
docker-compose build --progress plain
if ($LASTEXITCODE -ne 0) {
    Write-Error "Container build failed, see errors above."
}

Write-Host "Docker pre start took $($stopwatch.Elapsed.Minutes) minutes $($stopwatch.Elapsed.Seconds) seconds" -ForegroundColor Yellow

# Start the Sitecore instance
Write-Host "Starting Sitecore environment..." -ForegroundColor Green
docker-compose up -d

# Wait for Traefik to expose CM route
Write-Host "Waiting for CM to become available..." -ForegroundColor Green

do {
    Start-Sleep -Milliseconds 30000
    try {
        $status = Invoke-RestMethod "http://localhost:8079/api/http/routers/cm-secure@docker"
    } catch {
        if ($_.Exception.Response.StatusCode.value__ -ne "404") {
            throw
        }
    }
} while ($status.status -ne "enabled" -and $startTime.AddSeconds(15) -gt (Get-Date))
if (-not $status.status -eq "enabled") {
    $status
    Write-Error "Timeout waiting for Sitecore CM to become available via Traefik proxy. Check CM container logs."
}

$stopwatch.Stop()
Write-Host "Docker start took $($stopwatch.Elapsed.Minutes) minutes $($stopwatch.Elapsed.Seconds) seconds" -ForegroundColor Yellow

dotnet tool restore
dotnet sitecore login --cm https://cm.dreamteam.localhost/ --auth https://id.dreamteam.localhost/ --allow-write true

if ($LASTEXITCODE -ne 0) {
    Write-Error "Unable to log into Sitecore, did the Sitecore environment start correctly? See logs above."
}

# Populate Solr managed schemas to avoid errors during item deploy
Write-Host "Populating Solr managed schema..." -ForegroundColor Green
$token = (Get-Content .\.sitecore\user.json | ConvertFrom-Json).endpoints.default.accessToken
Invoke-RestMethod "https://cm.dreamteam.localhost/sitecore/admin/PopulateManagedSchema.aspx?indexes=sitecore_master_index|sitecore_core_index|sitecore_web_index" -Headers @{Authorization = "Bearer $token"} -UseBasicParsing | Out-Null

Write-Host "Pushing items to Sitecore..." -ForegroundColor Green
dotnet sitecore ser push

Write-Host "Opening site..." -ForegroundColor Green

Start-Process https://cm.dreamteam.localhost/sitecore/

Write-Host ""
Write-Host "Use the following command to monitor your Rendering Host:" -ForegroundColor Green
Write-Host "docker-compose logs -f rendering"
Write-Host ""