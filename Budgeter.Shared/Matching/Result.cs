using Budgeter.Shared.PTCU;
using Budgeter.Shared.YNAB;

namespace Budgeter.Shared.Matching
{
    public class Result
    {
        public Result(YNABTransaction ynabTransaction, PTCUTransaction ptcuTransaction)
        {
            YNABTransaction = ynabTransaction;
            PTCUTransaction = ptcuTransaction;
        }

        public YNABTransaction YNABTransaction { get; set; }
        public PTCUTransaction PTCUTransaction { get; set; }

        public bool IsMatch => YNABTransaction != null && PTCUTransaction != null;
    }
}
