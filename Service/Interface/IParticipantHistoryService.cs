using Repository.DTOs;

namespace Service.Interface
{
    public interface IParticipantHistoryService
    {
        Task<bool> CreateParticipantHistory(List<ParticipantAuctionHistoryDto> list, int auctionAccountingId);
        Task<IEnumerable<ParticipateAuctionFinalDto>> GetAllParticipates(int auctionId);
    }
}
