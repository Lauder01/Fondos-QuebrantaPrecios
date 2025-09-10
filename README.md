# Fondos QuebrantaPrecios

Sistema de gestión de edificios y apartamentos desarrollado con Angular y .NE## 📚 Documentación

### Documentación Online
- **Wiki del Proyecto**: [https://deepwiki.com/Lauder01/Fondos-QuebrantaPrecios](https://deepwiki.com/Lauder01/Fondos-QuebrantaPrecios)

### Documentación del Proyecto
- **Documentación Técnica**: `Docs/Documentación-ProyectoFQP.docx`
- **Diagramas de Actividad**:
  - `Docs/Diagrams/Activity/PurchaseBuildingActivityDiagram.vpp`
  - `Docs/Diagrams/Activity/RegisterBuildingActivityDiagram.vpp`
  - `Docs/Diagrams/Activity/ViewBuildingListActivityDiagram.vpp`

### Documentación de Configuración
- **Configuración de Vercel**: `FrontendApp/VERCEL.md`
- **Configuración de Servidor**: `FrontendApp/SERVE.md`
- **Bootstrap Setup**: `FrontendApp/BOOTSTRAP.md`
- **Configuración de Deploy**: `FrontendApp/DEPLOY.md`
## 🚀 Aplicación en Producción

### Acceso Directo
**URL de la aplicación:** [https://fondos-quebranta-precios-six.vercel.app/]

### Arquitectura de Despliegue
- **Frontend**: Desplegado en Vercel (Angular)
- **Backend API**: Desplegado en Azure (`https://devdemoapi1.azurewebsites.net/api`)
- **Base de Datos**: SQL Server en Azure

## 🔧 Getting Started

### Prerrequisitos
- Node.js (para Angular)
- .NET SDK (para WebAPI)
- SQL Server

### Instalación Local

1. **Clonar el repositorio**
```bash
git clone <repository-url>
cd Fondos-QuebrantaPrecios
```

2. **Configurar Frontend (Angular)**
```bash
cd FrontendApp
npm install
npm run setup-bootstrap  # Instala Bootstrap automáticamente
```

3. **Configurar Backend (.NET)**
```bash
cd WebAPI
dotnet restore
dotnet build
```

## 🌐 Configuración de Ambientes

### URLs por Ambiente
- **Development**: `https://devdemoapi1.azurewebsites.net/api`
- **Staging**: `https://devdemoapi1.azurewebsites.net/api`  
- **Production**: `https://devdemoapi1.azurewebsites.net/api`

### Configuración Local del Backend
- **HTTP**: `http://localhost:5192`
- **HTTPS**: `https://172.30.137.209:7124`
- **Swagger**: `http://localhost:5192/swagger`

Ver `FrontendApp/SERVE.md` para configurar las URLs de la API según el ambiente.

## 🚀 Ejecutar la Aplicación

### Frontend Local
```bash
cd FrontendApp

# Desarrollo con API remota (recomendado)
npm run start:staging
# o
ng serve --configuration=staging

# Desarrollo local
ng serve --configuration=development
```

### Backend Local
```bash
cd WebAPI
dotnet run
```

### Acceso en Producción
Accede a la aplicación directamente desde: [https://fondos-quebranta-precios-3w4os1it8-lauder01s-projects.vercel.app/](https://fondos-quebranta-precios-3w4os1it8-lauder01s-projects.vercel.app/)

## 📦 Despliegue

### Frontend (Vercel)
- **Despliegue automático**: Push a la rama principal
- **Build Command**: `npm run build`
- **Output Directory**: `dist/FrontendApp/browser`
- **Install Command**: `npm install && npm run setup-bootstrap-ci`

### Backend (Azure)
- Desplegado en Azure App Service
- **URL de API**: `https://devdemoapi1.azurewebsites.net/api`

## 📁 Estructura del Proyecto

```
Fondos-QuebrantaPrecios/
├── FrontendApp/          # Aplicación Angular (Vercel)
├── WebAPI/              # API .NET (Azure)
├── ClassLibraryProject/ # Entidades y modelos
├── ServiceLibraryProject/ # Servicios de negocio
├── RepositoryLibraryProject/ # Acceso a datos
├── Scripts/             # Scripts de base de datos
└── Docs/               # Documentación
```

## � Documentación

### Documentación Online
- **Wiki del Proyecto**: [https://deepwiki.com/Lauder01/Fondos-QuebrantaPrecios](https://deepwiki.com/Lauder01/Fondos-QuebrantaPrecios)
- **Documentación Técnica**: Ver `Docs/Documentación-ProyectoFQP.docx`

### Documentación Local
- **Configuración de Vercel**: Ver `FrontendApp/VERCEL.md`
- **Configuración de Servidor**: Ver `FrontendApp/SERVE.md`
- **Bootstrap Setup**: Ver `FrontendApp/BOOTSTRAP.md`
- **Configuración de Deploy**: Ver `FrontendApp/DEPLOY.md`

## �🔗 Enlaces Útiles

- **Aplicación en Producción**: [https://fondos-quebranta-precios-3w4os1it8-lauder01s-projects.vercel.app/](https://fondos-quebranta-precios-3w4os1it8-lauder01s-projects.vercel.app/)
- **API en Azure**: [https://devdemoapi1.azurewebsites.net/api](https://devdemoapi1.azurewebsites.net/api)
- **Documentación del Proyecto**: [https://deepwiki.com/Lauder01/Fondos-QuebrantaPrecios](https://deepwiki.com/Lauder01/Fondos-QuebrantaPrecios)

## 📋 Scripts Disponibles

### Frontend
```bash
npm run start:staging    # Desarrollo con API remota
npm run build           # Build de producción
npm run setup-bootstrap # Instalar Bootstrap
```

### Backend
```bash
dotnet run             # Ejecutar API
dotnet build           # Compilar proyecto
dotnet restore         # Restaurar dependencias
```

---

If you want to learn more about creating good readme files then refer the following [guidelines](https://docs.microsoft.com/en-us/azure/devops/repos/git/create-a-readme?view=azure-devops). You can also seek inspiration from the below readme files:
- [ASP.NET Core](https://github.com/aspnet/Home)
- [Visual Studio Code](https://github.com/Microsoft/vscode)
- [Chakra Core](https://github.com/Microsoft/ChakraCore)
