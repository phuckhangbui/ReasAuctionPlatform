using API.DTOs;
using API.Entity;

namespace API.Interface.Repository
{
    public interface IParticipantHistoryRepository : IBaseRepository<ParticipateAuctionHistory>
    {
        Task<List<AuctionAttenderDto>> GetLosingAttendees(int reasId);
        Task<IEnumerable<ParticipateAuctionFinalDto>> GetAllParticipates(int auctionId);
    }
}
