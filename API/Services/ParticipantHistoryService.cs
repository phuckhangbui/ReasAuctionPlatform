using API.DTOs;
using API.Entity;
using API.Interface.Repository;
using API.Interface.Service;

namespace API.Services
{
    public class ParticipantHistoryService : IParticipantHistoryService
    {
        private readonly IParticipantHistoryRepository _participantHistoryRepository;

        public ParticipantHistoryService(IParticipantHistoryRepository participantHistoryRepository)
        {
            _participantHistoryRepository = participantHistoryRepository;
        }

        public async Task<bool> CreateParticipantHistory(List<ParticipantAuctionHistoryDto> list, int auctionAccountingId)
        {
            if (list != null && list.Count > 0)
            {
                foreach (var history in list)
                {
                    ParticipateAuctionHistory participateAuctionHistory = new ParticipateAuctionHistory();
                    participateAuctionHistory.AccountBidId = history.AccountId;
                    participateAuctionHistory.LastBid = history.LastBidAmount;
                    participateAuctionHistory.AuctionAccountingId = auctionAccountingId;
                    await _participantHistoryRepository.CreateAsync(participateAuctionHistory);
                }

                return true;
            }

            return false;
        }
    }
}
