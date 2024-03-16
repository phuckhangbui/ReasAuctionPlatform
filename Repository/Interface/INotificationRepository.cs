using BusinessObject.Entity;

namespace Repository.Interface
{
    public interface INotificationRepository : IBaseRepository<Notification>
    {
        Task<List<Notification>> GetNotificationsBaseOnAccountId(int accountId);

    }
}
