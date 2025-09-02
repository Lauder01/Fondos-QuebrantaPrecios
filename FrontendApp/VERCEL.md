# Deployment a Vercel

## Pasos para desplegar en Vercel

### 1. Preparación del repositorio
El proyecto ya está configurado para Vercel con:
- `vercel.json` con configuración de build
- Scripts npm para CI/CD
- Environment de producción configurado

### 2. Deployment automático
1. Ve a [vercel.com](https://vercel.com)
2. Conecta tu cuenta de GitHub
3. Importa este repositorio
4. Selecciona la carpeta `FrontendApp` como directorio raíz
5. Vercel detectará automáticamente que es un proyecto Angular

### 3. Configuración en Vercel
- **Framework Preset**: Angular
- **Root Directory**: `FrontendApp`
- **Build Command**: `npm run build`
- **Output Directory**: `dist/FrontendApp/browser`
- **Install Command**: `npm install && npm run setup-bootstrap-ci`

### 4. Variables de entorno (opcional)
Si necesitas una API diferente en producción:
- Ve a Settings > Environment Variables en tu proyecto Vercel
- Agrega: `API_URL` = `https://tu-api-de-produccion.com/api`

### 5. Build local para testing
```bash
# Instalar dependencias
npm install

# Instalar Bootstrap
npm run setup-bootstrap-ci

# Build de producción
npm run build
```

### 6. Dominios personalizados
Una vez desplegado, puedes configurar un dominio personalizado en la configuración de Vercel.

## API Configuration
El frontend está configurado para usar: `https://devdemoapi1.azurewebsites.net/api`

Esto se puede cambiar editando:
- `src/environments/environment.prod.ts` para producción
- `src/environments/environment.ts` para desarrollo

## Troubleshooting
- Si Bootstrap no se carga, ejecuta `npm run setup-bootstrap-ci`
- Para problemas de routing, verifica que `vercel.json` tenga las rewrites correctas
- Para problemas de API, verifica CORS en el backend
