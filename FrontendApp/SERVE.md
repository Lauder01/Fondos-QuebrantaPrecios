# Scripts de desarrollo

## Para desarrollo local (localhost):
```bash
ng serve --configuration=development
```

## Para pruebas con API remota (staging):
```bash
ng serve --configuration=staging
# O con npm:
npm run start:staging
```

## Para compilar con diferentes configuraciones:
```bash
# Desarrollo
ng build --configuration=development

# Staging (API remota)
ng build --configuration=staging

# Producción
ng build --configuration=production
```

## URLs actuales por configuración:
- **Development**: `https://devdemoapi1.azurewebsites.net/api`
- **Staging**: `https://devdemoapi1.azurewebsites.net/api`  
- **Production**: `https://api.production.domain.com/api`

Para conectar con la API en Azure, usa:
```bash
ng serve --configuration=staging
```
