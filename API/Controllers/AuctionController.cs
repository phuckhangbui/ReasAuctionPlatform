using API.DTOs;
using API.Entity;
using API.Errors;
using API.Exceptions;
using API.Extension;
using API.Helper;
using API.Helper.VnPay;
using API.Interface.Service;
using API.MessageResponse;
using API.Param;
using API.Param.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Controllers
{
    public class AuctionController : BaseApiController
    {
        private readonly IAuctionService _auctionService;
        private readonly IAuctionAccountingService _auctionAccountingService;
        private readonly IDepositAmountService _depositAmountService;
        private readonly IMoneyTransactionService _moneyTransactionService;
        private readonly IRealEstateService _realEstateService;
        private readonly VnPayProperties _vnPayProperties;
        private readonly IVnPayService _vnPayService;

        public AuctionController(IAuctionService auctionService, IAuctionAccountingService auctionAccountingService, IDepositAmountService depositAmountService, IMoneyTransactionService moneyTransactionService, IOptions<VnPayProperties> vnPayProperties, IVnPayService vnPayService, IRealEstateService realEstateService)
        {
            _auctionService = auctionService;
            _auctionAccountingService = auctionAccountingService;
            _depositAmountService = depositAmountService;
            _moneyTransactionService = moneyTransactionService;
            _vnPayProperties = vnPayProperties.Value;
            _vnPayService = vnPayService;
            _realEstateService = realEstateService;
        }

        [HttpGet("/auctions/{reasId}")]
        public async Task<IActionResult> GetAuctionDetailByReasId(int reasId)
        {
            try
            {
                var auctionDetail = await _auctionService.GetAuctionDetailByReasId(reasId);

                return Ok(auctionDetail);
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

        [HttpGet("/auctions/{auctionId}/attenders")]
        public async Task<IActionResult> GetAuctionAttenders(int auctionId)
        {
            try
            {
                var attenderIds = await _auctionService.GetAuctionAttenders(auctionId);

                return Ok(attenderIds);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, ex.Message));
            }
        }

        [HttpGet("auctions")]
        public async Task<IActionResult> GetAuctionsForMember([FromQuery] AuctionParam auctionParam)
        {
            var auctions = await _auctionService.GetNotyetAndOnGoingAuction(auctionParam);

            Response.AddPaginationHeader(new PaginationHeader(auctions.CurrentPage, auctions.PageSize,
            auctions.TotalCount, auctions.TotalPages));

            return Ok(auctions);
        }

        [Authorize]
        [HttpGet("auctions/all/detail/{id}")]
        public async Task<ActionResult<AuctionDetailOnGoing>> GetAuctionsDetailNotYetAndOnGoing(int id)
        {
            var auctions = await _auctionService.GetAuctionDetailOnGoing(id);

            if (auctions != null)
            {
                return Ok(auctions);
            }
            else
            {
                return null;
            }
        }

        //for search also
        [Authorize(policy: "AdminAndStaff")]
        [HttpGet("admin/auctions/all")]
        public async Task<ActionResult<IEnumerable<AuctionDto>>> GetAuctionsNotYetAndOnGoing()
        {
            var auctions = await _auctionService.GetAuctionsNotYetAndOnGoing();
            if (auctions != null)
            {
                return Ok(auctions);
            }
            else
            {
                return null;
            }
        }




        [Authorize(policy: "AdminAndStaff")]
        [HttpGet("admin/auctions/complete")]
        public async Task<ActionResult<IEnumerable<AuctionDto>>> GetAuctionsFinish()
        {
            var auctions = await _auctionService.GetAuctionsFinish();

            if (auctions != null)
            {
                return Ok(auctions);
            }
            else
            {
                return null;
            }
        }

        [Authorize(policy: "AdminAndStaff")]
        [HttpGet("auctions/complete/detail/{id}")]
        public async Task<ActionResult<AuctionDetailFinish>> GetAuctionsDetailFinish(int id)
        {
            var auctions = await _auctionService.GetAuctionDetailFinish(id);

            if (auctions != null)
            {
                return Ok(auctions);
            }
            else
            {
                return null;
            }
        }

        [Authorize(policy: "AdminAndStaff")]
        [HttpGet("admin/edit/status")]
        public async Task<ActionResult<ApiResponseMessage>> ToggleAuctionStatus([FromQuery] string auctionId, string statusCode)
        {
            try
            {
                bool check = await _auctionService.ToggleAuctionStatus(auctionId, statusCode);
                if (check) return Ok(new ApiResponseMessage("MSG03"));
                else return BadRequest(new ApiResponse(401, "Have an error when excute operation."));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(400, "No AuctionId matched"));
            }

        }




        [Authorize(policy: "Member")]
        [HttpPost("success")]
        public async Task<ActionResult<AuctionAccountingDto>> AuctionSuccess(AuctionDetailDto auctionDetailDto)
        {
            AuctionAccountingDto auctionAccountingDto = new AuctionAccountingDto();
            try
            {
                //update/add auction accounting
                auctionAccountingDto = await _auctionAccountingService.UpdateAuctionAccounting(auctionDetailDto);

                if (auctionAccountingDto == null)
                {
                    return BadRequest(new ApiResponse(400, "Real estate is not auctioning"));
                }

                //update auction status
                int statusFinish = (int)AuctionStatus.Finish;
                bool result = await _auctionService.ToggleAuctionStatus(auctionDetailDto.AuctionId.ToString(), statusFinish.ToString());

                //update status of the remain looser user



                if (result)
                {
                    //send email
                    await _auctionAccountingService.SendWinnerEmail(auctionAccountingDto);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(404));
            }
            //return calculate result in auction accounting

            return Ok(auctionAccountingDto);
        }

        [Authorize]
        [HttpGet("owner/auction-history")]
        public async Task<IActionResult> GetOwnerAuctionHistory([FromQuery] AuctionHistoryParam auctionHisotoryParam)
        {
            try
            {
                var auctionHistory = await _auctionService.GetAuctionHisotoryForOwner(auctionHisotoryParam);

                Response.AddPaginationHeader(new PaginationHeader(auctionHistory.CurrentPage, auctionHistory.PageSize,
                auctionHistory.TotalCount, auctionHistory.TotalPages));

                return Ok(auctionHistory);
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

        [Authorize]
        [HttpGet("owner/auction-history/{auctionId}")]
        public async Task<IActionResult> GetOwnerAuctionAccouting(int auctionId)
        {
            try
            {
                var auctionAccouting = await _auctionAccountingService.GetAuctionAccounting(auctionId);

                return Ok(auctionAccouting);
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

        [Authorize]
        [HttpGet("attender/auction-history")]
        public async Task<IActionResult> GetAttenderAuctionHistory([FromQuery] AuctionHistoryParam auctionHisotoryParam)
        {
            try
            {
                var auctionHistory = await _auctionService.GetAuctionHisotoryForAttender(auctionHisotoryParam);

                Response.AddPaginationHeader(new PaginationHeader(auctionHistory.CurrentPage, auctionHistory.PageSize,
                auctionHistory.TotalCount, auctionHistory.TotalPages));

                return Ok(auctionHistory);
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

        [Authorize(policy: "Member")]
        [HttpGet("register")]
        public async Task<ActionResult<DepositAmountDtoWithPaymentUrl>> RegisterAuction([FromQuery] string customerId, string reasId, string returnUrl)
        {
            if (GetLoginAccountId() != int.Parse(customerId))
            {
                return BadRequest(new ApiResponse(400));
            }



            try
            {
                var realEstate = await _realEstateService.ViewRealEstateDetail(int.Parse(reasId));
                if (realEstate == null)
                {
                    return BadRequest(new ApiResponse(400));
                }

                if (realEstate.ReasStatus != (int)RealEstateStatus.Selling)
                {
                    return BadRequest(new ApiResponse(400));

                }

                var depositAmountDto = _depositAmountService.GetDepositAmount(int.Parse(customerId), int.Parse(reasId));
                if (depositAmountDto == null)
                {
                    depositAmountDto = await _depositAmountService.CreateDepositAmount(int.Parse(customerId), int.Parse(reasId));
                    if (depositAmountDto == null)
                    {
                        return BadRequest(new ApiResponse(400, "Real estate is not selling"));
                    }
                }
                if (depositAmountDto.Status.Equals(UserDepositEnum.Pending.ToString()))
                {
                    return BadRequest(new ApiResponse(400, "Deposit is not pending"));
                }

                //create new vnpayment url
                string paymentUrl = _vnPayService.CreateDepositePaymentURL(HttpContext, depositAmountDto, _vnPayProperties, returnUrl);

                DepositAmountDtoWithPaymentUrl depositAmountDtoWithPaymentUrl = new DepositAmountDtoWithPaymentUrl
                {
                    DepositAmountDto = depositAmountDto,
                    PaymentUrl = paymentUrl
                };
                return Ok(depositAmountDtoWithPaymentUrl);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(400));
            }
        }


        // sample get request https://localhost:44383/api/auction/pay/deposit/returnUrl/4?vnp_Amount=2500000&vnp_BankCode=NCB&vnp_BankTranNo=VNP14313776&vnp_CardType=ATM&vnp_OrderInfo=Auction+Deposit+Fee&vnp_PayDate=20240305102408&vnp_ResponseCode=00&vnp_TmnCode=6EMYCUD2&vnp_TransactionNo=14313776&vnp_TransactionStatus=00&vnp_TxnRef=638452310013886970&vnp_SecureHash=c85ad2998d07545289cce3c8085f78174cfdfdc5cf6a218945254f0161cedb166c25b89e08006b6d7dc59879a12594ca3be283cd62eae2741eb0dbb695846ddd
        [Authorize(policy: "Member")]
        [HttpGet("pay/deposit/returnUrl/{depositId}")]
        public async Task<ActionResult> PayAuctionDeposit([FromQuery] Dictionary<string, string> vnpayData, int depositId)
        {
            DepositAmount depositAmount = _depositAmountService.GetDepositAmount(depositId);

            if (depositAmount == null)
            {
                return BadRequest(new ApiResponse(400, "Customer has not registered to bid in this real estate"));
            }

            if (depositAmount.Status != (int)UserDepositEnum.Pending)
            {
                return BadRequest(new ApiResponse(400, "Customer has already paid the deposit"));

            }


            string vnp_HashSecret = _vnPayProperties.HashSecret;

            try
            {
                MoneyTransaction transaction = ReturnUrl.ProcessReturnUrl(vnpayData, vnp_HashSecret, TransactionType.Deposit_Auction_Fee);

                if (transaction != null)
                {
                    transaction.AccountSendId = depositAmount.AccountSignId;
                    transaction.DepositId = depositId;

                    var result = await _moneyTransactionService.CreateMoneyTransaction(transaction);
                    if (!result)
                    {
                        return BadRequest(new ApiResponse(400));
                    }
                    DepositAmountDto depositAmountDto = await _depositAmountService.UpdateStatusToDeposited(depositId, transaction.DateExecution);
                }

                return Ok(transaction);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(400));
            }
        }


        [Authorize(policy: "AdminAndStaff")]
        [HttpGet("admin/realfordeposit")]
        public async Task<ActionResult<IEnumerable<ReasForAuctionDto>>> GetAuctionsReasForCreate()
        {
            var real = await _auctionService.GetAuctionsReasForCreate();

            if (real != null)
            {
                return Ok(real);
            }
            else
            {
                return null;
            }
        }

        [Authorize(policy: "AdminAndStaff")]
        [HttpGet("admin/realfordeposit/{reasId}")]
        public async Task<ActionResult<IEnumerable<DepositAmountUserDto>>> GetAllUserForDeposit(int reasId)
        {
            var deposit = await _auctionService.GetAllUserForDeposit(reasId);

            if (deposit != null)
            {
                return Ok(deposit);
            }
            else
            {
                return null;
            }
        }

        [Authorize(policy: "AdminAndStaff")]
        [HttpPost("admin/create")]
        public async Task<ActionResult<ApiResponseMessage>> CreateAuction(AuctionCreateParam auctionCreateParam)
        {
            bool check = await _auctionService.CreateAuction(auctionCreateParam);
            if (check)
            {
                return new ApiResponseMessage("MSG05");
            }
            else
            {
                return BadRequest(new ApiResponse(400, "Have any error when excute operation."));
            }
        }

    }
}
