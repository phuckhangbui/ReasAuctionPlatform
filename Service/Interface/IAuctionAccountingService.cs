using Repository.DTOs;

namespace Service.Interface
{
        public interface IAuctionAccountingService
        {
                Task<AuctionAccountingDto> UpdateAuctionAccounting(AuctionDetailDto auctionDetailDto);
                Task SendWinnerEmail(AuctionAccountingDto auctionAccounting);
                Task<AuctionAccountingDto> GetAuctionAccounting(int auctionId);
        }
}
