using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
using System.Collections.Generic;

namespace GlobalAPINFe.SwaggerUtils
{
    public class FileUploadOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Obter os parâmetros do método
            var parameters = context.MethodInfo.GetParameters();

            // Verificar se há parâmetros do tipo IFormFile ou IFormFileCollection
            var fileParameters = parameters
                .Where(p => p.ParameterType == typeof(IFormFile) || p.ParameterType == typeof(IFormFileCollection))
                .ToList();

            if (fileParameters.Count == 0)
                return;

            // Criar o esquema para o corpo da requisição
            var properties = new Dictionary<string, OpenApiSchema>();
            foreach (var parameter in parameters)
            {
                var schema = new OpenApiSchema { Type = "string" };
                if (parameter.ParameterType == typeof(IFormFile) || parameter.ParameterType == typeof(IFormFileCollection))
                {
                    schema.Format = "binary";
                }
                properties[parameter.Name] = schema;
            }

            var requiredProperties = parameters
                .Where(p => !p.IsOptional && !p.HasDefaultValue)
                .Select(p => p.Name)
                .ToHashSet();

            operation.RequestBody = new OpenApiRequestBody
            {
                Content =
            {
                ["multipart/form-data"] = new OpenApiMediaType
                {
                    Schema = new OpenApiSchema
                    {
                        Type = "object",
                        Properties = properties,
                        Required = requiredProperties
                    }
                }
            }
            };
        }
    }
}
