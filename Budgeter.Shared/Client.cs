using Budgeter.Shared.Transactions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Budgeter.Shared
{
    public abstract class Client<T> where T : ITransaction
    {
        //protected IConfiguration _configuration;

        //public Client(IConfiguration configuration) => _configuration = configuration;

        protected List<T> _transactions = new();

        public IEnumerable<T> Transactions => _transactions;

        public abstract Task FetchTransactions();
    }
}
