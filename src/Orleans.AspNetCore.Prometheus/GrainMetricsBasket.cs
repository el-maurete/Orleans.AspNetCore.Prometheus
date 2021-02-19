using Prometheus;

namespace Orleans.AspNetCore.Prometheus
{
    public static class GrainMetricsBasket
    {
        public static readonly Histogram MethodCallDuration = Metrics.CreateHistogram(
            "orleans_grain_method_call_duration", "The amount of time needed by the grain method to return.",
            new HistogramConfiguration
            {
                LabelNames = new[] {"grain_type", "grain_method"}
            });
        
        public static readonly Counter ErrorCount = Metrics.CreateCounter(
            "orleans_grain_method_error_count", "The number of times the method ended with an error.",
            new CounterConfiguration
            {
                LabelNames = new[] {"grain_type", "grain_method"}
            });
    }
}
