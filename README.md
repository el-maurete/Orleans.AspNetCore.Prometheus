# Orleans.AspNetCore.Prometheus
Simple library to help exposing Orleans metrics to Prometheus.

## Integrate in your AspNetCore + Orleans service 

### Install the package

```Orleans.AspNetCore.Prometheus```

### In the Startup class:

```c#
using Prometheus; // ADD THIS
using Orleans.AspNetCore.Prometheus; // ADD THIS
...
```

```c#
public void ConfigureServices(IServiceCollection services)
{
    services.AddOrleansMetrics(); // ADD THIS
    ...
}
```

```c#
public void Configure(IApplicationBuilder app)
{
    ...
    app.UseOrleansMetrics(); // ADD THIS
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapMetrics(); // ADD THIS
        ...
    });
    ...
}
```

```c#
public static void ConfigureOrleans(ISiloBuilder siloBuilder)
{
    siloBuilder
        .ConfigureApplicationParts(parts => 
            parts.AddOrleansMetricsPart() // ADD THIS
    ...
}
```

## A demo application

### Build and run the demo application locally

```powershell
cd demo/Demo.Orleans.AspNetCore.Prometheus
dotnet run 
```

- Navigate to http://localhost:5000/ for Orleans dashboard;
- Navigate to http://localhost:5000/hello to trigger some demo work;
- Navigate to http://localhost:5000/metrics for raw Prometheus metrics.

### Build and run the demo service on docker-compose

```powershell
cd demo
docker-compose up --build
```

- Navigate to http://localhost:5000/ for Orleans dashboard;
- Navigate to http://localhost:5000/hello to trigger some demo work;
- Navigate to http://localhost:5000/metrics for raw Prometheus metrics;
- Navigate to http://localhost:9090 for Prometheus dashboard.

### Understand the demo application

The demo application is an AspNetCore + Orleans API that expose both Orleans and AspNetCore metrics in a Prometheus compatible format on the /metrics endpoint.

The API exposes a single /hello endpoint. When hit:

- the controller picks one grain at random (id between 1 and 10);
- it invokes DemoGrain's GetMessage method and increments a visit count metric;
- the DemoGrain simulates some async work that takes a random amount of time (up to half second);
- sometimes(10% probability) it throws an exception to simulate an error,
- but more often (90% probability) returns "Hello from Grain {{ ID }}".

By hitting the /hello endpoint a few times, the /metrics endpoint should return something like this:

```
# HELP orleans_grain_method_error_count The number of times the method ended with an error.
# TYPE orleans_grain_method_error_count counter
orleans_grain_method_error_count{grain_type="Demo.Orleans.AspNetCore.Prometheus.Grains.DemoGrain",grain_method="GetMessage"} 1
# HELP demo_visits_counter How many times the demo endpoint has been called since last restart.
# TYPE demo_visits_counter counter
demo_visits_counter 28
# HELP process_virtual_memory_bytes Virtual memory size in bytes.
# TYPE process_virtual_memory_bytes gauge
process_virtual_memory_bytes 23715778560
# HELP process_start_time_seconds Start time of the process since unix epoch in seconds.
# TYPE process_start_time_seconds gauge
process_start_time_seconds 1613693166.09
# HELP orleans_metrics_load_duration_bucket The amount of time needed to load the metrics.
# TYPE orleans_metrics_load_duration_bucket histogram
orleans_metrics_load_duration_bucket_sum 0.08951139999999999
orleans_metrics_load_duration_bucket_count 20
orleans_metrics_load_duration_bucket_bucket{le="0.005"} 17
orleans_metrics_load_duration_bucket_bucket{le="0.01"} 19
orleans_metrics_load_duration_bucket_bucket{le="0.025"} 19
orleans_metrics_load_duration_bucket_bucket{le="0.05"} 20
orleans_metrics_load_duration_bucket_bucket{le="0.075"} 20
orleans_metrics_load_duration_bucket_bucket{le="0.1"} 20
orleans_metrics_load_duration_bucket_bucket{le="0.25"} 20
orleans_metrics_load_duration_bucket_bucket{le="0.5"} 20
orleans_metrics_load_duration_bucket_bucket{le="0.75"} 20
orleans_metrics_load_duration_bucket_bucket{le="1"} 20
orleans_metrics_load_duration_bucket_bucket{le="2.5"} 20
orleans_metrics_load_duration_bucket_bucket{le="5"} 20
orleans_metrics_load_duration_bucket_bucket{le="7.5"} 20
orleans_metrics_load_duration_bucket_bucket{le="10"} 20
orleans_metrics_load_duration_bucket_bucket{le="+Inf"} 20
# HELP orleans_activation_count Number of grains activated per silo.
# TYPE orleans_activation_count gauge
orleans_activation_count{silo_address="127.0.0.1:11111@351389167"} 14
# HELP process_open_handles Number of open handles
# TYPE process_open_handles gauge
process_open_handles 265
# HELP orleans_client_count Number of clients connected to the silo.
# TYPE orleans_client_count gauge
orleans_client_count{silo_address="127.0.0.1:11111@351389167"} 0
# HELP orleans_send_queue_length The length of the send queue of the silo.
# TYPE orleans_send_queue_length gauge
orleans_send_queue_length{silo_address="127.0.0.1:11111@351389167"} 0
# HELP process_working_set_bytes Process working set
# TYPE process_working_set_bytes gauge
process_working_set_bytes 170385408
# HELP dotnet_collection_count_total GC collection count
# TYPE dotnet_collection_count_total counter
dotnet_collection_count_total{generation="0"} 2
dotnet_collection_count_total{generation="2"} 2
dotnet_collection_count_total{generation="1"} 2
# HELP orleans_grain_method_call_duration The amount of time needed by the grain method to return.
# TYPE orleans_grain_method_call_duration histogram
orleans_grain_method_call_duration_sum{grain_type="Demo.Orleans.AspNetCore.Prometheus.Grains.DemoGrain",grain_method="GetMessage"} 8.0393434
orleans_grain_method_call_duration_count{grain_type="Demo.Orleans.AspNetCore.Prometheus.Grains.DemoGrain",grain_method="GetMessage"} 28
orleans_grain_method_call_duration_bucket{grain_type="Demo.Orleans.AspNetCore.Prometheus.Grains.DemoGrain",grain_method="GetMessage",le="0.005"} 0
orleans_grain_method_call_duration_bucket{grain_type="Demo.Orleans.AspNetCore.Prometheus.Grains.DemoGrain",grain_method="GetMessage",le="0.01"} 0
orleans_grain_method_call_duration_bucket{grain_type="Demo.Orleans.AspNetCore.Prometheus.Grains.DemoGrain",grain_method="GetMessage",le="0.025"} 0
orleans_grain_method_call_duration_bucket{grain_type="Demo.Orleans.AspNetCore.Prometheus.Grains.DemoGrain",grain_method="GetMessage",le="0.05"} 1
orleans_grain_method_call_duration_bucket{grain_type="Demo.Orleans.AspNetCore.Prometheus.Grains.DemoGrain",grain_method="GetMessage",le="0.075"} 3
orleans_grain_method_call_duration_bucket{grain_type="Demo.Orleans.AspNetCore.Prometheus.Grains.DemoGrain",grain_method="GetMessage",le="0.1"} 5
orleans_grain_method_call_duration_bucket{grain_type="Demo.Orleans.AspNetCore.Prometheus.Grains.DemoGrain",grain_method="GetMessage",le="0.25"} 10
orleans_grain_method_call_duration_bucket{grain_type="Demo.Orleans.AspNetCore.Prometheus.Grains.DemoGrain",grain_method="GetMessage",le="0.5"} 27
orleans_grain_method_call_duration_bucket{grain_type="Demo.Orleans.AspNetCore.Prometheus.Grains.DemoGrain",grain_method="GetMessage",le="0.75"} 28
orleans_grain_method_call_duration_bucket{grain_type="Demo.Orleans.AspNetCore.Prometheus.Grains.DemoGrain",grain_method="GetMessage",le="1"} 28
orleans_grain_method_call_duration_bucket{grain_type="Demo.Orleans.AspNetCore.Prometheus.Grains.DemoGrain",grain_method="GetMessage",le="2.5"} 28
orleans_grain_method_call_duration_bucket{grain_type="Demo.Orleans.AspNetCore.Prometheus.Grains.DemoGrain",grain_method="GetMessage",le="5"} 28
orleans_grain_method_call_duration_bucket{grain_type="Demo.Orleans.AspNetCore.Prometheus.Grains.DemoGrain",grain_method="GetMessage",le="7.5"} 28
orleans_grain_method_call_duration_bucket{grain_type="Demo.Orleans.AspNetCore.Prometheus.Grains.DemoGrain",grain_method="GetMessage",le="10"} 28
orleans_grain_method_call_duration_bucket{grain_type="Demo.Orleans.AspNetCore.Prometheus.Grains.DemoGrain",grain_method="GetMessage",le="+Inf"} 28
# HELP process_cpu_seconds_total Total user and system CPU time spent in seconds.
# TYPE process_cpu_seconds_total counter
process_cpu_seconds_total 5.86
# HELP orleans_host_count Hosts count by silo status.
# TYPE orleans_host_count gauge
orleans_host_count{silo_status="Active"} 1
# HELP orleans_is_silo_overloaded Is silo overloaded? (0 false, 1 true)
# TYPE orleans_is_silo_overloaded gauge
orleans_is_silo_overloaded{silo_address="127.0.0.1:11111@351389167"} 0
# HELP orleans_grains_activation_count Grains activation count, filterable by silo address and grain type.
# TYPE orleans_grains_activation_count gauge
orleans_grains_activation_count{silo_address="127.0.0.1:11111@351389167",grain_type="Orleans.Runtime.Management.ManagementGrain"} 1
orleans_grains_activation_count{silo_address="127.0.0.1:11111@351389167",grain_type="OrleansDashboard.Metrics.Grains.SiloGrain"} 1
orleans_grains_activation_count{silo_address="127.0.0.1:11111@351389167",grain_type="Demo.Orleans.AspNetCore.Prometheus.Grains.DemoGrain"} 10
orleans_grains_activation_count{silo_address="127.0.0.1:11111@351389167",grain_type="Orleans.AspNetCore.Prometheus.PrometheusMetricsGrain"} 1
orleans_grains_activation_count{silo_address="127.0.0.1:11111@351389167",grain_type="OrleansDashboard.DashboardGrain"} 1
# HELP process_private_memory_bytes Process private memory size
# TYPE process_private_memory_bytes gauge
process_private_memory_bytes 298053632
# HELP dotnet_total_memory_bytes Total known allocated memory
# TYPE dotnet_total_memory_bytes gauge
dotnet_total_memory_bytes 22577792
# HELP orleans_received_messages_count Number of messages received by the silo.
# TYPE orleans_received_messages_count gauge
orleans_received_messages_count{silo_address="127.0.0.1:11111@351389167"} 0
# HELP orleans_silo_available_memory Silo available memory. Must be explicitly enabled.
# TYPE orleans_silo_available_memory gauge
# HELP orleans_receive_queue_length The length of the receive queue of the silo.
# TYPE orleans_receive_queue_length gauge
orleans_receive_queue_length{silo_address="127.0.0.1:11111@351389167"} 0
# HELP orleans_active_host_rate The percentage of hosts that report as 'active'.
# TYPE orleans_active_host_rate gauge
orleans_active_host_rate 1
# HELP process_num_threads Total number of threads
# TYPE process_num_threads gauge
process_num_threads 31
# HELP orleans_silo_memory_usage Silo memory usage. Must be explicitly enabled.
# TYPE orleans_silo_memory_usage gauge
orleans_silo_memory_usage{silo_address="127.0.0.1:11111@351389167"} 22708864
# HELP orleans_sent_messages_count Number of messages sent by the silo.
# TYPE orleans_sent_messages_count gauge
orleans_sent_messages_count{silo_address="127.0.0.1:11111@351389167"} 0
# HELP orleans_recently_used_activation_count Recently used activation count per silo
# TYPE orleans_recently_used_activation_count gauge
orleans_recently_used_activation_count{silo_address="127.0.0.1:11111@351389167"} 14
# HELP orleans_silo_cpu_usage Silo CPU usage. Must be explicitly enabled.
# TYPE orleans_silo_cpu_usage gauge
```
