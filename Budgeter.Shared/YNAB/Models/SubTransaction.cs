namespace Budgeter.Shared.YNAB.Models
{
    public class SubTransaction : Model
    {
        public string TransactionID { get; set; }
        public int Amount { get; set; }
        public string Memo { get; set; }
        public string PayeeID { get; set; }
        public string PayeeName { get; set; }
        public string CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string TransferAccountID { get; set; }
        public string TransferTransactionID { get; set; }
        public bool Deleted { get; set; }
    }
}
