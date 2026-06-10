param(
    [string]$InstallDir = ""
)

Write-Host "🚀 Installation de QwenAgent" -ForegroundColor Cyan

# 1. Demander le dossier si non fourni
if (-not $InstallDir) {
    Write-Host "Où veux-tu installer QwenAgent ?" -ForegroundColor Cyan
    Write-Host "Exemples : C:\DevTools\QwenAgent ou D:\Tools\QwenAgent"
    $InstallDir = Read-Host "Chemin d'installation"
}

# 2. Créer le dossier si nécessaire
if (-not (Test-Path $InstallDir)) {
    New-Item -ItemType Directory -Path $InstallDir | Out-Null
}

# 3. Télécharger la dernière release GitHub
$Repo = "https://github.com/SaintSiboire/QwenAgent"
Write-Host "📦 Téléchargement de la dernière version..."
$release = Invoke-RestMethod "$Repo/releases/latest"
$asset = $release.assets | Where-Object { $_.name -like "*.zip" }
$zipPath = "$env:TEMP\QwenAgent.zip"

Invoke-WebRequest $asset.browser_download_url -OutFile $zipPath

# 4. Extraire
Write-Host "📂 Extraction..."
Expand-Archive $zipPath -DestinationPath $InstallDir -Force

# 5. Écrire la version
Write-Host $release.tag_name | Set-Content "$InstallDir\version.txt"

# 6. Créer le lanceur global
$l = "$InstallDir\qwen-fix.cmd"
@"
@echo off
"$InstallDir\QwenAgent.Cli.exe" %*
"@ | Set-Content $l

# 7. Ajouter au PATH
Write-Host "🔧 Ajout au PATH..."
$oldPath = [Environment]::GetEnvironmentVariable("PATH", "User")
if ($oldPath -notlike "*$InstallDir*") {
    [Environment]::SetEnvironmentVariable("PATH", "$oldPath;$InstallDir", "User")
}

Write-Host "🎉 Installation terminée ! Ouvre un nouveau terminal et tape : qwen-fix" -ForegroundColor Green
