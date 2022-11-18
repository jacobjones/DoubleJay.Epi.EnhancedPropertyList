$version = "2.0.0"

mkdir -p .\.nuget -ea 0

dotnet build -c Release /p:Version=$version
dotnet pack -c Release -o .\.nuget /p:Version=$version --no-build