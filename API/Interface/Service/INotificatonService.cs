using API.Entity;
using API.Param;

namespace API.Interface.Service
{
    public interface INotificatonService
    {
        Task<List<Notification>> GetNotificationsOrderByDateCreate(int accountId);
        System.Threading.Tasks.Task SendNotificationWhenMemberCreateReal(RealEstate realEstate);
        System.Threading.Tasks.Task SendNotificationWhenApproveRealEstate(ReasStatusParam reasStatusParam, RealEstate realEstate);
    }
}
