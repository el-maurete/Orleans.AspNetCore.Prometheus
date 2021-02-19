using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Demo.Orleans.AspNetCore.Prometheus
{
    public class Program
    {
        public static void Main(string[] args) =>
            CreateHostBuilder(args).Build().Run();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseOrleans(Startup.ConfigureOrleans)
                .ConfigureWebHostDefaults(webBuilder =>
                    webBuilder.UseStartup<Startup>());
    }
}
