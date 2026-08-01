namespace Waybon.Application.Interfaces
{
    public interface IGroupMembershipNotifier
    {
        Task OnMembershipChangedAsync(int groupId);
    }
}
