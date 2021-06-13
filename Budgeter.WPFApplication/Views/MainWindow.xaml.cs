using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;

namespace Budgeter.WPFApplication.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            PresentationTraceSources.DataBindingSource.Listeners.Add(new ConsoleTraceListener());
            PresentationTraceSources.DataBindingSource.Switch.Level = SourceLevels.Error;
            InitializeComponent();

            ViewModel.Logger = LogView.ViewModel;
            Menu.ViewModel.MainWindow = this;
            Menu.ViewModel.Logger = LogView.ViewModel;

            YNABGrid.ViewModel.SyncWith.Add(BankGrid);
            BankGrid.ViewModel.SyncWith.Add(YNABGrid);

            ViewModel.Results.CollectionChanged += Results_CollectionChanged;
        }

        private void Results_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            YNABGrid.ViewModel.Transactions.Clear();
            BankGrid.ViewModel.Transactions.Clear();
            
            foreach (var result in ViewModel.Results)
            {
                YNABGrid.ViewModel.Transactions.Add(result.YNABTransaction);
                BankGrid.ViewModel.Transactions.Add(result.BankTransaction);
            }
        }

        private void OnLoaded(object sender, EventArgs e) { }

        private void OnClosing(object sender, CancelEventArgs e) => Application.Current.Shutdown();
    }
}
