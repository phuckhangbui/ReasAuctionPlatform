using API.DTOs;
using API.Entity;
using API.Param;

namespace API.Interface.Repository
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
