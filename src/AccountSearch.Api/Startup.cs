﻿using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quantum.AccountSearch.Api.Infrastructure;
using Quantum.AccountSearch.Api.Repositories;
using Quantum.AccountSearch.Persistence;
using Quantum.Lib.Common;

namespace Quantum.AccountSearch.Api;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<AccountSearchDbContext>(options =>
            options.UseNpgsql(Configuration.GetValue<string>("AccountSearchDb:ConnectionString")));

        services.AddScoped<CustomerAccountRepository>();

        services
            .AddControllers()
            .AddNewtonsoftJson(options =>
            {
                QuantumJson.ApplyDefaultSettings(options.SerializerSettings);
            });

        services
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
            .AddFluentValidationAutoValidation();

        services.AddHealthChecks();
    }

    public void Configure(IApplicationBuilder app)
    {
        if (Configuration.GetValue<bool>("migrate"))
        {
            app.RunEfMigrations<AccountSearchDbContext>(true);
            return;
        }

        app.UseDeveloperExceptionPage();

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHealthChecks("/health");
        });
    }
}
