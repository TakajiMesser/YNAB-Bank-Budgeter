using Budgeter.Shared;
using Budgeter.Shared.Matching;
using Budgeter.Shared.PTCU;
using Budgeter.Shared.YNAB;
using System.Threading.Tasks;

namespace Budgeter.WPFApplication.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        private RelayCommand _performMatchingCommand;
        private RelayCommand _clearConsoleCommand;

        public MainWindowViewModel() => Title = "Budgeter";

        public string Title { get; set; }
        public ILogger Logger { get; set; }

        public Configuration Configuration { get; set; }

        public RelayCommand PerformMatchingCommand => _performMatchingCommand ?? (_performMatchingCommand = new RelayCommand(
            async p =>
            {
                if (Configuration != null)
                {
                    Logger.WriteLine("Beginning matching.");
                    Logger.WriteLine();

                    var ynabClient = new YNABClient(Configuration.YNABConfiguration);
                    var ptcuClient = new PTCUClient(Configuration.PTCUConfiguration);

                    var ynabFetchTask = ynabClient.FetchTransactions();
                    var ptcuFetchTask = ptcuClient.FetchTransactions();

                    await Task.WhenAll(ynabFetchTask, ptcuFetchTask);

                    var matcher = new Matcher()
                    {
                        YNABTransactionSet = ynabClient.Transactions,
                        PTCUTransactionSet = ptcuClient.Transactions,
                        RuleSet = Configuration.RuleSet
                    };

                    matcher.PerformMatching();
                    matcher.ResultSet.SaveAsCSV(Configuration.OutputFilePath);

                    Logger.WriteLine("Finished matching - " + matcher.ResultSet.Count + " rows in result.");
                    Logger.WriteLine("Results written to '" + Configuration.OutputFilePath + "'.");
                }
                else
                {
                    Logger.WriteLine("No configuration found (go to Edit->Settings).");
                }
            },
            p => true
        ));

        public RelayCommand ClearConsoleCommand => _clearConsoleCommand ?? (_clearConsoleCommand = new RelayCommand(
            p => Logger.Clear(),
            p => true
        ));
    }
}