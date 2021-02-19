using Prometheus;

namespace Orleans.AspNetCore.Prometheus
{
    public static class MetricsBasket
    {
        public static readonly Histogram MetricsLoadDuration = Metrics.CreateHistogram(
            "orleans_metrics_load_duration_bucket",
            "The amount of time needed to load the metrics.");

        public static readonly Gauge ActiveHostRate = Metrics.CreateGauge(
            "orleans_active_host_rate",
            "The percentage of hosts that report as 'active'.");
        
        public static readonly Gauge GrainsActivationCount = Metrics.CreateGauge(
            "orleans_grains_activation_count",
            "Grains activation count, filterable by silo address and grain type.",
            new GaugeConfiguration
            {
                LabelNames = new [] { "silo_address", "grain_type" }
            });
        
        public static readonly Gauge SilosActivationCount = Metrics.CreateGauge(
            "orleans_activation_count",
            "Number of grains activated per silo.",
            new GaugeConfiguration
            {
                LabelNames = new [] { "silo_address" }
            });

        public static readonly Gauge SilosClientCount = Metrics.CreateGauge(
            "orleans_client_count",
            "Number of clients connected to the silo.",
            new GaugeConfiguration
            {
                LabelNames = new [] { "silo_address" }
            });
        
        public static readonly Gauge SilosReceivedMessagesCount = Metrics.CreateGauge(
            "orleans_received_messages_count",
            "Number of messages received by the silo.",
            new GaugeConfiguration
            {
                LabelNames = new [] { "silo_address" }
            });

        public static readonly Gauge SilosSentMessagesCount = Metrics.CreateGauge(
            "orleans_sent_messages_count",
            "Number of messages sent by the silo.",
            new GaugeConfiguration
            {
                LabelNames = new [] { "silo_address" }
            });
        
        public static readonly Gauge SilosReceiveQueueLength = Metrics.CreateGauge(
            "orleans_receive_queue_length",
            "The length of the receive queue of the silo.",
            new GaugeConfiguration
            {
                LabelNames = new [] { "silo_address" }
            });
        
        public static readonly Gauge SilosSendQueueLength = Metrics.CreateGauge(
            "orleans_send_queue_length",
            "The length of the send queue of the silo.",
            new GaugeConfiguration
            {
                LabelNames = new [] { "silo_address" }
            });
        
        public static readonly Gauge SilosRecentlyUsedActivationCount = Metrics.CreateGauge(
            "orleans_recently_used_activation_count",
            "Recently used activation count per silo",
            new GaugeConfiguration
            {
                LabelNames = new [] { "silo_address" }
            });
        
        public static readonly Gauge SilosIsOverloaded = Metrics.CreateGauge(
            "orleans_is_silo_overloaded",
            "Is silo overloaded? (0 false, 1 true)",
            new GaugeConfiguration
            {
                LabelNames = new [] { "silo_address" }
            });
        
        public static readonly Gauge SilosAvailableMemory = Metrics.CreateGauge(
            "orleans_silo_available_memory",
            "Silo available memory. Must be explicitly enabled.",
            new GaugeConfiguration
            {
                LabelNames = new [] { "silo_address" }
            });
        
        public static readonly Gauge SilosCpuUsage = Metrics.CreateGauge(
            "orleans_silo_cpu_usage",
            "Silo CPU usage. Must be explicitly enabled.",
            new GaugeConfiguration
            {
                LabelNames = new [] { "silo_address" }
            });
        
        public static readonly Gauge SilosMemoryUsage = Metrics.CreateGauge(
            "orleans_silo_memory_usage",
            "Silo memory usage. Must be explicitly enabled.",
            new GaugeConfiguration
            {
                LabelNames = new [] { "silo_address" }
            });
        
        public static readonly Gauge SilosTotalPhysicalMemory = Metrics.CreateGauge(
            "orleans_silo_memory_usage",
            "Silo's total physical memory. Must be explicitly enabled.",
            new GaugeConfiguration
            {
                LabelNames = new [] { "silo_address" }
            });
        
        public static readonly Gauge HostCountPerStatus =  Metrics.CreateGauge(
            "orleans_host_count",
            "Hosts count by silo status.",
            new GaugeConfiguration
            {
                LabelNames = new [] { "silo_status" }
            });
    }
}
