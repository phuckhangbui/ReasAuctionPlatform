namespace API.Interface.Service
{
    public interface IBackgroundTaskService
    {
        Task ScheduleAuctionStatus();
        Task ScheduleAuctionEndTime(int auctionId);
    }
}