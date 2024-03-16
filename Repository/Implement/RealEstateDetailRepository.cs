using BusinessObject.Entity;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.DTOs;
using Repository.Interface;

namespace Repository.Implement
{
    public class RealEstateDetailRepository : BaseRepository<RealEstateDetail>, IRealEstateDetailRepository
    {
        private readonly DataContext _context;

        public RealEstateDetailRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public async Task<RealEstateDetailDto> GetRealEstateDetail(int id)
        {
            var _real_estate_detail = await _context.RealEstate.Where(x => x.ReasId == id).Select(x => new RealEstateDetailDto
            {
                ReasId = x.ReasId,
                ReasName = x.ReasName,
                ReasPrice = Convert.ToDouble(x.ReasPrice),
                ReasArea = x.ReasArea,
                ReasAddress = x.ReasAddress,
                ReasDescription = x.ReasDescription,
                AccountOwnerId = x.AccountOwnerId,
                AccountOwnerName = x.AccountOwnerName,
                Photos = _context.RealEstatePhoto.Where(z => z.ReasId == x.ReasId).Select(z => new RealEstatePhotoDto
                {
                    ReasPhotoId = z.ReasPhotoId,
                    ReasPhotoUrl = z.ReasPhotoUrl,
                }).ToList(),
                Type_REAS_Name = _context.type_REAS.Where(y => y.Type_ReasId == x.Type_Reas).Select(y => y.Type_Reas_Name).FirstOrDefault(),
                ReasStatus = x.ReasStatus,
                DateStart = x.DateStart,
                DateEnd = x.DateEnd,
                DateCreated = x.DateCreated,
            }).FirstOrDefaultAsync();
            return _real_estate_detail;
        }

        public async Task<RealEstateDetailDto> GetRealEstateDetailByAdminOrStaff(int id)
        {
            var _real_estate_detail = await _context.RealEstate.Where(x => x.ReasId == id).Select(x => new RealEstateDetailDto
            {
                ReasId = x.ReasId,
                ReasName = x.ReasName,
                ReasPrice = Convert.ToDouble(x.ReasPrice),
                ReasArea = x.ReasArea,
                ReasAddress = x.ReasAddress,
                ReasDescription = x.ReasDescription,
                AccountOwnerId = x.AccountOwnerId,
                AccountOwnerName = x.AccountOwnerName,
                Photos = _context.RealEstatePhoto.Where(z => z.ReasId == x.ReasId).Select(z => new RealEstatePhotoDto
                {
                    ReasPhotoId = z.ReasPhotoId,
                    ReasPhotoUrl = z.ReasPhotoUrl,
                }).ToList(),
                Type_REAS_Name = _context.type_REAS.Where(y => y.Type_ReasId == x.Type_Reas).Select(y => y.Type_Reas_Name).FirstOrDefault(),
                ReasStatus = x.ReasStatus,
                DateStart = x.DateStart,
                DateEnd = x.DateEnd,
                DateCreated = x.DateCreated,
                Detail = _context.RealEstateDetail.Where(z => z.ReasId == x.ReasId).Select(z => new RealEstatePaper
                {
                    Documents_Proving_Marital_Relationship = z.Documents_Proving_Marital_Relationship,
                    Reas_Cert_Of_Home_Ownership = z.Reas_Cert_Of_Home_Ownership,
                    Reas_Cert_Of_Land_Img_After = z.Reas_Cert_Of_Land_Img_Front,
                    Reas_Cert_Of_Land_Img_Front = z.Reas_Cert_Of_Land_Img_Front,
                    Reas_Registration_Book = z.Reas_Registration_Book,
                    Sales_Authorization_Contract = z.Sales_Authorization_Contract,
                }).FirstOrDefault(),
            }).FirstOrDefaultAsync();
            return _real_estate_detail;
        }

        public async Task<RealEstateDetailDto> GetRealEstateMemberDetail(int id)
        {
            var _real_estate_detail = await _context.RealEstate.Where(x => x.ReasId == id).Select(x => new RealEstateDetailDto
            {
                ReasId = x.ReasId,
                ReasName = x.ReasName,
                ReasPrice = Convert.ToDouble(x.ReasPrice),
                ReasArea = x.ReasArea,
                ReasAddress = x.ReasAddress,
                ReasDescription = x.ReasDescription,
                AccountOwnerId = x.AccountOwnerId,
                AccountOwnerName = x.AccountOwnerName,
                Photos = _context.RealEstatePhoto.Where(z => z.ReasId == x.ReasId).Select(z => new RealEstatePhotoDto
                {
                    ReasPhotoId = z.ReasPhotoId,
                    ReasPhotoUrl = z.ReasPhotoUrl,
                }).ToList(),
                Type_REAS_Name = _context.type_REAS.Where(y => y.Type_ReasId == x.Type_Reas).Select(y => y.Type_Reas_Name).FirstOrDefault(),
                ReasStatus = x.ReasStatus,
                DateStart = x.DateStart,
                DateEnd = x.DateEnd,
                DateCreated = x.DateCreated,
                Detail = _context.RealEstateDetail.Where(z => z.ReasId == x.ReasId).Select(z => new RealEstatePaper
                {
                    Documents_Proving_Marital_Relationship = z.Documents_Proving_Marital_Relationship,
                    Reas_Cert_Of_Home_Ownership = z.Reas_Cert_Of_Home_Ownership,
                    Reas_Cert_Of_Land_Img_After = z.Reas_Cert_Of_Land_Img_Front,
                    Reas_Cert_Of_Land_Img_Front = z.Reas_Cert_Of_Land_Img_Front,
                    Reas_Registration_Book = z.Reas_Registration_Book,
                    Sales_Authorization_Contract = z.Sales_Authorization_Contract,
                }).FirstOrDefault(),
            }).FirstOrDefaultAsync();
            return _real_estate_detail;
        }
    }
}
