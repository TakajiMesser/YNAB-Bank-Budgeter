using Budgeter.Shared.Rules;

namespace Budgeter.Shared.Transactions
{
    public interface ITransactionSet<T> where T : ITransaction
    {
        int TransactionCount { get; }

        T TransactionAt(int index);
        void Add(T transaction);
        void Sort(IRule rule, int startIndex = 0);
    }
}
