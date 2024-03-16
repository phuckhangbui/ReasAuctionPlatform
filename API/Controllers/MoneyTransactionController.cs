using API.Extensions;
using API.MessageResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Repository.Paging;
using Repository.Param;
using Service.Exceptions;
using Service.Interface;

namespace API.Controllers
{
    public class MoneyTransactionController : BaseApiController
    {
        private readonly IMoneyTransactionService _moneyTransactionService;

        public MoneyTransactionController(IMoneyTransactionService moneyTransactionService)
        {
            _moneyTransactionService = moneyTransactionService;
        }

        private const string BaseUri = "/api/transactions";
        private const string DetailUri = BaseUri + "/{transactionId}";
        private const string MemberUri = BaseUri + "/member/{accountId}";
        private const string MemberDetailUri = MemberUri + "/{transactionId}";


        [HttpPost(BaseUri)]
        [Authorize(policy: "AdminAndStaff")]
        public async Task<IActionResult> GetTransactionHistory([FromBody] MoneyTransactionRequest moneyTransactionRequest)
        {
            try
            {
                var transactionsHistory = await _moneyTransactionService.GetMoneyTransactions(moneyTransactionRequest);

                Response.AddPaginationHeader(new PaginationHeader(transactionsHistory.CurrentPage, transactionsHistory.PageSize,
                transactionsHistory.TotalCount, transactionsHistory.TotalPages));

                return Ok(transactionsHistory);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, ex.Message));
            }
        }

        [HttpGet(DetailUri)]
        [Authorize(policy: "AdminAndStaff")]
        public async Task<IActionResult> GetTransactionHistoryDetail(int transactionId)
        {
            try
            {
                var transactionDetail = await _moneyTransactionService.GetMoneyTransactionDetail(transactionId);

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

        [HttpGet(MemberUri)]
        [Authorize(policy: "Member")]
        public async Task<IActionResult> GetMemberTransactionHistory([FromQuery] MemberMoneyTransactionParam moneyTransactionParam, int accountId)
        {
            try
            {
                var transactionsHistory = await _moneyTransactionService.GetMemberMoneyTransactions(moneyTransactionParam, accountId);

                Response.AddPaginationHeader(new PaginationHeader(transactionsHistory.CurrentPage, transactionsHistory.PageSize,
                transactionsHistory.TotalCount, transactionsHistory.TotalPages));

                return Ok(transactionsHistory);
            }
            catch (BaseNotFoundException ex)
            {
                return NotFound(new ApiResponse(404, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, ex.Message));
            }
        }

        [HttpGet(MemberDetailUri)]
        [Authorize(policy: "Member")]
        public async Task<IActionResult> GetMemberTransactionHistoryDetail(int accountId, int transactionId)
        {
            try
            {
                var transactionDetail = await _moneyTransactionService.GetMemberMoneyTransactionDetail(accountId, transactionId);

                return Ok(transactionDetail);
            }
            catch (BaseNotFoundException ex)
            {
                return NotFound(new ApiResponse(404, ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, ex.Message));
            }
        }
    }
}
