#!/bin/bash
set -euo pipefail

# Script zum Zurücksetzen der Datenbank, Anwenden von Migrations und Laden von Fixtures
# Usage: ./reset-database.sh [--skip-fixtures]

SKIP_FIXTURES=false

# Parse arguments
while [[ $# -gt 0 ]]; do
    case $1 in
        --skip-fixtures)
            SKIP_FIXTURES=true
            shift
            ;;
        *)
            echo "Unknown option: $1"
            echo "Usage: $0 [--skip-fixtures]"
            exit 1
            ;;
    esac
done

# Outputs a formatted header with bold font and blue color
function h1() {
    local text="${*:-}"
    local BOLD BLUE RESET
    BOLD=$'\033[1m'
    BLUE=$'\033[34m'
    RESET=$'\033[0m'
    printf '\n%s%s%s\n' "${BOLD}${BLUE}" "-->  $text" "$RESET"
}

SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"
PROJECT_ROOT="$(cd "$SCRIPT_DIR/../.." && pwd)"
SERVER_DIR="$PROJECT_ROOT/server"
INFRASTRUCTURE_PROJECT="$SERVER_DIR/Ernaehrbar.Adapters.Infrastructure"
API_PROJECT="$SERVER_DIR/Ernaehrbar.Api"

h1 "Resetting Ernährbär Database"
echo "Project Root: $PROJECT_ROOT"
echo "Server Dir: $SERVER_DIR"
echo ""

# Prüfe ob dotnet ef installiert ist
h1 "Checking dotnet ef tools"
if ! dotnet tool list -g | grep -q "dotnet-ef"; then
    echo "⚠️  dotnet-ef not found. Installing..."
    dotnet tool install --global dotnet-ef
    if [ $? -ne 0 ]; then
        echo "❌ Failed to install dotnet-ef"
        exit 1
    fi
    echo "✅ dotnet-ef installed"
else
    echo "✅ dotnet-ef is installed"
fi

# Wechsle ins Server-Verzeichnis
cd "$SERVER_DIR"

# 1. Datenbank löschen
h1 "Dropping database"
if dotnet ef database drop --force --project "$INFRASTRUCTURE_PROJECT" --startup-project "$API_PROJECT"; then
    echo "✅ Database dropped"
else
    echo "⚠️  Database drop failed (might not exist)"
fi

# 2. Migrations anwenden
h1 "Applying migrations"
if ! dotnet ef database update --project "$INFRASTRUCTURE_PROJECT" --startup-project "$API_PROJECT"; then
    echo "❌ Failed to apply migrations"
    exit 1
fi
echo "✅ Migrations applied"

# 3. Fixtures laden (wenn vorhanden)
if [ "$SKIP_FIXTURES" = false ]; then
    h1 "Loading fixtures"
    
    # Prüfe ob ein Fixture-Projekt existiert
    FIXTURES_PROJECT="$SERVER_DIR/Ernaehrbar.Fixtures"
    if [ -d "$FIXTURES_PROJECT" ]; then
        echo "Found Fixtures project, loading..."
        
        # Load fixtures via dotnet run
        cd "$FIXTURES_PROJECT"
        # Check if executable exists, otherwise build first
        EXE_PATH="bin/Debug/net9.0/Ernaehrbar.Fixtures.exe"
        if [ -f "$EXE_PATH" ]; then
            if dotnet run --no-build 2>/dev/null; then
                echo "✅ Fixtures loaded successfully"
            else
                echo "⚠️  Failed to load fixtures"
                exit 1
            fi
        else
            if dotnet run 2>/dev/null; then
                echo "✅ Fixtures loaded successfully"
            else
                echo "⚠️  Failed to load fixtures"
                exit 1
            fi
        fi
            echo "✅ Fixtures loaded successfully"
        else
            echo "⚠️  Failed to load fixtures"
            exit 1
        fi
        cd "$SERVER_DIR"
    else
        echo "⚠️  No Fixtures project found (Ernaehrbar.Fixtures)"
        echo "   Skipping fixture loading"
    fi
else
    echo "⏭️  Skipping fixtures (--skip-fixtures flag)"
fi

h1 "Done"
echo "✅ Database reset and migrations applied successfully!"
