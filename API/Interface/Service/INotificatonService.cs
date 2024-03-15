using API.Entity;
using API.Param;

namespace API.Interface.Service
{
    public interface INotificatonService
    {
        Task<List<Notification>> GetNotificationsOrderByDateCreate(int accountId);
        System.Threading.Tasks.Task SendNotificationWhenMemberCreateReal(RealEstate realEstate);
        System.Threading.Tasks.Task SendNotificationWhenApproveRealEstate(ReasStatusParam reasStatusParam, RealEstate realEstate);
        System.Threading.Tasks.Task SendNotificationWhenCreateAuction(int auctionId);
        System.Threading.Tasks.Task SendNotificationWhenAuctionAboutToStart(int auctionId);
        System.Threading.Tasks.Task SendNotificationToStaffandAdminWhenAuctionFinish(int auctionId);
        System.Threading.Tasks.Task SendNotificationWhenWinAuction(int auctionId);
        System.Threading.Tasks.Task SendNotificationWhenLoseAuction(List<int> accountIdParticipateInAuction, int auctionId);
        System.Threading.Tasks.Task SendNotificationWhenNotAttendAuction(List<int> accountIdParticipateInAuction, int auctionId);

    }
}
