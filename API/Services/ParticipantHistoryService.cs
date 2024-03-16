using API.DTOs;
using API.Entity;
using API.Interface.Repository;
using API.Interface.Service;

namespace API.Services
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
                        participateAuctionHistory.isWinner = true;
                    }
                    else
                    {
                        participateAuctionHistory.isWinner = false;
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
            var participates = await _participantHistoryRepository.GetAllParticipates(auctionId);

            // Filter participants who have bid exactly the currently highest winning amount or higher
            var higherBidder = participates.Where(p => p.lastBid >= currentlyHighestWinningAmount).OrderBy(p => p.lastBid).FirstOrDefault();

            var remainingParticipants = participates.Where(p => p != higherBidder);

            if (remainingParticipants.Count() == 0)
            {
                return null;
            }
            var nextHighestBidder = remainingParticipants.OrderByDescending(p => p.lastBid).FirstOrDefault();

            return nextHighestBidder;
        }
    }
}
