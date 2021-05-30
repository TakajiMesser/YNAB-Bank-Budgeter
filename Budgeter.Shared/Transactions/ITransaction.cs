using System;

namespace Budgeter.Shared.Transactions
{
    public enum ValueTypes
    {
        Name,
        Amount,
        Date
    }

    public interface ITransaction
    {
        string GetName();
        float GetAmount();
        DateTime GetDate();

        IComparable GetValue(ValueTypes valueType);
        IComparable GetValue(string propertyName);
    }
}
