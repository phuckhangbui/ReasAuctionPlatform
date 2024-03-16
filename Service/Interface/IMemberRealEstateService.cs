using BusinessObject.Entity;
using Repository.DTOs;
using Repository.Interface;
using Repository.Paging;
using Repository.Param;

namespace Service.Interface
{
    public interface IMemberRealEstateService
    {
        IAccountRepository AccountRepository { get; }
        Task<PageList<RealEstateDto>> GetOnwerRealEstate(int userMember);
        Task<PageList<RealEstateDto>> SearchOwnerRealEstateForMember(SearchRealEstateParam searchRealEstateParam, int userMember);
        Task<IEnumerable<CreateNewRealEstatePage>> ViewCreateNewRealEstatePage();
        Task<RealEstate> CreateNewRealEstate(NewRealEstateParam newRealEstateParam, int userMember);
        Task<RealEstateDetailDto> ViewOwnerRealEstateDetail(int id);
        //Task<bool> PaymentAmountToUpRealEstaeAfterApprove(TransactionMoneyCreateParam transactionMoneyCreateParam, int userMember);
        Task<bool> UpdateRealEstateStatus(RealEstateDetailDto realEstateDetailDto, string message);
    }
}
