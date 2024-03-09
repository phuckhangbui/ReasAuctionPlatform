using API.DTOs;
using API.Entity;
using API.Helper;
using API.Interface.Repository;
using API.Param;


namespace API.Interface.Service
{
    public interface IRuleService
    {
        IAccountRepository AccountRepository { get; }
        Task<IEnumerable<RuleDto>> GetAllRule();
        Task<bool> CreateNewRule(RuleCreateParam ruleCreate);
        Task<bool> UpdateRule(RuleChangeContentParam ruleChangeContent);
        Task<Rule> GetDetailRule(int id);
    }
}
