echo "build: Build started"

Push-Location $PSScriptRoot

$branch = @{ $true = $env:APPVEYOR_REPO_BRANCH; $false = $(git symbolic-ref --short -q HEAD) }[$env:APPVEYOR_REPO_BRANCH -ne $NULL];
$branch = $branch -replace '_', '-'

$revision = @{ $true = "{0:00000}" -f [convert]::ToInt32("0" + $env:APPVEYOR_BUILD_NUMBER, 10); $false = "local" }[$env:APPVEYOR_BUILD_NUMBER -ne $NULL];
$suffix = @{ $true = ""; $false = "$($branch.Substring(0, [math]::Min(10,$branch.Length)))-$revision"}[$branch -eq "master" -and $revision -ne "local"]
$commitHash = $(git rev-parse --short HEAD)
$buildSuffix = @{ $true = "$($suffix)-$($commitHash)"; $false = "$($branch)-$($commitHash)" }[$suffix -ne ""]


echo "build: Package version suffix is $suffix"
echo "build: Build version suffix is $buildSuffix" 

& dotnet restore SerliogTTransformer\SerliogTTransformer\SerliogTTransformer.csproj --no-cache

if ($buildSuffix) {
	& dotnet build SerliogTTransformer\SerliogTTransformer\SerliogTTransformer.csproj --no-restore --version-suffix=$buildSuffix
} else {
	& dotnet build SerliogTTransformer\SerliogTTransformer\SerliogTTransformer.csproj --no-restore
}
if($LASTEXITCODE -ne 0) { exit 1 }    

if ($suffix) {
	& dotnet pack SerliogTTransformer\SerliogTTransformer\SerliogTTransformer.csproj -c Release --include-symbols -o ..\..\artifacts --version-suffix=$suffix --no-build
} else {
	& dotnet pack SerliogTTransformer\SerliogTTransformer\SerliogTTransformer.csproj -c Release --include-symbols -o ..\..\artifacts --no-build
}
if($LASTEXITCODE -ne 0) { exit 1 }   

& dotnet test SerliogTTransformer\SerliogTTransformer.Tests\SerliogTTransformer.Tests.csproj -c Release


Pop-Location

