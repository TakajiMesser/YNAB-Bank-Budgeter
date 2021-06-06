using Budgeter.Shared.Banks;
using Budgeter.Shared.YNAB;

namespace Budgeter.Shared.Rules
{
    public abstract class Rule : IRule
    {
        public RuleOrder Order { get; set; }

        public abstract int Compare(YNABTransaction ynabTransaction, BankTransaction bankTransaction);
    }
}
