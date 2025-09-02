# Configuración de API

## Ambientes

El proyecto está configurado para usar diferentes URLs de API según el ambiente:

### Desarrollo Local (`environment.ts`)
- URL: `https://devdemoapi1.azurewebsites.net/api`
- Para desarrollo con API de Azure

### Staging (`environment.staging.ts`)
- URL: `https://devdemoapi1.azurewebsites.net/api`
- Para pruebas con API de Azure

### Producción (`environment.prod.ts`)
- URL: `https://api.production.domain.com/api`
- Para el servidor de producción (actualizar con la URL real)

## Comandos de Build

```bash
# Desarrollo
ng serve

# Staging
ng build --configuration=staging

# Producción
ng build --configuration=production
```

## Configuración de Angular.json

Agregar la configuración de staging en `angular.json`:

```json
"configurations": {
  "staging": {
    "fileReplacements": [
      {
        "replace": "src/environments/environment.ts",
        "with": "src/environments/environment.staging.ts"
      }
    ]
  }
}
```

## Cambio de URLs

Para cambiar la URL de la API:

1. **Desarrollo**: Edita `src/environments/environment.ts`
2. **Staging**: Edita `src/environments/environment.staging.ts`
3. **Producción**: Edita `src/environments/environment.prod.ts`

No es necesario tocar ningún otro archivo, ya que todos los servicios usan `environment.apiUrl`.
