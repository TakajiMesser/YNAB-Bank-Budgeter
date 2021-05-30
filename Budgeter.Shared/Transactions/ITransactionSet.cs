using Budgeter.Shared.Rules;

namespace Budgeter.Shared
{
    public interface ITransactionSet<T> where T : ITransaction
    {
        int TransactionCount { get; }

        T TransactionAt(int index);

        void Sort(IRule rule, int startIndex = 0);
    }
}
