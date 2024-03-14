using API.Entity;
using API.Interface.Repository;
using API.Interface.Service;

namespace API.Services
{
    public class NotificationService : INotificatonService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<List<Notification>> GetNotificationsOrderByDateCreate(int accountId)
        {
            var notificationList = await _notificationRepository.GetNotificationsBaseOnAccountId(accountId);
            var orderNotificationList = notificationList.OrderByDescending(n => n.DateCreated).ToList();

            return orderNotificationList;
        }



    }

}