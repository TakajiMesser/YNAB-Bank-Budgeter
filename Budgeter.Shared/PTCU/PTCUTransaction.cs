using Budgeter.Shared.CSV;
using System;

namespace Budgeter.Shared.PTCU
{
    public class PTCUTransaction : Transaction
    {
        [CSVHeader(Header = "Post Date")]
        public DateTime PostDate { get; set; }

        public string Description { get; set; }

        public float Debit { get; set; }

        public float Credit { get; set; }

        public override string GetName() => Description ?? "";
        public override float GetAmount() => Credit - Debit;
        public override DateTime GetDate() => PostDate;
    }
}
