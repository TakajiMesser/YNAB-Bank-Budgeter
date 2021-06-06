using Budgeter.Shared.Banks;
using Budgeter.Shared.YNAB;

namespace Budgeter.Shared.Rules
{
    public enum RuleOrder
    {
        Ascending,
        Descending
    }

    public interface IRule
    {
        RuleOrder Order { get; }

        /// <returns>Less than zero means that the YNAB transaction is less than the Bank transaction.<br/>
        /// Greater than zero means that the YNAB transaction is greater than the Bank transaction.<br/>
        /// Zero means that the YNAB transaction is equal to the Bank transaction.</returns>
        int Compare(YNABTransaction ynabTransaction, BankTransaction bankTransaction);
    }
}
