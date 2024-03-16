
using BusinessObject.Entity;

namespace Repository.DTOs
{
    public class AuctionCreationResult
    {
        public int AuctionId { get; set; } 
        public IEnumerable<NameUserDto> Users { get; set; }
        public string ReasName { get; set; }
    }
}
