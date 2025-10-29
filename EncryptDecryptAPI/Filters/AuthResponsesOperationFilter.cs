using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Encrpt_Decrpt_API.Filters
{
    public class AuthResponsesOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var declaringType = context.MethodInfo.DeclaringType;
            var authAttributes = Enumerable.Empty<AuthorizeAttribute>();
            if (declaringType != null)
            {
                authAttributes = declaringType.GetCustomAttributes(true)
                    .Union(context.MethodInfo.GetCustomAttributes(true))
                    .OfType<AuthorizeAttribute>();
            }
            else
            {
                authAttributes = context.MethodInfo.GetCustomAttributes(true)
                    .OfType<AuthorizeAttribute>();
            }

            if (authAttributes.Any())
            {
                var securityRequirement = new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new List<string>()
                    }
                };
                operation.Security = new List<OpenApiSecurityRequirement> {
                    securityRequirement
                };
                operation.Responses.Add("401", new OpenApiResponse
                {
                    Description = "Unauthorized"
                });
            }

            // get roles at method level first.
            var roles = authAttributes
                 .Select(a => a.Roles)
                 .Distinct()
                 .ToArray();

            // we dont find roles at method level then check for controller level.
            if (!roles.Any() && declaringType != null)
            {
                roles = declaringType
                     .GetCustomAttributes(true)
                     .OfType<AuthorizeAttribute>()
                     .Select(attr => attr.Roles)
                     .Distinct()
                     .ToArray();
            }

            if (roles.Any())
            {
                string rolesStr = string.Join(",", roles);
                // we can choose summary or description as per our preference
                operation.Description += $"<p> Required Roles ({rolesStr})</p>";
            }
        }
    }
}
