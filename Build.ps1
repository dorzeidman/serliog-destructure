echo "build: Build started"

Push-Location $PSScriptRoot/SerliogTTransformer

& dotnet restore --no-cache

& dotnet build --no-restore -c Release

& dotnet test -c Release


Pop-Location

