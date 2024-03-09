using API.DTOs;
using API.Entity;
using API.Helper;
using API.Interface.Repository;
using API.Interface.Service;
using API.Param;

namespace API.Services
{
    public class RuleService : IRuleService
    {
        private readonly IRuleRepository _rule_repository;
        private readonly IAccountRepository _account_repository;

        public RuleService(IRuleRepository rule_repository, IAccountRepository account_repository)
        {
            _rule_repository = rule_repository;
            _account_repository = account_repository;
        }

        public IAccountRepository AccountRepository => _account_repository;

        public async Task<bool> CreateNewRule(RuleCreateParam ruleCreate)
        {
            try
            {
                bool rule = await _rule_repository.CreateNewRule(ruleCreate);
                if (rule) return true;
                else return false;
            }
            catch (Exception ex) { return false; }
        }

        public async Task<IEnumerable<RuleDto>> GetAllRule()
        {
            var rule = await _rule_repository.GetAllRule();
            return rule;
        }

        public async Task<Rule> GetDetailRule(int id)
        {
            var rule = await _rule_repository.GetDetailRule(id);
            return rule;
        }

        public async Task<bool> UpdateRule(RuleChangeContentParam ruleChangeContent)
        {
            try
            {
                bool rule = await _rule_repository.UpdateRuleByContentChange(ruleChangeContent);
                if (rule) return true;
                else return false;
            }
            catch (Exception ex) { return false; }
        }
    }
}
