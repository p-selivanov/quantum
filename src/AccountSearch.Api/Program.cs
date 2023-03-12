using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Quantum.AccountSearch.Api;

public class Program
{
    public static int Main(string[] args)
    {
        try
        {
            CreateHostBuilder(args).Build().Run();
        }
        catch(OperationCanceledException) 
        {
        }

        return 0;
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}