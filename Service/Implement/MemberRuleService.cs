using BusinessObject.Entity;
using Repository.Interface;
using Service.Interface;

namespace Service.Implement
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
