using API.DTOs;
using API.Entity;
using API.Errors;
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
    [Authorize(policy: "member")]
    public class MemberRealEstateController : BaseApiController
    {
        private readonly IMemberRealEstateService _memberRealEstateService;
        private readonly VnPayProperties _vnPayProperties;
        private readonly IVnPayService _vnPayService;
        private readonly IMoneyTransactionService _moneyTransactionService;

        private const string BaseUri = "/api/home/";
        public MemberRealEstateController(IMemberRealEstateService memberRealEstateService, IVnPayService vnPayService, IOptions<VnPayProperties> vnPayProperties, IMoneyTransactionService moneyTransactionService)
        {
            _memberRealEstateService = memberRealEstateService;
            _vnPayService = vnPayService;
            _vnPayProperties = vnPayProperties.Value;
            _moneyTransactionService = moneyTransactionService;
        }

        [HttpGet(BaseUri + "my_real_estate")]
        public async Task<ActionResult<RealEstateDto>> GetOnwerRealEstate([FromQuery] PaginationParams paginationParams)
        {
            int userMember = GetIdMember(_memberRealEstateService.AccountRepository);
            if (userMember != 0)
            {
                var reals = await _memberRealEstateService.GetOnwerRealEstate(userMember);
                Response.AddPaginationHeader(new PaginationHeader(reals.CurrentPage, reals.PageSize,
                reals.TotalCount, reals.TotalPages));
                if (reals.PageSize != 0)
                {
                    if (!ModelState.IsValid)
                        return BadRequest(ModelState);
                    return Ok(reals);
                }
                else
                {
                    return BadRequest(new ApiResponse(404, "No data with your search"));
                }
            }
            else
            {
                return BadRequest(new ApiResponse(401));
            }
        }

        [HttpPost(BaseUri + "my_real_estate/search")]
        public async Task<IActionResult> SearchOwnerRealEstateForMember(SearchRealEstateParam searchRealEstateParam)
        {
            int userMember = GetIdMember(_memberRealEstateService.AccountRepository);
            if (userMember != 0)
            {
                var reals = await _memberRealEstateService.SearchOwnerRealEstateForMember(searchRealEstateParam, userMember);
                Response.AddPaginationHeader(new PaginationHeader(reals.CurrentPage, reals.PageSize,
                reals.TotalCount, reals.TotalPages));
                if (reals.PageSize == 0)
                {
                    var apiResponseMessage = new ApiResponseMessage("MSG01");
                    return Ok(new List<ApiResponseMessage> { apiResponseMessage });
                }
                else
                {
                    if (!ModelState.IsValid)
                        return BadRequest(ModelState);
                    return Ok(reals);
                }
            }
            else
            {
                return BadRequest(new ApiResponse(401));
            }
        }

        [HttpGet(BaseUri + "my_real_estate/view")]
        public async Task<ActionResult<CreateNewRealEstatePage>> ViewCreateNewRealEstatePage()
        {
            int userMember = GetIdMember(_memberRealEstateService.AccountRepository);
            if (userMember != 0)
            {
                var list_type_reas = await _memberRealEstateService.ViewCreateNewRealEstatePage();
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                return Ok(list_type_reas);
            }
            else
            {
                return BadRequest(new ApiResponse(401));
            }
        }

        [HttpPost(BaseUri + "my_real_estate/create")]
        public async Task<ActionResult<ApiResponseMessage>> CreateNewRealEstate(NewRealEstateParam newRealEstateDto)
        {
            int userMember = GetIdMember(_memberRealEstateService.AccountRepository);
            if (userMember != 0)
            {
                bool flag = await _memberRealEstateService.CreateNewRealEstate(newRealEstateDto, userMember);
                if (flag)
                    return new ApiResponseMessage("MSG16");
                else
                {
                    return BadRequest(new ApiResponse(400, "Have any error when excute operation."));
                }
            }
            else
            {
                return BadRequest(new ApiResponse(401));
            }
        }

        [HttpGet(BaseUri + "my_real_estate/detail/{id}")]
        public async Task<ActionResult<RealEstateDetailDto>> ViewOwnerRealEstateDetail(int id)
        {
            int userMember = GetIdMember(_memberRealEstateService.AccountRepository);
            if (userMember != 0)
            {
                var _real_estate_detail = _memberRealEstateService.ViewOwnerRealEstateDetail(id);
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                return Ok(_real_estate_detail);
            }
            else
            {
                return BadRequest(new ApiResponse(401));
            }
        }

        [HttpPost(BaseUri + "my_real_estate/detail/{reasId}/createPaymentLink")]
        public async Task<ActionResult> CreatePaymentLink(CreatePaymentLinkDto createPaymentLinkDto)
        {
            if (GetLoginAccountId() != createPaymentLinkDto.AccountId)
            {
                return BadRequest(new ApiResponse(400));
            }


            var realEstateDetail = await _memberRealEstateService.ViewOwnerRealEstateDetail(createPaymentLinkDto.ReasId);
            if (realEstateDetail.AccountOwnerId != createPaymentLinkDto.AccountId)
            {
                return BadRequest(new ApiResponse(401, "Not match real estate with userId"));
            }

            if (realEstateDetail.ReasStatus != (int)RealEstateStatus.Approved)
            {
                return BadRequest(new ApiResponse(401, "Not in the pproved state"));
            }

            //default fee is 100,000 VND
            string paymentUrl = _vnPayService.CreatePostRealEstatePaymentURL(HttpContext, _vnPayProperties, createPaymentLinkDto.ReturnUrl);

            return Ok(paymentUrl);
        }

        [HttpGet(BaseUri + "pay/fee/returnUrl/{reasId}")]
        public async Task<ActionResult> PayRealEstateUploadFee([FromQuery] Dictionary<string, string> vnpayData, int reasId)
        {
            int customerId = GetLoginAccountId();

            if (customerId == 0)
            {
                return BadRequest(new ApiResponse(401));
            }

            var realEstateDetail = await _memberRealEstateService.ViewOwnerRealEstateDetail(reasId);

            if (realEstateDetail.AccountOwnerId != customerId)
            {
                return BadRequest(new ApiResponse(401, "Not match real estate with userId"));
            }

            if (realEstateDetail.ReasStatus != (int)RealEstateStatus.Approved)
            {
                return BadRequest(new ApiResponse(401, "Not in the payment state"));
            }

            string vnp_HashSecret = _vnPayProperties.HashSecret;
            try
            {
                MoneyTransaction transaction = ReturnUrl.ProcessReturnUrl(vnpayData, vnp_HashSecret, TransactionType.Upload_Fee);

                if (transaction != null)
                {
                    transaction.AccountSendId = customerId;
                    transaction.ReasId = reasId;

                    var result = await _moneyTransactionService.CreateMoneyTransaction(transaction);
                    if (!result)
                    {
                        return BadRequest(new ApiResponse(400));
                    }

                    //update realestate status
                    realEstateDetail.ReasStatus = (int)RealEstateStatus.Selling;
                    result = await _memberRealEstateService.UpdateRealEstateStatus(realEstateDetail, "");

                }
                return Ok(new
                {
                    transactionStatus = transaction.TransactionStatus
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(400));
            }

        }


        //[HttpPost(BaseUri + "my_real_estate/payment")]
        //public async Task<ActionResult<ApiResponseMessage>> PaymentAmountToUpRealEstaeAfterApprove(TransactionMoneyCreateParam transactionMoneyCreateParam)
        //{
        //    int userMember = GetIdMember(_memberRealEstateService.AccountRepository);
        //    if (userMember != 0)
        //    {
        //        if (transactionMoneyCreateParam.Money != transactionMoneyCreateParam.MoneyPaid)
        //        {
        //            return new ApiResponseMessage("MSG20");
        //        }
        //        else
        //        {
        //            try
        //            {
        //                bool check = await _memberRealEstateService.PaymentAmountToUpRealEstaeAfterApprove(transactionMoneyCreateParam, userMember);
        //                if (check)
        //                    return new ApiResponseMessage("MSG19");
        //                else return BadRequest(new ApiResponse(401, "Have any error when excute operation"));
        //            }
        //            catch (Exception ex)
        //            {
        //                return BadRequest(new ApiResponse(400, "Have any error when excute operation"));
        //            }
        //        }
        //    }
        //    else
        //    {
        //        return BadRequest(new ApiResponse(401));
        //    }
        //}
    }
}
