using BusinessObject.Entity;
using Repository.Interface;

namespace Service.Interface
{
    public interface IMemberRuleService
    {
        IAccountRepository AccountRepository { get; }
        Task<Rule> GetRuleContractWhenUserSignInAuction();
    }
}
