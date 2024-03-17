interface transactionDetail {
  accountSendId: number;
  accountSendBankingCode: string;
  accountSendBankingNumber: string;
  accountSendName: string;
  accountReceiveId: number;
  accountReceiveBankingCode: string;
  accountReceiveBankingNumber: string;
  accountReceiveName: string;
  reasId: number;
  reasName: string;
  depositId: number;
  txnRef: string;
  transactionId: number;
  transactionStatus: number;
  transactionNo: string;
  money: number;
  dateExecution: Date;
  transactionType: string;
}
