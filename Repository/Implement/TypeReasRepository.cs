using BusinessObject.Entity;
using Repository.Data;
using Repository.Interface;

namespace Repository.Implement
{
    public class TypeReasRepository : BaseRepository<Type_REAS>, ITypeReasRepository
    {
        public TypeReasRepository(DataContext context) : base(context)
        {
        }
    }
}
