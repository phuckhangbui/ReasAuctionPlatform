using BusinessObject.Entity;
using Repository.DTOs;
using Repository.Paging;
using Repository.Param;

namespace Repository.Interface
{
    public interface IDepositAmountRepository : IBaseRepository<DepositAmount>
    {
        Task<PageList<DepositAmountDto>> GetDepositAmoutForMember(int id);
        Task<PageList<DepositAmountDto>> GetDepositAmoutForMemberBySearch(SearchDepositAmountParam searchDepositAmountDto, int id);
        Task<IEnumerable<DepositDto>> GetRealEstateForDepositAsync();
        List<DepositAmount> GetDepositAmounts(int accountSignId, int reasId);
        DepositAmount GetDepositAmount(int accountSignId, int reasId);
        DepositAmount GetDepositAmount(int depositId);
        Task<bool> ChangeStatusWaiting(int id);

        Task<PageList<AccountDepositedDto>> GetAccountsHadDeposited(PaginationParams paginationParams, int reasId);
        Task UpdateDepositStatusToWaitingForRefund(int reasId);
        Task UpdateDepositStatusToLostDepositInCaseAuctionNoAttender(int reasId);
    }
}
