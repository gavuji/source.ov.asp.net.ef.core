using FM21.Core;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace FM21.API.Extensions
{
    public class SwaggerHeaderFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "Content-Language",
                In = ParameterLocation.Header,
                AllowEmptyValue = false,
                Required = true,
                Description = ApplicationConstants.SupportedLocalization,
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Default = new OpenApiString("en-us")
                }
            });

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "UserID",
                In = ParameterLocation.Header,
                AllowEmptyValue = true,
                Required = true,
                Schema = new OpenApiSchema
                {
                    Type = "integer"
                }
            });
        }
    }
}