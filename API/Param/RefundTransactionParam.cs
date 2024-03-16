using API.Entity;

namespace API.Param
{
    public class RefundTransactionParam
    {
        public int accountReceiveId { get; set; }
        public int reasId { get; set; }
        public double money { get; set; }
        public int depositId { get; set; }
    }
}
