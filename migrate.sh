#!/bin/bash

# Script para crear migraciones automáticas con timestamp

if [ -z "$1" ]; then
    echo "Uso: ./migrate.sh <nombre_migracion>"
    echo "Ejemplo: ./migrate.sh AddCategory"
    exit 1
fi

# Genera timestamp
TIMESTAMP=$(date +%Y%m%d_%H%M%S)
MIGRATION_NAME="${1}_${TIMESTAMP}"

echo "📝 Creando migración: $MIGRATION_NAME"
dotnet ef migrations add "$MIGRATION_NAME"

if [ $? -eq 0 ]; then
    echo "✅ Migración creada. Iníciá la app para aplicarla:"
    echo "   dotnet run"
else
    echo "❌ Error al crear la migración"
    exit 1
fi
