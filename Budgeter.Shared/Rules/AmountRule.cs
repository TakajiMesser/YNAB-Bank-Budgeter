using Budgeter.Shared.PTCU;
using Budgeter.Shared.YNAB;

namespace Budgeter.Shared.Rules
{
    public class AmountRule : Rule
    {
        public override int Compare(YNABTransaction ynabTransaction, PTCUTransaction ptcuTransaction) => ynabTransaction.GetAmount().CompareTo(ptcuTransaction.GetAmount());
    }
}
