using API.Helper;

namespace API.Param
{
    public class TaskParam : PaginationParams
    {
        public string? AccountCreateName { get; set; }
        public string? AccountAssignedName { get; set; }
        public int Status { get; set; } = -1;
    }
}
