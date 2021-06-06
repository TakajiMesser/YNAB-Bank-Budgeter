using Budgeter.Shared.Banks;
using Budgeter.Shared.YNAB;

namespace Budgeter.Shared.Rules
{
    public class TimeRule : Rule
    {
        public override int Compare(YNABTransaction ynabTransaction, BankTransaction bankTransaction) => ynabTransaction.Time.CompareTo(bankTransaction.Time);
    }
}
