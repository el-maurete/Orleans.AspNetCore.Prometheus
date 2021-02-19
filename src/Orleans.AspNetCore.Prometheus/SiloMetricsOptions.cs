using System;

namespace Orleans.AspNetCore.Prometheus
{
    public class SiloMetricsOptions
    {
        public TimeSpan DelayBeforeStartPolling { get; set; } = TimeSpan.FromSeconds(5);
        public TimeSpan PollingFrequency { get; set; } = TimeSpan.FromSeconds(5);
    }
}