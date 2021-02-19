using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Orleans.Runtime;
using Prometheus;
using static Orleans.AspNetCore.Prometheus.MetricsBasket;

namespace Orleans.AspNetCore.Prometheus
{
    [ExcludeFromMetricsCollectionAttribute]
    public class PrometheusMetricsGrain : Grain, IPrometheusMetricsGrain
    {
        private readonly ILogger<PrometheusMetricsGrain> _logger;

        public PrometheusMetricsGrain(ILogger<PrometheusMetricsGrain> logger) =>
            _logger = logger;

        public async Task Collect()
        {
            _logger.LogDebug("Collecting metrics");
            using (MetricsLoadDuration.NewTimer())
            {
                var grain = GrainFactory.GetGrain<IManagementGrain>(0);
                var allHosts = await grain.GetHosts();
                var hostsPerStatus = allHosts.ToLookup(h => h.Value);
                var activeHosts = hostsPerStatus.Contains(SiloStatus.Active)
                    ? hostsPerStatus[SiloStatus.Active].Select(x => x.Key).ToArray()
                    : new SiloAddress[0];

                foreach (var status in hostsPerStatus)
                {
                    HostCountPerStatus
                        .WithLabels(status.Key.ToString())
                        .Set(status.Count());
                }

                var activeHostsCount = activeHosts.Length;

                ActiveHostRate.Set((double) activeHostsCount / allHosts.Count);

                if (activeHostsCount > 0)
                {
                    var simpleGrainStatsTask = grain.GetSimpleGrainStatistics(activeHosts.ToArray());
                    var runtimeStatsTask = grain.GetRuntimeStatistics(activeHosts.ToArray());

                    foreach (var grainStats in await simpleGrainStatsTask)
                    {
                        GrainsActivationCount
                            .WithLabels(grainStats.SiloAddress.ToParsableString(), grainStats.GrainType)
                            .Set(grainStats.ActivationCount);
                    }

                    var silos = activeHosts
                        .Zip(await runtimeStatsTask, (address, stats) => (address, stats));

                    foreach (var (siloAddress, siloStats) in silos)
                    {
                        SilosActivationCount
                            .WithLabels(siloAddress.ToParsableString())
                            .Set(siloStats.ActivationCount);
                        SilosClientCount
                            .WithLabels(siloAddress.ToParsableString())
                            .Set(siloStats.ClientCount);
                        SilosReceivedMessagesCount
                            .WithLabels(siloAddress.ToParsableString())
                            .Set(siloStats.ReceivedMessages);
                        SilosSentMessagesCount
                            .WithLabels(siloAddress.ToParsableString())
                            .Set(siloStats.SentMessages);
                        SilosReceiveQueueLength
                            .WithLabels(siloAddress.ToParsableString())
                            .Set(siloStats.ReceiveQueueLength);
                        SilosSendQueueLength
                            .WithLabels(siloAddress.ToParsableString())
                            .Set(siloStats.SendQueueLength);
                        SilosRecentlyUsedActivationCount
                            .WithLabels(siloAddress.ToParsableString())
                            .Set(siloStats.RecentlyUsedActivationCount);
                        SilosIsOverloaded
                            .WithLabels(siloAddress.ToParsableString())
                            .Set(siloStats.IsOverloaded ? 1 : 0);

                        if (siloStats.AvailableMemory.HasValue)
                            SilosAvailableMemory
                                .WithLabels(siloAddress.ToParsableString())
                                .Set(siloStats.AvailableMemory.Value);

                        if (siloStats.CpuUsage.HasValue)
                            SilosCpuUsage
                                .WithLabels(siloAddress.ToParsableString())
                                .Set(siloStats.CpuUsage.Value);

                        if (siloStats.MemoryUsage.HasValue)
                            SilosMemoryUsage
                                .WithLabels(siloAddress.ToParsableString())
                                .Set(siloStats.MemoryUsage.Value);

                        if (siloStats.TotalPhysicalMemory.HasValue)
                            SilosTotalPhysicalMemory
                                .WithLabels(siloAddress.ToParsableString())
                                .Set(siloStats.TotalPhysicalMemory.Value);
                    }
                }
            }

            _logger.LogDebug("Metrics updated");
        }
    }
}
