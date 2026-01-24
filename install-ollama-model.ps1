# Install Ollama Model Script
# This script installs the llama3.2 model in the Ollama Docker container

Write-Host "🚀 Installing Ollama model llama3.2..." -ForegroundColor Cyan

# Check if container is running
$containerStatus = docker ps --filter "name=ollama-ai" --format "{{.Status}}"
if (-not $containerStatus) {
    Write-Host "❌ Ollama container is not running. Starting it..." -ForegroundColor Yellow
    docker-compose up -d ollama
    Write-Host "⏳ Waiting for container to be ready..." -ForegroundColor Yellow
    Start-Sleep -Seconds 10
}

# Try to pull the model using docker exec
Write-Host "📥 Pulling llama3.2 model..." -ForegroundColor Cyan
$result = docker exec ollama-ai ollama pull llama3.2 2>&1

if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ Model installed successfully!" -ForegroundColor Green
    
    # List installed models
    Write-Host "`n📋 Installed models:" -ForegroundColor Cyan
    docker exec ollama-ai ollama list
} else {
    Write-Host "❌ Error installing model:" -ForegroundColor Red
    Write-Host $result -ForegroundColor Red
    
    Write-Host "`n💡 Alternative: Try installing via HTTP API..." -ForegroundColor Yellow
    Write-Host "You can also try:" -ForegroundColor Yellow
    Write-Host "  docker exec -it ollama-ai sh" -ForegroundColor Gray
    Write-Host "  ollama pull llama3.2" -ForegroundColor Gray
}
