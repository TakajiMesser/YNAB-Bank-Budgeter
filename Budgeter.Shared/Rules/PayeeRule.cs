using Budgeter.Shared.Banks;
using Budgeter.Shared.YNAB;

namespace Budgeter.Shared.Rules
{
    public class PayeeRule : Rule
    {
        public override int Compare(YNABTransaction ynabTransaction, BankTransaction bankTransaction) => ynabTransaction.Payee.CompareTo(bankTransaction.Payee);
    }
}
