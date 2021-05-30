using System;

namespace Budgeter.Shared
{
    public abstract class Transaction : ITransaction
    {
        public abstract string GetName();
        public abstract float GetAmount();
        public abstract DateTime GetDate();

        public IComparable GetValue(ValueTypes valueType)
        {
            switch (valueType)
            {
                case ValueTypes.Name:
                    return GetName();
                case ValueTypes.Amount:
                    return GetAmount();
                case ValueTypes.Date:
                    return GetDate();
            }

            throw new NotImplementedException("Could not handle value type " + valueType);
        }

        public IComparable GetValue(string propertyName) => GetType().GetProperty(propertyName).GetValue(this, null) as IComparable;
    }
}
