using API.DTOs;
using API.Errors;
using API.Interface.Service;
using API.Param;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("login/google")]
        public async Task<ActionResult<UserDto>> LoginGoogle([FromBody] LoginGoogleParam loginGoogleDto)
        {
            try
            {
                var account = await _accountService.LoginGoogleByMember(loginGoogleDto);
                if (account == null)
                {
                    return BadRequest(new ApiResponse(400, "Your account has been blocked"));
                }
                return Ok(account);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiException(400));

            }
        }

        [HttpPost("login/admin")]
        public async Task<ActionResult<UserDto>> LoginAdminOrStaff(LoginDto loginDto)
        {
            var accountLogin = await _accountService.LoginByAdminOrStaff(loginDto);
            if (accountLogin != null)
            {
                //HttpContext.Session.SetString("token", accountLogin.Token);
                return Ok(accountLogin);
            }
            else
            {
                return Unauthorized(new ApiResponse(401));
            }
        }
    }
}
