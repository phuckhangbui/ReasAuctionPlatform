using API.DTOs;
using API.Entity;
using API.Helper;
using API.Interface.Repository;
using API.Interface.Service;
using API.Interfaces;
using API.Param;
using API.Param.Enums;

namespace API.Services
{
    public class MemberRealEstateService : IMemberRealEstateService
    {
        private readonly IRealEstateRepository _real_estate_repository;
        private readonly IAccountRepository _account_repository;
        private readonly IRealEstatePhotoRepository _real_estate_photo_repository;
        private readonly IRealEstateDetailRepository _real_estate_detail_repository;
        private readonly IMoneyTransactionRepository _money_transaction_repository;
        private readonly IPhotoService _photoService;
        private readonly ITypeReasRepository _typeReasRepository;

        public MemberRealEstateService(IRealEstateRepository real_estate_repository, IAccountRepository account_repository, IRealEstatePhotoRepository real_estate_photo_repository, IRealEstateDetailRepository real_estate_detail_repository, IMoneyTransactionRepository money_transaction_repository, IPhotoService photoService, ITypeReasRepository typeReasRepository)
        {
            _real_estate_repository = real_estate_repository;
            _account_repository = account_repository;
            _real_estate_photo_repository = real_estate_photo_repository;
            _real_estate_detail_repository = real_estate_detail_repository;
            _money_transaction_repository = money_transaction_repository;
            _photoService = photoService;
            _typeReasRepository = typeReasRepository;
        }

        public IAccountRepository AccountRepository => _account_repository;

        public async Task<RealEstate> CreateNewRealEstate(NewRealEstateParam newRealEstateParam, int userMember)
        {
            var newRealEstate = new RealEstate();
            var newPhotoList = new RealEstatePhoto();
            var newDetail = new RealEstateDetail();
            bool checkProcess = false;
            newRealEstate.ReasName = newRealEstateParam.ReasName;
            newRealEstate.ReasPrice = newRealEstateParam.ReasPrice;
            newRealEstate.ReasAddress = newRealEstateParam.ReasAddress;
            newRealEstate.ReasArea = newRealEstateParam.ReasArea;
            newRealEstate.ReasDescription = newRealEstateParam.ReasDescription;
            newRealEstate.Message = "";
            newRealEstate.AccountOwnerId = userMember;
            newRealEstate.DateCreated = DateTime.UtcNow;
            newRealEstate.Type_Reas = newRealEstateParam.Type_Reas;
            newRealEstate.DateStart = newRealEstateParam.DateStart;
            newRealEstate.DateEnd = newRealEstateParam.DateEnd;
            newRealEstate.ReasStatus = (int)RealEstateStatus.InProgress;
            try
            {
                newRealEstate.AccountOwnerName = await _account_repository.GetNameAccountByAccountIdAsync(userMember);
                await _real_estate_repository.CreateAsync(newRealEstate);
                foreach (PhotoFileDto photos in newRealEstateParam.Photos)
                {
                    newPhotoList.ReasPhotoId = 0;
                    newPhotoList.ReasId = newRealEstate.ReasId;
                    newPhotoList.ReasPhotoUrl = photos.ReasPhotoUrl;
                    await _real_estate_photo_repository.CreateAsync(newPhotoList);
                }
                try
                {
                    newDetail.ReasDetailId = 0;
                    newDetail.Reas_Cert_Of_Land_Img_Front = newRealEstateParam.Detail.Reas_Cert_Of_Land_Img_Front;
                    newDetail.Reas_Cert_Of_Land_Img_After = newRealEstateParam.Detail.Reas_Cert_Of_Land_Img_After;
                    newDetail.Reas_Cert_Of_Home_Ownership = newRealEstateParam.Detail.Reas_Cert_Of_Home_Ownership;
                    newDetail.Reas_Registration_Book = newRealEstateParam.Detail.Reas_Registration_Book;
                    newDetail.Sales_Authorization_Contract = newRealEstateParam.Detail.Sales_Authorization_Contract;
                    newDetail.Documents_Proving_Marital_Relationship = newRealEstateParam.Detail.Documents_Proving_Marital_Relationship;
                    newDetail.ReasId = newRealEstate.ReasId;
                    bool flag = await _real_estate_detail_repository.CreateAsync(newDetail);
                    if (flag) return newRealEstate;
                    else return null;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<PageList<RealEstateDto>> GetOnwerRealEstate(int userMember)
        {
            var reals = await _real_estate_repository.GetOwnerRealEstate(userMember);
            return reals;
        }


        //public async Task<bool> PaymentAmountToUpRealEstaeAfterApprove(TransactionMoneyCreateParam transactionMoneyCreateParam, int userMember)
        //{
        //    ReasStatusParam reasStatusDto = new ReasStatusParam();
        //    reasStatusDto.reasId = transactionMoneyCreateParam.IdReas;
        //    reasStatusDto.reasStatus = (int)RealEstateStatus.Selling;
        //    reasStatusDto.messageString = "";
        //    bool check = await _real_estate_repository.UpdateRealEstateStatusAsync(reasStatusDto);
        //    if (check)
        //    {
        //        bool check_trans = await _money_transaction_repository.CreateNewMoneyTransaction(transactionMoneyCreateParam, userMember);
        //        if (check_trans)
        //        {
        //            int idTransaction = await _money_transaction_repository.GetIdTransactionWhenCreateNewTransaction();
        //            //bool check_trans_detail = await _moneyTransactionDetailRepository.CreateNewMoneyTransaction(transactionMoneyCreateParam, idTransaction);
        //            //if (check_trans_detail) return true;
        //            //else return false;
        //            return true;
        //        }
        //        else return false;
        //    }
        //    else return false;
        //}

        public async Task<PageList<RealEstateDto>> SearchOwnerRealEstateForMember(SearchRealEstateParam searchRealEstateParam, int userMember)
        {
            var reals = await _real_estate_repository.GetOwnerRealEstateBySearch(userMember, searchRealEstateParam);
            return reals;
        }

        public async Task<IEnumerable<CreateNewRealEstatePage>> ViewCreateNewRealEstatePage()
        {
            var list_type_reas = _typeReasRepository.GetAllAsync().Result.Select(x => new CreateNewRealEstatePage
            {
                TypeReasId = x.Type_ReasId,
                TypeName = x.Type_Reas_Name,
            });
            return list_type_reas;
        }

        public async Task<RealEstateDetailDto> ViewOwnerRealEstateDetail(int id)
        {
            var _real_estate_detail = await _real_estate_detail_repository.GetRealEstateDetail(id);
            return _real_estate_detail;
        }

        public async Task<bool> UpdateRealEstateStatus(RealEstateDetailDto realEstateDetailDto, string message)
        {
            ReasStatusParam realEstateStatus = new ReasStatusParam
            {
                reasId = realEstateDetailDto.ReasId,
                reasStatus = realEstateDetailDto.ReasStatus,
                messageString = message,
            };
            bool check = await _real_estate_repository.UpdateRealEstateStatusAsync(realEstateStatus);

            return check;
        }


    }
}
