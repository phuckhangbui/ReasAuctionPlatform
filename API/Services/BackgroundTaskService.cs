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
        private readonly IDepositAmountRepository _depositAmountRepository;
        private readonly IParticipantHistoryRepository _participantHistoryRepository;
        private readonly ILogger<BackgroundTaskService> _logger;

        public BackgroundTaskService(IAuctionRepository auctionRepository, 
            ILogger<BackgroundTaskService> logger,
            IRealEstateRepository realEstateRepository,
            IDepositAmountRepository depositAmountRepository,
            IParticipantHistoryRepository participantHistoryRepository)
        {
            _auctionRepository = auctionRepository;
            _logger = logger;
            _realEstateRepository = realEstateRepository;
            _depositAmountRepository = depositAmountRepository;
            _participantHistoryRepository = participantHistoryRepository;
        }

        public async Task ChangeAuctionStatusToPending(int auctionId)
        {
            try
            {
                var auction = _auctionRepository.GetAuction(auctionId);

                if (auction != null)
                {
                    //Update auction to Pending
                    auction.Status = (int)AuctionStatus.Pending;
                    await _auctionRepository.UpdateAsync(auction);
                    _logger.LogInformation($"Auction id: {auction.AuctionId} status updated to 'Pending' successfully at {DateTime.Now}.");

                    //Update real estate to Auctioning
                    var realEstateToBeUpdated = _realEstateRepository.GetRealEstate(auction.ReasId);
                    realEstateToBeUpdated.ReasStatus = (int)RealEstateStatus.Auctioning;
                    await _realEstateRepository.UpdateAsync(realEstateToBeUpdated);
                    _logger.LogInformation($"Real estate id: {realEstateToBeUpdated.ReasId} status updated to 'Auctioning' successfully at {DateTime.Now}.");

                    //Auction is Pending status when DateEnd
                    var currentDateTime = DateTime.Now;
                    TimeSpan delayToEnd = auction.DateEnd - currentDateTime;

                    BackgroundJob.Schedule(() => ChangeAuctionStatusToFinishInCaseNoAttender(auction.AuctionId), delayToEnd);

                    _logger.LogInformation($"Auction id: {auction.AuctionId} scheduled for status change: " +
                        $"'Finish' at {auction.DateEnd}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating auction status.");
            }
        }

        public async Task ChangeAuctionStatusToFinishInCaseNoAttender(int auctionId)
        {
            try
            {
                var auction = _auctionRepository.GetAuction(auctionId);

                if (auction != null && auction.Status == (int)AuctionStatus.Pending)
                {
                    //Update auction to Finish
                    auction.Status = (int)AuctionStatus.Finish;
                    await _auctionRepository.UpdateAsync(auction);
                    _logger.LogInformation($"Auction id: {auction.AuctionId} status updated to 'Finish' successfully at {DateTime.Now}.");

                    //Update real estate to Rollback
                    var realEstateToBeUpdated = _realEstateRepository.GetRealEstate(auction.ReasId);
                    realEstateToBeUpdated.ReasStatus = (int)RealEstateStatus.Rollback;
                    await _realEstateRepository.UpdateAsync(realEstateToBeUpdated);
                    _logger.LogInformation($"Real estate id: {realEstateToBeUpdated.ReasId} status updated to 'Rollback' successfully at {DateTime.Now}.");

                    //Update deposit to LostDeposit
                    await _depositAmountRepository.UpdateDepositStatusToLostDepositInCaseAuctionNoAttender(auction.ReasId);
                    _logger.LogInformation($"Deposits for real estate id: {realEstateToBeUpdated.ReasId} status updated to 'LostDeposit' successfully at {DateTime.Now}.");
                }
                else if (auction != null && auction.Status == (int)AuctionStatus.OnGoing)
                {
                    _logger.LogInformation($"Auction id: {auction.AuctionId} is in status 'OnGoing'");
                    var currentDateTime = DateTime.Now;
                    TimeSpan delayToSchedule = auction.DateEnd.AddMinutes(1) - currentDateTime;

                    BackgroundJob.Schedule(() => ScheduleSendMailForLoserAttendees(auction), delayToSchedule);
                    _logger.LogInformation($"Send mail for loser attendees scheduled in {delayToSchedule}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating auction status.");
            }
        }

        public async Task ScheduleAuction()
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

                    BackgroundJob.Schedule(() => ChangeAuctionStatusToPending(auction.AuctionId), delayToStart);
                    _logger.LogInformation($"Auction id: {auction.AuctionId} scheduled for status change: " +
                        $"'Pending' at {auction.DateStart}");

                    //SendMailToAnnounceAuctionStartTime
                    await ScheduleSendEmailNoticeAttenders(auction);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while scheduling auction status change.");
            }
        }

        public async Task ScheduleSendMailForLoserAttendees(Entity.Auction auction)
        {
            try
            {
                var losingAttendees = await _participantHistoryRepository.GetLosingAttendees(auction.ReasId);
                var realEstate = _realEstateRepository.GetRealEstate(auction.ReasId);

                foreach (var attender in losingAttendees)
                {
                    SendMailLosingAuction.SendMailLosingAuctionToAttender(attender, realEstate.ReasName, auction.DateStart, auction.DateEnd);
                    
                    _logger.LogInformation($"Send mail to losing attender email: {attender.AccountEmail} attend " +
                        $"auction id {auction.AuctionId} successfully at {DateTime.Now}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while sending mail for loser attendees");
            }
        }

        public async Task ScheduleSendEmailNoticeAttenders(Entity.Auction auction)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while sending mail to announce auction attenders");
            }
        }
    }
}
