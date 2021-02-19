using System.Threading.Tasks;

namespace Orleans.AspNetCore.Prometheus
{
    public interface IPrometheusMetricsGrain : IGrainWithIntegerKey
    {
        Task Collect();
    }
}