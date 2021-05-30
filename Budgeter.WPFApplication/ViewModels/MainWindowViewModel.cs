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

        public MainWindowViewModel() => Title = "Budgeter";

        public string Title { get; set; }

        public Configuration Configuration { get; set; }

        public RelayCommand PerformMatchingCommand => _performMatchingCommand ?? (_performMatchingCommand = new RelayCommand(
            async p =>
            {
                if (Configuration != null)
                {
                    //Console.WriteLine("Beginning matching.");
                    //Console.WriteLine();

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

                    //Console.WriteLine("Finished matching - " + matcher.ResultSet.Count + " rows in result.");
                    //Console.WriteLine("Results written to '" + Configuration.OutputFilePath + "'.");
                }
            },
            p => true
        ));
    }
}