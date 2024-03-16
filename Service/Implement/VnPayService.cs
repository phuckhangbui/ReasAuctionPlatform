using Microsoft.AspNetCore.Http;
using Repository.DTOs;
using Service.Interface;
using Service.VnPay;

namespace Service.Implement
{
    public class VnPayService : IVnPayService
    {
        public string CreateDepositePaymentURL(HttpContext context, DepositAmountDto dto, VnPayProperties vnPayProperties, string returnUrl)
        {
            VnPayPaymentUrlDto vnPayPaymentUrlDto = new VnPayPaymentUrlDto();
            vnPayPaymentUrlDto.Url = vnPayProperties.Url;
            vnPayPaymentUrlDto.TmnCode = vnPayProperties.TmnCode;
            vnPayPaymentUrlDto.Version = vnPayProperties.Version;
            vnPayPaymentUrlDto.HashSecret = vnPayProperties.HashSecret;

            vnPayPaymentUrlDto.Amount = (long)dto.Amount;
            vnPayPaymentUrlDto.CreateDate = DateTime.Now;
            vnPayPaymentUrlDto.ExpireDate = DateTime.Now.AddMinutes(1);
            vnPayPaymentUrlDto.Locale = "vn";
            vnPayPaymentUrlDto.OrderInfo = "Auction Deposit Fee";
            vnPayPaymentUrlDto.ReturnUrl = returnUrl;
            vnPayPaymentUrlDto.TxnRef = DateTime.Now.Ticks.ToString();

            string paymentUrl = CreateUrl.CreatePaymentUrl(vnPayPaymentUrlDto, context);

            return paymentUrl;
        }

        public string CreatePostRealEstatePaymentURL(HttpContext context, VnPayProperties vnPayProperties, string returnUrl)
        {
            VnPayPaymentUrlDto vnPayPaymentUrlDto = new VnPayPaymentUrlDto();
            vnPayPaymentUrlDto.Url = vnPayProperties.Url;
            vnPayPaymentUrlDto.TmnCode = vnPayProperties.TmnCode;
            vnPayPaymentUrlDto.Version = vnPayProperties.Version;
            vnPayPaymentUrlDto.HashSecret = vnPayProperties.HashSecret;

            vnPayPaymentUrlDto.Amount = 100000;
            vnPayPaymentUrlDto.CreateDate = DateTime.Now;
            vnPayPaymentUrlDto.ExpireDate = DateTime.Now.AddMinutes(1);
            vnPayPaymentUrlDto.Locale = "vn";
            vnPayPaymentUrlDto.OrderInfo = "Real Estate Fee";
            vnPayPaymentUrlDto.ReturnUrl = returnUrl;
            vnPayPaymentUrlDto.TxnRef = DateTime.Now.Ticks.ToString();

            string paymentUrl = CreateUrl.CreatePaymentUrl(vnPayPaymentUrlDto, context);

            return paymentUrl;
        }
    }
}
