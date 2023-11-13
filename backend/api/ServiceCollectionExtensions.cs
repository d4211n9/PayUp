using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using service.services;
using infrastructure;
using Microsoft.OpenApi.Models;

namespace api;

public static class ServiceCollectionExtensions
{
    public static void AddJwtService(this IServiceCollection services)
    {
        services.AddSingleton<JwtOptions>(services =>
        {
            var configuration = services.GetRequiredService<IConfiguration>();
            var options = configuration.GetRequiredSection("JWT").Get<JwtOptions>()!;
    
            // If address isn't set in the config then we are likely running in development mode.
            // We will use the address of the server as *issuer* for JWT.
            if (string.IsNullOrEmpty(options?.Address))
            {
                var server = services.GetRequiredService<IServer>();
                var addresses = server.Features.Get<IServerAddressesFeature>()?.Addresses;
                options.Address = addresses?.FirstOrDefault();
            }

            return options;
        });
        services.AddSingleton<JwtService>();
    }
}