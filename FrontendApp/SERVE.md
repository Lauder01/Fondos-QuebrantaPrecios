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
- **Development**: `https://localhost:7124/api`
- **Staging**: `https://172.30.137.209:7124/api`  
- **Production**: `https://api.production.domain.com/api`

Para conectar con la API en `https://172.30.137.209:7124`, usa:
```bash
ng serve --configuration=staging
```
