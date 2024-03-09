using API.Param.Enums;

namespace API.Helper
{
    public class GetStatusName
    {
        public string GetRealEstateStatusName(int status)
        {
            RealEstateStatus realEstateStatus = (RealEstateStatus)status;
            string statusName = realEstateStatus.ToString();
            return statusName;
        }

        public string GetDepositAmountStatusName(int status)
        {
            UserDepositEnum depositAmountStatus = (UserDepositEnum)status;
            string statusName = depositAmountStatus.ToString();
            return statusName;
        }

        public string GetStatusAccountName(int status)
        {
            AccountStatus AccountStatus = (AccountStatus)status;
            string statusName = AccountStatus.ToString();
            return statusName;
        }

        public string GetStatusAuctionName(int status)
        {
            AuctionStatus AuctionStatus = (AuctionStatus)status;
            string statusName = AuctionStatus.ToString();
            return statusName;
        }

        public string GetStatusDepositName(int status)
        {
            UserDepositEnum DepositStatus = (UserDepositEnum)status;
            string statusName = DepositStatus.ToString();
            return statusName;
        }
    }
}
