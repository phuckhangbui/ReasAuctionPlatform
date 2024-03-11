using API.Data;
using API.Entity;
using API.Interface.Repository;

namespace API.Repository
{
    public class ParticipantHistoryRepository : BaseRepository<ParticipateAuctionHistory>, IParticipantHistoryRepository
    {
        private readonly DataContext _context;

        public ParticipantHistoryRepository(DataContext context) : base(context)
        {
            _context = context;
        }


    }
}
