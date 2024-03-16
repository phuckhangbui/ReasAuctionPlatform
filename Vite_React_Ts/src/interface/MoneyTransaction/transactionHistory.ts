interface transactionHistory{
    transactionId: number;
    transactionStatus: number;
    transactionNo?: string | null;
    money: number;
    dateExecution: Date;
    transactionType?: string | null;
}