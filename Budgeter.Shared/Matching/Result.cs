using Budgeter.Shared.Banks;
using Budgeter.Shared.YNAB;

namespace Budgeter.Shared.Matching
{
    public class Result
    {
        public Result(YNABTransaction ynabTransaction, BankTransaction bankTransaction)
        {
            YNABTransaction = ynabTransaction;
            BankTransaction = bankTransaction;
        }

        public YNABTransaction YNABTransaction { get; set; }
        public BankTransaction BankTransaction { get; set; }

        public bool IsMatch => YNABTransaction != null && BankTransaction != null;
    }
}
