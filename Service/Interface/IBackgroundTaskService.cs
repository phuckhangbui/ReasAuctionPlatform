namespace Service.Interface
{
    public interface IBackgroundTaskService
    {
        Task ScheduleAuction();
        Task ScheduleGetAuctionResultFromFirebase(int auctionId);
    }
}