using API.MessageResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.DTOs;
using Service.Interface;

namespace API.Controllers
{

    public class MemberAccountController : BaseApiController
    {
        private readonly IMemberAccountService _memberAccountService;

        public MemberAccountController(IMemberAccountService memberAccountService)
        {
            _memberAccountService = memberAccountService;
        }


        [Authorize(policy: "Member")]
        [HttpGet("profile")]
        public async Task<ActionResult<UserProfileDto>> GetUserProfile()
        {
            int accountId = GetLoginAccountId();
            if (accountId == 0)
            {
                return BadRequest(new ApiException(400));
            }

            return await _memberAccountService.GetUserProfile(accountId);
        }

        [Authorize(policy: "Member")]
        [HttpPut("profile/update")]
        public async Task<ActionResult> UpdateUserInfomation(UserProfileDto userProfileDto)
        {
            int accountId = GetLoginAccountId();
            if (accountId == 0)
            {
                return BadRequest(new ApiException(400));
            }
            if (accountId != userProfileDto.AccountId)
            {
                return BadRequest(new ApiException(400, "Not match accountId"));
            }
            try
            {
                var result = await _memberAccountService.UpdateUserProfile(userProfileDto);

                if (result == null)
                {
                    return BadRequest(new ApiException(400, "Not match email"));
                }
                else if ((bool)result)
                {
                    return Ok("Update success");
                }
                else
                {
                    return BadRequest(new ApiException(400, "Update fail"));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiException(500, ex.Message));

            }
        }
    }
}
