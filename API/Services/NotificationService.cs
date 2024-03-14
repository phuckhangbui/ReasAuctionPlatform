using API.Entity;
using API.Interface.Repository;
using API.Interface.Service;
using API.Param.Enums;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    public class NotificationService : INotificatonService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IFirebaseMessagingService _messagingService;
        private readonly IAccountRepository _accountRepository;

        public NotificationService(INotificationRepository notificationRepository, IFirebaseMessagingService messagingService, IAccountRepository accountRepository)
        {
            _notificationRepository = notificationRepository;
            _messagingService = messagingService;
            _accountRepository = accountRepository;
        }

        public async Task<List<Notification>> GetNotificationsOrderByDateCreate(int accountId)
        {
            var notificationList = await _notificationRepository.GetNotificationsBaseOnAccountId(accountId);
            var orderNotificationList = notificationList.OrderByDescending(n => n.DateCreated).ToList();

            return orderNotificationList;
        }


        public async System.Threading.Tasks.Task SendNotificationWhenMemberCreateReal(RealEstate realEstate)
        {
            List<Account> staffAndAdminAccount = await _accountRepository.GetAllStaffAndAdminAccounts();
            Account realEstateOwnerAccount = await _accountRepository.GetAccountByAccountIdAsync(realEstate.AccountOwnerId);

            string title = "New real estate posted!";
            string body = $"New real estate with name of {realEstate.ReasName} has been created by {realEstateOwnerAccount.AccountName} at {realEstate.DateCreated.ToString("dd/MM/yyyy HH:mm")}";

            int type = (int)NotificationTypeEnum.NewRealEstateCreate;
            Notification notification = new Notification
            {
                NotificationType = type,
                Title = title,
                Body = body,
                DateCreated = DateTime.Now,
            };

            Dictionary<string, string> data = new Dictionary<string, string>
            {
                { "type", type.ToString() }
            };

            foreach (var account in staffAndAdminAccount)
            {
                notification.AccountReceiveId = account.AccountId;
                _notificationRepository.CreateAsync(notification);

                if (!account.FirebaseToken.IsNullOrEmpty())
                {
                    _messagingService.SendPushNotification(account.FirebaseToken, title, body, data);
                }
            }
        }


    }

}