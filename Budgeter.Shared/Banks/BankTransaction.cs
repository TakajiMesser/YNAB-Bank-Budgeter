using Budgeter.Shared.CSV;
using Budgeter.Shared.Transactions;

namespace Budgeter.Shared.Banks
{
    public abstract class BankTransaction : Transaction
    {
        [CSVIgnore]
        public abstract string BankName { get; }
    }
}
