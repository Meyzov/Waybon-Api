using Waybon.Application.Dtos.Metrics;
using Waybon.Application.Interfaces;

namespace Waybon.Application.Services
{
    public class MetricsCollector(ICacheMetrics cacheMetrics, IConnectionMetrics connectionMetrics) : IMetricsCollector
    {
        private readonly ICacheMetrics _cacheMetrics = cacheMetrics;
        private readonly IConnectionMetrics _connectionMetrics = connectionMetrics;

        public AppMetricsDto Collect()
        {
            var cache = _cacheMetrics.GetMetrics();
            var conn = _connectionMetrics.GetMetrics();
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