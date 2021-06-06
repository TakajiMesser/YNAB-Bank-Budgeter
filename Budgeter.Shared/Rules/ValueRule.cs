using Budgeter.Shared.Banks;
using Budgeter.Shared.YNAB;
using System;

namespace Budgeter.Shared.Rules
{
    public class ValueRule : Rule //where T : IComparable, IEquatable<T>
    {
        public string YNABColumnName { get; set; }
        public string BankColumnName { get; set; }

        public override int Compare(YNABTransaction ynabTransaction, BankTransaction bankTransaction)
        {
            var ynabValue = ynabTransaction.GetValue(YNABColumnName);
            var ptcuValue = bankTransaction.GetValue(BankColumnName);

            if (ynabValue == null) throw new InvalidOperationException("YNAB value for " + YNABColumnName + " is not a comparable type");
            if (ptcuValue == null) throw new InvalidOperationException("Bank value for " + BankColumnName + " is not a comparable type");

            return ynabValue.CompareTo(ptcuValue);
        }
    }
}
