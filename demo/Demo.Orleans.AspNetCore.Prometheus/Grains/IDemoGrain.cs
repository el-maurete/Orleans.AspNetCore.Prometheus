using Orleans;
using System.Threading.Tasks;

namespace Demo.Orleans.AspNetCore.Prometheus.Grains
{
    public interface IDemoGrain : IGrainWithIntegerKey
    {
        Task<string> GetMessage();
    }
}