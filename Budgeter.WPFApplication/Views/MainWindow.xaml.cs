﻿using Budgeter.Shared.PTCU;
using Budgeter.Shared.YNAB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

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

            foreach (var column in GetGridColumns<YNABTransaction>())
            {
                YNABGrid.Columns.Add(column);
            }

            foreach (var column in GetGridColumns<PTCUTransaction>())
            {
                BankGrid.Columns.Add(column);
            }

            /*<GridView x:Name="YNABGrid">
                <GridViewColumn Header="Amount" Width="120" DisplayMemberBinding="{Binding YNABTransaction.Amount}" />
            </GridView>*/
        }

        private IEnumerable<GridViewColumn> GetGridColumns<T>()
        {
            foreach (var property in typeof(T).GetProperties())
            {
                yield return new GridViewColumn
                {
                    Header = property.Name,
                    DisplayMemberBinding = new Binding(typeof(T).Name + "." + property.Name)
                };
            }
        }

        private void OnLoaded(object sender, EventArgs e) { }

        private void OnClosing(object sender, CancelEventArgs e) => Application.Current.Shutdown();
    }
}
