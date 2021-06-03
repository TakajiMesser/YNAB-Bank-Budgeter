using Budgeter.Shared.CSV;
using Budgeter.Shared.PTCU;
using Budgeter.Shared.Transactions;
using Budgeter.Shared.YNAB;
using System.Collections.Generic;
using System.Reflection;

namespace Budgeter.Shared.Matching
{
    public class ResultSet
    {
        private List<Result> _results = new List<Result>();

        public int Count => _results.Count;

        public void AddResult(Result result) => _results.Add(result);

        public Result ResultAt(int index) => _results[index];

        public void Clear() => _results.Clear();

        public void MarkResult(int ynabIndex, int ptcuIndex) { }

        public void SaveAsCSV(string filePath)
        {
            var csvFile = new CSVFile(filePath);

            var ynabProperties = typeof(YNABTransaction).GetProperties();
            var ptcuProperties = typeof(PTCUTransaction).GetProperties();

            foreach (var property in ynabProperties)
            {
                csvFile.Headers.Add("YNAB" + property.Name);
            }

            foreach (var property in ptcuProperties)
            {
                csvFile.Headers.Add("PTCU" + property.Name);
            }

            foreach (var result in _results)
            {
                var csvRow = new CSVRow();

                foreach (var property in ynabProperties)
                {
                    var value = GetValue(result.YNABTransaction, property);
                    csvRow.Values.Add(value);
                }

                foreach (var property in ptcuProperties)
                {
                    var value = GetValue(result.PTCUTransaction, property);
                    csvRow.Values.Add(value);
                }

                csvFile.Rows.Add(csvRow);
            }

            csvFile.Save();
        }

        private string GetValue(ITransaction transaction, PropertyInfo propertyInfo)
        {
            if (transaction != null)
            {
                var value = propertyInfo.GetValue(transaction);

                if (value != null)
                {
                    return value.ToString();
                }
            }

            return "";
        }

        /*public class YNABTransaction : Transaction
        {
            public DateTime Date { get; set; }
            public float Amount { get; set; }
            public string Memo { get; set; }

            public string Cleared { get; set; }
            public bool Approved { get; set; }
            public string FlagColor { get; set; }

            public string PayeeName { get; set; }
            public string CategoryName { get; set; }

            public override string GetName() => PayeeName ?? "";
            public override float GetAmount() => Amount;
            public override DateTime GetDate() => Date;
        }*/

        /*public class PTCUTransaction : Transaction
        {
            [CSVHeader(Header = "Post Date")]
            public DateTime PostDate { get; set; }

            public string Description { get; set; }

            public float Debit { get; set; }

            public float Credit { get; set; }

            public override string GetName() => Description ?? "";
            public override float GetAmount() => Credit - Debit;
            public override DateTime GetDate() => PostDate;
        }*/
    }
}
