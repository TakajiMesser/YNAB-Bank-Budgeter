using Budgeter.Shared.PTCU;
using Budgeter.Shared.YNAB;

namespace Budgeter.Shared.Rules
{
    public abstract class Rule : IRule
    {
        public RuleTypes RuleType { get; set; }
        public RuleOrder Order { get; set; }

        public abstract int Compare(YNABTransaction ynabTransaction, PTCUTransaction ptcuTransaction);
    }
}
