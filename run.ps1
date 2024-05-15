$env:DOTNET_WATCH_SUPPRESS_LAUNCH_BROWSER = 'true';
$env:DOTNET_WATCH_RESTART_ON_RUDE_EDIT = 'true';

Start-Process dotnet -ArgumentList @('watch', '--project', 'MyExample.WidgetApi/MyExample.WidgetApi.csproj') -WorkingDirectory $PSScriptRoot;
Start-Process dotnet -ArgumentList @('watch', '--project', 'MyExample.SprocketApi/MyExample.SprocketApi.csproj') -WorkingDirectory $PSScriptRoot;
