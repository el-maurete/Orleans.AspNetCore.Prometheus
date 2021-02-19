using Orleans;
using Orleans.AspNetCore.Prometheus;
using System;
using System.Threading.Tasks;

namespace Demo.Orleans.AspNetCore.Prometheus.Grains
{
    public class DemoGrain : Grain, IDemoGrain
    {
        private static readonly Random Random = new Random();

        [CollectMetrics]
        public async Task<string> GetMessage()
        {
            var grainId = this.GetPrimaryKeyLong();
            
            // Simulate some async work that takes up to half second
            await Task.Delay(Random.Next(500));
            
            // Simulate 10% probability of throwing an exception
            if (Random.Next(10) == 1)
                throw new Exception($"Simulate an error on grain {grainId}");

            // Return hello
            return $"Hello from Grain {grainId}";
        }
    }
}
