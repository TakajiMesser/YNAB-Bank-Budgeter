using Budgeter.Shared;
using Budgeter.Shared.Matching;
using Budgeter.Shared.PTCU;
using Budgeter.Shared.YNAB;
using System.Collections.ObjectModel;
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

        public ObservableCollection<Result> Results { get; set; } = new();

        public RelayCommand PerformMatchingCommand => _performMatchingCommand ??= new RelayCommand(
            async p =>
            {
                if (Configuration != null)
                {
                    Logger.Message("Beginning matching.");
                    Logger.LineBreak();

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

                    Logger.Message("Finished matching - " + matcher.ResultSet.Count + " rows in result.");
                    Logger.Message("Results written to '" + Configuration.OutputFilePath + "'.");

                    Results.Clear();

                    for (var i = 0; i < matcher.ResultSet.Count; i++)
                    {
                        Results.Add(matcher.ResultSet.ResultAt(i));
                    }
                }
                else
                {
                    Logger.Warning("No configuration found (go to Edit->Settings).");
                }
            },
            p => true
        );

        public RelayCommand ClearConsoleCommand => _clearConsoleCommand ??= new RelayCommand(
            p => Logger.Clear(),
            p => true
        );

        public void DoShit()
        {
            /*<GridView x:Name="YNABGrid">
                <GridViewColumn Header="Amount" Width="120" DisplayMemberBinding="{Binding YNABTransaction.Amount}" />
            </GridView>*/
        }
    }
}