using BusinessObject.Enum;
using Hangfire;
using Microsoft.Extensions.Logging;
using Repository.DTOs;
using Repository.Interface;
using Service.Interface;
using Service.Mail;

namespace Service.Implement
{
    public class BackgroundTaskService : IBackgroundTaskService
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly IRealEstateRepository _realEstateRepository;
        private readonly IDepositAmountRepository _depositAmountRepository;
        private readonly IParticipantHistoryRepository _participantHistoryRepository;
        private readonly ILogger<BackgroundTaskService> _logger;

        private readonly IFirebaseAuctionService _firebaseAuctionService;
        private readonly INotificatonService _notificatonService;
        private readonly IAuctionAccountingService _auctionAccountingService;
        private readonly IDepositAmountService _depositAmountService;
        private readonly IParticipantHistoryService _participantHistoryService;
        private readonly IRealEstateService _realEstateService;

        public BackgroundTaskService(IAuctionRepository auctionRepository,
            ILogger<BackgroundTaskService> logger,
            IRealEstateRepository realEstateRepository,
            IDepositAmountRepository depositAmountRepository,
            IParticipantHistoryRepository participantHistoryRepository,
            IFirebaseAuctionService firebaseAuctionService,
            INotificatonService notificatonService,
            IAuctionAccountingService auctionAccountingService,
            IDepositAmountService depositAmountService,
            IParticipantHistoryService participantHistoryService,
            IRealEstateService realEstateService)
        {
            _auctionRepository = auctionRepository;
            _logger = logger;
            _realEstateRepository = realEstateRepository;
            _depositAmountRepository = depositAmountRepository;
            _participantHistoryRepository = participantHistoryRepository;
            _firebaseAuctionService = firebaseAuctionService;
            _notificatonService = notificatonService;
            _auctionAccountingService = auctionAccountingService;
            _depositAmountService = depositAmountService;
            _participantHistoryService = participantHistoryService;
            _realEstateService = realEstateService;
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

        public async Task ScheduleGetAuctionResultFromFirebase(int auctionId)
        {
            try
            {
                var auction = _auctionRepository.GetAuction(auctionId);

                if (auction != null && auction.Status == (int)AuctionStatus.OnGoing)
                {
                    _logger.LogInformation($"Auction id: {auction.AuctionId} is in status 'OnGoing'");
                    var currentDateTime = DateTime.Now;
                    TimeSpan delayScheduleSendMailToLoser = auction.DateEnd.AddMinutes(5) - currentDateTime;
                    TimeSpan delayUpdateAuctionResultFromFirebase = auction.DateEnd.AddMinutes(1) - currentDateTime;

                    BackgroundJob.Schedule(() => UpdateAuctionResultFromFirebase(auction.AuctionId), delayUpdateAuctionResultFromFirebase);
                    _logger.LogInformation($"UpdateAuctionResultFromFirebase in {delayUpdateAuctionResultFromFirebase}");

                    BackgroundJob.Schedule(() => ScheduleSendMailForLoserAttendees(auction), delayScheduleSendMailToLoser);
                    _logger.LogInformation($"Send mail for loser attendees scheduled in {delayScheduleSendMailToLoser}");
                }
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting auction result from firebase.");
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

                    //Send noti for users not attend auction
                    List<int> userNotAttendAuctionIds = await _auctionRepository.GetAuctionAttenders(auction.ReasId);
                    await _notificatonService.SendNotificationWhenNotAttendAuction(userNotAttendAuctionIds, auctionId);
                }
                //else if (auction != null && auction.Status == (int)AuctionStatus.OnGoing)
                //{
                //    _logger.LogInformation($"Auction id: {auction.AuctionId} is in status 'OnGoing'");
                //    var currentDateTime = DateTime.Now;
                //    TimeSpan delayScheduleSendMailToLoser = auction.DateEnd.AddMinutes(5) - currentDateTime;
                //    TimeSpan delayUpdateAuctionResultFromFirebase = auction.DateEnd.AddMinutes(1) - currentDateTime;

                //    BackgroundJob.Schedule(() => UpdateAuctionResultFromFirebase(auction.AuctionId), delayUpdateAuctionResultFromFirebase);
                //    _logger.LogInformation($"UpdateAuctionResultFromFirebase in {delayUpdateAuctionResultFromFirebase}");

                //    BackgroundJob.Schedule(() => ScheduleSendMailForLoserAttendees(auction), delayScheduleSendMailToLoser);
                //    _logger.LogInformation($"Send mail for loser attendees scheduled in {delayScheduleSendMailToLoser}");
                //}
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating auction status.");
            }
        }

        public async Task UpdateAuctionResultFromFirebase(int auctionId)
        {
            try
            {
                var auctionResult = await _firebaseAuctionService.GetAuctionDataAsync(auctionId);

                if (auctionResult != null)
                {
                    _logger.LogInformation($"Auction result with id {auctionId}: {auctionResult}");

                    var auctionDetailDto = new AuctionDetailDto()
                    {
                        AuctionId = auctionResult.Auction.AuctionId,
                        AccountWinId = auctionResult.GetAccountWinId(),
                        WinAmount = auctionResult.CurrentBid
                    };

                    var auctionHistoryDtos = new List<ParticipantAuctionHistoryDto>();
                    
                    foreach (var participate in auctionResult.Users)
                    {
                        var auctionHistoryDto = new ParticipantAuctionHistoryDto()
                        {
                            AccountId = int.Parse(participate.Value.UserId),
                            LastBidAmount = participate.Value.CurrentUserBid,
                        };

                        auctionHistoryDtos.Add(auctionHistoryDto);
                    }

                    var auctionAccountingDto = await _auctionAccountingService.CreateAuctionAccounting(auctionDetailDto);


                    List<int> userIdParticipateInAuction = auctionHistoryDtos.Select(a => a.AccountId).ToList();

                    //update status for user participate
                    foreach (int userId in userIdParticipateInAuction)
                    {
                        await _depositAmountService.UpdateStatus(userId, auctionAccountingDto.ReasId, (int)UserDepositEnum.Waiting_for_refund);
                    }

                    // change the status of winner
                    await _depositAmountService.UpdateStatus(auctionDetailDto.AccountWinId, auctionAccountingDto.ReasId, (int)UserDepositEnum.Winner);


                    //add to participant history
                    await _participantHistoryService.CreateParticipantHistory(auctionHistoryDtos, auctionAccountingDto.AuctionAccountingId, auctionDetailDto.WinAmount);


                    //update auction status
                    int statusFinish = (int)AuctionStatus.Finish;
                    bool result = await _auctionRepository.EditAuctionStatus(auctionDetailDto.AuctionId.ToString(), statusFinish.ToString());

                    //update real estate status
                    await _realEstateService.UpdateRealEstateStatus(auctionAccountingDto.ReasId, (int)RealEstateStatus.Sold);

                    if (result)
                    {
                        //send email
                        await _auctionAccountingService.SendWinnerEmail(auctionAccountingDto);

                        userIdParticipateInAuction.Remove(auctionDetailDto.AccountWinId);

                        //send notification
                        await _notificatonService.SendNotificationToStaffandAdminWhenAuctionFinish(auctionAccountingDto.AuctionId);

                        await _notificatonService.SendNotificationWhenWinAuction(auctionAccountingDto.AuctionId, (float)auctionDetailDto.WinAmount);

                        await _notificatonService.SendNotificationWhenLoseAuction(userIdParticipateInAuction, auctionAccountingDto.AuctionId);

                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating auction result from firebase");
            }
        }

        public async Task ChangeRealEsateStatusToWaiting(int reasId)
        {
            try
            {
                var realEstateToBeUpdated = _realEstateRepository.GetRealEstate(reasId);

                if (realEstateToBeUpdated != null && realEstateToBeUpdated.ReasStatus == (int)RealEstateStatus.Selling)
                {
                    realEstateToBeUpdated.ReasStatus = (int)RealEstateStatus.WaitingAuction;
                    await _realEstateRepository.UpdateAsync(realEstateToBeUpdated);
                    _logger.LogInformation($"Real estate id: {realEstateToBeUpdated.ReasId} status updated to 'WaitingAuction' successfully at {DateTime.Now}.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating real estate status.");
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
                    TimeSpan delayChangeReasStatus = auction.DateStart.AddHours(-1) - currentDateTime;

                    //Update reas estate to 'Waiting' before 1 hour before auction start
                    BackgroundJob.Schedule(() => ChangeRealEsateStatusToWaiting(auction.ReasId), delayChangeReasStatus);
                    _logger.LogInformation($"Reas id: {auction.ReasId} scheduled for status change: " +
                        $"'Waiting' at {auction.DateStart.AddHours(-1)}");

                    //SendMailToAnnounceAuctionStartTime
                    await ScheduleSendEmailNoticeAttenders(auction);

                    //Update auction to 'Pending' when it's time to start
                    BackgroundJob.Schedule(() => ChangeAuctionStatusToPending(auction.AuctionId), delayToStart);
                    _logger.LogInformation($"Auction id: {auction.AuctionId} scheduled for status change: " +
                        $"'Pending' at {auction.DateStart}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while scheduling auction status change.");
            }
        }

        public async Task ScheduleSendMailForLoserAttendees(BusinessObject.Entity.Auction auction)
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

        public async Task ScheduleSendEmailNoticeAttenders(BusinessObject.Entity.Auction auction)
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
                        $"with mail {mail}: had sent at {emailSendingTime}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while sending mail to announce auction attenders");
            }
        }


    }
}
