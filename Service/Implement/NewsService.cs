using Repository.DTOs;
using Repository.Interface;
using Repository.Paging;
using Repository.Param;
using Service.Interface;

namespace Service.Implement
{
    public class NewsService : INewsService
    {
        private readonly INewsRepository _newsRepository;

        public NewsService(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public async Task<PageList<NewsDto>> GetAllNews()
        {
            var listNews = await _newsRepository.GetAllInNewsPage();
            return listNews;
        }

        public async Task<NewsDetailDto> GetNewsDetail(int id)
        {
            var newsDetail = await _newsRepository.GetDetailOfNews(id);
            return newsDetail;
        }

        public async Task<PageList<NewsDto>> SearchNews(SearchNewsParam searchNews)
        {
            var reals = await _newsRepository.SearchNewByKey(searchNews);
            return reals;
        }
    }
}
