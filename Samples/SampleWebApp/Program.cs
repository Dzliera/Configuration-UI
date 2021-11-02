using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using ConfigurationUi.Extensions;
using NJsonSchema;

namespace SampleWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var schema = JsonSchema4.FromTypeAsync<Settings>().Result;
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .AddConfigurationUi<Settings>("app_data/settings.json")
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        
    }
}