using BusinessObject.Entity;
using Repository.DTOs;
using Repository.Interface;
using Repository.Param;

namespace Service.Interface
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
