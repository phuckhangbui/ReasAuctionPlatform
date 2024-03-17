using AutoMapper;
using AutoMapper.QueryableExtensions;
using BusinessObject.Entity;
using BusinessObject.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Repository.Data;
using Repository.DTOs;
using Repository.Interface;
using Repository.Paging;
using Repository.Param;

namespace Repository.Implement
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

        public async Task<Account> UpdateRealEstateStatusAsync(ReasStatusParam reasStatusDto)
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
                realEstate.ReasStatus = reasStatusDto.reasStatus;
                realEstate.Message = reasStatusDto.messageString;
                try
                {
                    bool check = await UpdateAsync(realEstate);
                    if (check) return accountOwner;
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    return null;
                }
            }

            return null;
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
                Flag = x.IsReupYet,
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
            var query = _context.RealEstate.Where(x => x.AccountOwnerId.Equals(idOwner) && (searchRealEstateDto.ReasStatus == -1
                || searchRealEstateDto.ReasStatus == x.ReasStatus) &&
                (searchRealEstateDto.ReasName == null || x.ReasName.Contains(searchRealEstateDto.ReasName)) &&
                (searchRealEstateDto.ReasPriceFrom == 0 && searchRealEstateDto.ReasPriceTo == 0 ||
                x.ReasPrice >= searchRealEstateDto.ReasPriceFrom &&
                x.ReasPrice <= searchRealEstateDto.ReasPriceTo))
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
                (new[] { (int)RealEstateStatus.Selling, (int)RealEstateStatus.Auctioning, (int)RealEstateStatus.WaitingAuction}.Contains(x.ReasStatus) && searchRealEstateDto.ReasStatus == -1
                || searchRealEstateDto.ReasStatus == x.ReasStatus) &&
                (searchRealEstateDto.ReasName == null || x.ReasName.Contains(searchRealEstateDto.ReasName)) &&
                (searchRealEstateDto.ReasPriceFrom == 0 && searchRealEstateDto.ReasPriceTo == 0 ||
                x.ReasPrice >= searchRealEstateDto.ReasPriceFrom &&
                x.ReasPrice <= searchRealEstateDto.ReasPriceTo))
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
            var query = _context.RealEstate.Where(x => new[] { (int)RealEstateStatus.Selling, (int)RealEstateStatus.Auctioning }.Contains(x.ReasStatus)).Select(x => new RealEstateDto
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

            var query = _context.RealEstate.OrderByDescending(a => a.DateStart).Where(x => (x.ReasStatus == (int)RealEstateStatus.InProgress && searchRealEstateDto.reasStatus.Contains(x.ReasStatus)
                || searchRealEstateDto.reasStatus == null) &&
                (searchRealEstateDto.reasName == null || x.ReasName.Contains(searchRealEstateDto.reasName)) &&
                (searchRealEstateDto.reasPriceFrom == 0 && searchRealEstateDto.reasPriceTo == 0 ||
                x.ReasPrice >= searchRealEstateDto.reasPriceFrom &&
                x.ReasPrice <= searchRealEstateDto.reasPriceTo))
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

            var query = _context.RealEstate.OrderByDescending(a => a.DateStart).Where(x => (x.ReasStatus != (int)RealEstateStatus.InProgress && searchRealEstateDto.reasStatus.Contains(x.ReasStatus)
                || searchRealEstateDto.reasStatus == null) &&
                (searchRealEstateDto.reasName == null || x.ReasName.Contains(searchRealEstateDto.reasName)) &&
                (searchRealEstateDto.reasPriceFrom == 0 && searchRealEstateDto.reasPriceTo == 0 ||
                x.ReasPrice >= searchRealEstateDto.reasPriceFrom &&
                x.ReasPrice <= searchRealEstateDto.reasPriceTo))
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

        public async Task<string> GetRealEstateName(int id)
        {
            var reasName = _context.RealEstate.Where(x => x.ReasId == id).Select(y => y.ReasName).FirstOrDefault();
            return reasName;
        }

        public async Task<RealEstate> CreateRealEstateAsync(NewRealEstateParam newRealEstateParam, int userMember)
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
            newRealEstate.DateCreated = DateTime.Now;
            newRealEstate.Type_Reas = newRealEstateParam.Type_Reas;
            newRealEstate.DateStart = DateTime.Now;
            newRealEstate.DateEnd = newRealEstateParam.DateEnd;
            newRealEstate.ReasStatus = (int)RealEstateStatus.InProgress;
            try
            {
                newRealEstate.AccountOwnerName = await _context.Account.Where(x => x.AccountId == userMember).Select(x => x.AccountName).FirstOrDefaultAsync();
                await CreateAsync(newRealEstate);
                foreach (PhotoFileDto photos in newRealEstateParam.Photos)
                {
                    newPhotoList.ReasPhotoId = 0;
                    newPhotoList.ReasId = newRealEstate.ReasId;
                    newPhotoList.ReasPhotoUrl = photos.ReasPhotoUrl;
                    _context.Set<RealEstatePhoto>().Add(newPhotoList);
                    await _context.SaveChangesAsync();
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
                    _context.Set<RealEstateDetail>().Add(newDetail);
                    await _context.SaveChangesAsync();
                    return newRealEstate;
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
    }
}
