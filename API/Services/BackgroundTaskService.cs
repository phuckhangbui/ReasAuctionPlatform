using API.Interface.Repository;
using API.Interface.Service;
using API.Param.Enums;
using Hangfire;

namespace API.Services
{
    public class BackgroundTaskService : IBackgroundTaskService
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly IRealEstateRepository _realEstateRepository;
        private readonly ILogger<BackgroundTaskService> _logger;

        public BackgroundTaskService(IAuctionRepository auctionRepository, 
            ILogger<BackgroundTaskService> logger,
            IRealEstateRepository realEstateRepository)
        {
            _auctionRepository = auctionRepository;
            _logger = logger;
            _realEstateRepository = realEstateRepository;
        }

        public async Task ChangeAuctionStatusToOnGoing(int auctionId)
        {
            try
            {
                var auctionToBeUpdated = _auctionRepository.GetAuction(auctionId);

                if (auctionToBeUpdated != null)
                {
                    auctionToBeUpdated.Status = (int)AuctionStatus.OnGoing;
                    await _auctionRepository.UpdateAsync(auctionToBeUpdated);
                    _logger.LogInformation($"Auction id: {auctionId} status updated successfully at {DateTime.Now}.");

                    var realEstateToBeUpdated = _realEstateRepository.GetRealEstate(auctionToBeUpdated.ReasId);
                    realEstateToBeUpdated.ReasStatus = (int)RealEstateStatus.Auctioning;
                    await _realEstateRepository.UpdateAsync(realEstateToBeUpdated);
                    _logger.LogInformation($"Real estate id: {realEstateToBeUpdated.ReasId} status updated successfully at {DateTime.Now}.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating auction status.");
            }
        }

        public async Task ScheduleAuctionStatus()
        {
            try
            {
                var currentDateTime = DateTime.Now;

                var auctionsToBeScheduled = await _auctionRepository.GetAllAsync();

                auctionsToBeScheduled = auctionsToBeScheduled
                    .Where(a => a.Status == (int)AuctionStatus.NotYet && a.DateStart > currentDateTime).ToList();

                foreach (var auction in auctionsToBeScheduled)
                {
                    TimeSpan delay = auction.DateStart - currentDateTime;

                    BackgroundJob.Schedule(() => ChangeAuctionStatusToOnGoing(auction.AuctionId), delay);

                    _logger.LogInformation($"Auction id: {auction.AuctionId} scheduled for status change at {auction.DateStart}.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while scheduling auction status change.");
            }
        }
    }
}
