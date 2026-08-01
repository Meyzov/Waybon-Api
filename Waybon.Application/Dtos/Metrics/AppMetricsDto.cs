namespace Waybon.Application.Dtos.Metrics
{
    public record AppMetricsDto
    (
        CacheMetricsDto LocationCache,
        ConnectionMetricsDto Connections,
        double CombinedCacheKb,
        double CombinedCacheMb
    );
}
