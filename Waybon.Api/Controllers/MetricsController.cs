using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Waybon.Application.Interfaces;

namespace Waybon.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MetricsController(IMetricsCollector metricsCollector) : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var process = Process.GetCurrentProcess();
            var appMetrics = metricsCollector.Collect();

            var totalRamMb = process.WorkingSet64 / 1024 / 1024;
            var managedRamMb = GC.GetTotalMemory(false) / 1024 / 1024;

            return Ok(new
            {
                serverTime = new
                {
                    utc = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                    zone = TimeZoneInfo.Local.StandardName
                },

                totalApiMemory = new
                {
                    value = totalRamMb,
                    unit = "MB",
                    description = "Total RAM consumed by the .NET process (runtime, caches, connections, etc.)"
                },

                managedMemory = new
                {
                    value = managedRamMb,
                    unit = "MB",
                    description = "RAM managed by the Garbage Collector (live objects)"
                },

                locationCache = appMetrics.LocationCache,
                signalRConnections = appMetrics.Connections,

                cachesSummary = new
                {
                    combinedSize = new
                    {
                        value = appMetrics.CombinedCacheMb < 1 ? appMetrics.CombinedCacheKb : appMetrics.CombinedCacheMb,
                        unit = appMetrics.CombinedCacheMb < 1 ? "KB" : "MB"
                    },
                    description = "Combined size of LocationCache + ConnectionManager in RAM"
                },

                uptime = DateTime.UtcNow - process.StartTime.ToUniversalTime()
            });
        }
    }
}