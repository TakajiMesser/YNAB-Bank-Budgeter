using Budgeter.Shared.PTCU;
using Budgeter.Shared.YNAB;

namespace Budgeter.Shared.Rules
{
    public class DateRule : Rule
    {
        public override int Compare(YNABTransaction ynabTransaction, PTCUTransaction ptcuTransaction) => ynabTransaction.GetDate().CompareTo(ptcuTransaction.GetDate());
    }
}
