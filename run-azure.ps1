# Script para ejecutar: API Azure + Web Local

Write-Host "=== Iniciando configuración: API Azure + Web Local ===" -ForegroundColor Blue
Write-Host "=======================================================" -ForegroundColor Blue
Write-Host ""

$projectRoot = "C:\SC25\Fondos-QuebrantaPrecios"
if (!(Test-Path $projectRoot)) {
    Write-Host "Error: No se encuentra el directorio del proyecto en $projectRoot" -ForegroundColor Red
    exit 1
}

Set-Location $projectRoot
Write-Host "Directorio del proyecto: $projectRoot" -ForegroundColor Yellow

Write-Host "Verificando herramientas..." -ForegroundColor Cyan
if (!(Get-Command "npm" -ErrorAction SilentlyContinue)) {
    Write-Host "npm no está instalado o no está en el PATH" -ForegroundColor Red
    exit 1
}

Write-Host "Herramientas verificadas" -ForegroundColor Green

function Test-Port {
    param([int]$Port)
    try {
        $listener = [System.Net.Sockets.TcpListener]::new([System.Net.IPAddress]::Any, $Port)
        $listener.Start()
        $listener.Stop()
        return $false
    } catch {
        return $true
    }
}

Write-Host "Verificando puerto 4200..." -ForegroundColor Cyan
if (Test-Port 4200) {
    Write-Host "Puerto 4200 (Web) ya está ocupado" -ForegroundColor Yellow
    $continue = Read-Host "¿Continuar de todas formas? (y/N)"
    if ($continue -ne 'y' -and $continue -ne 'Y') {
        exit 1
    }
}

Write-Host ""
Write-Host "Configuración seleccionada:" -ForegroundColor Magenta
Write-Host "   API: https://devdemoapi1.azurewebsites.net (Azure)" -ForegroundColor White
Write-Host "   Web: https://localhost:4200 (Local)" -ForegroundColor White
Write-Host "   Environment: environment.azure.ts" -ForegroundColor White
Write-Host ""

Write-Host "Verificando conectividad con API de Azure..." -ForegroundColor Cyan
try {
    $response = Invoke-WebRequest -Uri "https://devdemoapi1.azurewebsites.net/health" -Method GET -TimeoutSec 10 -ErrorAction Stop
    Write-Host "API de Azure accesible (Status: $($response.StatusCode))" -ForegroundColor Green
} catch {
    Write-Host "No se pudo conectar a la API de Azure" -ForegroundColor Red
    Write-Host "   Error: $($_.Exception.Message)" -ForegroundColor Yellow
    $continue = Read-Host "¿Continuar de todas formas? (y/N)"
    if ($continue -ne 'y' -and $continue -ne 'Y') {
        exit 1
    }
}

Write-Host "Preparando Frontend..." -ForegroundColor Cyan
Set-Location "$projectRoot\FrontendApp"

if (!(Test-Path "node_modules")) {
    Write-Host "Instalando dependencias de Node..." -ForegroundColor Yellow
    npm install --silent
    if ($LASTEXITCODE -ne 0) {
        Write-Host "Error instalando dependencias" -ForegroundColor Red
        exit 1
    }
}

Write-Host "Frontend preparado" -ForegroundColor Green
Write-Host ""

Write-Host "Iniciando Web con configuración Azure..." -ForegroundColor Green

$webJob = Start-Job -ScriptBlock {
    param($frontendPath)
    Set-Location $frontendPath
    npm run start:azure
} -ArgumentList "$projectRoot\FrontendApp"

Write-Host ""
Write-Host "Servicio Web iniciado!" -ForegroundColor Green
Write-Host "URLs disponibles:" -ForegroundColor Cyan
Write-Host "   API:         https://devdemoapi1.azurewebsites.net/api" -ForegroundColor White
Write-Host "   Health:      https://devdemoapi1.azurewebsites.net/health" -ForegroundColor White
Write-Host "   Swagger:     https://devdemoapi1.azurewebsites.net/swagger" -ForegroundColor White
Write-Host "   Web:         https://localhost:4200" -ForegroundColor White
Write-Host ""
Write-Host "ID de Job Web: $($webJob.Id)" -ForegroundColor Magenta
Write-Host ""
Write-Host "Para ver logs del Web:" -ForegroundColor Cyan
Write-Host "   Receive-Job $($webJob.Id) -Keep" -ForegroundColor Gray
Write-Host ""
Write-Host "Para detener el servicio:" -ForegroundColor Red
Write-Host "   Stop-Job $($webJob.Id); Remove-Job $($webJob.Id)" -ForegroundColor Gray
Write-Host ""

Start-Sleep -Seconds 10
Write-Host "Abriendo navegador..." -ForegroundColor Green
try {
    Start-Process "https://localhost:4200"
    Start-Sleep -Seconds 2
    Start-Process "https://devdemoapi1.azurewebsites.net/swagger"
} catch {
    Write-Host "No se pudo abrir el navegador automáticamente" -ForegroundColor Yellow
}

Write-Host "Presiona CTRL+C para detener el servicio" -ForegroundColor Yellow
try {
    $counter = 0
    while ($true) {
        Start-Sleep -Seconds 10
        $counter++
        
        if ($counter % 3 -eq 0) {
            if ($webJob.State -ne 'Running') {
                Write-Host "El job del Web se detuvo inesperadamente" -ForegroundColor Red
                Receive-Job $webJob | Select-Object -Last 10
                break
            }
        }
        
        Write-Host "." -NoNewline -ForegroundColor Green
    }
} catch {
    Write-Host ""
    Write-Host "Interrupción detectada" -ForegroundColor Yellow
} finally {
    Write-Host ""
    Write-Host "Deteniendo servicio Web..." -ForegroundColor Yellow
    Stop-Job $webJob.Id -ErrorAction SilentlyContinue
    Remove-Job $webJob.Id -ErrorAction SilentlyContinue
    Write-Host "Servicio detenido" -ForegroundColor Green
}
