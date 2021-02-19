using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Threading.Tasks;
using Prometheus;
using static Orleans.AspNetCore.Prometheus.GrainMetricsBasket;

namespace Orleans.AspNetCore.Prometheus
{
    public class GrainMetricsFilter : IIncomingGrainCallFilter
    {
        private static readonly ConcurrentDictionary<MethodInfo, bool> MethodCache =
            new ConcurrentDictionary<MethodInfo, bool>();

        private readonly GrainMetricsOptions _options;

        public GrainMetricsFilter(GrainMetricsOptions options) =>
            _options = options;

        public async Task Invoke(IIncomingGrainCallContext context)
        {
            if (ShouldTrack(context, out var grainTypeName, out var grainMethodName))
                await Track(grainTypeName, grainMethodName, context.Invoke);
            else
                await context.Invoke();
        }

        private static async Task Track(string grainTypeName, string grainMethodName, Func<Task> invoke)
        {
            using (MethodCallDuration
                .WithLabels(grainTypeName, grainMethodName)
                .NewTimer())
            {
                await ErrorCount
                    .WithLabels(grainTypeName, grainMethodName)
                    .CountExceptionsAsync(invoke);
            }
        } 

        private bool ShouldTrack(
            IIncomingGrainCallContext context, 
            out string grainTypeName,
            out string grainMethodName)
        {
            var grainType = context.Grain.GetType();
            grainTypeName = grainType.FullName;
            
            var implementationMethod = context.ImplementationMethod;
            grainMethodName = implementationMethod.Name;
            
            return MethodCache.GetOrAdd(implementationMethod, grainMethod =>
                grainType.GetCustomAttribute<ExcludeFromMetricsCollectionAttribute>() == null &&
                grainMethod.GetCustomAttribute<ExcludeFromMetricsCollectionAttribute>() == null &&
                (_options.DefaultCollectMetrics 
                 || grainType.GetCustomAttribute<CollectMetricsAttribute>() != null
                 || grainMethod.GetCustomAttribute<CollectMetricsAttribute>() != null));
        }
    }
}
