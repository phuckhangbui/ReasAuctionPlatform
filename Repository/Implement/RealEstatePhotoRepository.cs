using BusinessObject.Entity;
using Repository.Data;
using Repository.Interface;

namespace Repository.Implement
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
