$root = $(Get-Location).Path
$src = "$($root)\..\src\"
$pattern = "*Unit.Tests.csproj"
$test = "$($root)\..\test\toggl-cli"
$configuration = "Release"
$framework = "net471"

Get-ChildItem $src -Filter $pattern -Recurse -File | ForEach-Object { dotnet test --output $test --configuration $configuration --framework $framework --verbosity normal $_.FullName }