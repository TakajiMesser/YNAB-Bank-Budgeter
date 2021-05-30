using Budgeter.Shared.PTCU;
using Budgeter.Shared.YNAB;

namespace Budgeter.Shared.Rules
{
    public class NameRule : Rule
    {
        public override int Compare(YNABTransaction ynabTransaction, PTCUTransaction ptcuTransaction) => ynabTransaction.GetName().CompareTo(ptcuTransaction.GetName());
    }
}
