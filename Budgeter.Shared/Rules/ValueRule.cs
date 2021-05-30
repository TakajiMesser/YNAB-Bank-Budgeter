using Budgeter.Shared.PTCU;
using Budgeter.Shared.YNAB;
using System;

namespace Budgeter.Shared.Rules
{
    public class ValueRule : Rule //where T : IComparable, IEquatable<T>
    {
        public string YNABColumnName { get; set; }
        public string PTCUColumnName { get; set; }

        public override int Compare(YNABTransaction ynabTransaction, PTCUTransaction ptcuTransaction)
        {
            var ynabValue = ynabTransaction.GetValue(YNABColumnName);
            var ptcuValue = ptcuTransaction.GetValue(PTCUColumnName);

            if (ynabValue == null) throw new Exception("YNAB value for " + YNABColumnName + " is not a comparable type");
            if (ptcuValue == null) throw new Exception("PTCU value for " + PTCUColumnName + " is not a comparable type");

            return ynabValue.CompareTo(ptcuValue);
        }
    }
}
