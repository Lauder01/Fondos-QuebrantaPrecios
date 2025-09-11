using Serilog;
using System.Net;
using System.Text.Json;

namespace WebAPI.Middleware
{
    /// <summary>
    /// Middleware para manejo global de excepciones con logging detallado
    /// </summary>
    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;
        private readonly IWebHostEnvironment _environment;

        public GlobalExceptionHandlingMiddleware(
            RequestDelegate next, 
            ILogger<GlobalExceptionHandlingMiddleware> logger,
            IWebHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var requestId = context.Items["RequestId"]?.ToString() ?? Guid.NewGuid().ToString();
            
            // Determinar el tipo de error y status code
            var (statusCode, errorType, userMessage) = GetErrorInfo(exception);

            // Crear información detallada del error
            var errorDetails = new
            {
                RequestId = requestId,
                Timestamp = DateTime.UtcNow,
                Path = context.Request.Path.Value,
                Method = context.Request.Method,
                StatusCode = (int)statusCode,
                ErrorType = errorType,
                Message = userMessage,
                ExceptionType = exception.GetType().Name,
                InnerException = exception.InnerException?.Message,
                UserAgent = context.Request.Headers["User-Agent"].FirstOrDefault(),
                RemoteIP = context.Connection.RemoteIpAddress?.ToString()
            };

            // Log detallado del error con contexto
            using (Serilog.Context.LogContext.PushProperty("RequestId", requestId))
            using (Serilog.Context.LogContext.PushProperty("RequestPath", context.Request.Path.Value))
            using (Serilog.Context.LogContext.PushProperty("HttpMethod", context.Request.Method))
            using (Serilog.Context.LogContext.PushProperty("StatusCode", (int)statusCode))
            using (Serilog.Context.LogContext.PushProperty("ErrorType", errorType))
            using (Serilog.Context.LogContext.PushProperty("RemoteIP", context.Connection.RemoteIpAddress?.ToString()))
            {
                if (statusCode == HttpStatusCode.InternalServerError)
                {
                    _logger.LogError(exception, 
                        "Error interno del servidor - RequestId: {RequestId}, Path: {Path}, ErrorType: {ErrorType}, Message: {Message}",
                        requestId, context.Request.Path.Value, errorType, exception.Message);
                }
                else
                {
                    _logger.LogWarning(exception,
                        "Error de cliente - RequestId: {RequestId}, Path: {Path}, StatusCode: {StatusCode}, ErrorType: {ErrorType}, Message: {Message}",
                        requestId, context.Request.Path.Value, (int)statusCode, errorType, exception.Message);
                }

                // Log adicional con información de contexto
                _logger.LogDebug("Contexto del error - User-Agent: {UserAgent}, IP: {RemoteIP}, Headers: {@Headers}",
                    context.Request.Headers["User-Agent"].FirstOrDefault(),
                    context.Connection.RemoteIpAddress?.ToString(),
                    context.Request.Headers.ToDictionary(h => h.Key, h => string.Join(";", h.Value)));
            }

            // Preparar respuesta para el cliente
            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";

            var response = new
            {
                Error = new
                {
                    RequestId = requestId,
                    Timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"),
                    Status = (int)statusCode,
                    Type = errorType,
                    Message = userMessage,
                    Path = context.Request.Path.Value,
                    // Solo incluir detalles técnicos en desarrollo
                    Details = _environment.IsDevelopment() ? new
                    {
                        ExceptionType = exception.GetType().Name,
                        StackTrace = exception.StackTrace,
                        InnerException = exception.InnerException?.Message
                    } : null
                }
            };

            var jsonResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            });

            await context.Response.WriteAsync(jsonResponse);
        }

        private static (HttpStatusCode statusCode, string errorType, string userMessage) GetErrorInfo(Exception exception)
        {
            return exception switch
            {
                // Errores de base de datos - Los más específicos primero
                Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException => (HttpStatusCode.Conflict, "ConcurrencyError", "Los datos han sido modificados por otro usuario"),
                Microsoft.EntityFrameworkCore.DbUpdateException => (HttpStatusCode.Conflict, "DatabaseError", "Error al actualizar los datos"),
                
                // Errores de validación
                ArgumentNullException => (HttpStatusCode.BadRequest, "ValidationError", "Faltan datos requeridos"),
                ArgumentException => (HttpStatusCode.BadRequest, "ValidationError", "Los datos proporcionados no son válidos"),
                InvalidOperationException => (HttpStatusCode.BadRequest, "OperationError", "La operación solicitada no es válida en este contexto"),
                
                // Errores de no encontrado
                FileNotFoundException => (HttpStatusCode.NotFound, "NotFound", "El archivo solicitado no fue encontrado"),
                KeyNotFoundException => (HttpStatusCode.NotFound, "NotFound", "El recurso solicitado no fue encontrado"),
                
                // Errores de acceso no autorizado
                UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "Unauthorized", "No tiene permisos para acceder a este recurso"),
                
                // Errores de timeout
                TimeoutException => (HttpStatusCode.RequestTimeout, "Timeout", "La operación ha excedido el tiempo límite"),
                
                // Error genérico
                _ => (HttpStatusCode.InternalServerError, "InternalError", "Ha ocurrido un error interno en el servidor")
            };
        }
    }

    /// <summary>
    /// Extensión para registrar el middleware de manejo global de errores
    /// </summary>
    public static class GlobalExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GlobalExceptionHandlingMiddleware>();
        }
    }
}
