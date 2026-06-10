param(
    [string]$InstallDir = "",
    [string]$Repo = "https://github.com/SaintSiboire/QwenAgent"
)

if (-not $InstallDir) {
    $InstallDir = Split-Path $MyInvocation.MyCommand.Path -Parent
}

Write-Host "🔄 Mise à jour de QwenAgent..." -ForegroundColor Cyan

$localVersion = ""
$versionFile = "$InstallDir\version.txt"
if (Test-Path $versionFile) {
    $localVersion = Get-Content $versionFile
}

$release = Invoke-RestMethod "$Repo/releases/latest"
$remoteVersion = $release.tag_name

if ($localVersion -eq $remoteVersion) {
    Write-Host "✔ Déjà à jour ($localVersion)" -ForegroundColor Green
    exit
}

Write-Host "📦 Nouvelle version trouvée : $remoteVersion"

$asset = $release.assets | Where-Object { $_.name -like "*.zip" }
$zipPath = "$env:TEMP\QwenAgent.zip"

Invoke-WebRequest $asset.browser_download_url -OutFile $zipPath

Expand-Archive $zipPath -DestinationPath $InstallDir -Force

Write-Host $remoteVersion | Set-Content "$InstallDir\version.txt"

Write-Host "🎉 Mise à jour terminée !" -ForegroundColor Green
