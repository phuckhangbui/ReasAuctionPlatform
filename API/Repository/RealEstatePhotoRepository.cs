using API.Data;
using API.Entity;
using API.Interface.Repository;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class RealEstatePhotoRepository : BaseRepository<RealEstatePhoto>, IRealEstatePhotoRepository
    {
        private readonly DataContext _dataContext;
        public RealEstatePhotoRepository(DataContext context) : base(context)
        {
            _dataContext = context;
        }
    }
}
