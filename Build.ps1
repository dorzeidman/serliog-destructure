echo "build: Build started"

Push-Location $PSScriptRoot

& dotnet restore SerliogTTransformer\SerliogTTransformer\SerliogTTransformer.csproj --no-cache

& dotnet build SerliogTTransformer\SerliogTTransformer\SerliogTTransformer.csproj --no-restore -c Release -o .\artifacs

& dotnet test SerliogTTransformer\SerliogTTransformer.Tests\SerliogTTransformer.Tests.csproj -c Release


Pop-Location

