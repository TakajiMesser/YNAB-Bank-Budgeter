using Budgeter.Shared.Transactions;
using Newtonsoft.Json;
using System;

namespace Budgeter.Shared.YNAB
{
    public class YNABTransaction : Transaction
    {
        public DateTime Date { get; set; }
        public float Amount { get; set; }
        public string Memo { get; set; }

        public string Cleared { get; set; }
        public bool Approved { get; set; }
        public string FlagColor { get; set; }

        [JsonProperty("payee_name")]
        public string PayeeName { get; set; }
        public string CategoryName { get; set; }

        public override string Payee => PayeeName ?? "";
        public override float Quantity => Amount;
        public override DateTime Time => Date;
    }
}
