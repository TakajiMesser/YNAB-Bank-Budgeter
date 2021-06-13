using Budgeter.WPFApplication.Helpers;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;

namespace Budgeter.WPFApplication.Views.Custom
{
    /// <summary>
    /// Interaction logic for TransactionGrid.xaml
    /// </summary>
    public partial class TransactionGrid : UserControl
    {
        private GridViewColumnHeader _currentSortColumn;
        private SortAdorner _sortAdorner;

        public TransactionGrid()
        {
            InitializeComponent();
            ViewModel.Transactions.CollectionChanged += Transactions_CollectionChanged;
            ViewModel.SyncWith.CollectionChanged += SyncWith_CollectionChanged;
        }

        public static readonly DependencyProperty LeadingColumnProperty = DependencyProperty.Register("LeadingColumn", typeof(string), typeof(TransactionGrid), new FrameworkPropertyMetadata(OnLeadingColumnPropertyChanged));

        public string LeadingColumn
        {
            get => (string)GetValue(LeadingColumnProperty);
            set => SetValue(LeadingColumnProperty, value);
        }

        private static void OnLeadingColumnPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            var control = (TransactionGrid)source;
            var newValue = (string)e.NewValue;

            if (!string.IsNullOrEmpty((string)e.OldValue))
            {
                control.Grid.Columns.RemoveAt(0);
            }

            if (!string.IsNullOrEmpty(newValue))
            {
                // TODO - This is mad janky
                var newName = newValue;
                if (newValue.EndsWith("Name"))
                {
                    newName = newValue.Substring(0, newValue.IndexOf("Name"));
                }

                control.Grid.Columns.Insert(0, new GridViewColumn
                {
                    Header = newName,
                    DisplayMemberBinding = new Binding(newValue)
                });
            }
        }

        private void Transactions_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            foreach (var item in List.Items)
            {

            }

            //Grid.Columns.Add();
            //ViewModel.Transactions;
        }

        /*foreach (var column in GetGridColumns<YNABTransaction>())
        {
            YNABGrid.Columns.Add(column);
        }*/

        /*foreach (var column in GetGridColumns<PTCUTransaction>())
        {
            BankGrid.Columns.Add(column);
        }*/

        /*private static IEnumerable<GridViewColumn> GetGridColumns<T>()
        {
            foreach (var property in typeof(T).GetProperties())
            {
                yield return new GridViewColumn
                {
                    Header = property.Name,
                    DisplayMemberBinding = new Binding(typeof(T).Name + "." + property.Name)
                };
            }
        }*/

        private void SyncWith_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            
        }

        private void ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            foreach (var grid in ViewModel.SyncWith)
            {
                ViewHelper.GetChild<ScrollViewer>(grid).ScrollToVerticalOffset(e.VerticalOffset);
            }
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var item in e.AddedItems)
            {
                var index = List.Items.IndexOf(item);

                foreach (var grid in ViewModel.SyncWith)
                {
                    var match = grid.List.Items.GetItemAt(index);
                    grid.List.SelectedItems.Add(match);
                }
            }

            foreach (var item in e.RemovedItems)
            {
                var index = List.Items.IndexOf(item);

                foreach (var grid in ViewModel.SyncWith)
                {
                    var match = grid.List.Items.GetItemAt(index);
                    grid.List.SelectedItems.Remove(match);
                }
            }
        }

        private void GridColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            if (_currentSortColumn != null)
            {
                AdornerLayer.GetAdornerLayer(_currentSortColumn).Remove(_sortAdorner);
                List.Items.SortDescriptions.Clear();
            }

            var columnHeader = (GridViewColumnHeader)sender;
            var sortBy = columnHeader.Tag.ToString();
            var direction = _currentSortColumn != columnHeader || _sortAdorner.Direction == ListSortDirection.Descending
                ? ListSortDirection.Ascending
                : ListSortDirection.Descending;

            _currentSortColumn = columnHeader;

            _sortAdorner = new SortAdorner(_currentSortColumn, direction);
            AdornerLayer.GetAdornerLayer(_currentSortColumn).Add(_sortAdorner);

            List.Items.SortDescriptions.Add(new SortDescription(sortBy, direction));
        }
    }
}
