using Microsoft.AspNetCore.Http;
using Repository.DTOs;
using Service.VnPay;

namespace Service.Interface
{
    public interface IVnPayService
    {
        string CreateDepositePaymentURL(HttpContext context, DepositAmountDto dto, VnPayProperties vnPayProperties, string returnUrl);

        string CreatePostRealEstatePaymentURL(HttpContext context, VnPayProperties vnPayProperties, string returnUrl);
    }
}
