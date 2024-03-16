using BusinessObject.Entity;
using Repository.DTOs;

namespace Repository.Interface
{
    public interface IParticipantHistoryRepository : IBaseRepository<ParticipateAuctionHistory>
    {
        Task<List<AuctionAttenderDto>> GetLosingAttendees(int reasId);
        Task<IEnumerable<ParticipateAuctionFinalDto>> GetAllParticipates(int auctionId);
    }
}
