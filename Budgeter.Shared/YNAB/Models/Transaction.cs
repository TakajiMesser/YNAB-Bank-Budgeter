using Newtonsoft.Json;
using System.Collections.Generic;

namespace Budgeter.Shared.YNAB.Models
{
    public class Transaction : Model
    {
        public string Date { get; set; }
        public int Amount { get; set; }
        public string Memo { get; set; }
        public string Cleared { get; set; }
        public bool Approved { get; set; }
        public string FlagColor { get; set; }
        public string AccountID { get; set; }
        public string PayeeID { get; set; }
        public string CategoryID { get; set; }
        public string TransferAccountID { get; set; }
        public string MatchedTransactionID { get; set; }
        public string ImportID { get; set; }
        public bool Deleted { get; set; }
        public string AccountName { get; set; }

        [JsonProperty("payee_name")]
        public string PayeeName { get; set; }
        public string CategoryName { get; set; }
        public List<SubTransaction> Subtransactions { get; set; }
    }
}
