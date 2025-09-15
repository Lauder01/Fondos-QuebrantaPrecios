# Solución para el problema del mapa en Vercel

## Problema
El mapa no aparece en Vercel pero sí funciona en desarrollo local.

## Causas identificadas
1. **Content Security Policy (CSP)**: Vercel puede bloquear recursos externos
2. **Importación dinámica**: La importación dinámica de Leaflet puede fallar en producción
3. **APIs de geocodificación**: Requests a Nominatim pueden ser bloqueados
4. **Server-Side Rendering**: Problemas de hidratación en SSR

## Soluciones implementadas

### 1. Mejora en la carga de Leaflet
- **Fallback a CDN**: Si la importación dinámica falla, carga desde CDN
- **Verificación global**: Comprueba si Leaflet ya está disponible globalmente
- **Manejo de errores**: Muestra mensaje de error si no se puede cargar

### 2. Headers de CSP en vercel.json
```json
{
  "headers": [
    {
      "source": "/(.*)",
      "headers": [
        {
          "key": "Content-Security-Policy",
          "value": "default-src 'self'; script-src 'self' 'unsafe-inline' 'unsafe-eval' https://unpkg.com https://cdnjs.cloudflare.com; style-src 'self' 'unsafe-inline' https://unpkg.com https://cdnjs.cloudflare.com; img-src 'self' data: https: http:; connect-src 'self' https://nominatim.openstreetmap.org https://nominatim1.openstreetmap.org https://nominatim2.openstreetmap.org https://devdemoapi1.azurewebsites.net https://*.tile.openstreetmap.org; font-src 'self' data:; frame-src 'self';"
        }
      ]
    }
  ]
}
```

### 3. Geocodificación mejorada
- **Múltiples servidores**: Fallback a diferentes servidores de Nominatim
- **Timeout**: Controla timeouts para evitar bloqueos
- **Headers correctos**: Incluye User-Agent y Accept headers
- **Restricción por país**: Limita búsquedas a España (`countrycodes=es`)

### 4. Leaflet como dependencia estática
- Agregado `node_modules/leaflet/dist/leaflet.js` a los scripts en `angular.json`
- Esto asegura que Leaflet esté disponible globalmente

## Pasos para desplegar

1. **Verifica que las dependencias estén instaladas**:
   ```bash
   npm install
   ```

2. **Construye para producción**:
   ```bash
   npm run build
   ```

3. **Despliega a Vercel**:
   ```bash
   vercel --prod
   ```

## Verificación

Para verificar que el mapa funciona en Vercel:

1. **Abre las herramientas de desarrollador**
2. **Busca estos mensajes en la consola**:
   - `✅ Leaflet ya disponible globalmente` o `✅ Leaflet cargado dinámicamente`
   - `✅ Geocodificación exitosa con [servidor]`
   - `✅ Mapa inicializado correctamente`

3. **Si ves errores**:
   - `❌ Error cargando Leaflet`: Problema de CSP o red
   - `⚠️ Error con servidor [nominatim]`: Problema de geocodificación
   - Verificar que los headers CSP estén configurados correctamente

## Alternativas adicionales

Si el problema persiste:

1. **Usar mapas estáticos**: Cambiar `[interactive]="false"` en el componente
2. **API de mapas alternativa**: Implementar Google Maps o Mapbox
3. **Proxy de geocodificación**: Crear un endpoint en tu API para geocodificar

## Archivos modificados

- `src/app/shared/map/map.component.ts`: Mejorada la carga de Leaflet y geocodificación
- `vercel.json`: Agregados headers CSP
- `angular.json`: Agregado Leaflet como script estático
