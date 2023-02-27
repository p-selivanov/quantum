using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Converters;
using Quantum.Customer.Api.Infrastructure;
using Quantum.Customer.Api.Services;
using Quantum.Customer.Repositories;
using Quantum.Lib.AspNet;

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
        services.AddDynamoDbClient(Configuration.GetValue<string>("DynamoDB:Region"));

        services.AddScoped<CustomerRepository>();
        services.AddScoped<CustomerService>();

        services
            .AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
                options.SerializerSettings.Converters.Add(new SpecifiableConverter());
                options.SerializerSettings.ContractResolver = new CustomJsonContractResolver();
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
