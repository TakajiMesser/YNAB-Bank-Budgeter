using Budgeter.Shared.CSV;
using Newtonsoft.Json;
using System;

namespace Budgeter.Shared.Transactions
{
    public abstract class Transaction : ITransaction
    {
        [CSVIgnore]
        [JsonIgnore]
        public abstract string Payee { get; }

        [CSVIgnore]
        [JsonIgnore]
        public abstract float Quantity { get; }

        [CSVIgnore]
        [JsonIgnore]
        public abstract DateTime Time { get; }

        public IComparable GetValue(string propertyName) => GetType().GetProperty(propertyName).GetValue(this, null) as IComparable;
    }
}
