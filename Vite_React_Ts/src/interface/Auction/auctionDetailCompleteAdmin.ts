interface AuctionDetailCompleteAdmin{
    auctionId: number;
    reasId: number;
    reasName: string;//
    floorBid: number;
    status: string;
    dateStart: Date;
    dateEnd: Date;
    accountCreateId : number;
    accountOwnerId : number;
    accountWinnerId : number;
    accountCreateName : string;//
    accountOwnerName : string;//
    accountOwnerEmail : string;//
    accountOwnerPhone : string;//
    accountWinnerName : string;//
    accountWinnerEmail : string;//
    accountWinnerPhone : string;//
    finalAmount : number;
    depositAmout : number;
    commisionAmount : number;
    ownerReceiveAmount : number;
}