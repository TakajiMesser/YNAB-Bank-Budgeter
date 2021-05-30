using Budgeter.Shared.CSV;

namespace Budgeter.Shared.PTCU
{
    public class PTCUClient : TransactionSet<PTCUTransaction>
    {
        public void AddFromCSV(string filePath)
        {
            var csvFile = new CSVFile(filePath);
            csvFile.Load();

            AddFromCSV(csvFile);
        }

        public void AddFromCSV(CSVFile csvFile)
        {
            _transactions.AddRange(csvFile.Deserialize<PTCUTransaction>());

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
