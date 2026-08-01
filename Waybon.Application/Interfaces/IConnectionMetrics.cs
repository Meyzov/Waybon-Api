using Waybon.Application.Dtos.Metrics;

namespace Waybon.Application.Interfaces
{
    public interface IConnectionMetrics
    {
        ConnectionMetricsDto GetMetrics();
    }
}