using API.Entity;
using API.Interface.Repository;

namespace API.Interface.Service
{
    public interface IMemberRuleService
    {
        IAccountRepository AccountRepository { get; }
        Task<Rule> GetRuleContractWhenUserSignInAuction();
    }
}
