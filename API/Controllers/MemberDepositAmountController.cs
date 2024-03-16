using API.Extensions;
using API.MessageResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Paging;
using Repository.Param;
using Service.Interface;

namespace API.Controllers
{
    public class MemberDepositAmountController : BaseApiController
    {
        private readonly IMemberDepositAmountService _memberDepositAmountService;
        private const string BaseUri = "/api/home/";

        public MemberDepositAmountController(IMemberDepositAmountService memberDepositAmountService)
        {
            _memberDepositAmountService = memberDepositAmountService;
        }

        [Authorize(policy: "Member")]
        [HttpGet(BaseUri + "my-deposit")]
        public async Task<IActionResult> ListDepositAmoutByMember([FromQuery] PaginationParams paginationParams)
        {
            var deposit = await _memberDepositAmountService.ListDepositAmoutByMember(GetLoginAccountId());
            Response.AddPaginationHeader(new PaginationHeader(deposit.CurrentPage, deposit.PageSize,
            deposit.TotalCount, deposit.TotalPages));
            if (deposit.PageSize == 0)
            {
                var apiResponseMessage = new ApiResponseMessage("MSG01");
                return Ok(new List<ApiResponseMessage> { apiResponseMessage });
            }
            else
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                return Ok(deposit);
            }
        }


        [Authorize(policy: "Member")]
        [HttpGet(BaseUri + "my-deposit/search")]
        public async Task<IActionResult> ListDepositAmoutByMemberWhenSearch([FromQuery] SearchDepositAmountParam searchDepositAmountParam)
        {
            int userMember = GetLoginAccountId();
            if (userMember != 0)
            {
                var deposit = await _memberDepositAmountService.ListDepositAmoutByMemberWhenSearch(searchDepositAmountParam, userMember);
                Response.AddPaginationHeader(new PaginationHeader(deposit.CurrentPage, deposit.PageSize,
                deposit.TotalCount, deposit.TotalPages));
                if (deposit.PageSize == 0)
                {
                    var apiResponseMessage = new ApiResponseMessage("MSG01");
                    return Ok(new List<ApiResponseMessage> { apiResponseMessage });
                }
                else
                {
                    if (!ModelState.IsValid)
                        return BadRequest(ModelState);
                    return Ok(deposit);
                }
            }
            else
            {
                return BadRequest(new ApiResponse(401));
            }
        }
    }
}
