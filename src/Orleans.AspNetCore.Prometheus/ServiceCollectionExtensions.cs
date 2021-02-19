using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Orleans.ApplicationParts;
using Prometheus;

namespace Orleans.AspNetCore.Prometheus
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOrleansMetrics(
            this IServiceCollection services,
            Action<GrainMetricsOptions> options)
        {
            var opt = new GrainMetricsOptions();
            options?.Invoke(opt);
            return AddOrleansMetrics(services, opt);
        }
        
        public static IServiceCollection AddOrleansMetrics(
            this IServiceCollection services,
            GrainMetricsOptions options = null)
        {
            return services
                .AddSingleton(options ?? new GrainMetricsOptions())
                .AddSingleton<IIncomingGrainCallFilter, GrainMetricsFilter>();
        }
        
        public static IApplicationPartManager AddOrleansMetricsPart(
            this IApplicationPartManager parts)
        {
            return parts
                .AddApplicationPart(typeof(PrometheusMetricsGrain).Assembly)
                .WithReferences();
        }

        public static void UseOrleansMetrics(
            this IApplicationBuilder builder)
        {
            Metrics.DefaultRegistry.AddBeforeCollectCallback(_ =>
            {
                var grainFactory = builder.ApplicationServices.GetRequiredService<IGrainFactory>();
                var metricsGrain = grainFactory.GetGrain<IPrometheusMetricsGrain>(0);
                return metricsGrain.Collect();
            });
        }
    }
}
