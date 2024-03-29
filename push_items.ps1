##
## This script will sync content items with Sitecore on first run, and then serialize it.
##
dotnet tool restore
dotnet sitecore login --cm https://cm.dreamteam.localhost/ --auth https://id.dreamteam.localhost/ --allow-write true

if ($LASTEXITCODE -ne 0) {
    Write-Error "Unable to log into Sitecore, did the Sitecore environment start correctly? See logs above."
}

Write-Host "Pushing items to Sitecore..." -ForegroundColor Green
dotnet sitecore ser push
	
if ($LASTEXITCODE -ne 0) {
  Write-Error "Serialization push failed, see errors above."
}
else{
  dotnet sitecore publish
}

