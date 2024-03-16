using BusinessObject.Entity;
using Repository.DTOs;
using Repository.Interface;
using Service.Interface;

namespace Service.Implement
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

        public async Task<IEnumerable<ParticipateAuctionFinalDto>> GetAllParticipates(int auctionId)
        {
            var participate = await _participantHistoryRepository.GetAllParticipates(auctionId);
            return participate;
        }
    }
}
