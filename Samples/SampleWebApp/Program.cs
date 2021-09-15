using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using ConfigurationUi.Extensions;

namespace SampleWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .AddConfigurationUi<Settings>("app_data/settings.json")
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}