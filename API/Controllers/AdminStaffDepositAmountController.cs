using API.Errors;
using API.Exceptions;
using API.Extension;
using API.Helper;
using API.Interface.Service;
using API.Param;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AdminStaffDepositAmountController : BaseApiController
    {
        private readonly IDepositAmountService _depositAmountService;

        public AdminStaffDepositAmountController(IDepositAmountService depositAmountService)
        {
            _depositAmountService = depositAmountService;
        }

        private const string BaseUri = "/api/deposits";
        private const string DetailUri = BaseUri + "/{depositId}";
        private const string DepositedUri = BaseUri + "/{reasId}/deposited";

        [HttpGet(BaseUri)]
        [Authorize(policy: "AdminAndStaff")]
        public async Task<IActionResult> GetDepositAmounts([FromQuery] DepositAmountParam depositAmountParam)
        {
            try
            {
                var depositAmounts = await _depositAmountService.GetDepositAmounts(depositAmountParam);

                Response.AddPaginationHeader(new PaginationHeader(depositAmounts.CurrentPage, depositAmounts.PageSize,
                depositAmounts.TotalCount, depositAmounts.TotalPages));

                return Ok(depositAmounts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, ex.Message));
            }
        }

        [HttpGet(DetailUri)]
        [Authorize(policy: "AdminAndStaff")]
        public IActionResult GetDepositDetail(int depositId)
        {
            try
            {
                var transactionDetail = _depositAmountService.GetDepositDetail(depositId);

                return Ok(transactionDetail);
            }
            catch (BaseNotFoundException ex)
            {
                return BadRequest(new ApiResponse(404, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, ex.Message));
            }
        }

        [HttpGet(DepositedUri)]
        [Authorize(policy: "AdminAndStaff")]
        public async Task<IActionResult> GetAccountsHadDeposited([FromQuery] PaginationParams paginationParams, int reasId)
        {
            try
            {
                var accounts = await _depositAmountService.GetAccountsHadDeposited(paginationParams, reasId);

                Response.AddPaginationHeader(new PaginationHeader(accounts.CurrentPage, accounts.PageSize,
                accounts.TotalCount, accounts.TotalPages));

                return Ok(accounts);
            }
            catch (BaseNotFoundException ex)
            {
                return BadRequest(new ApiResponse(404, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, ex.Message));
            }
        }
    }
}
