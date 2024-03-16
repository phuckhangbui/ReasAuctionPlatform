using API.Entity;
using API.Interface.Repository;
using API.Interface.Service;

namespace API.Services
{
    public class MemberRuleService : IMemberRuleService
    {
        private readonly IRuleRepository _rule_repository;
        private readonly IAccountRepository _account_repository;

        public MemberRuleService(IRuleRepository rule_repository, IAccountRepository account_repository)
        {
            _rule_repository = rule_repository;
            _account_repository = account_repository;
        }

        public IAccountRepository AccountRepository => _account_repository;

        public async Task<Rule> GetRuleContractWhenUserSignInAuction()
        {
            var rule = await _rule_repository.GetRuleWhenUserSignInAuction();
            return rule;
        }
    }
}
