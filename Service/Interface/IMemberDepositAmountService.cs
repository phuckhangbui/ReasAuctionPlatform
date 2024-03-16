using Repository.DTOs;
using Repository.Interface;
using Repository.Paging;
using Repository.Param;

namespace Service.Interface
{
    public interface IMemberDepositAmountService
    {
        IAccountRepository AccountRepository { get; }
        Task<PageList<DepositAmountDto>> ListDepositAmoutByMember(int userMember);
        Task<PageList<DepositAmountDto>> ListDepositAmoutByMemberWhenSearch(SearchDepositAmountParam searchDepositAmountParam, int userMember);
    }
}
