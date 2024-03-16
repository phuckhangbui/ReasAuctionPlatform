using API.Data;
using API.Entity;
using API.Interface.Repository;
using AutoMapper;

namespace API.Repository
{
    public class AuctionAccountingRepository : BaseRepository<AuctionAccounting>, IAuctionAccountingRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public AuctionAccountingRepository(DataContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public AuctionAccounting GetAuctionAccountingByAuctionId(int auctionId)
        {
            return _context.AuctionsAccounting.FirstOrDefault(auction => auction.AuctionId == auctionId);
        }
    }
}
