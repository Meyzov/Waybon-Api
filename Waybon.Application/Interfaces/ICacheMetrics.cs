using Waybon.Application.Dtos.Metrics;

namespace Waybon.Application.Interfaces
{
    public interface ICacheMetrics
    {
        CacheMetricsDto GetMetrics();
    }
}