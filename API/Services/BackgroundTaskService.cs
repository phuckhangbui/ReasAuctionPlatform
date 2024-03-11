using API.Interface.Repository;
using API.Interface.Service;
using API.Param.Enums;
using API.ThirdServices;
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

        public async Task ChangeAuctionStatusToPending(int auctionId)
        {
            try
            {
                var auctionToBeUpdated = _auctionRepository.GetAuction(auctionId);

                if (auctionToBeUpdated != null)
                {
                    auctionToBeUpdated.Status = (int)AuctionStatus.Pending;
                    await _auctionRepository.UpdateAsync(auctionToBeUpdated);
                    _logger.LogInformation($"Auction id: {auctionId} status updated to 'Pending' successfully at {DateTime.Now}.");

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

        public async Task ChangeAuctionStatusOnSuccess(int auctionId)
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

        public async Task ScheduleAuctionEndTime(int auctionId)
        {
            try
            {
                var currentDateTime = DateTime.Now;

                var auctionsToBeScheduled = await _auctionRepository.GetAuctionByAuctionId(auctionId);

                if (auctionsToBeScheduled != null)
                {
                    auctionsToBeScheduled.DateEnd = DateTime.Now.AddSeconds(30);

                    await _auctionRepository.UpdateAsync(auctionsToBeScheduled);

                    TimeSpan delayToEnd = auctionsToBeScheduled.DateEnd - currentDateTime;

                    BackgroundJob.Schedule(() => ChangeAuctionStatusOnSuccess(auctionId), delayToEnd);

                    _logger.LogInformation($"Auction id: {auctionId} scheduled for status change: " +
                        $"'Finish' at {auctionsToBeScheduled.DateEnd}.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while scheduling auction status change.");
            }
        }

        public async Task ScheduleAuctionPending()
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

                    BackgroundJob.Schedule(() => ChangeAuctionStatusToPending(auction.AuctionId), delayToStart);
                    //BackgroundJob.Schedule(() => ChangeAuctionStatusOnSuccess(auction.AuctionId), delayToEnd);

                    _logger.LogInformation($"Auction id: {auction.AuctionId} scheduled for status change: " +
                        $"'Pending' at {auction.DateStart}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while scheduling auction status change.");
            }
        }

        public async Task SendEmailNoticeAttenders()
        {
            try
            {
                var currentDateTime = DateTime.Now;

                var auctionsToBeScheduled = await _auctionRepository.GetAllAsync();

                auctionsToBeScheduled = auctionsToBeScheduled
                    .Where(a => a.Status == (int)AuctionStatus.NotYet && a.DateStart > currentDateTime).ToList();

                foreach (var auction in auctionsToBeScheduled)
                {
                    DateTime emailSendingTime = auction.DateStart.AddMinutes(-5);

                    var attenderMails = await _auctionRepository.GetAuctionAttendersEmail(auction.AuctionId);
                    var realEstate = _realEstateRepository.GetRealEstate(auction.ReasId);

                    foreach (var mail in attenderMails)
                    {
                        BackgroundJob.Schedule(() =>
                        SendMailAnnounceAuction.SendMailToAnnounceAuctionStartTime(mail, realEstate.ReasName, auction.DateStart), emailSendingTime);

                        _logger.LogInformation($"Attender register auction id {auction.AuctionId}, " +
                            $"with mail {mail}: had sent at {DateTime.Now}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while sending mail to announce auction attenders");
            }
        }
    }
}
