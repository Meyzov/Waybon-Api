using Waybon.Application.Dtos.Metrics;
using Waybon.Application.Interfaces;

namespace Waybon.Application.Services
{
    public class MetricsCollector(ICacheMetrics cacheMetrics, IConnectionMetrics connectionMetrics) : IMetricsCollector
    {
        public AppMetricsDto Collect()
        {
            var cache = cacheMetrics.GetMetrics();
            var conn = connectionMetrics.GetMetrics();
            var totalCacheKb = cache.EstimatedKb + conn.EstimatedKb;
            var totalCacheMb = Math.Round(totalCacheKb / 1024.0, 3);

            return new AppMetricsDto
            (
                cache,
                conn,
                totalCacheKb,
                totalCacheMb
            );
        }
    }
}
