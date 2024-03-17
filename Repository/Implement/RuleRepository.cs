using AutoMapper;
using BusinessObject.Entity;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.DTOs;
using Repository.Interface;
using Repository.Param;

namespace Repository.Implement
{
    public class RuleRepository : BaseRepository<Rule>, IRuleRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public RuleRepository(DataContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> CreateNewRule(RuleCreateParam ruleCreate)
        {
            var rule = new Rule
            {
                Title = ruleCreate.Title,
                Content = ruleCreate.Content,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
            };
            try
            {
                bool check = await CreateAsync(rule);
                if (check) { return true; }
                else return false;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public async Task<IEnumerable<RuleDto>> GetAllRule()
        {
            var rule = _context.Rule.Select(x => new RuleDto
            {
                RuleId = x.RuleId,
                Title = x.Title,
                DateCreated = x.DateCreated,
            });
            return await rule.ToListAsync();
        }

        public Task<Rule> GetDetailRule(int id)
        {
            var rule = _context.Rule.Where(x => x.RuleId == id).FirstOrDefaultAsync();
            return rule;
        }

        public async Task<Rule> GetRuleWhenUserSignInAuction()
        {
            return await _context.Rule.Where(x => x.Title == "Auction Contract").Select(x => new Rule
            {
                Title = x.Title,
                Content = x.Content
            }).SingleOrDefaultAsync();
        }

        public async Task<bool> UpdateRuleByContentChange(RuleChangeContentParam ruleChangeContent)
        {
            var query = await _context.Rule.Where(x => x.RuleId == ruleChangeContent.idRule).SingleOrDefaultAsync();
            if (query != null)
            {
                query.Content = ruleChangeContent.content;
                query.DateUpdated = DateTime.UtcNow;
                try
                {
                    bool check = await UpdateAsync(query);
                    if (check) return true;
                    else return false;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }
    }
}
