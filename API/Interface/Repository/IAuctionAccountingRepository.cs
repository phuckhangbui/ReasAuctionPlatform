using API.Entity;

namespace API.Interface.Repository
{
    public interface IAuctionAccountingRepository : IBaseRepository<AuctionAccounting>
    {
        AuctionAccounting GetAuctionAccountingByAuctionId(int AuctionId);
    }
}
