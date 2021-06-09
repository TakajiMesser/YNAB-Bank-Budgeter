using Budgeter.Shared.CSV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Budgeter.Shared.Banks
{
    public class BankClient : Client<BankTransaction>
    {
        private List<BankConfiguration> _configurations = new();

        public BankClient(IEnumerable<BankConfiguration> configurations) => _configurations.AddRange(configurations);

        public override async Task FetchTransactions() => await Task.WhenAll(_configurations.Select(c => FetchTransactions(c)));

        private async Task FetchTransactions(BankConfiguration configuration)
        {
            var csvFile = new CSVFile(configuration.CSVFilePath);
            await csvFile.Load();

            for (var i = 0; i < csvFile.Rows.Count; i++)
            {
                var transaction = CreateTransaction(configuration.BankName);
                transaction.AccountName = configuration.AccountName;

                csvFile.DeserializeInto(transaction, i);
                _transactions.Add(transaction);
            }
        }

        /*var csvFile = new CSVFile(_configuration.CSVFilePath);
        await csvFile.Load();

        foreach (var transaction in csvFile.Deserialize<PTCUTransaction>())
        {
            Transactions.Add(transaction);
        }*/

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

        private static BankTransaction CreateTransaction(string bankName) => bankName switch
        {
            "PTCU" => new PTCUTransaction(),
            "University Credit Union" => new UniversityTransaction(),
            _ => throw new NotImplementedException("Could not handle bank " + bankName),
        };
    }
}
