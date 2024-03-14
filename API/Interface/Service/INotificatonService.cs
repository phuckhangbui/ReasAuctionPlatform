using API.Entity;

namespace API.Interface.Service
{
    public interface INotificatonService
    {
        Task<List<Notification>> GetNotificationsOrderByDateCreate(int accountId);
    }
}
