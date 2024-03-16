using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class AuctionDetailDto
    {
        [Required]
        public int AuctionId { get; set; }

        [Required]
        public int AccountWinId { get; set; }

        [Required]
        public double WinAmount { get; set; }
    }

    public class ParticipantAuctionHistoryDto
    {
        [Required]
        public int AccountId { get; set; }
        [Required]
        public double LastBidAmount { get; set; }
    }

    public class AuctionSuccessDto
    {
        [Required]
        public AuctionDetailDto AuctionDetailDto { get; set; }

        [Required]
        public List<ParticipantAuctionHistoryDto> AuctionHistory { get; set; }
    }
}
