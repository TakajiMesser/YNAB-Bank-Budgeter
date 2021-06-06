using Budgeter.Shared.Rules;
using System;
using System.Collections.Generic;

namespace Budgeter.Shared.Transactions
{
    public class TransactionSet : ITransactionSet
    {
        private Dictionary<Type, List<ITransaction>> _transactionsByType = new();

        public IEnumerable<Type> Types => _transactionsByType.Keys;

        public void Add(ITransaction transaction)
        {
            var type = transaction.GetType();

            if (!_transactionsByType.ContainsKey(type))
            {
                _transactionsByType.Add(type, new List<ITransaction>());
            }

            _transactionsByType[type].Add(transaction);
        }

        public void AddRange(IEnumerable<ITransaction> transactions)
        {
            foreach (var transaction in transactions)
            {
                Add(transaction);
            }
        }

        public void Sort(IRule rule, int startIndex = 0)
        {
            foreach (var transactions in _transactionsByType.Values)
            {
                transactions.Sort(startIndex, transactions.Count - startIndex, new TransactionComparer(rule));
            }
        }

        public int Count(Type type) => _transactionsByType[type].Count;
        public int Count<T>() where T : ITransaction => _transactionsByType[typeof(T)].Count;

        public ITransaction TransactionAt(Type type, int index) => _transactionsByType[type][index];
        public T TransactionAt<T>(int index) where T : ITransaction => (T)_transactionsByType[typeof(T)][index];

        private class TransactionComparer : IComparer<ITransaction>
        {
            private IRule _rule;

            public TransactionComparer(IRule rule) => _rule = rule;

            public int Compare(ITransaction x, ITransaction y)
            {
                switch (_rule)
                {
                    case PayeeRule _:
                        return _rule.Order == RuleOrder.Descending
                            ? y.Payee.CompareTo(x.Payee)
                            : x.Payee.CompareTo(y.Payee);
                    case QuantityRule _:
                        return _rule.Order == RuleOrder.Descending
                            ? y.Quantity.CompareTo(x.Quantity)
                            : x.Quantity.CompareTo(y.Quantity);
                    case TimeRule _:
                        return _rule.Order == RuleOrder.Descending
                            ? y.Time.CompareTo(x.Time)
                            : x.Time.CompareTo(y.Time);
                }

                throw new ArgumentException("Could not handle rule type " + _rule.GetType());
            }
        }
    }
}
