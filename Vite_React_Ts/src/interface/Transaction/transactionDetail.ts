interface transactionDetail {
  accountSendId: number;
  sendBankCode: string;
  sendBankAccount: string;
  accountSendName: string;
  accountReceiveId: number;
  receivedBankCode: string;
  receiveBankAccount: string;
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
