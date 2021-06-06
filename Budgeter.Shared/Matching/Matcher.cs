using Budgeter.Shared.Banks;
using Budgeter.Shared.Rules;
using Budgeter.Shared.Transactions;
using Budgeter.Shared.YNAB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Budgeter.Shared.Matching
{
    public class Matcher
    {
        private IndexSet _indexSet = new();

        public ITransactionSet TransactionSet { get; } = new TransactionSet();
        public RuleSet RuleSet { get; set; }
        public ResultSet ResultSet { get; } = new ResultSet();

        public void PerformMatching()
        {
            _indexSet.Clear();
            ResultSet.Clear();

            _indexSet.Initialize(TransactionSet.Types);

            // TODO - For now, support only the first rule
            var rule = RuleSet.Rules.First();

            TransactionSet.Sort(rule, 0);
            ApplyRule(rule);

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

        private void ApplyRule(IRule rule)
        {
            while (_indexSet.Index<YNABTransaction>() < TransactionSet.Count<YNABTransaction>())
            {
                var ynabTransaction = TransactionSet.TransactionAt<YNABTransaction>(_indexSet.Index<YNABTransaction>());
                var needsYNABIncrement = false;

                foreach (var transactionType in TransactionSet.Types.Where(t => t != typeof(YNABTransaction)))
                {
                    if (_indexSet.Index(transactionType) < TransactionSet.Count(transactionType))
                    {
                        var bankTransaction = (BankTransaction)TransactionSet.TransactionAt(transactionType, _indexSet.Index(transactionType));
                        var result = GetResult(rule, ynabTransaction, bankTransaction);

                        if (result.YNABTransaction != null)
                        {
                            needsYNABIncrement = true;
                        }

                        if (result.BankTransaction != null)
                        {
                            _indexSet.Increment(transactionType);
                        }

                        // TODO - If this result includes the YNAB row, we don't want to add it now (YNAB row should only exist once for all bank type loop)
                        ResultSet.AddResult(result);
                    }
                }

                if (needsYNABIncrement)
                {
                    _indexSet.Increment<YNABTransaction>();
                }
            }
        }

        private static Result GetResult(IRule rule, YNABTransaction ynabTransaction, BankTransaction bankTransaction)
        {
            var comparisonResult = rule.Compare(ynabTransaction, bankTransaction);

            if (comparisonResult > 0) // YNAB > Bank
            {
                if (rule.Order == RuleOrder.Ascending)
                {
                    return new Result(null, bankTransaction);
                }
                else if (rule.Order == RuleOrder.Descending)
                {
                    return new Result(ynabTransaction, null);
                }
            }
            else if (comparisonResult < 0) // YNAB < Bank
            {
                if (rule.Order == RuleOrder.Ascending)
                {
                    return new Result(ynabTransaction, null);
                }
                else if (rule.Order == RuleOrder.Descending)
                {
                    return new Result(null, bankTransaction);
                }
            }
            else // YNAB == Bank
            {
                return new Result(ynabTransaction, bankTransaction);
            }

            throw new Exception("Something went wrong");
        }
    }
}
