using API.Entity;

namespace API.Interface.Service
{
    public interface INotificatonService
    {
        Task<List<Notification>> GetNotificationsOrderByDateCreate(int accountId);
        System.Threading.Tasks.Task SendNotificationWhenMemberCreateReal(RealEstate realEstate);
    }
}
