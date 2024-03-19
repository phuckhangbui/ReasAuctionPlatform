using AutoMapper;
using BusinessObject.Entity;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.DTOs;
using Repository.Interface;

namespace Repository.Implement
{
    public class NotificationRepository : BaseRepository<Notification>, INotificationRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public NotificationRepository(DataContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<NotificationDto>> GetNotificationsBaseOnAccountId(int accountId)
        {
            var notifications = await _context.Notification.Where(n => n.AccountReceiveId == accountId).ToListAsync();

            var notificationDtoList = new List<NotificationDto>();

            foreach (var notification in notifications)
            {
                var notificationDto = _mapper.Map<NotificationDto>(notification);
                if (notificationDto != null)
                {
                    notificationDtoList.Add(notificationDto);
                }
            }

            return notificationDtoList;
        }

    }
}
