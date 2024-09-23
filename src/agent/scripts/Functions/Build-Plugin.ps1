function Build-Plugin {
    Write-Host "Building declarative agent"
    Push-Location "../"

    # If node_modules folder is not present then install the dependencies
    if(!(Test-Path "node_modules")) 
    {
        npm install
    }

    # Compile the API Description
    tsp compile main.tsp

    $FirstTime = !(Test-Path ".kiota")

    # TODO: Fix single plugin generation because no namespace is generated
    Get-ChildItem -Path ".generated/openapi" -Filter "examplesapi.json" | ForEach-Object {
        $FileName = $_.BaseName.Split('.')
        $PluginName = $FileName[$FileName.Length - 1]

        if($FirstTime) 
        {
            kiota plugin add -d $_.FullName --plugin-name $PluginName --output .generated/plugins --type apiplugin
        }
        else 
        {
            kiota plugin generate --plugin-name $PluginName --refresh
        }

        Copy-Item -Path ".generated/plugins/*" -Include *.json,*.yml -Destination "appPackage" -Recurse -Force
    }

    Copy-Item -Path ".generated/openapi/declarativeAgent.json" -Destination "appPackage/declarativeAgent.json"  -Force
    Pop-Location
}