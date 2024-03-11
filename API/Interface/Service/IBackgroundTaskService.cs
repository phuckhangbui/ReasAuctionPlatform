namespace API.Interface.Service
{
    public interface IBackgroundTaskService
    {
        Task ScheduleAuctionPending();
        Task ScheduleAuctionEndTime(int auctionId);
        Task SendEmailNoticeAttenders();
    }
}