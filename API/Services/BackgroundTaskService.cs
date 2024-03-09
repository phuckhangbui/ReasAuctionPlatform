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
                    _logger.LogInformation($"Auction id: {auctionId} status updated to 'OnGoing' successfully at {DateTime.Now}.");

                    var realEstateToBeUpdated = _realEstateRepository.GetRealEstate(auctionToBeUpdated.ReasId);
                    realEstateToBeUpdated.ReasStatus = (int)RealEstateStatus.Auctioning;
                    await _realEstateRepository.UpdateAsync(realEstateToBeUpdated);
                    _logger.LogInformation($"Real estate id: {realEstateToBeUpdated.ReasId} status updated to 'Auctioning' successfully at {DateTime.Now}.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating auction status.");
            }
        }

        public async Task ChangeAuctionStatusToFinish(int auctionId)
        {
            try
            {
                var auctionToBeUpdated = _auctionRepository.GetAuction(auctionId);

                if (auctionToBeUpdated != null && auctionToBeUpdated.Status == (int)AuctionStatus.OnGoing)
                {
                    auctionToBeUpdated.Status = (int)AuctionStatus.Finish;
                    await _auctionRepository.UpdateAsync(auctionToBeUpdated);
                    _logger.LogInformation($"Auction id: {auctionId} status updated to 'Finish' at {DateTime.Now}.");

                    var realEstateToBeUpdated = _realEstateRepository.GetRealEstate(auctionToBeUpdated.ReasId);
                    realEstateToBeUpdated.ReasStatus = (int)RealEstateStatus.Sold;
                    await _realEstateRepository.UpdateAsync(realEstateToBeUpdated);
                    _logger.LogInformation($"Real estate id: {realEstateToBeUpdated.ReasId} status updated to 'Sold' successfully at {DateTime.Now}.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating auction status to");
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
                    TimeSpan delayToStart = auction.DateStart - currentDateTime;
                    TimeSpan delayToEnd = auction.DateEnd - currentDateTime;

                    BackgroundJob.Schedule(() => ChangeAuctionStatusToOnGoing(auction.AuctionId), delayToStart);
                    BackgroundJob.Schedule(() => ChangeAuctionStatusToFinish(auction.AuctionId), delayToEnd);

                    _logger.LogInformation($"Auction id: {auction.AuctionId} scheduled for status change: " +
                        $"'OnGoing' at {auction.DateStart} and " +
                        $"'Finish' at {auction.DateEnd}.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while scheduling auction status change.");
            }
        }
    }
}
