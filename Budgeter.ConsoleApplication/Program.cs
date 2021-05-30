using Budgeter.Shared.Matching;
using Budgeter.Shared.PTCU;
using Budgeter.Shared.Settings.Configuration;
using Budgeter.Shared.YNAB;
using Newtonsoft.Json;
using System;
using System.IO;

namespace Budgeter.ConsoleApplication
{
    class Program
    {
        public const string CONFIG_JSON_PATH = @"C:\Users\takaj\source\repos\Budgeter\config.json";

        static void Main(string[] args)
        {
            var configuration = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(CONFIG_JSON_PATH));

            var ynabClient = new YNABClient(configuration.YNABCredentials);
            ynabClient.FetchTransactions();

            var ptcuClient = new PTCUClient();
            ptcuClient.AddFromCSV(configuration.PTCUFilePath);

            var matcher = new Matcher()
            {
                YNABTransactionSet = ynabClient,
                PTCUTransactionSet = ptcuClient,
                RuleSet = configuration.RuleSet
            };

            matcher.PerformMatching();
            matcher.ResultSet.SaveAsCSV(configuration.OutputFilePath);

            Console.WriteLine("Finished matching.");
            Console.WriteLine(matcher.ResultSet.Count + " rows in result.");
            Console.WriteLine("Results written to '" + configuration.OutputFilePath + "'.");

            Console.ReadKey();
        }
    }
}
