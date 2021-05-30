using Budgeter.Shared;
using Budgeter.Shared.Matching;
using Budgeter.Shared.PTCU;
using Budgeter.Shared.YNAB;
using System;
using System.Threading.Tasks;

namespace Budgeter.ConsoleApplication
{
    class Program
    {
        public const string CONFIG_JSON_PATH = @"D:\GitHub\YNAB-Bank-Budgeter\config.json";

        static void Main(string[] args)
        {
            Console.WriteLine("Beginning matching.");
            Console.WriteLine();

            var configuration = new Configuration(CONFIG_JSON_PATH);
            configuration.Load();

            var ynabClient = new YNABClient(configuration.YNABConfiguration);
            var ptcuClient = new PTCUClient(configuration.PTCUConfiguration);

            var ynabFetchTask = ynabClient.FetchTransactions();
            var ptcuFetchTask = ptcuClient.FetchTransactions();

            Task.WaitAll(ynabFetchTask, ptcuFetchTask);

            var matcher = new Matcher()
            {
                YNABTransactionSet = ynabClient.Transactions,
                PTCUTransactionSet = ptcuClient.Transactions,
                RuleSet = configuration.RuleSet
            };

            matcher.PerformMatching();
            matcher.ResultSet.SaveAsCSV(configuration.OutputFilePath);

            Console.WriteLine("Finished matching - " + matcher.ResultSet.Count + " rows in result.");
            Console.WriteLine("Results written to '" + configuration.OutputFilePath + "'.");
            Console.WriteLine();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
