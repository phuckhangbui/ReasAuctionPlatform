using BusinessObject.Entity;

namespace Repository.Interface
{
    public interface IAuctionAccountingRepository : IBaseRepository<AuctionAccounting>
    {
        AuctionAccounting GetAuctionAccountingByAuctionId(int AuctionId);
    }
}
