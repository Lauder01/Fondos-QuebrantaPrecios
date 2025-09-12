using Microsoft.EntityFrameworkCore;
using RepositoryLibraryProject.Data;
using RepositoryLibraryProject.Interfaces;
using RepositoryLibraryProject;
using ClassLibraryProject.Entities;
using AutoMapper;
using ServiceLibraryProject;
using ServiceLibraryProject.Interfaces;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;
using System.Data;
using WebAPI.Middleware;
using WebAPI.Filters;

var builder = WebApplication.CreateBuilder(args);

// Configuración avanzada de Serilog
ConfigureSerilog(builder);

static void ConfigureSerilog(WebApplicationBuilder builder)
{
    // Crear carpeta de logs si no existe
    var logDir = Path.Combine(AppContext.BaseDirectory, "logs");
    if (!Directory.Exists(logDir))
    {
        Directory.CreateDirectory(logDir);
    }

    // Configuración de columnas personalizadas para SQL Server
    var columnOptions = new ColumnOptions
    {
        AdditionalColumns = new Collection<SqlColumn>
        {
            new SqlColumn("UserId", SqlDbType.NVarChar) { DataLength = 128 },
            new SqlColumn("UserName", SqlDbType.NVarChar) { DataLength = 255 },
            new SqlColumn("RequestId", SqlDbType.NVarChar) { DataLength = 128 },
            new SqlColumn("RequestPath", SqlDbType.NVarChar) { DataLength = 255 },
            new SqlColumn("HttpMethod", SqlDbType.NVarChar) { DataLength = 10 },
            new SqlColumn("StatusCode", SqlDbType.Int),
            new SqlColumn("ElapsedMilliseconds", SqlDbType.BigInt),
            new SqlColumn("MachineName", SqlDbType.NVarChar) { DataLength = 128 },
            new SqlColumn("Environment", SqlDbType.NVarChar) { DataLength = 50 }
        }
    };

    // Configuración de Serilog (sin SQL Server en desarrollo)
    var loggerConfiguration = new LoggerConfiguration()
        .MinimumLevel.Information()
        .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.Hosting.Lifetime", Serilog.Events.LogEventLevel.Information)
        .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Warning)
        .MinimumLevel.Override("Microsoft.AspNetCore", Serilog.Events.LogEventLevel.Warning)
        
        // Enrichers para información contextual
        .Enrich.FromLogContext()
        .Enrich.WithEnvironmentName()
        .Enrich.WithMachineName()
        
        // Sink para consola con formato estructurado
        .WriteTo.Console(
            outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {SourceContext}: {Message:lj} {Properties:j}{NewLine}{Exception}")
        
        // Sink para archivo general con rotación diaria
        .WriteTo.File(
            path: Path.Combine(logDir, "webapi-.log"),
            rollingInterval: RollingInterval.Day,
            retainedFileCountLimit: 30,
            fileSizeLimitBytes: 10_485_760, // 10MB
            rollOnFileSizeLimit: true,
            outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {Level:u3}] {SourceContext}: {Message:lj} {Properties:j}{NewLine}{Exception}")
        
        // Sink para errores en archivo separado
        .WriteTo.File(
            path: Path.Combine(logDir, "errors-.log"),
            restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Warning,
            rollingInterval: RollingInterval.Day,
            retainedFileCountLimit: 90, // Mantener errores por más tiempo
            fileSizeLimitBytes: 10_485_760,
            rollOnFileSizeLimit: true,
            outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {Level:u3}] {SourceContext}: {Message:lj} {Properties:j}{NewLine}{Exception}");

    // Solo añadir SQL Server en producción con conexión válida
    if (builder.Environment.IsProduction())
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        if (!string.IsNullOrEmpty(connectionString))
        {
            try
            {
                loggerConfiguration.WriteTo.MSSqlServer(
                    connectionString: connectionString,
                    sinkOptions: new MSSqlServerSinkOptions 
                    { 
                        TableName = "Logs",
                        SchemaName = "dbo",
                        AutoCreateSqlTable = true,
                        BatchPostingLimit = 50
                    },
                    columnOptions: columnOptions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Warning: No se pudo configurar SQL Server logging: {ex.Message}");
            }
        }
    }
        
    Log.Logger = loggerConfiguration.CreateLogger();

    // Configurar Serilog como proveedor de logging
    builder.Host.UseSerilog();

    // Log inicial para verificar configuración
    Log.Information("=== Aplicación FQP WebAPI iniciando ===");
    Log.Information("Entorno: {Environment}", builder.Environment.EnvironmentName);
    Log.Information("Directorio de logs configurado: {LogDirectory}", logDir);
}

builder.Host.UseSerilog();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(7124, listenOptions =>
    {
        listenOptions.UseHttps();
    });
}); 

// Add services to the container.
builder.Services.AddControllers();

// Configura el DbContext para SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registro de repositorio genérico para DI
builder.Services.AddScoped(typeof(IRepository<>), typeof(RepositoryLibraryProject.RepositoryIMP<>));

// Registro de servicios de dominio para todas las entidades principales
builder.Services.AddScoped<IService<District>, DistrictService>();
builder.Services.AddScoped<DistrictService>();
builder.Services.AddScoped<StreetService>();
builder.Services.AddScoped<BuildingService>();
builder.Services.AddScoped<ApartmentService>();
builder.Services.AddScoped<FloorService>();
builder.Services.AddScoped<AddressService>();
builder.Services.AddScoped<BuildingCompanyService>();
builder.Services.AddScoped<RequestService>();
builder.Services.AddScoped<ZipcodeService>();
builder.Services.AddScoped<StatusService>();
builder.Services.AddScoped<BuildingImageService>();
builder.Services.AddScoped<PurchaseService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "FQP Web API",
        Version = "v1",
        Description = "API Fondos QuebrantaPrecios"
    });
    
    // Configuración avanzada para manejar file uploads con multipart/form-data
    options.MapType<IFormFile>(() => new Microsoft.OpenApi.Models.OpenApiSchema
    {
        Type = "string",
        Format = "binary"
    });

    // Operación personalizada para uploads de archivos
    options.OperationFilter<FileUploadOperationFilter>();
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowedOrigins", policy =>
    {
        var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() ??
        [
            "http://localhost:4200",        // Angular local development
            "https://localhost:4200",       // Angular local development (HTTPS)
            "http://localhost:3000",        // Alternative development port
            "https://localhost:3000",       // Alternative development port (HTTPS)
            "https://fondos-quebranta-precios-six.vercel.app", // Production Vercel
            "https://fondos-quebranta-precios-*.vercel.app"    // Preview Vercel deployments
        ];

        policy.WithOrigins(allowedOrigins)
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });

    // Política más permisiva solo para desarrollo
    if (builder.Environment.IsDevelopment())
    {
        options.AddPolicy("AllowAll", policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
    }
});

// Registro de AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.
Log.Information("Configurando pipeline de middlewares...");

// 1. Manejo global de excepciones (debe ir primero)
app.UseGlobalExceptionHandling();

// 2. Logging de requests HTTP detallado
app.UseRequestLogging();

// 3. Configuración de desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.DocumentTitle = "FQP Web API";
        options.RoutePrefix = "swagger"; // Swagger disponible en /swagger
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "FQP Web API V1");
    });
    
    Log.Information("Swagger UI habilitado en: https://localhost:7124/swagger");
}
else
{
    // En producción, usar HSTS para mayor seguridad
    app.UseHsts();
}

// 4. Redirección HTTPS y seguridad
app.UseHttpsRedirection();

// 5. CORS - Configuración específica por entorno
if (app.Environment.IsDevelopment())
{
    app.UseCors("AllowAll");
    Log.Information("CORS configurado: Política permisiva para desarrollo");
}
else
{
    app.UseCors("AllowedOrigins");
    Log.Information("CORS configurado: Orígenes específicos para producción");
}

// 6. Autenticación y autorización (cuando se implemente)
app.UseAuthentication();
app.UseAuthorization();

// 7. Logging de Serilog para ASP.NET Core
app.UseSerilogRequestLogging(options =>
{
    // Personalizar el mensaje de log de requests
    options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} respondió {StatusCode} en {Elapsed:0.0000} ms";
    
    // Agregar información adicional
    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
        diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
        diagnosticContext.Set("RequestId", httpContext.Items["RequestId"] ?? "unknown");
        diagnosticContext.Set("UserAgent", httpContext.Request.Headers["User-Agent"].FirstOrDefault() ?? "unknown");
        diagnosticContext.Set("RemoteIP", httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown");
    };
});

// 8. Mapear controladores
app.MapControllers();

// 9. Endpoint de salud para monitoreo
app.MapGet("/health", () => new { 
    status = "healthy", 
    timestamp = DateTime.UtcNow,
    environment = app.Environment.EnvironmentName,
    version = "1.0.0"
}).WithTags("Health");

// Log de inicio completo
Log.Information("=== Aplicación FQP WebAPI iniciada correctamente ===");
Log.Information("Entorno: {Environment}", app.Environment.EnvironmentName);
Log.Information("URLs disponibles:");
Log.Information("  - HTTPS: https://localhost:7124");
Log.Information("  - Health Check: https://localhost:7124/health");

if (app.Environment.IsDevelopment())
{
    Log.Information("  - Swagger UI: https://localhost:7124");
}

try
{
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "La aplicación falló al iniciarse");
    throw;
}
finally
{
    Log.Information("=== Aplicación FQP WebAPI finalizando ===");
    Log.CloseAndFlush();
}
