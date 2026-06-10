param(
    [string]$InstallDir = ""
)

# Forcer TLS 1.2 pour GitHub
[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12

# User-Agent obligatoire pour GitHub API
$headers = @{
    "User-Agent" = "QwenAgentInstaller"
}

Write-Host "Installation de QwenAgent..."

# 1. Demander le dossier si non fourni
if (-not $InstallDir) {
    Write-Host "Où veux-tu installer QwenAgent ?"
    Write-Host "Exemples : C:\DevTools\QwenAgent ou D:\Tools\QwenAgent"
    $InstallDir = Read-Host "Chemin d'installation"
}

# 2. Créer le dossier si nécessaire
if (-not (Test-Path $InstallDir)) {
    New-Item -ItemType Directory -Path $InstallDir | Out-Null
}

# 3. Télécharger la dernière release GitHub (méthode robuste)
$RepoApi = "https://api.github.com/repos/SaintSiboire/QwenAgent"
Write-Host "Téléchargement de la dernière version..."

try {
    # On récupère toutes les releases (car /latest est buggé)
    $releases = Invoke-RestMethod "$RepoApi/releases" -Headers $headers
} catch {
    Write-Host "Erreur : impossible de contacter GitHub."
    exit 1
}

# On prend la première release stable (pas draft, pas prerelease)
$release = $releases | Where-Object { -not $_.draft -and -not $_.prerelease } | Select-Object -First 1

if ($release -eq $null) {
    Write-Host "Erreur : aucune release stable trouvée."
    exit 1
}

Write-Host "Version trouvée : $($release.tag_name)"

# On récupère l'asset ZIP
$asset = $release.assets | Where-Object { $_.name -like "*.zip" } | Select-Object -First 1

if ($asset -eq $null) {
    Write-Host "Erreur : aucun fichier ZIP trouvé dans la release."
    exit 1
}

# Téléchargement
$zipPath = "$env:TEMP\QwenAgent.zip"
Invoke-WebRequest $asset.browser_download_url -OutFile $zipPath -Headers $headers

# 4. Extraire
Write-Host "Extraction..."
Expand-Archive $zipPath -DestinationPath $InstallDir -Force

# 5. Écrire la version
$release.tag_name | Set-Content "$InstallDir\version.txt"

# 6. Créer le lanceur global
$l = "$InstallDir\qwen-fix.cmd"
@"
@echo off
"$InstallDir\QwenAgent.Cli.exe" %*
"@ | Set-Content $l

# 7. Ajouter au PATH
Write-Host "Ajout au PATH..."
$oldPath = [Environment]::GetEnvironmentVariable("PATH", "User")
if ($oldPath -notlike "*$InstallDir*") {
    [Environment]::SetEnvironmentVariable("PATH", "$oldPath;$InstallDir", "User")
}

Write-Host "Installation terminée. Ouvre un nouveau terminal et tape : qwen-fix"
