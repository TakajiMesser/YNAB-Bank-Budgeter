using Budgeter.Shared.Banks;
using Budgeter.Shared.Rules;
using Budgeter.Shared.Transactions;
using Budgeter.Shared.YNAB;
using System;
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

        // Increment through bank transactions until we either 
        private Result FindMatch(IRule rule, YNABTransaction ynabTransaction, Type transactionType)
        {
            while (_indexSet.Index(transactionType) < TransactionSet.Count(transactionType))
            {
                var bankTransaction = (BankTransaction)TransactionSet.TransactionAt(transactionType, _indexSet.Index(transactionType));
                var result = GetResult(rule, ynabTransaction, bankTransaction);

                // This means that YNAB > Bank, so we need to keep incrementing...
                if (result.BankTransaction != null)
                {
                    _indexSet.Increment(transactionType);

                    if (result.YNABTransaction != null)
                    {
                        // Match found!
                        return result;
                    }
                }
                else
                {
                    return null;
                }

                ResultSet.AddResult(result);
            }

            return null;
        }

        private void ApplyRule(IRule rule)
        {
            /*foreach (var transactionType in TransactionSet.Types.Where(t => t != typeof(YNABTransaction)))
            {
                var ynabIndex = 0;
                var bankIndex = 0;

                while (ynabIndex < TransactionSet.Count(typeof(YNABTransaction)) && bankIndex < TransactionSet.Count(transactionType))
                {
                    var ynabTransaction TransactionSet.TransactionAt<YNABTransaction>(ynabIndex);
                    var bankTransaction
                }
            }*/

            while (_indexSet.Index<YNABTransaction>() < TransactionSet.Count<YNABTransaction>())
            {
                var ynabTransaction = TransactionSet.TransactionAt<YNABTransaction>(_indexSet.Index<YNABTransaction>());
                var isMatchFound = false;
                
                // TODO - Loop through the other transaction types, incrementing them forward until:
                //          - The YNAB row has been matched
                //          - The Bank row > the YNAB row
                //      - If we reach the point where ALL Bank rows > YNAB row and the YNAB row still hasn't been matched,
                //          - Then create a result with just the YNAB row
                foreach (var transactionType in TransactionSet.Types.Where(t => t != typeof(YNABTransaction)))
                {
                    var result = FindMatch(rule, ynabTransaction, transactionType);

                    if (result != null)
                    {
                        if (isMatchFound)
                        {
                            // The YNAB transaction has already been matched in another row, so remove it from this result
                            result.YNABTransaction = null;
                        }

                        ResultSet.AddResult(result);
                        isMatchFound = true;
                    }
                }

                if (!isMatchFound)
                {
                    // Because we haven't matched the YNAB row, add it as a result row and increment
                    ResultSet.AddResult(new Result(ynabTransaction, null));
                }

                _indexSet.Increment<YNABTransaction>();

                // TODO - What if NO bank transactions match this YNAB transaction?
                // In that case, we need to add a result row with just the YNAB transaction listed, and then increment the YNAB row forward
                /*var hasBankTransaction = false;
                
                foreach (var transactionType in TransactionSet.Types.Where(t => t != typeof(YNABTransaction)))
                {
                    if (_indexSet.Index(transactionType) < TransactionSet.Count(transactionType))
                    {
                        hasBankTransaction = true;

                        var bankTransaction = (BankTransaction)TransactionSet.TransactionAt(transactionType, _indexSet.Index(transactionType));
                        var result = GetResult(rule, ynabTransaction, bankTransaction);

                        if (result.YNABTransaction != null)
                        {
                            if (isYNABMatchFound)
                            {
                                // The YNAB transaction has already been matched in another row, so remove it from this result
                                result.YNABTransaction = null;
                            }
                            else
                            {
                                isYNABMatchFound = true;
                            }
                        }

                        // TODO - This is going to misorder the bank transaction rows, since we aren't comparing bank transactions against each other for each YNAB transaction iteration
                        // If the results are going to be separated by bank anyways, then this might not be an issue
                        if (result.BankTransaction != null)
                        {
                            _indexSet.Increment(transactionType);
                        }

                        ResultSet.AddResult(result);
                    }
                }

                if (!hasBankTransaction && !isYNABMatchFound)
                {
                    // Because we have no bank transactions left and haven't matched out YNAB row (which is a redundant check...), add it as a result row and increment
                    ResultSet.AddResult(new Result(ynabTransaction, null));
                    isYNABMatchFound = true;
                }

                if (isYNABMatchFound)
                {
                    _indexSet.Increment<YNABTransaction>();
                }*/
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
