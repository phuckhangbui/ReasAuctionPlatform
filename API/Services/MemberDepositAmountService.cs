using API.DTOs;
using API.Helper;
using API.Interface.Repository;
using API.Interface.Service;
using API.Param;

namespace API.Services
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
