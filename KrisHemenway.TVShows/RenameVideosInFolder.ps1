param ([string]$path)
$body = @{ path=$path } | ConvertTo-Json;
Invoke-RestMethod -Uri http://localhost:8090/api/tvshows/rename -Method POST -ContentType "application/json" -Body $body