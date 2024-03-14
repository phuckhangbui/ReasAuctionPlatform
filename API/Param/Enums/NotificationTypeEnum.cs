namespace API.Param.Enums
{
    public enum NotificationTypeEnum
    {
        NewRealEstateCreate = 1,
        NewRealEstateApproved = 2,
        NewAuctionCreate = 3,
        AuctionAboutToStart = 4,
        AuctionFinishSuccess = 5,
        AuctionFinishLose = 6,
        AuctionFinishNotAttended = 7,
        AuctionFinishAdminAndStaff = 8
    }
}
