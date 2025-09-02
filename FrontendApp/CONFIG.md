# Configuración de API

## Ambientes

El proyecto está configurado para usar diferentes URLs de API según el ambiente:

### Desarrollo Local (`environment.ts`)
- URL: `http://localhost:7124/api`
- Para desarrollo local con backend en localhost

### Staging (`environment.staging.ts`)
- URL: `https://172.30.137.209:7124/api`
- Para pruebas en servidor de staging

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
