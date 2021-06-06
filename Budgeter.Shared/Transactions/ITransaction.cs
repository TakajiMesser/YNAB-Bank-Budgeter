using System;

namespace Budgeter.Shared.Transactions
{
    public interface ITransaction
    {
        string Payee { get; }
        float Quantity { get; }
        DateTime Time { get; }

        IComparable GetValue(string propertyName);
    }
}
