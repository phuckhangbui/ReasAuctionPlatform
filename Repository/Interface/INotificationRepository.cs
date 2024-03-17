using BusinessObject.Entity;
using Repository.DTOs;

namespace Repository.Interface
{
    public interface INotificationRepository : IBaseRepository<Notification>
    {
        Task<List<NotificationDto>> GetNotificationsBaseOnAccountId(int accountId);

    }
}
