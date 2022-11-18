mkdir -p .\.nuget -ea 0

dotnet build -c Release
dotnet pack -c Release -o .\.nuget --no-build