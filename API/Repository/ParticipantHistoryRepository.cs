using API.Data;
using API.DTOs;
using API.Entity;
using API.Interface.Repository;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Repository
{
    public class ParticipantHistoryRepository : BaseRepository<ParticipateAuctionHistory>, IParticipantHistoryRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ParticipantHistoryRepository(DataContext context, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ParticipateAuctionFinalDto>> GetAllParticipates(int auctionId)
        {
            var participate = _context.ParticipateAuctionHistories.OrderByDescending(x => x.LastBid).Where(x => x.AuctionAccountingId ==
            (_context.AuctionsAccounting.Where(y => y.AuctionId == auctionId).Select(z => z.AuctionAccountingId).FirstOrDefault())).Select(x => new ParticipateAuctionFinalDto
            {
                idAccount = x.AccountBidId,
                accountName = _context.Account.Where(a => a.AccountId == x.AccountBidId).Select(b => b.AccountName).FirstOrDefault(),
                accountEmail = _context.Account.Where(a => a.AccountId == x.AccountBidId).Select(b => b.AccountEmail).FirstOrDefault(),
                accountPhone = _context.Account.Where(a => a.AccountId == x.AccountBidId).Select(b => b.PhoneNumber).FirstOrDefault(),
                lastBid = Convert.ToDouble(x.LastBid),
            });
            return participate;
        }

        public async Task<List<AuctionAttenderDto>> GetLosingAttendees(int reasId)
        {
            var nonWinningAttendees = await _context.ParticipateAuctionHistories
                .Join(
                    _context.AuctionsAccounting,
                    pah => pah.AuctionAccountingId,
                    aa => aa.AuctionAccountingId,
                    (pah, aa) => new { ParticipateAuctionHistory = pah, AuctionsAccounting = aa }
                )
            .Join(
                _context.Account,
                joinResult => joinResult.ParticipateAuctionHistory.AccountBidId,
                account => account.AccountId,
                (joinResult, account) => new { JoinResult = joinResult, Account = account }
            )
            .Where(joinResult => joinResult.JoinResult.AuctionsAccounting.AccountWinId != joinResult.JoinResult.ParticipateAuctionHistory.AccountBidId &&
                                  joinResult.JoinResult.AuctionsAccounting.ReasId == reasId)
            .Select(joinResult => joinResult.Account)
            .ToListAsync();

            return _mapper.Map<List<AuctionAttenderDto>>(nonWinningAttendees); ;
        }
    }
}
