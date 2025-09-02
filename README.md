# Fondos QuebrantaPrecios

Sistema de gestión de edificios y apartamentos desarrollado con Angular y .NET WebAPI.

## Getting Started

### Prerrequisitos
- Node.js (para Angular)
- .NET SDK (para WebAPI)
- SQL Server

### Instalación

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

### Configuración de API

Ver `FrontendApp/SERVE.md` para configurar las URLs de la API según el ambiente.

- **Development**: API local
- **Staging**: API remota en servidor de pruebas  
- **Production**: API de producción

### Ejecutar la aplicación

**Frontend:**
```bash
cd FrontendApp
npm run start:staging  # Para conectar con API remota
# o
ng serve --configuration=staging
```

**Backend:**
```bash
cd WebAPI
dotnet run
```

# Build and Test
TODO: Describe and show how to build your code and run the tests. 

# Contribute
TODO: Explain how other users and developers can contribute to make your code better. 

If you want to learn more about creating good readme files then refer the following [guidelines](https://docs.microsoft.com/en-us/azure/devops/repos/git/create-a-readme?view=azure-devops). You can also seek inspiration from the below readme files:
- [ASP.NET Core](https://github.com/aspnet/Home)
- [Visual Studio Code](https://github.com/Microsoft/vscode)
- [Chakra Core](https://github.com/Microsoft/ChakraCore)