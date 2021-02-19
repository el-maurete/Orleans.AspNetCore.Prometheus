using Demo.Orleans.AspNetCore.Prometheus.Grains;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using Prometheus;
using System;
using System.Threading.Tasks;

namespace Demo.Orleans.AspNetCore.Prometheus.Controllers
{
    [Route("/hello")]
    public class DemoController : ControllerBase
    {
        // add some custom metrics to the mix
        private static readonly Counter VisitsCounter =
            Metrics.CreateCounter("demo_visits_counter", 
                "How many times the demo endpoint has been called since last restart.");
        
        private readonly IGrainFactory _client;

        public DemoController(IGrainFactory client) => _client = client;
        
        [HttpGet]
        public async Task<ActionResult<string>> Get()
        {
            // increment visit count in metrics
            VisitsCounter.Inc();
            
            // pick at random 1 of 10 demo grains
            var randomDemoGrain = _client.GetGrain<IDemoGrain>(new Random().Next(10));
            
            // return the message from the grain
            string message;
            try {
                message = await randomDemoGrain.GetMessage();
            } catch (Exception exc) {
                message = exc.Message;
            }
            
            return Ok(message);
        }
    }
}
