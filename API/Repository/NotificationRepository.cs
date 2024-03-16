using API.Data;
using API.Entity;
using API.Interface.Repository;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class NotificationRepository : BaseRepository<Notification>, INotificationRepository
    {
        private readonly DataContext _context;
        public NotificationRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Notification>> GetNotificationsBaseOnAccountId(int accountId)
        {
            return await _context.Notification.Where(n => n.AccountReceiveId == accountId).ToListAsync();
        }

    }
}
