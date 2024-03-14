using API.Entity;
using API.Interface.Repository;
using API.Interface.Service;

namespace API.Services
{
    public class NotificationService : INotificatonService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IFirebaseMessagingService _messagingService;

        public NotificationService(INotificationRepository notificationRepository, IFirebaseMessagingService messagingService)
        {
            _notificationRepository = notificationRepository;
            _messagingService = messagingService;
        }

        public async Task<List<Notification>> GetNotificationsOrderByDateCreate(int accountId)
        {
            var notificationList = await _notificationRepository.GetNotificationsBaseOnAccountId(accountId);
            var orderNotificationList = notificationList.OrderByDescending(n => n.DateCreated).ToList();

            return orderNotificationList;
        }

        //Task<bool> SendNotificationWhenMemberCreateReal(List<Account> staffAndAdminAccount, Account realEstateOwnerAccount, RealEstate realEstate)
        //{
        //    string title = "New real estate posted!";
        //    string body = $"New real estate with name of {realEstate.ReasName} has been created by {realEstateOwnerAccount.AccountName} at {realEstate.DateCreated.}";
        //}


    }

}