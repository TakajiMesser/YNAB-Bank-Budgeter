using Budgeter.Shared.Rules;
using System;
using System.Collections.Generic;

namespace Budgeter.Shared.Transactions
{
    public interface ITransactionSet
    {
        IEnumerable<Type> Types { get; }

        void Add(ITransaction transaction);
        void AddRange(IEnumerable<ITransaction> transactions);

        void Sort(IRule rule, int startIndex = 0);

        int Count(Type type);
        int Count<T>() where T : ITransaction;

        ITransaction TransactionAt(Type type, int index);
        T TransactionAt<T>(int index) where T : ITransaction;
    }
}
