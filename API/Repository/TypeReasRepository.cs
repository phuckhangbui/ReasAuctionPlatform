using API.Data;
using API.Entity;
using API.Interface.Repository;

namespace API.Repository
{
    public class TypeReasRepository : BaseRepository<Type_REAS>, ITypeReasRepository
    {
        public TypeReasRepository(DataContext context) : base(context)
        {
        }
    }
}
