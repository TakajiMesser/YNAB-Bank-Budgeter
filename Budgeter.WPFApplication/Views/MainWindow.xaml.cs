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

            Menu.ViewModel.MainWindow = this;
        }

        private void OnLoaded(object sender, EventArgs e) { }

        private void OnClosing(object sender, CancelEventArgs e) => Application.Current.Shutdown();
    }
}
