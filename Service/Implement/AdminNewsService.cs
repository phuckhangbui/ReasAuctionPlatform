using Repository.DTOs;
using Repository.Interface;
using Service.Interface;

namespace Service.Implement
{
    public class AdminNewsService : IAdminNewsService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly INewsRepository _newsRepository;
        private readonly IPhotoService _photoService;

        public AdminNewsService(IAccountRepository accountRepository, INewsRepository newsRepository, IPhotoService photoService)
        {
            _accountRepository = accountRepository;
            _newsRepository = newsRepository;
            _photoService = photoService;
        }

        public IAccountRepository AccountRepository => _accountRepository;

        public async Task<bool> AddNewNews(NewsCreate newCreate, int idAdmin)
        {
            //ConvertStringToFile convertStringToFile = new ConvertStringToFile();
            //IFormFile formFile = convertStringToFile.ConvertToIFormFile(newCreate.ThumbnailUri);
            //var result = await _photoService.AddPhotoNewsAsync(formFile, newCreate.NewsTitle);
            //if (result.Error != null)
            //{
            //    return false;
            //}

            //newCreate.ThumbnailUri = result.SecureUrl.AbsoluteUri;
            string name = await _accountRepository.GetNameAccountByAccountIdAsync(idAdmin);
            bool news = await _newsRepository.CreateNewNewsByAdmin(newCreate, idAdmin, name);
            if (news)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<IEnumerable<NewsAdminDto>> GetAllNewsByAdmin()
        {
            var listNews = await _newsRepository.GetAllInNewsAdmin();
            return listNews;
        }

        public async Task<NewsDetailDto> GetNewsDetailByAdmin(int id)
        {
            var newsDetail = await _newsRepository.GetDetailOfNews(id);
            return newsDetail;
        }

        public async Task<IEnumerable<NewsAdminDto>> SearchNewsByAdmin(SearchNewsAdminParam searchNews)
        {
            var reals = await _newsRepository.SearchNewsAdminByKey(searchNews);
            return reals;
        }

        public async Task<bool> UpdateNewNews(NewsDetailDto newsDetailDto)
        {
            //ConvertStringToFile convertStringToFile = new ConvertStringToFile();
            //IFormFile formFile = convertStringToFile.ConvertToIFormFile(newsDetailDto.Thumbnail);
            //var result = await _photoService.AddPhotoNewsAsync(formFile, newsDetailDto.NewsTitle);
            //if (result.Error != null)
            //{
            //    return false;
            //}

            //newsDetailDto.Thumbnail = result.SecureUrl.AbsoluteUri;
            bool news = await _newsRepository.UpdateNewsByAdmin(newsDetailDto);
            if (news)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
