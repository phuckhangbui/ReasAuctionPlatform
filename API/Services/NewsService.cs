using API.DTOs;
using API.Helper;
using API.Interface.Repository;
using API.Interface.Service;
using API.Param;

namespace API.Services
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
