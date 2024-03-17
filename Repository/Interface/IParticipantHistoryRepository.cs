using BusinessObject.Entity;
using Repository.DTOs;

namespace Repository.Interface
{
    public interface IParticipantHistoryRepository : IBaseRepository<ParticipateAuctionHistory>
    {
        Task<List<AuctionAttenderDto>> GetLosingAttendees(int reasId);
        Task<IEnumerable<ParticipateAuctionFinalDto>> GetAllParticipates(int auctionId);

        Task<ParticipateAuctionHistory> GetParticipant(int auctionAccountingId, int participantId);
        Task<List<ParticipateAuctionFinalDto>> GetAllParticipateList(int auctionId);
    }
}
