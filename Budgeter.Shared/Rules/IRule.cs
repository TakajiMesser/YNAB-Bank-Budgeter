using Budgeter.Shared.PTCU;
using Budgeter.Shared.YNAB;

namespace Budgeter.Shared.Rules
{
    public enum RuleTypes
    {
        Equality,
        Comparison
    }

    public enum RuleOrder
    {
        Ascending,
        Descending
    }

    public interface IRule
    {
        RuleTypes RuleType { get; }
        RuleOrder Order { get; }

        /// <returns>Less than zero means that the YNAB transaction is less than the PTCU transaction.<br/>
        /// Greater than zero means that the YNAB transaction is greater than the PTCU transaction.<br/>
        /// Zero means that the YNAB transaction is equal to the PTCU transaction.</returns>
        int Compare(YNABTransaction ynabTransaction, PTCUTransaction ptcuTransaction);
    }
}
