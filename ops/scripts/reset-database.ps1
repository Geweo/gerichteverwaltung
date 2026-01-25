#!/usr/bin/env pwsh
# Script zum Zurücksetzen der Datenbank, Anwenden von Migrations und Laden von Fixtures
# Usage: ./reset-database.ps1 [--skip-fixtures]

param(
    [switch]$SkipFixtures = $false
)

$ErrorActionPreference = "Stop"

# Outputs a formatted header with bold font and blue color
function Write-Header {
    param([string]$Text)
    Write-Host "`n-->  $Text" -ForegroundColor Cyan
}

$SCRIPT_DIR = Split-Path -Parent $MyInvocation.MyCommand.Path
$PROJECT_ROOT = Split-Path -Parent (Split-Path -Parent $SCRIPT_DIR)
$SERVER_DIR = Join-Path $PROJECT_ROOT "server"
$INFRASTRUCTURE_PROJECT = Join-Path $SERVER_DIR "Ernaehrbar.Adapters.Infrastructure"
$API_PROJECT = Join-Path $SERVER_DIR "Ernaehrbar.Api"

Write-Header "Resetting Ernährbär Database"
Write-Host "Project Root: $PROJECT_ROOT"
Write-Host "Server Dir: $SERVER_DIR"
Write-Host ""

# Prüfe ob dotnet ef installiert ist
Write-Header "Checking dotnet ef tools"
$efInstalled = dotnet tool list -g | Select-String "dotnet-ef"
if (-not $efInstalled) {
    Write-Host "⚠️  dotnet-ef not found. Installing..." -ForegroundColor Yellow
    dotnet tool install --global dotnet-ef
    if ($LASTEXITCODE -ne 0) {
        Write-Host "❌ Failed to install dotnet-ef" -ForegroundColor Red
        exit 1
    }
    Write-Host "✅ dotnet-ef installed" -ForegroundColor Green
} else {
    Write-Host "✅ dotnet-ef is installed" -ForegroundColor Green
}

# Wechsle ins Server-Verzeichnis
Push-Location $SERVER_DIR

try {
    # 1. Datenbank löschen
    Write-Header "Dropping database"
    dotnet ef database drop --force --project $INFRASTRUCTURE_PROJECT --startup-project $API_PROJECT
    if ($LASTEXITCODE -ne 0) {
        Write-Host "⚠️  Database drop failed (might not exist)" -ForegroundColor Yellow
    } else {
        Write-Host "✅ Database dropped" -ForegroundColor Green
    }

    # 2. Migrations anwenden
    Write-Header "Applying migrations"
    dotnet ef database update --project $INFRASTRUCTURE_PROJECT --startup-project $API_PROJECT
    if ($LASTEXITCODE -ne 0) {
        Write-Host "❌ Failed to apply migrations" -ForegroundColor Red
        exit 1
    }
    Write-Host "✅ Migrations applied" -ForegroundColor Green

    # 3. Fixtures laden (wenn vorhanden)
    if (-not $SkipFixtures) {
        Write-Header "Loading fixtures"
        
        # Prüfe ob ein Fixture-Projekt existiert
        $FIXTURES_PROJECT = Join-Path $SERVER_DIR "Ernaehrbar.Fixtures"
        if (Test-Path $FIXTURES_PROJECT) {
            Write-Host "Found Fixtures project, loading..." -ForegroundColor Cyan
            
            # Load fixtures via dotnet run
            Push-Location $FIXTURES_PROJECT
            try {
                dotnet run --no-build 2>$null
                if ($LASTEXITCODE -eq 0) {
                    Write-Host "✅ Fixtures loaded successfully" -ForegroundColor Green
                } else {
                    dotnet run 2>$null
                    if ($LASTEXITCODE -eq 0) {
                        Write-Host "✅ Fixtures loaded successfully" -ForegroundColor Green
                    } else {
                        Write-Host "⚠️  Failed to load fixtures" -ForegroundColor Red
                        exit 1
                    }
                }
            }
            finally {
                Pop-Location
            }
        } else {
            Write-Host "⚠️  No Fixtures project found (Ernaehrbar.Fixtures)" -ForegroundColor Yellow
            Write-Host "   Skipping fixture loading" -ForegroundColor Yellow
        }
    } else {
        Write-Host "⏭️  Skipping fixtures (--skip-fixtures flag)" -ForegroundColor Yellow
    }

    Write-Header "Done"
    Write-Host "✅ Database reset and migrations applied successfully!" -ForegroundColor Green
}
finally {
    Pop-Location
}
