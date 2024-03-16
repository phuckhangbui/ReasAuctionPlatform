using BusinessObject.Entity;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Interface;

namespace Repository.Implement
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
