using System;

// ReSharper disable ClassNeverInstantiated.Global
namespace Orleans.AspNetCore.Prometheus
{
    public class CollectMetricsAttribute : Attribute { }
    public class ExcludeFromMetricsCollectionAttribute : Attribute { }
}
