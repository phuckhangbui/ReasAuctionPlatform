using API.DTOs;
using API.Helper;
using API.Interface.Repository;
using API.Param;

namespace API.Interface.Service
{
    public interface IMemberDepositAmountService
    {
        IAccountRepository AccountRepository { get; }
        Task<PageList<DepositAmountDto>> ListDepositAmoutByMember(int userMember);
        Task<PageList<DepositAmountDto>> ListDepositAmoutByMemberWhenSearch(SearchDepositAmountParam searchDepositAmountParam, int userMember);
    }
}
