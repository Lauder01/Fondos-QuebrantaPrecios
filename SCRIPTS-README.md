# 🚀 Scripts de Ejecución - Fondos Quebranta Precios

## 📋 Scripts Disponibles

### 1. 🏠 **run-local.ps1** - Desarrollo Local Completo
```powershell
.\run-local.ps1
```
- **API**: Local (https://localhost:7124)
- **Frontend**: Local (http://localhost:4200)
- **Configuración**: environment.local.ts
- **Proxy**: Habilitado para conectar frontend local con API local
- **Uso**: Desarrollo completo en local

### 2. ☁️ **run-azure.ps1** - API Azure + Frontend Local
```powershell
.\run-azure.ps1
```
- **API**: Azure (https://devdemoapi1.azurewebsites.net)
- **Frontend**: Local (http://localhost:4200)
- **Configuración**: environment.azure.ts
- **Uso**: Probar frontend local contra API de Azure

### 3. 🌐 **run-production.ps1** - Despliegue en Vercel
```powershell
.\run-production.ps1
```
- **API**: Azure (https://devdemoapi1.azurewebsites.net)
- **Frontend**: Vercel (Producción)
- **Configuración**: environment.production.ts
- **Uso**: Despliegue completo en la nube

## 🔧 Configuraciones Aplicadas

### ✅ Proxy Mejorado
- Configuración específica para `/api/Building/**`
- SSL deshabilitado para desarrollo local
- Headers de conexión optimizados
- Logging de debug habilitado

### ✅ Angular Routes Optimizadas
- Rutas reordenadas para evitar conflictos con API
- Configuración SSL específica por entorno

### ✅ Scripts Simplificados
- Eliminados scripts duplicados y de prueba
- Nomenclatura uniforme y clara
- Mejor manejo de errores y feedback visual

## 🎯 Uso Recomendado

1. **Para desarrollo diario**: `.\run-local.ps1`
2. **Para probar contra Azure**: `.\run-azure.ps1`
3. **Para desplegar**: `.\run-production.ps1`

## 🛑 Para detener servicios

Los scripts muestran los comandos necesarios al ejecutarse, pero en general:

```powershell
# Ver jobs activos
Get-Job

# Detener jobs específicos
Stop-Job [ID]; Remove-Job [ID]

# Detener todos los jobs
Get-Job | Stop-Job; Get-Job | Remove-Job
```

## 📊 URLs importantes

### Local
- **API**: https://localhost:7124/api
- **Swagger**: https://localhost:7124/swagger
- **Health**: https://localhost:7124/health
- **Frontend**: http://localhost:4200

### Azure
- **API**: https://devdemoapi1.azurewebsites.net/api
- **Swagger**: https://devdemoapi1.azurewebsites.net/swagger
- **Health**: https://devdemoapi1.azurewebsites.net/health
