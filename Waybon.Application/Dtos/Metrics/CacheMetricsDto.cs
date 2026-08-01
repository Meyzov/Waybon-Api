namespace Waybon.Application.Dtos.Metrics
{
    public record CacheMetricsDto
    (
        int TrackedUsers,
        int SharingEnabled,
        int RecipientLists,
        int TotalRecipientEntries,
        double EstimatedKb
    );
}