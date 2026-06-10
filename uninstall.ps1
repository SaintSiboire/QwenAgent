param(
    [string]$InstallDir = ""
)

if (-not $InstallDir) {
    $InstallDir = Read-Host "Chemin d'installation actuel de QwenAgent"
}

Write-Host "🗑 Désinstallation de QwenAgent..." -ForegroundColor Yellow

if (Test-Path $InstallDir) {
    Remove-Item $InstallDir -Recurse -Force
    Write-Host "✔ Dossier supprimé."
} else {
    Write-Host "ℹ Dossier introuvable."
}

Write-Host "⚠ Pense à retirer $InstallDir du PATH si nécessaire." -ForegroundColor Yellow
