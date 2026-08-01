namespace Waybon.Application.Dtos.Metrics
{
    public record ConnectionMetricsDto
    (
        int ActiveConnections,
        int UniqueUsers,
        double EstimatedKb
    );
}
