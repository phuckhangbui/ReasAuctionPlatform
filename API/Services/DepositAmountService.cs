using API.DTOs;
using API.Entity;
using API.Exceptions;
using API.Helper;
using API.Interface.Repository;
using API.Interface.Service;
using API.Param;
using API.Param.Enums;
using AutoMapper;

namespace API.Services
{
    public class DepositAmountService : IDepositAmountService
    {
        private readonly IDepositAmountRepository _depositAmountRepository;
        private readonly IRealEstateRepository _realEstateRepository;
        private readonly IMapper _mapper;

        public DepositAmountService(IDepositAmountRepository depositAmountRepository, IRealEstateRepository realEstateRepository, IMapper mapper)
        {
            _depositAmountRepository = depositAmountRepository;
            _realEstateRepository = realEstateRepository;
            _mapper = mapper;
        }

        readonly float DEPOSIT_PERCENT = 0.02f;

        public async Task<PageList<DepositDto>> GetDepositAmounts(DepositAmountParam depositAmountParam)
        {
            return await _depositAmountRepository.GetDepositAmountsAsync(depositAmountParam);
        }

        public async Task<DepositAmountDto> CreateDepositAmount(int customerId, int reasId)
        {
            DepositAmountDto depositAmountDto = new DepositAmountDto();
            RealEstate realEstate = _realEstateRepository.GetRealEstate(reasId);

            if (realEstate.ReasStatus != (int)RealEstateStatus.Selling)
            {
                return null;
            }


            List<DepositAmount> depositAmountList = _depositAmountRepository.GetDepositAmounts(customerId, reasId);
            if (depositAmountList.Count != 0)
            {
                await _depositAmountRepository.DeleteAsync(depositAmountList);
            }

            DepositAmount depositAmount = new DepositAmount();

            depositAmount = new DepositAmount();
            depositAmount.RuleId = 1; //fix later if needed
            depositAmount.AccountSignId = customerId;
            depositAmount.ReasId = reasId;
            depositAmount.Amount = ((Int64)(realEstate.ReasPrice * DEPOSIT_PERCENT));
            depositAmount.Status = (int)UserDepositEnum.Pending;
            depositAmount.CreateDepositDate = DateTime.Now;


            await _depositAmountRepository.CreateAsync(depositAmount);

            depositAmountDto = _mapper.Map<DepositAmount, DepositAmountDto>(depositAmount);

            return depositAmountDto;
        }

        public async Task<DepositAmountDto> UpdateStatusToDeposited(int depositId, DateTime paymentTime)
        {
            DepositAmountDto depositAmountDto = new DepositAmountDto();
            DepositAmount depositAmount = _depositAmountRepository.GetDepositAmount(depositId);
            if (depositAmount == null)
            {
                return null;
            }

            depositAmount.Status = (int)UserDepositEnum.Deposited;
            depositAmount.DepositDate = paymentTime;

            await _depositAmountRepository.UpdateAsync(depositAmount);

            depositAmountDto = _mapper.Map<DepositAmount, DepositAmountDto>(depositAmount);

            return depositAmountDto;
        }

        public async Task<DepositAmountDto> UpdateStatus(int accountId, int reasId, int status)
        {
            DepositAmountDto depositAmountDto = new DepositAmountDto();
            DepositAmount depositAmount = _depositAmountRepository.GetDepositAmount(accountId, reasId);
            if (depositAmount == null)
            {
                throw new ArgumentException();
            }

            depositAmount.Status = status;

            await _depositAmountRepository.UpdateAsync(depositAmount);

            depositAmountDto = _mapper.Map<DepositAmount, DepositAmountDto>(depositAmount);

            return depositAmountDto;
        }

        public DepositAmountDto GetDepositAmount(int customerId, int reasId)
        {
            var depositAmount = _depositAmountRepository.GetDepositAmount(customerId, reasId);
            var depositAmountDto = _mapper.Map<DepositAmount, DepositAmountDto>(depositAmount);
            return depositAmountDto;
        }

        public DepositAmount GetDepositAmount(int depositId)
        {
            return _depositAmountRepository.GetDepositAmount(depositId);
        }

        public DepositDetailDto GetDepositDetail(int depositId)
        {
            var depositDetail = _depositAmountRepository.GetDepositDetailAsync(depositId);

            if (depositDetail == null)
            {
                throw new BaseNotFoundException($"Depisit detail with ID {depositId} not found.");
            }

            return depositDetail;
        }

        public async Task<PageList<AccountDepositedDto>> GetAccountsHadDeposited(PaginationParams paginationParams, int reasId)
        {
            var realEstate = _realEstateRepository.GetRealEstate(reasId);

            if (realEstate == null)
            {
                throw new BaseNotFoundException($"Real estate with ID {reasId} not found.");
            }

            return await _depositAmountRepository.GetAccountsHadDeposited(paginationParams, reasId);
        }


    }
}
