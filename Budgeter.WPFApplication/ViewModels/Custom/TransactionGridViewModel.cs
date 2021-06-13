using Budgeter.Shared;
using Budgeter.Shared.Banks;
using Budgeter.Shared.Matching;
using Budgeter.Shared.Transactions;
using Budgeter.Shared.YNAB;
using Budgeter.WPFApplication.Views.Custom;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Budgeter.WPFApplication.ViewModels.Custom
{
    public class TransactionGridViewModel : ViewModel
    {
        public ObservableCollection<Transaction> Transactions { get; set; } = new();
        public ObservableCollection<TransactionGrid> SyncWith { get; set; } = new();

        public void DoShit()
        {
            /*<GridView x:Name="YNABGrid">
                <GridViewColumn Header="Amount" Width="120" DisplayMemberBinding="{Binding YNABTransaction.Amount}" />
            </GridView>*/
        }
    }
}