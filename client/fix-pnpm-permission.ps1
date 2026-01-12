# PowerShell Script zum Beheben des EPERM-Fehlers bei pnpm

Write-Host "🔍 Suche nach gesperrten Dateien..." -ForegroundColor Cyan

$workspaceStateFile = "node_modules\.pnpm-workspace-state-v1.json"

if (Test-Path $workspaceStateFile)
{
    Write-Host "📁 Datei gefunden: $workspaceStateFile" -ForegroundColor Yellow
    
    try
    {
        # Versuche die Datei zu löschen
        Remove-Item $workspaceStateFile -Force -ErrorAction Stop
        Write-Host "✅ Datei erfolgreich gelöscht!" -ForegroundColor Green
    }
    catch
    {
        Write-Host "❌ Datei ist gesperrt. Versuche Prozesse zu finden..." -ForegroundColor Red
        
        # Finde Prozesse, die auf node_modules zugreifen
        $nodeProcesses = Get-Process | Where-Object {
            $_.Path -like "*node*" -and 
            $_.Path -notlike "*cursor*" -and
            $_.Path -notlike "*nvidia*"
        }
        
        if ($nodeProcesses)
        {
            Write-Host "`n⚠️  Gefundene Node-Prozesse:" -ForegroundColor Yellow
            $nodeProcesses | Format-Table Id, ProcessName, Path
            
            Write-Host "`n💡 Lösung:" -ForegroundColor Cyan
            Write-Host "1. Beende alle Node-Prozesse (z.B. Dev-Server, Build-Prozesse)" -ForegroundColor White
            Write-Host "2. Oder führe dieses Skript als Administrator aus" -ForegroundColor White
            Write-Host "3. Oder lösche die Datei manuell im Explorer" -ForegroundColor White
        }
        else
        {
            Write-Host "❌ Keine Node-Prozesse gefunden, die die Datei sperren könnten." -ForegroundColor Red
            Write-Host "💡 Versuche, die Datei als Administrator zu löschen." -ForegroundColor Yellow
        }
    }
}
else
{
    Write-Host "ℹ️  Datei existiert nicht. Das Problem könnte bereits behoben sein." -ForegroundColor Green
}

Write-Host "`n🚀 Versuche jetzt, die Komponenten zu installieren..." -ForegroundColor Cyan
Write-Host "Führe aus: pnpm dlx shadcn@latest add button --yes" -ForegroundColor White
