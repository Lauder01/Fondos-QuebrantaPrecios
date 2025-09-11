# Script para ejecutar: API Local + Web Local

Write-Host "=== Iniciando Desarrollo Local ===" -ForegroundColor Green
Write-Host "===================================" -ForegroundColor Green
Write-Host ""

$projectRoot = "C:\SC25\Fondos-QuebrantaPrecios"
if (!(Test-Path $projectRoot)) {
    Write-Host "Error: No se encuentra el directorio del proyecto en $projectRoot" -ForegroundColor Red
    exit 1
}

Set-Location $projectRoot
Write-Host "Directorio del proyecto: $projectRoot" -ForegroundColor Yellow

Write-Host "Verificando herramientas..." -ForegroundColor Cyan
if (!(Get-Command "dotnet" -ErrorAction SilentlyContinue)) {
    Write-Host ".NET no está instalado o no está en el PATH" -ForegroundColor Red
    exit 1
}

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

Write-Host "Verificando puertos..." -ForegroundColor Cyan
if (Test-Port 7124) {
    Write-Host "Puerto 7124 (API) ya está ocupado" -ForegroundColor Yellow
    $continue = Read-Host "¿Continuar de todas formas? (y/N)"
    if ($continue -ne 'y' -and $continue -ne 'Y') {
        exit 1
    }
}

if (Test-Port 4200) {
    Write-Host "Puerto 4200 (Web) ya está ocupado" -ForegroundColor Yellow
    $continue = Read-Host "¿Continuar de todas formas? (y/N)"
    if ($continue -ne 'y' -and $continue -ne 'Y') {
        exit 1
    }
}

Write-Host ""
Write-Host "Configuración seleccionada:" -ForegroundColor Magenta
Write-Host "   API: https://localhost:7124 (Local)" -ForegroundColor White
Write-Host "   Web: https://localhost:4200 (Local con proxy)" -ForegroundColor White
Write-Host "   Environment: environment.local.ts" -ForegroundColor White
Write-Host ""

Write-Host "Compilando API..." -ForegroundColor Cyan
Set-Location "$projectRoot\WebAPI"
dotnet clean --configuration Debug --verbosity quiet
dotnet build --configuration Debug --verbosity quiet
if ($LASTEXITCODE -ne 0) {
    Write-Host "Error compilando la API" -ForegroundColor Red
    exit 1
}
Write-Host "API compilada correctamente" -ForegroundColor Green

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

Write-Host "Iniciando servicios..." -ForegroundColor Green

$apiJob = Start-Job -ScriptBlock {
    param($apiPath)
    Set-Location $apiPath
    dotnet run --configuration Debug --no-build
} -ArgumentList "$projectRoot\WebAPI"

Write-Host "Esperando a que la API inicie..." -ForegroundColor Yellow
Start-Sleep -Seconds 8

$webJob = Start-Job -ScriptBlock {
    param($frontendPath)
    Set-Location $frontendPath
    npm run start:local
} -ArgumentList "$projectRoot\FrontendApp"

Write-Host ""
Write-Host "Servicios iniciados!" -ForegroundColor Green
Write-Host "URLs disponibles:" -ForegroundColor Cyan
Write-Host "   API:         https://localhost:7124/api" -ForegroundColor White
Write-Host "   Health:      https://localhost:7124/health" -ForegroundColor White
Write-Host "   Swagger:     https://localhost:7124/swagger" -ForegroundColor White
Write-Host "   Web:         https://localhost:4200" -ForegroundColor White
Write-Host ""
Write-Host "IDs de Jobs:" -ForegroundColor Magenta
Write-Host "   API Job ID:  $($apiJob.Id)" -ForegroundColor Gray
Write-Host "   Web Job ID:  $($webJob.Id)" -ForegroundColor Gray
Write-Host ""
Write-Host "Para ver logs:" -ForegroundColor Cyan
Write-Host "   Receive-Job $($apiJob.Id) -Keep" -ForegroundColor Gray
Write-Host "   Receive-Job $($webJob.Id) -Keep" -ForegroundColor Gray
Write-Host ""
Write-Host "Para detener los servicios:" -ForegroundColor Red
Write-Host "   Stop-Job $($apiJob.Id), $($webJob.Id); Remove-Job $($apiJob.Id), $($webJob.Id)" -ForegroundColor Gray
Write-Host ""

Start-Sleep -Seconds 10
Write-Host "Abriendo navegador..." -ForegroundColor Green
try {
    Start-Process "https://localhost:4200"
    Start-Sleep -Seconds 2
    Start-Process "https://localhost:7124/swagger"
} catch {
    Write-Host "No se pudo abrir el navegador automáticamente" -ForegroundColor Yellow
}

Write-Host "Presiona CTRL+C para detener todos los servicios" -ForegroundColor Yellow
try {
    $counter = 0
    while ($true) {
        Start-Sleep -Seconds 10
        $counter++
        
        if ($counter % 3 -eq 0) {
            if ($apiJob.State -ne 'Running') {
                Write-Host "El job de la API se detuvo inesperadamente" -ForegroundColor Red
                Receive-Job $apiJob | Select-Object -Last 10
                break
            }
            
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
    Write-Host "Deteniendo servicios..." -ForegroundColor Yellow
    Stop-Job $apiJob.Id, $webJob.Id -ErrorAction SilentlyContinue
    Remove-Job $apiJob.Id, $webJob.Id -ErrorAction SilentlyContinue
    Write-Host "Servicios detenidos" -ForegroundColor Green
}
