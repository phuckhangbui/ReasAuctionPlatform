using API.Entity;

namespace API.Interface.Repository
{
    public interface INotificationRepository : IBaseRepository<Notification>
    {
        Task<List<Notification>> GetNotificationsBaseOnAccountId(int accountId);

    }
}
