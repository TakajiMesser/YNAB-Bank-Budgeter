using Budgeter.Shared.Transactions;
using System.Threading.Tasks;

namespace Budgeter.Shared
{
    public abstract class Client<T> where T : ITransaction
    {
        //protected IConfiguration _configuration;

        //public Client(IConfiguration configuration) => _configuration = configuration;

        public TransactionSet<T> Transactions { get; } = new TransactionSet<T>();

        public abstract Task FetchTransactions();
    }
}
