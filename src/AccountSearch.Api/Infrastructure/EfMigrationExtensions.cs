using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Quantum.AccountSearch.Api.Infrastructure;

internal static class EfMigrationExtensions
{
    public static void RunEfMigrations<TDbContext>(this IApplicationBuilder app, bool exit)
        where TDbContext : DbContext
    {
        using var scope = app.ApplicationServices.CreateScope();
        var dbContext = scope.ServiceProvider.GetService<TDbContext>();
        dbContext.Database.Migrate();

        if (exit)
        {
            var hostLifetime = app.ApplicationServices.GetService<IHostApplicationLifetime>();
            hostLifetime.StopApplication();
        }
    }
}
