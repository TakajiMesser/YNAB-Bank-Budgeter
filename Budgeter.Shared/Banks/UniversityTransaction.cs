using Budgeter.Shared.CSV;
using System;

namespace Budgeter.Shared.Banks
{
    public class UniversityTransaction : BankTransaction
    {
        public DateTime Date { get; set; }
        public string Memo { get; set; }

        [CSVHeader(Header = "Amount Debit")]
        public float Debit { get; set; }

        [CSVHeader(Header = "Amount Credit")]
        public float Credit { get; set; }

        [CSVIgnore]
        public override string BankName => "University Credit Union";
        public override string Payee => Memo ?? "";
        public override float Quantity => Credit - Debit;
        public override DateTime Time => Date;
    }
}
