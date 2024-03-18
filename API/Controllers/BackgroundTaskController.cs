using API.MessageResponse;
using FireSharp;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Repository.DTOs;
using Repository.Interface;
using Service.Interface;
using static Repository.DTOs.AuctionResultDto;

namespace API.Controllers
{
    public class BackgroundTaskController : BaseApiController
    {
        private readonly IBackgroundTaskService _backgroundTaskService;
        private readonly IDepositAmountRepository _depositAmountRepository;
        private readonly IAuctionRepository _auctionRepository;
        private readonly IParticipantHistoryRepository _participantHistoryRepository;
        private readonly ILogger<BackgroundTaskController> _logger;
        private readonly IFirebaseAuctionService _firebaseAuctionService;

        private const string BaseUri = "/api/backgroundService";

        public BackgroundTaskController(IBackgroundTaskService backgroundTaskService, 
            ILogger<BackgroundTaskController> logger,
            IDepositAmountRepository depositAmountRepository,
            IAuctionRepository auctionRepository,
            IParticipantHistoryRepository participantHistoryRepository,
            IFirebaseAuctionService firebaseAuctionService)
        {
            _backgroundTaskService = backgroundTaskService;
            _logger = logger;
            _depositAmountRepository = depositAmountRepository;
            _auctionRepository = auctionRepository;
            _participantHistoryRepository = participantHistoryRepository;
            _firebaseAuctionService = firebaseAuctionService;
        }

        [HttpGet(BaseUri + "/losingAttendees/{reasId}")]
        public async Task<IActionResult> GetLosingAttendees(int reasId)
        {
            try
            {
                var losingAttendees = await _participantHistoryRepository.GetLosingAttendees(reasId);

                return Ok(losingAttendees);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, ex.Message));
            }
        }

        [HttpGet(BaseUri + "/trigger/{auctionId}")]
        public async Task<IActionResult> TriggerScheduleAuctionEndTime(int auctionId)
        {
            try
            {
                await _backgroundTaskService.ScheduleGetAuctionResultFromFirebase(auctionId);

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

        [HttpGet(BaseUri + "/auctionId")]
        public async Task<IActionResult> GetAuctionAttendersEmail(int auctionId)
        {
            try
            {
                var emails = await _auctionRepository.GetAuctionAttendersEmail(auctionId);

                return Ok(emails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, ex.Message));
            }
        }

        [HttpGet("/firebase")]
        public async Task<IActionResult> GetDataFromFirbase()
        {
            try
            {
                IFirebaseConfig config = new FirebaseConfig
                {
                    AuthSecret = "BKzPJclM4rnmLoj8JXIgWKkjwP0aprY6NK266RL9",
                    BasePath = "https://swd-reas-default-rtdb.asia-southeast1.firebasedatabase.app/"
                };


                IFirebaseClient client = new FirebaseClient(config);
                FirebaseResponse response = await client.GetAsync("auctions");

                var auctionData = JsonConvert.DeserializeObject<Dictionary<int, AuctionData>>(response.Body);

                //var auctionData = _firebaseAuctionService.GetAuctionDataAsync();

                return Ok(auctionData);
                //return Ok(auctionData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, ex.Message));
            }
        }
    }
}
