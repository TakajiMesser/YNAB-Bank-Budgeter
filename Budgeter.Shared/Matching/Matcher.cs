using Budgeter.Shared.PTCU;
using Budgeter.Shared.Rules;
using Budgeter.Shared.Transactions;
using Budgeter.Shared.YNAB;
using System;
using System.Linq;

namespace Budgeter.Shared.Matching
{
    public class Matcher
    {
        public ITransactionSet<YNABTransaction> YNABTransactionSet { get; set; }
        public ITransactionSet<PTCUTransaction> PTCUTransactionSet { get; set; }

        public RuleSet RuleSet { get; set; }
        public ResultSet ResultSet { get; } = new ResultSet();

        public void PerformMatching()
        {
            ResultSet.Clear();

            // TODO - For now, support only the first rule
            var rule = RuleSet.Rules.First();

            YNABTransactionSet.Sort(rule, 0);
            PTCUTransactionSet.Sort(rule, 0);

            ApplyRule(rule, 0, 0);

            /*var ruleIndex = 0;
            var ynabIndex = 0;
            var ptcuIndex = 0;

            while (ynabIndex < YNABTransactionSet.TransactionCount && ptcuIndex < PTCUTransactionSet.TransactionCount)
            {
                var rule = RuleSet.Rules[ruleIndex];

                if (rule != null)
                {
                    YNABTransactionSet.Sort(rule, ynabIndex);
                    PTCUTransactionSet.Sort(rule, ptcuIndex);
                }

                ApplyRule(rule, ynabIndex, ptcuIndex);
            }*/
        }

        private void ApplyRule(IRule rule, int ynabIndex, int ptcuIndex)
        {
            while (ynabIndex < YNABTransactionSet.TransactionCount && ptcuIndex < PTCUTransactionSet.TransactionCount)
            {
                var ynabTransaction = YNABTransactionSet.TransactionAt(ynabIndex);
                var ptcuTransaction = PTCUTransactionSet.TransactionAt(ptcuIndex);

                var result = GetResult(rule, ynabTransaction, ptcuTransaction);

                if (result.YNABTransaction != null)
                {
                    ynabIndex++;
                }

                if (result.PTCUTransaction != null)
                {
                    ptcuIndex++;
                }

                ResultSet.AddResult(result);
            }
        }

        private Result GetResult(IRule rule, YNABTransaction ynabTransaction, PTCUTransaction ptcuTransaction)
        {
            var comparisonResult = rule.Compare(ynabTransaction, ptcuTransaction);

            if (comparisonResult > 0) // YNAB > PTCU
            {
                if (rule.Order == RuleOrder.Ascending)
                {
                    return new Result(null, ptcuTransaction);
                }
                else if (rule.Order == RuleOrder.Descending)
                {
                    return new Result(ynabTransaction, null);
                }
            }
            else if (comparisonResult < 0) // YNAB < PTCU
            {
                if (rule.Order == RuleOrder.Ascending)
                {
                    return new Result(ynabTransaction, null);
                }
                else if (rule.Order == RuleOrder.Descending)
                {
                    return new Result(null, ptcuTransaction);
                }
            }
            else // YNAB == PTCU
            {
                return new Result(ynabTransaction, ptcuTransaction);
            }

            throw new Exception("Something went wrong");
        }
    }
}
