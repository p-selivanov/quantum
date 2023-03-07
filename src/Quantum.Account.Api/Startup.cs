﻿using System;
using System.Linq;
using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quantum.Account.Api.Configuration;
using Quantum.Account.Api.Repositories;
using Quantum.Lib.Common;
using Quantum.Lib.DynamoDb;

namespace Quantum.Account.Api;

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
            options.AccountTransactionTableName = Configuration.GetValue<string>("DynamoDB:Tables:AccountTransactions");
        });

        services.Configure<CurrencyOptions>(options =>
        {
            var currencyString = Configuration.GetValue<string>("Currencies");
            if (string.IsNullOrEmpty(currencyString) is false)
            {
                options.Currencies = currencyString.Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Trim().ToUpper())
                    .ToArray();
            }
        });

        services.Configure<CommissionOptions>(options =>
        {
            options.DepositCommisionPercent = Configuration.GetValue<decimal>("Commissions:Deposit");
            options.WithdrawalCommisionPercent = Configuration.GetValue<decimal>("Commissions:Withdrawal");
        });

        services.AddDynamoDbClient(Configuration.GetValue<string>("DynamoDB:Region"));

        services.AddScoped<CustomerRepository>();
        services.AddScoped<AccountRepository>();
        services.AddScoped<TransactionRepository>();

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
