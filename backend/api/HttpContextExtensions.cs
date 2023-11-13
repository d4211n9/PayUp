using infrastructure.dataModels;
using Microsoft.OpenApi.Models;
using service;

namespace api;

public static class HttpContextExtensions
{
    public static void SetSessionData(this HttpContext httpContext, SessionData data)
    {
        httpContext.Items["data"] = data;
    }

    public static SessionData? GetSessionData(this HttpContext httpContext)
    {
        return httpContext.Items["data"] as SessionData;
    }
    
    public static void AddSwaggerGenWithBearerJWT(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new string[] { }
                    }
                });
            }
        );
    }
}
