using BusinessObject.Entity;
using Repository.DTOs;
using Repository.Param;

namespace Repository.Interface
{
    public interface IRuleRepository : IBaseRepository<Rule>
    {
        Task<Rule> GetRuleWhenUserSignInAuction();
        Task<bool> CreateNewRule(RuleCreateParam ruleCreate);
        Task<IEnumerable<RuleDto>> GetAllRule();
        Task<Rule> GetDetailRule(int id);
        Task<bool> UpdateRuleByContentChange(RuleChangeContentParam ruleChangeContent);
    }
}
