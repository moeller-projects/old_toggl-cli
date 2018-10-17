$root = $(Get-Location).Path
$solution = "$($root)\..\src\togglhelper\togglhelper.csproj"
$pack = "$($root)\..\pack\nuget"
$configuration = "Release"
$runtime = "netcoreapp2.1"

dotnet restore $solution
dotnet clean $solution
dotnet restore $solution
dotnet pack --output $pack --configuration $configuration --runtime $runtime $solution