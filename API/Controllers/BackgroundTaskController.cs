using API.Errors;
using API.Interface.Repository;
using API.Interface.Service;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BackgroundTaskController : BaseApiController
    {
        private readonly IBackgroundTaskService _backgroundTaskService;
        private readonly IDepositAmountRepository _depositAmountRepository;
        private readonly ILogger<BackgroundTaskController> _logger;

        private const string BaseUri = "/api/backgroundService";

        public BackgroundTaskController(IBackgroundTaskService backgroundTaskService, 
            ILogger<BackgroundTaskController> logger,
            IDepositAmountRepository depositAmountRepository)
        {
            _backgroundTaskService = backgroundTaskService;
            _logger = logger;
            _depositAmountRepository = depositAmountRepository;
        }

        [HttpGet(BaseUri + "/trigger/{auctionId}")]
        public async Task<IActionResult> TriggerScheduleAuctionEndTime(int auctionId)
        {
            try
            {
                await _backgroundTaskService.ScheduleAuctionEndTime(auctionId);

                _logger.LogInformation($"Trigger schedule auction {auctionId} end time run successfully at {DateTime.Now}.");

                return Ok("Schedule successfully");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Trigger schedule auction {auctionId} end time run again fail at {DateTime.Now} with error {ex.Message}.");

                return StatusCode(500, new ApiResponse(500, ex.Message));
            }
        }

        [HttpPost(BaseUri + "/{reasId}")]
        public async Task<IActionResult> UpdateDepositStatusToWaitingForRefund(int reasId)
        {
            try
            {
                await _depositAmountRepository.UpdateDepositStatusToWaitingForRefund(reasId);

                return Ok("Update successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, ex.Message));
            }
        }
    }
}
