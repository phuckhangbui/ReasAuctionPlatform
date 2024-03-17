using Repository.DTOs;

namespace Service.Interface
{
    public interface IParticipantHistoryService
    {
        Task<bool> CreateParticipantHistory(List<ParticipantAuctionHistoryDto> list, int auctionAccountingId, double winningAmount);
        Task<IEnumerable<ParticipateAuctionFinalDto>> GetAllParticipates(int auctionId);
        Task<ParticipateAuctionFinalDto> GetNextHighestBidder(int auctionId, double currentlyHighestWinningAmount);
        Task UpdateParticipateHistoryStatus(int auctionAccountingId, int participantId, int status, string? message);
    }
}
