using API.DTOs;
using API.Entity;
using API.Helper;
using API.Param;

namespace API.Interface.Repository
{
    public interface IDepositAmountRepository : IBaseRepository<DepositAmount>
    {
        Task<PageList<DepositAmountDto>> GetDepositAmoutForMember(int id);
        Task<PageList<DepositAmountDto>> GetDepositAmoutForMemberBySearch(SearchDepositAmountParam searchDepositAmountDto, int id);
        Task<PageList<DepositDto>> GetDepositAmountsAsync(DepositAmountParam depositAmountParam);
        List<DepositAmount> GetDepositAmounts(int accountSignId, int reasId);
        DepositAmount GetDepositAmount(int accountSignId, int reasId);
        DepositAmount GetDepositAmount(int depositId);
        DepositDetailDto GetDepositDetailAsync(int depositId);
        Task<PageList<AccountDepositedDto>> GetAccountsHadDeposited(PaginationParams paginationParams, int reasId);
    }
}
