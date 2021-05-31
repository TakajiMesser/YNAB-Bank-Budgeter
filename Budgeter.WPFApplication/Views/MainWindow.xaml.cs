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
        }

        private void OnLoaded(object sender, EventArgs e) { }

        private void OnClosing(object sender, CancelEventArgs e) => Application.Current.Shutdown();
    }
}
