using API.Entity;
using API.Errors;
using API.Interface.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class NotificationController : BaseApiController
    {
        private readonly INotificatonService _notificationService;

        public NotificationController(INotificatonService notificationService)
        {
            _notificationService = notificationService;
        }

        [Authorize]
        [HttpGet("/account")]
        public async Task<ActionResult<List<Notification>>> GetAccountNotifications()
        {
            int accountId = GetLoginAccountId();

            if (accountId == 0)
            {
                return BadRequest(new ApiResponse(400, "Not match AccountId"));
            }

            var notifications = await _notificationService.GetNotificationsOrderByDateCreate(accountId);

            if (notifications == null)
            {
                return BadRequest(new ApiResponse(401, "No notification for this account"));
            }
            return Ok(notifications);
        }
    }
}
