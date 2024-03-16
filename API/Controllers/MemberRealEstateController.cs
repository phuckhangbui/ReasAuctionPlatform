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
using System.Collections.Specialized;
using System.Web;

namespace API.Controllers
{
    [Authorize(policy: "Member")]
    public class MemberRealEstateController : BaseApiController
    {
        private readonly IMemberRealEstateService _memberRealEstateService;
        private readonly INotificatonService _notificatonService;
        private readonly VnPayProperties _vnPayProperties;
        private readonly IVnPayService _vnPayService;
        private readonly IMoneyTransactionService _moneyTransactionService;

        private const string BaseUri = "/api/home/";
        public MemberRealEstateController(IMemberRealEstateService memberRealEstateService, IVnPayService vnPayService, IOptions<VnPayProperties> vnPayProperties, IMoneyTransactionService moneyTransactionService, INotificatonService notificatonService)
        {
            _memberRealEstateService = memberRealEstateService;
            _vnPayService = vnPayService;
            _vnPayProperties = vnPayProperties.Value;
            _moneyTransactionService = moneyTransactionService;
            _notificatonService = notificatonService;
        }


        [Authorize(policy: "Member")]
        [HttpGet(BaseUri + "my_real_estate")]
        public async Task<ActionResult<RealEstateDto>> GetOnwerRealEstate([FromQuery] PaginationParams paginationParams)
        {
            var reals = await _memberRealEstateService.GetOnwerRealEstate(GetLoginAccountId());
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


        [Authorize(policy: "Member")]
        [HttpPost(BaseUri + "my_real_estate/search")]
        public async Task<IActionResult> SearchOwnerRealEstateForMember(SearchRealEstateParam searchRealEstateParam)
        {
            var reals = await _memberRealEstateService.SearchOwnerRealEstateForMember(searchRealEstateParam, GetLoginAccountId());
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


        [Authorize(policy: "Member")]
        [HttpGet(BaseUri + "my_real_estate/view")]
        public async Task<ActionResult<CreateNewRealEstatePage>> ViewCreateNewRealEstatePage()
        {
            var list_type_reas = await _memberRealEstateService.ViewCreateNewRealEstatePage();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(list_type_reas);
        }


        [Authorize(policy: "Member")]
        [HttpPost(BaseUri + "my_real_estate/create")]
        public async Task<ActionResult<ApiResponseMessage>> CreateNewRealEstate(NewRealEstateParam newRealEstateDto)
        {
            int userMember = GetLoginAccountId();
            if (userMember != 0)
            {
                var realEstate = await _memberRealEstateService.CreateNewRealEstate(newRealEstateDto, userMember);
                if (realEstate != null)
                {


                    //send message here
                    await _notificatonService.SendNotificationWhenMemberCreateReal(realEstate);

                    return new ApiResponseMessage("MSG16");
                }
                else
                {
                    return BadRequest(new ApiResponse(400, "Have any error when excute operation."));
                }
            }
            return BadRequest(new ApiResponse(400, "Not match accountId"));

        }


        [Authorize(policy: "Member")]
        [HttpGet(BaseUri + "my_real_estate/detail/{id}")]
        public async Task<ActionResult<RealEstateDetailDto>> ViewOwnerRealEstateDetail(int id)
        {
            var _real_estate_detail = await _memberRealEstateService.ViewOwnerRealEstateDetail(id);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(_real_estate_detail);
        }

        [Authorize(policy: "Member")]
        [HttpPost(BaseUri + "my_real_estate/detail/{reasId}/createPaymentLink")]
        public async Task<ActionResult<RealEstatePaymentReponseDto>> CreatePaymentLink(CreatePaymentLinkDto createPaymentLinkDto)
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
                return BadRequest(new ApiResponse(401, "Not in the approved state"));
            }

            //default fee is 100,000 VND
            string paymentUrl = _vnPayService.CreatePostRealEstatePaymentURL(HttpContext, _vnPayProperties, createPaymentLinkDto.ReturnUrl);
            RealEstatePaymentReponseDto response = new RealEstatePaymentReponseDto
            {
                ReasId = createPaymentLinkDto.ReasId,
                paymentUrl = paymentUrl,
            };

            return Ok(response);
        }

        [Authorize(policy: "Member")]
        [HttpPost(BaseUri + "pay/fee/returnUrl/{reasId}")]
        public async Task<ActionResult> PayRealEstateUploadFee([FromBody] VnPayReturnUrlDto vnpayDataDto, int reasId)
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

            try
            {
                NameValueCollection queryParams = HttpUtility.ParseQueryString(HttpUtility.UrlDecode(vnpayDataDto.url));

                Dictionary<string, string> vnpayData = queryParams.AllKeys.ToDictionary(k => k, k => queryParams[k]);
                string vnp_HashSecret = _vnPayProperties.HashSecret;
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
