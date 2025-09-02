# Configuración de Bootstrap

Los archivos de Bootstrap no están incluidos en el repositorio para mantenerlo limpio.

## Instalación automática (recomendado)

Ejecuta el script de setup desde la carpeta `FrontendApp`:

```bash
cd FrontendApp
npm run setup-bootstrap
```

## Instalación manual

Si el script no funciona, puedes descargar manualmente:

1. Ve a https://getbootstrap.com/docs/5.3/getting-started/download/
2. Descarga "Compiled CSS and JS"
3. Extrae el archivo `bootstrap.min.css` 
4. Colócalo en: `FrontendApp/src/assets/bootstrap/bootstrap.min.css`

## Verificación

El archivo debe estar en:
```
FrontendApp/
  src/
    assets/
      bootstrap/
        bootstrap.min.css  ← Este archivo
```

Una vez instalado, Angular debería compilar sin errores.
