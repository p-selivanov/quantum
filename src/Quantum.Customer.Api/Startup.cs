using System;
using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quantum.Customer.Api.Configuration;
using Quantum.Customer.Api.Services;
using Quantum.Customer.Repositories;
using Quantum.Lib.Common;
using Quantum.Lib.DynamoDb;

namespace Quantum.Customer.Api;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.Configure<DynamoTableOptions>(options =>
        {
            options.CustomerTableName = Configuration.GetValue<string>("DynamoDB:Tables:Customers");
        });

        services.AddDynamoDbClient(options =>
        {
            options.Region = Configuration.GetValue<string>("DynamoDB:Region");
            options.LocalStackUri = Configuration.GetValue<string>("DynamoDB:LocalStackUri");
        });

        services.AddScoped<CustomerRepository>();
        services.AddScoped<CustomerService>();

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
        app.UseDeveloperExceptionPage();

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHealthChecks("/health");
        });
    }
}
