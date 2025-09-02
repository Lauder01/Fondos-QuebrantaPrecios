# Deploy to Vercel - Quick Start

## 📋 Checklist antes del deployment

✅ Bootstrap se instala automáticamente en Vercel  
✅ API configurada: `https://devdemoapi1.azurewebsites.net/api`  
✅ Build de producción funciona correctamente  
✅ Environment de producción configurado  

## 🚀 Pasos para desplegar

### 1. Commit y push del código
```bash
git add .
git commit -m "Configure for Vercel deployment"
git push origin Angular
```

### 2. En Vercel (vercel.com)
1. **Import Project** desde tu repositorio GitHub
2. **Configure Project Settings**:
   - Framework Preset: `Angular`
   - Root Directory: `FrontendApp`
   - Build Command: `npm run build:vercel`
   - Output Directory: `dist/FrontendApp/browser`
   - Install Command: `npm install && npm run setup-bootstrap-ci`

### 3. Deploy automático
- Vercel detectará los cambios y construirá automáticamente
- La URL será algo como: `https://your-project.vercel.app`

## 🔧 Configuración actual
- **API URL**: `https://devdemoapi1.azurewebsites.net/api`
- **Node Version**: 18 (especificado en .nvmrc)
- **Framework**: Angular con SSR
- **Bootstrap**: Se descarga automáticamente durante el build

## 📝 Comandos útiles
```bash
# Build local para testing
npm run build:vercel

# Servidor de desarrollo
npm run start

# Instalar Bootstrap manualmente
npm run setup-bootstrap-ci
```

## ⚠️ Notas importantes
- Bootstrap se instala automáticamente en cada build
- No necesitas configurar variables de entorno adicionales
- Los archivos dist/ están excluidos del repositorio (correcto)
- Las advertencias de budget size son normales y no afectan la funcionalidad
