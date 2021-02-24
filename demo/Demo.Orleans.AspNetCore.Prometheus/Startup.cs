using Demo.Orleans.AspNetCore.Prometheus.Grains;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Orleans;
using Orleans.Hosting;
using Prometheus;
using Orleans.AspNetCore.Prometheus;

namespace Demo.Orleans.AspNetCore.Prometheus
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDashboard(); // (optional) use nuget package 'OrleansDashboard'
            services.AddOrleansMetrics(); // ADD THIS - components for metrics collection
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseOrleansMetrics(); // ADD THIS - orleans metrics collection on demand
            app.UseOrleansDashboardOffline();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapMetrics();
                endpoints.MapControllers();
            });
        }
        
        public static void ConfigureOrleans(ISiloBuilder siloBuilder)
        {
            siloBuilder
                .UseLocalhostClustering()
                .ConfigureApplicationParts(parts => parts
                    .AddOrleansMetricsPart() // ADD THIS - orleans metrics components
                    .AddApplicationPart(typeof(DemoGrain).Assembly).WithReferences());
        }
    }
}
