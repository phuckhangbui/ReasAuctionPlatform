using API.DTOs;
using API.Helper.VnPay;

namespace API.Interface.Service
{
    public interface IVnPayService
    {
        string CreateDepositePaymentURL(HttpContext context, DepositAmountDto dto, VnPayProperties vnPayProperties, string returnUrl);

        string CreatePostRealEstatePaymentURL(HttpContext context, VnPayProperties vnPayProperties, string returnUrl);
    }
}
