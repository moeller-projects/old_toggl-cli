$root = $(Get-Location).Path
$solution = "$($root)\..\src\toggl-cli.sln"
$build = "$($root)\..\build\toggl-cli"
$configuration = "Release"
$runtime = "win-x86"
$framework = "net471"

dotnet restore $solution
dotnet clean $solution
dotnet restore $solution
dotnet build --output $build --configuration $configuration --runtime $runtime --framework $framework $solution