namespace API.Param.Enums
{
    public enum NotificationTypeEnum
    {
        NewRealEstateCreate = 1,
        NewRealEstateApproved = 2,
        NewRealEstateRejected = 3,
        NewAuctionCreate = 4,
        AuctionAboutToStart = 5,
        AuctionFinishSuccess = 6,
        AuctionFinishLose = 7,
        AuctionFinishNotAttended = 8,
        AuctionFinishAdminAndStaff = 9
    }
}
