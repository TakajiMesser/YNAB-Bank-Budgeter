using Budgeter.Shared.Rules;
using System;
using System.Collections.Generic;

namespace Budgeter.Shared.Transactions
{
    public class TransactionSet<T> : ITransactionSet<T> where T : ITransaction
    {
        private List<T> _transactions = new List<T>();

        public int TransactionCount => _transactions.Count;

        public T TransactionAt(int index) => _transactions[index];

        public void Add(T transaction) => _transactions.Add(transaction);

        public void Sort(IRule rule, int startIndex = 0) => _transactions.Sort(startIndex, _transactions.Count - startIndex, new TransactionComparer<T>(rule));

        private class TransactionComparer<U> : IComparer<U> where U : ITransaction
        {
            private IRule _rule;

            public TransactionComparer(IRule rule) => _rule = rule;

            public int Compare(U x, U y)
            {
                switch (_rule)
                {
                    case NameRule _:
                        return _rule.Order == RuleOrder.Descending
                            ? y.GetName().CompareTo(x.GetName())
                            : x.GetName().CompareTo(y.GetName());
                    case AmountRule _:
                        return _rule.Order == RuleOrder.Descending
                            ? y.GetAmount().CompareTo(x.GetAmount())
                            : x.GetAmount().CompareTo(y.GetAmount());
                    case DateRule _:
                        return _rule.Order == RuleOrder.Descending
                            ? y.GetDate().CompareTo(x.GetDate())
                            : x.GetDate().CompareTo(y.GetDate());
                }

                throw new ArgumentException("Could not handle rule type " + _rule.GetType());
            }
        }
    }
}
