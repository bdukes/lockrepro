param([int]$DelaySeconds = 30)

git fetch --all;

. "$PSScriptRoot/run.ps1";

while ($true)
{
    git switch one;
    Start-Sleep -Seconds $DelaySeconds;
    git switch two;
    Start-Sleep -Seconds $DelaySeconds;
    git switch three;
    Start-Sleep -Seconds $DelaySeconds;
}
