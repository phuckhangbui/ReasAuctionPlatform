using API.DTOs;
using API.Helper.VnPay;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace API.Controllers
{
    [ApiController]
    public class TestController : BaseApiController
    {
        private readonly VnPayProperties _vnPayProperties;


        public TestController(IOptions<VnPayProperties> vnPayProperties)
        {
            _vnPayProperties = vnPayProperties.Value;
        }

        [HttpGet("vnpayUrl/{TnxRef}")]
        public async Task<ActionResult<string>> TestCreatePayment(string TnxRef)
        {
            VnPayPaymentUrlDto vnPayPaymentUrlDto = new VnPayPaymentUrlDto();
            vnPayPaymentUrlDto.Url = _vnPayProperties.Url;
            vnPayPaymentUrlDto.TmnCode = _vnPayProperties.TmnCode;
            vnPayPaymentUrlDto.Version = _vnPayProperties.Version;
            vnPayPaymentUrlDto.HashSecret = _vnPayProperties.HashSecret;

            vnPayPaymentUrlDto.Amount = 100000;
            vnPayPaymentUrlDto.CreateDate = DateTime.Now;
            vnPayPaymentUrlDto.ExpireDate = DateTime.Now.AddMinutes(10);
            vnPayPaymentUrlDto.Locale = "vn";
            vnPayPaymentUrlDto.OrderInfo = "test vnpay";
            vnPayPaymentUrlDto.ReturnUrl = "https://localhost:44383/test";
            vnPayPaymentUrlDto.TxnRef = TnxRef;

            string paymentUrl = CreateUrl.CreatePaymentUrl(vnPayPaymentUrlDto, HttpContext);
            return Ok(paymentUrl);
        }


        [HttpGet("auth")]
        public async Task<ActionResult<int>> TestAuth()
        {
            return GetLoginAccountId();
        }

        [HttpGet("auth/member")]
        [Authorize(policy: "Member")]
        public async Task<ActionResult<String>> TestAuthMem()
        {
            return "You are good member";
        }

        [HttpGet("authAdmin")]
        [Authorize(policy: "Admin")]
        public async Task<ActionResult<String>> TestAuthAd()
        {
            return "You are good admin";
        }

        [HttpGet("auth/staff")]
        [Authorize(policy: "Staff")]
        public async Task<ActionResult<String>> TestAuthStaff()
        {
            return "You are good staff";
        }

        [HttpGet("auth/adminstaff")]
        [Authorize(policy: "AdminAndStaff")]
        public async Task<ActionResult<String>> TestAuthAdStaff()
        {
            return "You are good admin and staff";
        }
    }
}
