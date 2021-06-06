using Budgeter.Shared.CSV;
using System;

namespace Budgeter.Shared.Banks
{
    public class PTCUTransaction : BankTransaction
    {
        [CSVHeader(Header = "Post Date")]
        public DateTime PostDate { get; set; }

        public string Description { get; set; }

        public float Debit { get; set; }

        public float Credit { get; set; }

        [CSVIgnore]
        public override string BankName => "PTCU";
        public override string Payee => Description ?? "";
        public override float Quantity => Credit - Debit;
        public override DateTime Time => PostDate;
    }
}
