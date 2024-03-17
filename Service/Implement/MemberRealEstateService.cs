using BusinessObject.Entity;
using BusinessObject.Enum;
using Repository.DTOs;
using Repository.Interface;
using Repository.Paging;
using Repository.Param;
using Service.Interface;

namespace Service.Implement
{
    public class MemberRealEstateService : IMemberRealEstateService
    {
        private readonly IRealEstateRepository _real_estate_repository;
        private readonly IAccountRepository _account_repository;
        private readonly IRealEstateDetailRepository _real_estate_detail_repository;
        private readonly ITypeReasRepository _typeReasRepository;
        private readonly IRealEstateService _real_estate_service;

        public MemberRealEstateService(IRealEstateRepository real_estate_repository, IAccountRepository account_repository, IRealEstateDetailRepository real_estate_detail_repository, ITypeReasRepository typeReasRepository, IRealEstateService real_estate_service)
        {
            _real_estate_repository = real_estate_repository;
            _account_repository = account_repository;
            _real_estate_detail_repository = real_estate_detail_repository;
            _typeReasRepository = typeReasRepository;
            _real_estate_service = real_estate_service;
        }

        public IAccountRepository AccountRepository => _account_repository;

        public async Task<RealEstate> CreateNewRealEstate(NewRealEstateParam newRealEstateParam, int userMember)
        {
            var createReal = await _real_estate_repository.CreateRealEstateAsync(newRealEstateParam, userMember);
            if (createReal != null)
            {
                if (newRealEstateParam.OldReasId != 0)
                {
                    await _real_estate_service.UpdateRealEstateIsReupYet(newRealEstateParam.OldReasId, true);
                }



                return createReal;
            }
            else return null;
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
            var _real_estate_detail = await _real_estate_detail_repository.GetRealEstateMemberDetail(id);
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
            Account check = await _real_estate_repository.UpdateRealEstateStatusAsync(realEstateStatus);
            if (check != null)
            {
                return true;
            }

            else return false;
        }

        public async Task<bool> ReupRealEstate(RealEstate realEstate, DateTime dateEnd)
        {
            realEstate.DateStart = DateTime.Now;
            realEstate.DateEnd = dateEnd;
            realEstate.ReasStatus = (int)RealEstateStatus.Selling;

            return await _real_estate_repository.UpdateAsync(realEstate);
        }


    }
}
