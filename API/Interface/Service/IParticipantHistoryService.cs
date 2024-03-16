using API.DTOs;

namespace API.Interface.Service
{
    public interface IParticipantHistoryService
    {
        Task<bool> CreateParticipantHistory(List<ParticipantAuctionHistoryDto> list, int auctionAccountingId);
        Task<IEnumerable<ParticipateAuctionFinalDto>> GetAllParticipates(int auctionId);
    }
}
