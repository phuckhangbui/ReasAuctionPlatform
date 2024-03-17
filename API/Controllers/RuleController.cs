using API.Entity;
using API.Errors;
using API.Interface.Service;
using API.MessageResponse;
using API.Param;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class RuleController : BaseApiController
    {
        private readonly IRuleService _ruleService;
        private const string BaseUri = "/api/admin/";
        public RuleController(IRuleService ruleService)
        {
            _ruleService = ruleService;
        }

        [Authorize(policy: "Admin")]
        [HttpGet(BaseUri + "rule")]
        public async Task<ActionResult<Rule>> GetAllRule()
        {
                var rule = await _ruleService.GetAllRule();
                if (rule != null)
                {
                    if (!ModelState.IsValid)
                        return BadRequest(ModelState);
                    return Ok(rule);
                }
                else
                {
                    return null;
                }
        }


        [Authorize(policy: "Admin")]
        [HttpGet(BaseUri + "rule/detail/{id}")]
        public async Task<ActionResult<Rule>> GetDetailRule(int id)
        {
                var rule = await _ruleService.GetDetailRule(id);
                if (rule != null)
                {
                    if (!ModelState.IsValid)
                        return BadRequest(ModelState);
                    return Ok(rule);
                }
                else
                {
                    return null;
                }
        }


        [Authorize(policy: "Admin")]
        [HttpPost(BaseUri + "rule/add")]
        public async Task<ActionResult<ApiResponseMessage>> CreateNewRule(RuleCreateParam ruleCreate)
        {
                var rule = await _ruleService.CreateNewRule(ruleCreate);
                if (rule)
                {
                    return new ApiResponseMessage("MSG18");
                }
                else
                {
                    return BadRequest(new ApiResponse(400, "Have any error when excute operation"));
                }
        }

        [Authorize(policy: "Admin")]
        [HttpPost(BaseUri + "rule/update")]
        public async Task<ActionResult<ApiResponseMessage>> UpdateRule(RuleChangeContentParam ruleChangeContent)
        {
                var rule = await _ruleService.UpdateRule(ruleChangeContent);
                if (rule)
                {
                    return new ApiResponseMessage("MSG03");
                }
                else
                {
                    return BadRequest(new ApiResponse(400, "Have any error when excute operation"));
                }
        }
    }
}
