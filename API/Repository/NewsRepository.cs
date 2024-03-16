using API.Data;
using API.DTOs;
using API.Entity;
using API.Helper;
using API.Interface.Repository;
using API.Param;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class NewsRepository : BaseRepository<News>, INewsRepository
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        public NewsRepository(DataContext context, IMapper mapper) : base(context)
        {
            _dataContext = context;
            _mapper = mapper;
        }

        public async Task<bool> CreateNewNewsByAdmin(NewsCreate newsDto, int id, string name)
        {
            try
            {
                News news = new News();
                news.NewsTitle = newsDto.NewsTitle;
                news.NewsSumary = newsDto.NewsSumary;
                news.NewsContent = newsDto.NewsContent;
                news.Thumbnail = newsDto.ThumbnailUri;
                news.AccountCreateId = id;
                news.DateCreated = DateTime.UtcNow;
                news.AccountName = name;
                bool check = await CreateAsync(news);
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

        public async Task<PageList<NewsDto>> GetAllInNewsPage()
        {
            PaginationParams paginationParams = new PaginationParams();
            var newsList = _dataContext.News.AsQueryable();
            newsList = newsList.OrderByDescending(x => x.DateCreated);
            if(newsList != null)
            {
                return await PageList<NewsDto>.CreateAsync(
                newsList.AsNoTracking().ProjectTo<NewsDto>(_mapper.ConfigurationProvider),
                paginationParams.PageNumber,
                paginationParams.PageSize);
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<NewsAdminDto>> GetAllInNewsAdmin()
        { 
            var newsList = _dataContext.News.OrderByDescending(x => x.DateCreated).Select(x => new NewsAdminDto
            {
                NewsId = x.NewsId,
                NewsTitle = x.NewsTitle,
                NewsSumary = x.NewsSumary,
                Thumbnail = x.Thumbnail,
                DateCreated = x.DateCreated,
            });
            return await newsList.ToListAsync();
        }

        public async Task<NewsDetailDto> GetDetailOfNews(int id)
        {
            NewsDetailDto newsDetailDto = new NewsDetailDto();
            var newsDetail = await _dataContext.News.Where(x => x.NewsId == id).FirstOrDefaultAsync();
            newsDetailDto.NewsId = id;
            newsDetailDto.Thumbnail = newsDetail.Thumbnail;
            newsDetailDto.NewsTitle = newsDetail.NewsTitle;
            newsDetailDto.NewsSumary = newsDetail.NewsSumary;
            newsDetailDto.NewsContent = newsDetail.NewsContent;
            newsDetailDto.DateCreated = newsDetail.DateCreated;
            return newsDetailDto;
        }

        public async Task<PageList<NewsDto>> SearchNewByKey(SearchNewsParam searchNewsParam)
        {
            PaginationParams paginationParams = new PaginationParams();
            var newsList = _dataContext.News.AsQueryable();
            newsList = newsList.Where(x => x.NewsTitle.Contains(searchNewsParam.KeyWord) || 
            x.NewsSumary.Contains(searchNewsParam.KeyWord) ||
            x.NewsContent.Contains(searchNewsParam.KeyWord));
            newsList = newsList.OrderByDescending(x => x.DateCreated);
            if (newsList != null)
            {
                return await PageList<NewsDto>.CreateAsync(
                newsList.AsNoTracking().ProjectTo<NewsDto>(_mapper.ConfigurationProvider),
                paginationParams.PageNumber,
                paginationParams.PageSize);
            }
            else
            {
                return null;
            }
        }

        public async Task<IEnumerable<NewsAdminDto>> SearchNewsAdminByKey(SearchNewsAdminParam searchNewsParam)
        {
            var newsList = _dataContext.News.Where(x => x.NewsTitle.ToLower().Contains(searchNewsParam.KeyWord.ToLower()) ||
            x.NewsSumary.ToLower().Contains(searchNewsParam.KeyWord.ToLower()) ||
            x.NewsContent.ToLower().Contains(searchNewsParam.KeyWord.ToLower())).Select(x => new NewsAdminDto
            {
                NewsId = x.NewsId,
                NewsTitle = x.NewsTitle,
                NewsSumary = x.NewsSumary,
                Thumbnail = x.Thumbnail,
                DateCreated = x.DateCreated,
            }); ;
            newsList = newsList.OrderByDescending(x => x.DateCreated);
            return await newsList.ToListAsync();
        }

        public async Task<bool> UpdateNewsByAdmin(NewsDetailDto news)
        {
            var newsDto = await _dataContext.News.Where(x => x.NewsId == news.NewsId).FirstOrDefaultAsync();
            newsDto.NewsTitle= news.NewsTitle;
            newsDto.NewsSumary= news.NewsSumary;
            newsDto.NewsContent = news.NewsContent;
            try
            {
                bool check = await UpdateAsync(newsDto);
                if (check)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }catch (Exception ex)
            {
                return false;
            }
        }
    }
}
