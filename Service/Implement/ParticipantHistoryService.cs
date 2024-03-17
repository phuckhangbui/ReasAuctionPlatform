using BusinessObject.Entity;
using BusinessObject.Enum;
using Microsoft.IdentityModel.Tokens;
using Repository.DTOs;
using Repository.Interface;
using Service.Interface;

namespace Service.Implement
{
    public class ParticipantHistoryService : IParticipantHistoryService
    {
        private readonly IParticipantHistoryRepository _participantHistoryRepository;
        private readonly IAccountRepository _accountRepository;

        public ParticipantHistoryService(IParticipantHistoryRepository participantHistoryRepository, IAccountRepository accountRepository)
        {
            _participantHistoryRepository = participantHistoryRepository;
            _accountRepository = accountRepository;
        }

        public async Task<bool> CreateParticipantHistory(List<ParticipantAuctionHistoryDto> list, int auctionAccountingId, double winningAmount)
        {
            if (list != null && list.Count > 0)
            {
                foreach (var history in list)
                {
                    ParticipateAuctionHistory participateAuctionHistory = new ParticipateAuctionHistory();
                    participateAuctionHistory.AccountBidId = history.AccountId;
                    participateAuctionHistory.LastBid = history.LastBidAmount;
                    participateAuctionHistory.AuctionAccountingId = auctionAccountingId;
                    if (winningAmount == history.LastBidAmount)
                    {
                        participateAuctionHistory.Status = (int)ParticipateAuctionHistoryEnum.Winner;
                    }
                    else
                    {
                        participateAuctionHistory.Status = (int)ParticipateAuctionHistoryEnum.Others;
                    }
                    await _participantHistoryRepository.CreateAsync(participateAuctionHistory);
                }

                return true;
            }

            return false;
        }

        public async Task<IEnumerable<ParticipateAuctionFinalDto>> GetAllParticipates(int auctionId)
        {
            var participate = await _participantHistoryRepository.GetAllParticipates(auctionId);
            return participate;
        }

        public async Task<ParticipateAuctionFinalDto> GetNextHighestBidder(int auctionId, double currentlyHighestWinningAmount)
        {
            var participates = await _participantHistoryRepository.GetAllParticipateList(auctionId);

            // Filter participants who have bid exactly the currently highest winning amount or higher
            var higherBidders = participates.Where(p => p.lastBid >= currentlyHighestWinningAmount).OrderBy(p => p.lastBid).ToList();

            var remainingParticipants = participates.Except(higherBidders).ToList();

            if (remainingParticipants.Count() == 0)
            {
                return null;
            }
            var nextHighestBidder = remainingParticipants.OrderByDescending(p => p.lastBid).FirstOrDefault();

            if (nextHighestBidder.lastBid == 0)
            {
                return null;
            }
            return nextHighestBidder;
        }

        public async Task UpdateParticipateHistoryStatus(int auctionAccountingId, int participantId, int status, string? message)
        {
            ParticipateAuctionHistory participant = await _participantHistoryRepository.GetParticipant(auctionAccountingId, participantId);

            if (participant == null)
            {
                return;
            }

            if (!message.IsNullOrEmpty())
            {
                participant.Note = message;
            }

            participant.Status = status;

            await _participantHistoryRepository.UpdateAsync(participant);
        }

    }
}
