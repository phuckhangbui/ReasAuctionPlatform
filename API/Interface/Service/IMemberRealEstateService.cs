using API.DTOs;
using API.Helper;
using API.Interface.Repository;
using API.Param;

namespace API.Interface.Service
{
    public interface IMemberRealEstateService
    {
        IAccountRepository AccountRepository { get; }
        Task<PageList<RealEstateDto>> GetOnwerRealEstate(int userMember);
        Task<PageList<RealEstateDto>> SearchOwnerRealEstateForMember(SearchRealEstateParam searchRealEstateParam, int userMember);
        Task<IEnumerable<CreateNewRealEstatePage>> ViewCreateNewRealEstatePage();
        Task<bool> CreateNewRealEstate(NewRealEstateParam newRealEstateParam, int userMember);
        Task<RealEstateDetailDto> ViewOwnerRealEstateDetail(int id);
        Task<bool> PaymentAmountToUpRealEstaeAfterApprove(TransactionMoneyCreateParam transactionMoneyCreateParam, int userMember);
    }
}
