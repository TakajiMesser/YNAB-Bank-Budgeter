using Budgeter.Shared.CSV;
using System.Threading.Tasks;

namespace Budgeter.Shared.PTCU
{
    public class PTCUClient : Client<PTCUTransaction>
    {
        private PTCUConfiguration _configuration;

        public PTCUClient(PTCUConfiguration configuration) => _configuration = configuration;

        public async override Task FetchTransactions()
        {
            var csvFile = new CSVFile(_configuration.CSVFilePath);
            await csvFile.Load();

            foreach (var transaction in csvFile.Deserialize<PTCUTransaction>())
            {
                Transactions.Add(transaction);
            }

            /*//var accountNumberIndex = csvFile.IndexOf("Account Number");
            var postDateIndex = csvFile.IndexOf("Post Date");
            //var checkIndex = csvFile.IndexOf("Check");
            var descriptionIndex = csvFile.IndexOf("Description");
            var debitIndex = csvFile.IndexOf("Debit");
            var creditIndex = csvFile.IndexOf("Credit");
            //var statusIndex = csvFile.IndexOf("Status");
            //var balanceIndex = csvFile.IndexOf("Balance");

            foreach (var row in csvFile.Rows)
            {
                var transaction = new PTCUTransaction
                {
                    PostDate = DateTime.Parse(row.Values[postDateIndex]),
                    Description = row.Values[descriptionIndex],
                    Debit = float.Parse(row.Values[debitIndex]),
                    Credit = float.Parse(row.Values[creditIndex])
                };

                _transactions.Add(transaction);
            }*/
        }
    }
}
