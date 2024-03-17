interface depositRegister {
  depositAmountDto: {
    depositId: number;
    ruleId: number;
    accountSignId: number;
    reasId: number;
    amount: number;
    depositDate: Date;
    createDepositDate: Date;
    status: number;
    displayStatus: number;
  };
  paymentUrl: string;
}
