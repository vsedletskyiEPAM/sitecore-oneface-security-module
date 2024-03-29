#---------------------------------------------------
## stop and clean up
#---------------------------------------------------
$stopwatch = New-Object System.Diagnostics.Stopwatch
$stopwatch.Start()

docker-compose down
docker system prune -f


$stopwatch.Stop()
Write-Host "Docker stop.ps1 took $($stopwatch.Elapsed.Minutes) minutes $($stopwatch.Elapsed.Seconds) seconds" -ForegroundColor Yellow