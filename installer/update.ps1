param(
    [string]$InstallDir = "",
    [string]$Repo = "https://github.com/SaintSiboire/QwenAgent"
)

Write-Host "Mise à jour de QwenAgent..."

# Détecter le dossier d'installation si non fourni
if (-not $InstallDir) {
    $InstallDir = Split-Path $MyInvocation.MyCommand.Path -Parent
}

# Lire la version locale
$localVersion = ""
$versionFile = "$InstallDir\version.txt"
if (Test-Path $versionFile) {
    $localVersion = Get-Content $versionFile
}

# Lire la version distante
$release = Invoke-RestMethod "$Repo/releases/latest"
$remoteVersion = $release.tag_name

if ($localVersion -eq $remoteVersion) {
    Write-Host "Déjà à jour ($localVersion)"
    exit
}

Write-Host "Nouvelle version trouvée : $remoteVersion"

# Télécharger la nouvelle version
$asset = $release.assets | Where-Object { $_.name -like "*.zip" }
$zipPath = "$env:TEMP\QwenAgent.zip"

Invoke-WebRequest $asset.browser_download_url -OutFile $zipPath

# Extraire
Expand-Archive $zipPath -DestinationPath $InstallDir -Force

# Mettre à jour la version locale
$remoteVersion | Set-Content "$InstallDir\version.txt"

Write-Host "Mise à jour terminée."
