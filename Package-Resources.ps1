$basePath = "src\DoubleJay.Epi.EnhancedPropertyList"
$path = "$basePath\Properties\AssemblyInfo.cs"
$pattern = '^\[assembly: AssemblyVersion\("(.*)"\)\]'

[version]$version = $null

(Get-Content $path) | foreach {
    if($_ -match $pattern){
        $version = [version]$matches[1]
    }
}

# If we couldn't get a version we just return
if (!$version) {
    return
}

# Patch the version in the module.config
$moduleConfig = (Get-Content "$basePath\module.config") | foreach {$_.replace("`$version$",$version)}
$moduleConfig | Out-File "$basePath\bin\module.config" -Encoding utf8

$binPath = "$basePath\bin"
$tempBinPath = "$binPath\$version"

if (Test-Path $tempBinPath) { Remove-Item $tempBinPath -Recurse }

# Create a temp folder with the correct version, copy ClientResources into it
New-Item -ItemType Directory -Path $tempBinPath
Copy-Item -Path "$basePath\ClientResources" -Destination $tempBinPath -Recurse

# Compress our resources
Compress-Archive -Path $tempBinPath, "$binPath\module.config" -DestinationPath "$binPath\DoubleJay.Epi.EnhancedPropertyList.zip" -Force

# Clean-up
if (Test-Path $tempBinPath) { Remove-Item $tempBinPath -Recurse }
Remove-Item "$binPath\module.config"