using Serilog;
using System.Diagnostics;
using System.Text;

namespace WebAPI.Middleware
{
    /// <summary>
    /// Middleware para logging detallado de requests HTTP
    /// </summary>
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            var requestId = Guid.NewGuid().ToString();
            
            // Agregar RequestId al contexto para otros middlewares/controladores
            context.Items["RequestId"] = requestId;

            // Capturar información de la request
            var request = context.Request;
            var requestInfo = new
            {
                RequestId = requestId,
                Method = request.Method,
                Path = request.Path.Value,
                QueryString = request.QueryString.Value,
                Headers = request.Headers.ToDictionary(h => h.Key, h => string.Join(";", h.Value)),
                UserAgent = request.Headers["User-Agent"].FirstOrDefault(),
                RemoteIP = context.Connection.RemoteIpAddress?.ToString(),
                Timestamp = DateTime.UtcNow
            };

            // Log de inicio de request
            using (Serilog.Context.LogContext.PushProperty("RequestId", requestId))
            using (Serilog.Context.LogContext.PushProperty("RequestPath", request.Path.Value))
            using (Serilog.Context.LogContext.PushProperty("HttpMethod", request.Method))
            using (Serilog.Context.LogContext.PushProperty("RemoteIP", context.Connection.RemoteIpAddress?.ToString()))
            {
                _logger.LogInformation("Request iniciada: {Method} {Path} {QueryString}", 
                    request.Method, request.Path, request.QueryString);

                // Capturar body de la request para POST/PUT (solo para debugging)
                string requestBody = "";
                if (request.Method == "POST" || request.Method == "PUT")
                {
                    requestBody = await CaptureRequestBody(request);
                    if (!string.IsNullOrEmpty(requestBody) && requestBody.Length < 1000) // Solo loggear bodies pequeños
                    {
                        _logger.LogDebug("Request body: {RequestBody}", requestBody);
                    }
                }

                try
                {
                    // Ejecutar siguiente middleware
                    await _next(context);

                    stopwatch.Stop();

                    // Log de finalización exitosa
                    using (Serilog.Context.LogContext.PushProperty("StatusCode", context.Response.StatusCode))
                    using (Serilog.Context.LogContext.PushProperty("ElapsedMilliseconds", stopwatch.ElapsedMilliseconds))
                    {
                        _logger.LogInformation("Request completada: {Method} {Path} - Status: {StatusCode} - Duración: {ElapsedMs}ms",
                            request.Method, request.Path, context.Response.StatusCode, stopwatch.ElapsedMilliseconds);
                    }
                }
                catch (Exception ex)
                {
                    stopwatch.Stop();

                    // Log de error con información detallada
                    using (Serilog.Context.LogContext.PushProperty("StatusCode", 500))
                    using (Serilog.Context.LogContext.PushProperty("ElapsedMilliseconds", stopwatch.ElapsedMilliseconds))
                    {
                        _logger.LogError(ex, "Error en request: {Method} {Path} - Duración: {ElapsedMs}ms - Error: {ErrorMessage}",
                            request.Method, request.Path, stopwatch.ElapsedMilliseconds, ex.Message);

                        // Log de información detallada del error
                        _logger.LogError("Detalles del error - RequestId: {RequestId}, StackTrace: {StackTrace}, InnerException: {InnerException}",
                            requestId, ex.StackTrace, ex.InnerException?.Message);
                    }

                    // Re-lanzar la excepción para que otros middlewares la manejen
                    throw;
                }
            }
        }

        private async Task<string> CaptureRequestBody(HttpRequest request)
        {
            try
            {
                // Habilitar buffering para poder leer el stream múltiples veces
                request.EnableBuffering();
                
                using var reader = new StreamReader(
                    request.Body, 
                    encoding: Encoding.UTF8, 
                    detectEncodingFromByteOrderMarks: false, 
                    leaveOpen: true);
                
                var body = await reader.ReadToEndAsync();
                
                // Resetear la posición del stream para que otros middlewares puedan leerlo
                request.Body.Position = 0;
                
                return body;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "No se pudo capturar el body de la request");
                return "";
            }
        }
    }

    /// <summary>
    /// Extensión para registrar el middleware de logging
    /// </summary>
    public static class RequestLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestLoggingMiddleware>();
        }
    }
}
