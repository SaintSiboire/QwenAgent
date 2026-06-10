param(
    [string]$InstallDir = ""
)

Write-Host "Désinstallation de QwenAgent..."

# Demander le dossier si non fourni
if (-not $InstallDir) {
    $InstallDir = Read-Host "Chemin d'installation actuel de QwenAgent"
}

# Supprimer le dossier
if (Test-Path $InstallDir) {
    Remove-Item $InstallDir -Recurse -Force
    Write-Host "Dossier supprimé."
} else {
    Write-Host "Dossier introuvable."
}

Write-Host "Si nécessaire, retire manuellement le dossier du PATH."
