using BusinessObject.Entity;
using Repository.Param;

namespace Service.Interface
{
    public interface INotificatonService
    {
        Task<List<Notification>> GetNotificationsOrderByDateCreate(int accountId);
        Task SendNotificationWhenMemberCreateReal(RealEstate realEstate);
        Task SendNotificationWhenApproveRealEstate(ReasStatusParam reasStatusParam, RealEstate realEstate);
        Task SendNotificationWhenCreateAuction(int auctionId);
        Task SendNotificationWhenAuctionAboutToStart(int auctionId);
        Task SendNotificationToStaffandAdminWhenAuctionFinish(int auctionId);
        Task SendNotificationWhenWinAuction(int auctionId);
        Task SendNotificationWhenLoseAuction(List<int> accountIdParticipateInAuction, int auctionId);
        Task SendNotificationWhenNotAttendAuction(List<int> accountIdParticipateInAuction, int auctionId);

    }
}
