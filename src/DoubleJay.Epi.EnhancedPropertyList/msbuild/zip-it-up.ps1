Add-Type -assembly "system.io.compression.filesystem"

$moduleName = "DoubleJay.Epi.EnhancedPropertyList"
$source = $PSScriptRoot + "\modules\_protected\" + $moduleName
$destination = $PSScriptRoot + "\" + $moduleName + ".zip"

If(Test-path $destination) {Remove-item $destination}

[io.compression.zipfile]::CreateFromDirectory($Source, $destination)