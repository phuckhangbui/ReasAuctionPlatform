using API.Data;
using API.DTOs;
using API.Entity;
using API.Helper;
using API.Interface.Repository;
using API.Param;
using API.Param.Enums;
using API.ThirdServices;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class RealEstateRepository : BaseRepository<RealEstate>, IRealEstateRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public RealEstateRepository(DataContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> UpdateRealEstateStatusAsync(ReasStatusParam reasStatusDto)
        {
            var realEstate = await _context.RealEstate.Where(r => r.ReasId == reasStatusDto.reasId).Select(x => new RealEstate
            {
                AccountOwnerName = x.AccountOwnerName,
                AccountOwnerId = x.AccountOwnerId,
                ReasName = x.ReasName,
                ReasId = x.ReasId,
                DateCreated = x.DateCreated,
                DateEnd = x.DateEnd,
                DateStart = x.DateStart,
                Message = x.Message,
                ReasAddress = x.ReasAddress,
                ReasArea = x.ReasArea,
                ReasDescription = x.ReasDescription,
                ReasPrice = Convert.ToDouble(x.ReasPrice),
                Type_Reas = x.Type_Reas,
                ReasStatus = x.ReasStatus,
            }).FirstOrDefaultAsync();
            var accountOwner = await _context.Account.Where(x => x.AccountId == realEstate.AccountOwnerId).FirstOrDefaultAsync();
            if (realEstate != null)
            {
                if (reasStatusDto.reasStatus == 3 || reasStatusDto.reasStatus == 9)
                {
                    SendMailWhenRejectRealEstate.SendEmailWhenRejectRealEstate(accountOwner.AccountEmail, accountOwner.AccountName, reasStatusDto.messageString);
                }
                else if (reasStatusDto.reasStatus == 1)
                {
                    SendMailWhenApproveRealEstate.SendEmailWhenApproveRealEstate(accountOwner.AccountEmail, accountOwner.AccountName);
                    // send notification here

                    // then in the notification, forward user to the payment page

                }
                realEstate.ReasStatus = reasStatusDto.reasStatus;
                realEstate.Message = reasStatusDto.messageString;
                try
                {
                    bool check = await UpdateAsync(realEstate);
                    if (check) return true;
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

            return false;
        }

        public async Task<bool> CheckRealEstateExist(int reasId)
        {
            return await _context.RealEstateDetail.AnyAsync(r => r.ReasId == reasId);
        }

        public async Task<IEnumerable<ManageRealEstateDto>> GetRealEstateOnGoing()
        {
            var statusName = new GetStatusName();
            var page = new PaginationParams();

            var query = _context.RealEstate.Where(a => a.ReasStatus == (int)RealEstateStatus.InProgress).Select(x => new ManageRealEstateDto
            {
                ReasId = x.ReasId,
                ReasName = x.ReasName,
                ReasPrice = Convert.ToDouble(x.ReasPrice),
                ReasArea = x.ReasArea,
                ReasTypeName = _context.type_REAS.Where(y => y.Type_ReasId == x.Type_Reas).Select(z => z.Type_Reas_Name).FirstOrDefault(),
                ReasStatus = statusName.GetRealEstateStatusName(x.ReasStatus),
                DateStart = x.DateStart,
                DateEnd = x.DateEnd,
            });
            query = query.OrderByDescending(a => a.DateStart);
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<ManageRealEstateDto>> GetAllRealEstateExceptOnGoing()
        {
            var statusName = new GetStatusName();
            var page = new PaginationParams();

            var query = _context.RealEstate.Where(a => a.ReasStatus != (int)RealEstateStatus.InProgress).Select(x => new ManageRealEstateDto
            {
                ReasId = x.ReasId,
                ReasName = x.ReasName,
                ReasPrice = Convert.ToDouble(x.ReasPrice),
                ReasArea = x.ReasArea,
                ReasTypeName = _context.type_REAS.Where(y => y.Type_ReasId == x.Type_Reas).Select(z => z.Type_Reas_Name).FirstOrDefault(),
                ReasStatus = statusName.GetRealEstateStatusName(x.ReasStatus),
                DateStart = x.DateStart,
                DateEnd = x.DateEnd,
            });
            query = query.OrderByDescending(a => a.DateStart);
            return await query.ToListAsync();
        }

        public async Task<PageList<RealEstateDto>> GetOwnerRealEstate(int idOwner)
        {
            var statusName = new GetStatusName();
            var page = new PaginationParams();
            var query = _context.RealEstate.Where(a => a.AccountOwnerId.Equals(idOwner)).Select(x => new RealEstateDto
            {
                ReasId = x.ReasId,
                ReasName = x.ReasName,
                ReasPrice = Convert.ToDouble(x.ReasPrice),
                ReasArea = x.ReasArea,
                UriPhotoFirst = _context.RealEstatePhoto.Where(y => y.ReasId == x.ReasId).Select(z => z.ReasPhotoUrl).FirstOrDefault(),
                ReasTypeName = _context.type_REAS.Where(y => y.Type_ReasId == x.Type_Reas).Select(z => z.Type_Reas_Name).FirstOrDefault(),
                ReasStatus = statusName.GetRealEstateStatusName(x.ReasStatus),
                DateStart = x.DateStart,
                DateEnd = x.DateEnd,
            });
            query = query.OrderByDescending(a => a.DateStart);
            return await PageList<RealEstateDto>.CreateAsync(
            query.AsNoTracking().ProjectTo<RealEstateDto>(_mapper.ConfigurationProvider),
            page.PageNumber,
            page.PageSize);
        }

        public async Task<PageList<RealEstateDto>> GetOwnerRealEstateBySearch(int idOwner, SearchRealEstateParam searchRealEstateDto)
        {
            var statusName = new GetStatusName();
            var page = new PaginationParams();
            var query = _context.RealEstate.Where(x => x.AccountOwnerId.Equals(idOwner) && ((searchRealEstateDto.ReasStatus == -1)
                || searchRealEstateDto.ReasStatus == x.ReasStatus) &&
                (searchRealEstateDto.ReasName == null || x.ReasName.Contains(searchRealEstateDto.ReasName)) &&
                ((searchRealEstateDto.ReasPriceFrom == 0 && searchRealEstateDto.ReasPriceTo == 0) ||
                (x.ReasPrice >= searchRealEstateDto.ReasPriceFrom &&
                x.ReasPrice <= searchRealEstateDto.ReasPriceTo)))
                .Select(x => new RealEstateDto
                {
                    ReasId = x.ReasId,
                    ReasName = x.ReasName,
                    ReasPrice = Convert.ToDouble(x.ReasPrice),
                    ReasArea = x.ReasArea,
                    UriPhotoFirst = _context.RealEstatePhoto.Where(x => x.ReasId == x.ReasId).Select(x => x.ReasPhotoUrl).FirstOrDefault(),
                    ReasTypeName = _context.type_REAS.Where(y => y.Type_ReasId == x.Type_Reas).Select(z => z.Type_Reas_Name).FirstOrDefault(),
                    ReasStatus = statusName.GetRealEstateStatusName(x.ReasStatus),
                    DateStart = x.DateStart,
                    DateEnd = x.DateEnd,
                });
            query = query.OrderByDescending(a => a.DateStart);
            return await PageList<RealEstateDto>.CreateAsync(
            query.AsNoTracking().ProjectTo<RealEstateDto>(_mapper.ConfigurationProvider),
            page.PageNumber,
            page.PageSize);
        }

        public async Task<PageList<RealEstateDto>> SearchRealEstateByKey(SearchRealEstateParam searchRealEstateDto)
        {
            var statusName = new GetStatusName();
            var page = new PaginationParams();
            var query = _context.RealEstate.Where(x =>
                ((new[] { (int)RealEstateStatus.Selling, (int)RealEstateStatus.Auctioning, (int)RealEstateStatus.ReUp }.Contains(x.ReasStatus) && searchRealEstateDto.ReasStatus == -1)
                || searchRealEstateDto.ReasStatus == x.ReasStatus) &&
                (searchRealEstateDto.ReasName == null || x.ReasName.Contains(searchRealEstateDto.ReasName)) &&
                ((searchRealEstateDto.ReasPriceFrom == 0 && searchRealEstateDto.ReasPriceTo == 0) ||
                (x.ReasPrice >= searchRealEstateDto.ReasPriceFrom &&
                x.ReasPrice <= searchRealEstateDto.ReasPriceTo)))
                .Select(x => new RealEstateDto
                {
                    ReasId = x.ReasId,
                    ReasName = x.ReasName,
                    ReasPrice = Convert.ToDouble(x.ReasPrice),
                    ReasArea = x.ReasArea,
                    UriPhotoFirst = _context.RealEstatePhoto.Where(y => y.ReasId == x.ReasId).Select(x => x.ReasPhotoUrl).FirstOrDefault(),
                    ReasTypeName = _context.type_REAS.Where(y => y.Type_ReasId == x.Type_Reas).Select(z => z.Type_Reas_Name).FirstOrDefault(),
                    ReasStatus = statusName.GetRealEstateStatusName(x.ReasStatus),
                    DateStart = x.DateStart,
                    DateEnd = x.DateEnd,
                });
            query = query.OrderByDescending(a => a.DateStart);
            return await PageList<RealEstateDto>.CreateAsync(
            query.AsNoTracking().ProjectTo<RealEstateDto>(_mapper.ConfigurationProvider),
            page.PageNumber,
            page.PageSize);
        }

        public async Task<PageList<RealEstateDto>> GetAllRealEstateOnRealEstatePage()
        {
            var statusName = new GetStatusName();
            PaginationParams page = new PaginationParams();
            var query = _context.RealEstate.Where(x => new[] { (int)RealEstateStatus.Selling, (int)RealEstateStatus.ReUp, (int)RealEstateStatus.Auctioning }.Contains(x.ReasStatus)).Select(x => new RealEstateDto
            {
                ReasId = x.ReasId,
                ReasName = x.ReasName,
                ReasPrice = Convert.ToDouble(x.ReasPrice),
                ReasArea = x.ReasArea,
                UriPhotoFirst = _context.RealEstatePhoto.Where(y => y.ReasId == x.ReasId).Select(z => z.ReasPhotoUrl).FirstOrDefault(),
                ReasTypeName = _context.type_REAS.Where(y => y.Type_ReasId == x.Type_Reas).Select(z => z.Type_Reas_Name).FirstOrDefault(),
                ReasStatus = statusName.GetRealEstateStatusName(x.ReasStatus),
                DateStart = x.DateStart,
                DateEnd = x.DateEnd,
            }).AsQueryable();
            query = query.OrderByDescending(a => a.DateStart);
            return await PageList<RealEstateDto>.CreateAsync(query.AsNoTracking().ProjectTo<RealEstateDto>(_mapper.ConfigurationProvider),
            page.PageNumber,
            page.PageSize);
        }

        public async Task<IEnumerable<ManageRealEstateDto>> GetRealEstateOnGoingBySearch(SearchRealEsateAdminParam searchRealEstateDto)
        {
            var statusName = new GetStatusName();
            var page = new PaginationParams();

            var query = _context.RealEstate.OrderByDescending(a => a.DateStart).Where(x => ((x.ReasStatus == (int)RealEstateStatus.InProgress && searchRealEstateDto.reasStatus.Contains(x.ReasStatus))
                || searchRealEstateDto.reasStatus == null) &&
                (searchRealEstateDto.reasName == null || x.ReasName.Contains(searchRealEstateDto.reasName)) &&
                ((searchRealEstateDto.reasPriceFrom == 0 && searchRealEstateDto.reasPriceTo == 0) ||
                (x.ReasPrice >= searchRealEstateDto.reasPriceFrom &&
                x.ReasPrice <= searchRealEstateDto.reasPriceTo)))
                .Select(x => new ManageRealEstateDto
                {
                    ReasId = x.ReasId,
                    ReasName = x.ReasName,
                    ReasPrice = Convert.ToDouble(x.ReasPrice),
                    ReasArea = x.ReasArea,
                    ReasTypeName = _context.type_REAS.Where(y => y.Type_ReasId == x.Type_Reas).Select(z => z.Type_Reas_Name).FirstOrDefault(),
                    ReasStatus = statusName.GetRealEstateStatusName(x.ReasStatus),
                    DateStart = x.DateStart,
                    DateEnd = x.DateEnd,
                });
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<ManageRealEstateDto>> GetAllRealEstateExceptOnGoingBySearch(SearchRealEsateAdminParam searchRealEstateDto)
        {
            var statusName = new GetStatusName();

            var query = _context.RealEstate.OrderByDescending(a => a.DateStart).Where(x => ((x.ReasStatus != (int)RealEstateStatus.InProgress && searchRealEstateDto.reasStatus.Contains(x.ReasStatus))
                || searchRealEstateDto.reasStatus == null) &&
                (searchRealEstateDto.reasName == null || x.ReasName.Contains(searchRealEstateDto.reasName)) &&
                ((searchRealEstateDto.reasPriceFrom == 0 && searchRealEstateDto.reasPriceTo == 0) ||
                (x.ReasPrice >= searchRealEstateDto.reasPriceFrom &&
                x.ReasPrice <= searchRealEstateDto.reasPriceTo)))
                .Select(x => new ManageRealEstateDto
                {
                    ReasId = x.ReasId,
                    ReasName = x.ReasName,
                    ReasPrice = Convert.ToDouble(x.ReasPrice),
                    ReasArea = x.ReasArea,
                    ReasTypeName = _context.type_REAS.Where(y => y.Type_ReasId == x.Type_Reas).Select(z => z.Type_Reas_Name).FirstOrDefault(),
                    ReasStatus = statusName.GetRealEstateStatusName(x.ReasStatus),
                    DateStart = x.DateStart,
                    DateEnd = x.DateEnd,
                });
            return await query.ToListAsync();
        }

        public RealEstate GetRealEstate(int id)
        {
            return _context.RealEstate.Find(id);
        }
    }
}
