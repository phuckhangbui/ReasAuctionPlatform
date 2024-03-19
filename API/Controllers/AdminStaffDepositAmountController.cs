using API.MessageResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Param;
using Service.Exceptions;
using Service.Interface;

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
        private const string UpdateStatusUri = BaseUri + "/update/refund";

        [HttpGet(BaseUri)]
        [Authorize(policy: "AdminAndStaff")]
        public async Task<IActionResult> GetDepositAmounts()
        {
            try
            {
                var depositAmounts = await _depositAmountService.GetRealEstateForDepositAsync();
                return Ok(depositAmounts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, ex.Message));
            }
        }

        [HttpGet(DetailUri)]
        [Authorize(policy: "AdminAndStaff")]
        public async Task<IActionResult> GetDepositDetail(int depositId)
        {
            try
            {
                var transactionDetail = await _depositAmountService.GetDepositDetail(depositId);

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

        [HttpPost(UpdateStatusUri)]
        [Authorize(policy: "AdminAndStaff")]
        public async Task<ActionResult<ApiResponseMessage>> ChangeStatusRefund(RefundTransactionParam refundTransactionParam)
        {
            try
            {
                bool flag = await _depositAmountService.ChangeStatusWhenRefund(refundTransactionParam);

                if (flag)
                {
                    return new ApiResponseMessage("MSG26");
                }
                else
                {
                    return BadRequest(new ApiResponse(400, "Have any error when excute operation."));
                }
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
