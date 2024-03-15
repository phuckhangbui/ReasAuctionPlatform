using API.Entity;
using API.Interface.Repository;
using API.Interface.Service;
using API.Param;
using API.Param.Enums;
using Microsoft.IdentityModel.Tokens;
using Account = API.Entity.Account;

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
                notification.NotificationId = 0;
                await _notificationRepository.CreateAsync(notification);

                if (!account.FirebaseToken.IsNullOrEmpty())
                {
                    await _messagingService.SendPushNotification(account.FirebaseToken, title, body, data);
                }
            }
        }

        public async System.Threading.Tasks.Task SendNotificationWhenApproveRealEstate(ReasStatusParam reasStatusParam, RealEstate realEstate)
        {
            //defualt is denied
            string title = "Your real estate has been denied";
            string body = $"Your real estate {realEstate.ReasName} at {realEstate.ReasAddress} does not meet our requirements. Please check and make sure you upload all the required paper work. ";
            int type = (int)NotificationTypeEnum.NewRealEstateRejected;

            if (reasStatusParam.reasStatus == (int)RealEstateStatus.Approved)
            {
                title = "Your real estate has been approved";
                body = $"Congratulation, your real estate {realEstate.ReasName} at {realEstate.ReasAddress} has been approved. To continue with the process, please pay the uploading fee";
                type = (int)NotificationTypeEnum.NewRealEstateApproved;

            }
            else if (!reasStatusParam.messageString.IsNullOrEmpty())
            {
                //case not approve and has message from admin
                body = body + reasStatusParam.messageString;
            }

            Account ownerAccount = await _accountRepository.GetAccountByAccountIdAsync(realEstate.AccountOwnerId);
            Notification notification = new Notification
            {
                NotificationType = type,
                Title = title,
                Body = body,
                DateCreated = DateTime.Now,
                AccountReceiveId = ownerAccount.AccountId
            };

            Dictionary<string, string> data = new Dictionary<string, string>
            {
                { "type", type.ToString() }
            };

            if (!ownerAccount.FirebaseToken.IsNullOrEmpty())
            {
                await _messagingService.SendPushNotification(ownerAccount.FirebaseToken, title, body, data);
            }

            await _notificationRepository.CreateAsync(notification);

        }



    }


}