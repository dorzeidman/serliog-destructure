echo "build: Build started"

Push-Location $PSScriptRoot/SerliogTTransformer

& dotnet restore --no-cache




Pop-Location

