using Budgeter.Shared.Banks;
using Budgeter.Shared.YNAB;

namespace Budgeter.Shared.Rules
{
    public class QuantityRule : Rule
    {
        public override int Compare(YNABTransaction ynabTransaction, BankTransaction bankTransaction) => ynabTransaction.Quantity.CompareTo(bankTransaction.Quantity);
    }
}
