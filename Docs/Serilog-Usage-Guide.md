# Guía de Logging con Serilog - FQP WebAPI

## 📋 Resumen de Implementación

Se ha implementado un sistema de logging robusto usando **Serilog** que proporciona:

- ✅ **Logging estructurado** con información contextual
- ✅ **Middleware de manejo global de errores**
- ✅ **Logging detallado de requests HTTP**
- ✅ **Múltiples destinos de logs** (consola, archivos, base de datos)
- ✅ **Configuración por entorno**
- ✅ **Rotación automática de archivos**

## 📁 Ubicación de Logs

Los logs se guardan en la carpeta `logs/` del directorio de la aplicación:

```
WebAPI/
├── logs/
│   ├── webapi-20250910.log      # Log general del día
│   ├── webapi-20250909.log      # Log del día anterior
│   ├── errors-20250910.log      # Solo errores y warnings
│   └── errors-20250909.log      # Errores del día anterior
```

## 🔍 Tipos de Logs Generados

### 1. Logs de Request HTTP
```
[14:30:25 INF] WebAPI.Middleware.RequestLoggingMiddleware: Request iniciada: POST /api/Building/with-images  {"RequestId":"abc123","RequestPath":"/api/Building/with-images","HttpMethod":"POST","RemoteIP":"192.168.1.100"}

[14:30:28 INF] WebAPI.Middleware.RequestLoggingMiddleware: Request completada: POST /api/Building/with-images - Status: 201 - Duración: 2847ms {"RequestId":"abc123","StatusCode":201,"ElapsedMilliseconds":2847}
```

### 2. Logs de Creación de Edificios
```
[14:30:25 INF] WebAPI.Controllers.BuildingController: Iniciando creación de edificio con imágenes - Name: "Edificio Central", Images: 3, RequestId: "abc123"

[14:30:27 INF] WebAPI.Controllers.BuildingController: Procesando 3 imágenes para edificio - BuildingId: "def456", RequestId: "abc123"

[14:30:28 INF] WebAPI.Controllers.BuildingController: Edificio creado exitosamente - BuildingId: "def456", Name: "Edificio Central", Images: 3, Duración: 2847ms, RequestId: "abc123"
```

### 3. Logs de Upload de Imágenes
```
[14:30:26 INF] WebAPI.Controllers.ImageStorageController: Iniciando upload de imagen - BuildingId: "def456", FileName: "fachada.jpg", Size: 2.3MB, RequestId: "xyz789"

[14:30:27 INF] WebAPI.Controllers.ImageStorageController: Upload completado exitosamente - BuildingImageId: "ghi101", Size: 2415616bytes, Duración: 1230ms, RequestId: "xyz789"
```

### 4. Logs de Errores
```
[14:32:15 ERR] WebAPI.Controllers.BuildingController: Error al crear edificio con imágenes - BuildingName: "Edificio Test", Images: 2, Duración: 156ms, RequestId: "err404"
System.ArgumentException: Los datos proporcionados no son válidos
   at WebAPI.Controllers.BuildingController.CreateWithImages(BuildingWithImagesCreatorDto dto)
```

## 🛠️ Análisis de Logs

### Buscar Errores Específicos
```bash
# Buscar todos los errores de hoy
grep "ERR\|ERROR" logs/errors-$(date +%Y%m%d).log

# Buscar errores de un RequestId específico
grep "abc123" logs/errors-*.log

# Buscar timeouts o requests lentos (>5 segundos)
grep "Duración: [5-9][0-9][0-9][0-9]ms\|Duración: [0-9][0-9][0-9][0-9][0-9]ms" logs/webapi-*.log
```

### Monitorear Performance
```bash
# Requests más lentos del día
grep "Duración:" logs/webapi-$(date +%Y%m%d).log | sort -n -k7 | tail -10

# Uploads de imágenes fallidos
grep "Upload fallido" logs/webapi-*.log

# Cantidad de requests por hora
grep "Request iniciada" logs/webapi-$(date +%Y%m%d).log | cut -c2-8 | uniq -c
```

### Analizar Errores Comunes
```bash
# Errores de validación
grep "Validación fallida" logs/errors-*.log

# Errores de archivos demasiado grandes
grep "demasiado grande" logs/webapi-*.log

# Errores de tipos de archivo no permitidos
grep "Tipo de archivo no permitido" logs/webapi-*.log
```

## 📊 Campos de Log Estructurado

Cada log incluye información contextual:

| Campo | Descripción | Ejemplo |
|-------|-------------|---------|
| `RequestId` | ID único por request | `abc123-def456-ghi789` |
| `RequestPath` | Ruta de la API | `/api/Building/with-images` |
| `HttpMethod` | Método HTTP | `POST`, `GET`, `PUT` |
| `StatusCode` | Código de respuesta | `200`, `201`, `400`, `500` |
| `ElapsedMilliseconds` | Duración total | `2847` |
| `BuildingId` | ID del edificio | `550e8400-e29b-41d4-a716-446655440000` |
| `RemoteIP` | IP del cliente | `192.168.1.100` |
| `UserAgent` | Browser/Cliente | `Mozilla/5.0...` |

## 🚨 Alertas Recomendadas

### Errores Críticos
- **Error 500**: Monitorear logs de error para excepciones no controladas
- **Uploads fallidos**: Archivos rechazados por tamaño o tipo
- **Timeouts**: Requests que duran >10 segundos

### Performance
- **Requests lentos**: >5 segundos para creación de edificios
- **Memoria**: Uploads de imágenes muy grandes
- **Concurrencia**: Múltiples requests simultáneos del mismo IP

## 🔧 Configuración por Entorno

### Desarrollo (`appsettings.Development.json`)
- Nivel: `Debug` - Máximo detalle
- Incluye stack traces completos
- Logs de validación detallados

### Producción (`appsettings.json`)
- Nivel: `Information` - Información esencial
- Logs rotan cada 30 días
- Errores se mantienen 90 días
- Base de datos opcional para logs críticos

## 📈 Métricas Útiles

### Rendimiento de la API
```bash
# Tiempo promedio de creación de edificios
grep "Edificio creado exitosamente" logs/webapi-*.log | grep -o "Duración: [0-9]*ms" | grep -o "[0-9]*" | awk '{sum+=$1; count++} END {print "Promedio:", sum/count "ms"}'

# Uploads de imágenes por hora
grep "Upload completado exitosamente" logs/webapi-$(date +%Y%m%d).log | cut -c2-8 | uniq -c
```

### Calidad de Datos
```bash
# Edificios con imágenes vs sin imágenes
grep "Images: [1-9]" logs/webapi-*.log | wc -l  # Con imágenes
grep "Images: 0" logs/webapi-*.log | wc -l      # Sin imágenes
```

## 🆘 Troubleshooting Común

### Problema: Uploads Lentos
```bash
# Buscar uploads >30 segundos
grep "Upload completado\|Upload fallido" logs/webapi-*.log | grep "Duración: [3-9][0-9][0-9][0-9][0-9]ms"
```

### Problema: Errores de Validación
```bash
# Ver qué validaciones fallan más
grep "Validación fallida" logs/errors-*.log | grep -o "Errores: [^,]*" | sort | uniq -c | sort -nr
```

### Problema: Requests Duplicados
```bash
# Buscar múltiples requests del mismo IP en poco tiempo
grep "Request iniciada" logs/webapi-$(date +%Y%m%d).log | cut -c2-8,100-120 | sort | uniq -c | sort -nr | head -10
```

## 🔄 Rotación y Limpieza

Los logs se rotan automáticamente:
- **Diarios**: Nuevos archivos cada día
- **Por tamaño**: Máximo 10MB por archivo
- **Retención**: 30 días logs generales, 90 días errores
- **Compresión**: Los archivos antiguos se pueden comprimir

## 📧 Notificaciones (Futuras)

Se puede integrar con:
- **Email**: Para errores críticos
- **Slack**: Para alertas de rendimiento
- **Azure Monitor**: Para métricas en producción
- **Grafana**: Para dashboards visuales

## 🎯 Casos de Uso Específicos

### Debugging de Errores 404 en Imágenes
1. Buscar el RequestId en los logs
2. Seguir el flujo desde upload hasta descarga
3. Verificar si la imagen se guardó correctamente
4. Comprobar permisos y rutas de archivos

### Análisis de Performance
1. Identificar requests lentos por endpoint
2. Correlacionar con tamaño de imágenes
3. Analizar patrones por hora/día
4. Optimizar endpoints problemáticos

### Monitoreo de Salud del Sistema
1. Verificar que no hay errores 500
2. Confirmar que uploads funcionan
3. Validar tiempos de respuesta
4. Revisar uso de memoria y recursos
