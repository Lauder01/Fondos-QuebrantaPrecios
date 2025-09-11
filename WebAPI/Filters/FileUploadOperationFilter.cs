using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace WebAPI.Filters
{
    public class FileUploadOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var hasFileParameter = context.MethodInfo.GetParameters()
                .Any(p => p.ParameterType == typeof(IFormFile) || p.ParameterType == typeof(IFormFile[]));

            if (!hasFileParameter)
                return;

            // Asegurar que operation.RequestBody existe
            operation.RequestBody ??= new OpenApiRequestBody
            {
                Content = new Dictionary<string, OpenApiMediaType>()
            };

            operation.RequestBody.Content ??= new Dictionary<string, OpenApiMediaType>();

            operation.RequestBody.Content["multipart/form-data"] = new OpenApiMediaType
            {
                Schema = new OpenApiSchema
                {
                    Type = "object",
                    Properties = new Dictionary<string, OpenApiSchema>()
                }
            };

            // Agregar parámetros que tienen [FromForm]
            foreach (var parameter in context.MethodInfo.GetParameters())
            {
                var hasFromFormAttribute = parameter.GetCustomAttribute<Microsoft.AspNetCore.Mvc.FromFormAttribute>() != null;
                
                if (hasFromFormAttribute)
                {
                    if (parameter.ParameterType == typeof(IFormFile))
                    {
                        operation.RequestBody.Content["multipart/form-data"].Schema.Properties[parameter.Name!] = new OpenApiSchema
                        {
                            Type = "string",
                            Format = "binary",
                            Description = "Archivo a subir"
                        };
                    }
                    else if (parameter.ParameterType == typeof(string))
                    {
                        operation.RequestBody.Content["multipart/form-data"].Schema.Properties[parameter.Name!] = new OpenApiSchema
                        {
                            Type = "string",
                            Description = GetParameterDescription(parameter.Name!)
                        };
                    }
                }
            }

            // Limpiar parámetros originales para evitar duplicados en Swagger UI
            operation.Parameters = operation.Parameters?.Where(p => 
                !context.MethodInfo.GetParameters().Any(mp => 
                    mp.Name == p.Name && mp.GetCustomAttribute<Microsoft.AspNetCore.Mvc.FromFormAttribute>() != null)).ToList();
        }

        private static string GetParameterDescription(string parameterName)
        {
            return parameterName.ToLower() switch
            {
                "buildingid" => "ID del edificio al que pertenece la imagen",
                "filename" => "Nombre del archivo de imagen (opcional)",
                "alttext" => "Texto alternativo para accesibilidad (opcional)",
                "file" => "Archivo de imagen a subir",
                _ => $"Parámetro {parameterName}"
            };
        }
    }
}
