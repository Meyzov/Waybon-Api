namespace Waybon.Application.Interfaces
{
    public interface IUserRefreshNotifier
    {
        Task RefreshUserAsync(Guid userId);
    }
}