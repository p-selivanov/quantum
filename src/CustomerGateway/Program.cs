using CustomerGateway.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CustomerGateway;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddAuthentication()
            .AddScheme<FakeTokenAuthOptions, FakeTokenAuthHandler>("FakeToken", null);

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("ValidToken", policy =>
                policy.RequireAuthenticatedUser());
        });

        var reverseProxyConfig = new ReverseProxyConfig(builder.Configuration);

        builder.Services.AddReverseProxy()
            .LoadFromMemory(reverseProxyConfig.GetRoutes(), reverseProxyConfig.GetClusters());

        var app = builder.Build();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapReverseProxy();

        app.Run();
    }
}