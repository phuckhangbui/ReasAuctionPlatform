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

        public async Task<bool> CreateNewRealEstate(NewRealEstateParam newRealEstateParam, int userMember)
        {
            var newRealEstate = new RealEstate();
            var newPhotoList = new RealEstatePhoto();
            var newDetail = new RealEstateDetail();
            //ConvertStringToFile convertStringToFile = new ConvertStringToFile();
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

            newRealEstate.AccountOwnerName = await _account_repository.GetNameAccountByAccountIdAsync(userMember);
            await _real_estate_repository.CreateAsync(newRealEstate);
            foreach (PhotoFileDto photos in newRealEstateParam.Photos)
            {
                //IFormFile formFile = convertStringToFile.ConvertToIFormFile(photos.ReasPhotoUrl);
                //var result = await _photoService.AddPhotoAsync(formFile, newRealEstate.ReasId, newRealEstate.ReasName, newRealEstate.AccountOwnerName);
                //if (result.Error != null)
                //{
                //    return false;
                //}
                //else
                //{
                    try
                    {
                        //newPhotoList.ReasPhotoUrl = result.SecureUrl.AbsoluteUri;
                        newPhotoList.ReasId = newRealEstate.ReasId;
                        newPhotoList.ReasPhotoId = 0;
                        bool check = await _real_estate_photo_repository.CreateAsync(newPhotoList);
                        if (check)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                }
            try
            {
                //IFormFile file_documents_Proving_Marital_Relationship = convertStringToFile.ConvertToIFormFile(newRealEstateParam.Detail.Documents_Proving_Marital_Relationship);
                //IFormFile file_reas_Cert_Of_Land_Img_Front = convertStringToFile.ConvertToIFormFile(newRealEstateParam.Detail.Reas_Cert_Of_Land_Img_Front);
                //IFormFile file_reas_Cert_Of_Land_Img_After = convertStringToFile.ConvertToIFormFile(newRealEstateParam.Detail.Reas_Cert_Of_Land_Img_After);
                //IFormFile file_reas_Cert_Of_Home_Ownership = convertStringToFile.ConvertToIFormFile(newRealEstateParam.Detail.Reas_Cert_Of_Home_Ownership);
                //IFormFile file_reas_Registration_Book = convertStringToFile.ConvertToIFormFile(newRealEstateParam.Detail.Reas_Registration_Book);
                //IFormFile file_sales_Authorization_Contract = convertStringToFile.ConvertToIFormFile(newRealEstateParam.Detail.Sales_Authorization_Contract);
                //var documents_Proving_Marital_Relationship = await _photoService.AddPhotoAsync(file_documents_Proving_Marital_Relationship, newRealEstate.ReasId, newRealEstate.ReasName, newRealEstate.AccountOwnerName);
                //var reas_Cert_Of_Land_Img_Front = await _photoService.AddPhotoAsync(file_reas_Cert_Of_Land_Img_Front, newRealEstate.ReasId, newRealEstate.ReasName, newRealEstate.AccountOwnerName);
                //var reas_Cert_Of_Land_Img_After = await _photoService.AddPhotoAsync(file_reas_Cert_Of_Land_Img_After, newRealEstate.ReasId, newRealEstate.ReasName, newRealEstate.AccountOwnerName);
                //var reas_Cert_Of_Home_Ownership = await _photoService.AddPhotoAsync(file_reas_Cert_Of_Home_Ownership, newRealEstate.ReasId, newRealEstate.ReasName, newRealEstate.AccountOwnerName);
                //var reas_Registration_Book = await _photoService.AddPhotoAsync(file_reas_Registration_Book, newRealEstate.ReasId, newRealEstate.ReasName, newRealEstate.AccountOwnerName);
                //var sales_Authorization_Contract = await _photoService.AddPhotoAsync(file_sales_Authorization_Contract, newRealEstate.ReasId, newRealEstate.ReasName, newRealEstate.AccountOwnerName);
                //newDetail.Documents_Proving_Marital_Relationship = documents_Proving_Marital_Relationship.SecureUrl.AbsoluteUri;
                //newDetail.Reas_Cert_Of_Land_Img_Front = reas_Cert_Of_Land_Img_Front.SecureUrl.AbsoluteUri;
                //newDetail.Reas_Cert_Of_Land_Img_After = reas_Cert_Of_Land_Img_After.SecureUrl.AbsoluteUri;
                //newDetail.Reas_Cert_Of_Home_Ownership = reas_Cert_Of_Home_Ownership.SecureUrl.AbsoluteUri;
                //newDetail.Reas_Registration_Book = reas_Registration_Book.SecureUrl.AbsoluteUri;
                //newDetail.Sales_Authorization_Contract = sales_Authorization_Contract.SecureUrl.AbsoluteUri;
                newDetail.ReasId = newRealEstate.ReasId;
                bool check = await _real_estate_detail_repository.CreateAsync(newDetail);
                if (check) return true;
                else return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<PageList<RealEstateDto>> GetOnwerRealEstate(int userMember)
        {
            var reals = await _real_estate_repository.GetOwnerRealEstate(userMember);
            return reals;
        }


        public async Task<bool> PaymentAmountToUpRealEstaeAfterApprove(TransactionMoneyCreateParam transactionMoneyCreateParam, int userMember)
        {
            ReasStatusParam reasStatusDto = new ReasStatusParam();
            reasStatusDto.reasId = transactionMoneyCreateParam.IdReas;
            reasStatusDto.reasStatus = (int)RealEstateStatus.Selling;
            reasStatusDto.messageString = "";
            bool check = await _real_estate_repository.UpdateRealEstateStatusAsync(reasStatusDto);
            if (check)
            {
                bool check_trans = await _money_transaction_repository.CreateNewMoneyTransaction(transactionMoneyCreateParam, userMember);
                if (check_trans)
                {
                    int idTransaction = await _money_transaction_repository.GetIdTransactionWhenCreateNewTransaction();
                    //bool check_trans_detail = await _moneyTransactionDetailRepository.CreateNewMoneyTransaction(transactionMoneyCreateParam, idTransaction);
                    //if (check_trans_detail) return true;
                    //else return false;
                    return true;
                }
                else return false;
            }
            else return false;
        }

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


    }
}
