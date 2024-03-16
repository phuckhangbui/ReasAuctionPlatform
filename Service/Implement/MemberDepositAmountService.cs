using Repository.DTOs;
using Repository.Interface;
using Repository.Paging;
using Repository.Param;
using Service.Interface;

namespace Service.Implement
{
    public class MemberDepositAmountService : IMemberDepositAmountService
    {
        private readonly IDepositAmountRepository _depositAmountRepository;
        private readonly IAccountRepository _accountRepository;

        public MemberDepositAmountService(IDepositAmountRepository depositAmountRepository, IAccountRepository accountRepository)
        {
            _depositAmountRepository = depositAmountRepository;
            _accountRepository = accountRepository;
        }

        public IAccountRepository AccountRepository => _accountRepository;

        public async Task<PageList<DepositAmountDto>> ListDepositAmoutByMember(int userMember)
        {
            var deposit = await _depositAmountRepository.GetDepositAmoutForMember(userMember);
            return deposit;
        }

        public async Task<PageList<DepositAmountDto>> ListDepositAmoutByMemberWhenSearch(SearchDepositAmountParam searchDepositAmountParam, int userMember)
        {
            var deposit = await _depositAmountRepository.GetDepositAmoutForMemberBySearch(searchDepositAmountParam, userMember);
            return deposit;
        }
    }
}
