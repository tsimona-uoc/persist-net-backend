# Script para crear migraciones automáticas con timestamp

param(
    [Parameter(Mandatory=$true)]
    [string]$MigrationName
)

# Genera timestamp
$timestamp = Get-Date -Format "yyyyMMdd_HHmmss"
$fullMigrationName = "${MigrationName}_${timestamp}"

Write-Host "📝 Creando migración: $fullMigrationName" -ForegroundColor Cyan

# Ejecuta el comando de migración
dotnet ef migrations add "$fullMigrationName" --output-dir Data/Migrations

# Verifica si fue exitoso
if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Migración creada. Inicia la app para aplicarla:" -ForegroundColor Green
    Write-Host "   dotnet run" -ForegroundColor Yellow
} else {
    Write-Host "❌ Error al crear la migración" -ForegroundColor Red
    exit 1
}
