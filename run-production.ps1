# Script para ejecutar: API Azure + Web Vercel (Producción)

Write-Host "=== Configuración de Despliegue: API Azure + Web Vercel ===" -ForegroundColor Blue
Write-Host "============================================================" -ForegroundColor Blue
Write-Host ""

$projectRoot = "C:\SC25\Fondos-QuebrantaPrecios"
if (!(Test-Path $projectRoot)) {
    Write-Host "Error: No se encuentra el directorio del proyecto en $projectRoot" -ForegroundColor Red
    exit 1
}

Set-Location $projectRoot
Write-Host "Directorio del proyecto: $projectRoot" -ForegroundColor Yellow

Write-Host "Verificando herramientas..." -ForegroundColor Cyan
if (!(Get-Command "vercel" -ErrorAction SilentlyContinue)) {
    Write-Host "Vercel CLI no está instalado" -ForegroundColor Red
    Write-Host "Instalar con: npm i -g vercel" -ForegroundColor Yellow
    exit 1
}

if (!(Get-Command "git" -ErrorAction SilentlyContinue)) {
    Write-Host "Git no está disponible" -ForegroundColor Red
    exit 1
}

if (!(Get-Command "npm" -ErrorAction SilentlyContinue)) {
    Write-Host "npm no está instalado" -ForegroundColor Red
    exit 1
}

Write-Host "Herramientas verificadas" -ForegroundColor Green
Write-Host ""

Write-Host "Verificando estado de Git..." -ForegroundColor Cyan
$gitStatus = git status --porcelain
if ($gitStatus) {
    Write-Host "Hay cambios sin confirmar:" -ForegroundColor Yellow
    git status --short
    Write-Host ""
    $commit = Read-Host "¿Hacer commit automático de los cambios? (y/N)"
    if ($commit -eq 'y' -or $commit -eq 'Y') {
        Write-Host "Haciendo commit..." -ForegroundColor Cyan
        git add .
        git commit -m "Auto-commit: Preparando despliegue de producción"
        if ($LASTEXITCODE -eq 0) {
            Write-Host "Commit realizado" -ForegroundColor Green
        } else {
            Write-Host "Error en el commit" -ForegroundColor Red
            exit 1
        }
    } else {
        Write-Host "Continuando con cambios sin confirmar..." -ForegroundColor Yellow
    }
}

Write-Host "Verificando conectividad con API de Azure..." -ForegroundColor Cyan
try {
    $response = Invoke-WebRequest -Uri "https://devdemoapi1.azurewebsites.net/health" -Method GET -TimeoutSec 10 -ErrorAction Stop
    Write-Host "API de Azure accesible (Status: $($response.StatusCode))" -ForegroundColor Green
} catch {
    Write-Host "No se pudo conectar a la API de Azure" -ForegroundColor Red
    Write-Host "   Error: $($_.Exception.Message)" -ForegroundColor Yellow
    exit 1
}

Write-Host ""
Write-Host "Configuración de despliegue:" -ForegroundColor Magenta
Write-Host "   API: https://devdemoapi1.azurewebsites.net (Azure)" -ForegroundColor White
Write-Host "   Web: Vercel (Producción)" -ForegroundColor White
Write-Host "   Build: production environment" -ForegroundColor White
Write-Host ""

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

Write-Host "Construyendo para producción..." -ForegroundColor Cyan
npm run build:prod
if ($LASTEXITCODE -ne 0) {
    Write-Host "Error en el build de producción" -ForegroundColor Red
    exit 1
}
Write-Host "Build completado" -ForegroundColor Green

if (!(Test-Path "dist")) {
    Write-Host "No se encontró la carpeta dist después del build" -ForegroundColor Red
    exit 1
}

Write-Host ""
Write-Host "Desplegando en Vercel..." -ForegroundColor Green
Write-Host "Opciones de despliegue:" -ForegroundColor Cyan
Write-Host "   1. Despliegue de vista previa (preview)" -ForegroundColor White
Write-Host "   2. Despliegue de producción (production)" -ForegroundColor White
Write-Host ""

$deployType = Read-Host "Selecciona el tipo de despliegue (1 o 2)"

switch ($deployType) {
    "1" {
        Write-Host "Desplegando como vista previa..." -ForegroundColor Cyan
        vercel --prod=false
    }
    "2" {
        Write-Host "Desplegando en producción..." -ForegroundColor Cyan
        vercel --prod
    }
    default {
        Write-Host "Opción inválida. Usando vista previa por defecto..." -ForegroundColor Yellow
        vercel --prod=false
    }
}

if ($LASTEXITCODE -ne 0) {
    Write-Host "Error en el despliegue de Vercel" -ForegroundColor Red
    exit 1
}

Write-Host ""
Write-Host "Despliegue completado!" -ForegroundColor Green
Write-Host "URLs disponibles:" -ForegroundColor Cyan
Write-Host "   API:         https://devdemoapi1.azurewebsites.net" -ForegroundColor White
Write-Host "   Health:      https://devdemoapi1.azurewebsites.net/health" -ForegroundColor White
Write-Host "   Swagger:     https://devdemoapi1.azurewebsites.net/swagger" -ForegroundColor White
Write-Host "   Web:         [Ver output de Vercel arriba]" -ForegroundColor White
Write-Host ""
Write-Host "Estado del despliegue:" -ForegroundColor Magenta
vercel ls

Write-Host ""
Write-Host "Comandos útiles:" -ForegroundColor Cyan
Write-Host "   Ver logs:      vercel logs [deployment-url]" -ForegroundColor Gray
Write-Host "   Ver dominios:  vercel domains" -ForegroundColor Gray
Write-Host "   Ver proyectos: vercel projects" -ForegroundColor Gray
Write-Host ""
Write-Host "Proceso completado!" -ForegroundColor Green
