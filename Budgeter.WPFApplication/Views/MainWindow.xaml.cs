﻿using Budgeter.WPFApplication.Helpers;
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

            /*foreach (var column in GetGridColumns<YNABTransaction>())
            {
                YNABGrid.Columns.Add(column);
            }*/

            /*foreach (var column in GetGridColumns<PTCUTransaction>())
            {
                BankGrid.Columns.Add(column);
            }*/
        }

        private static IEnumerable<GridViewColumn> GetGridColumns<T>()
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

        private void List_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (sender == YNABList)
            {
                ViewHelper.GetChild<ScrollViewer>(BankList).ScrollToVerticalOffset(e.VerticalOffset);
            }
            else if (sender == BankList)
            {
                ViewHelper.GetChild<ScrollViewer>(YNABList).ScrollToVerticalOffset(e.VerticalOffset);
            }
        }

        private void List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender == YNABList)
            {
                foreach (var item in e.AddedItems)
                {
                    var index = YNABList.Items.IndexOf(item);
                    var match = BankList.Items.GetItemAt(index);

                    BankList.SelectedItems.Add(match);
                }

                foreach (var item in e.RemovedItems)
                {
                    var index = YNABList.Items.IndexOf(item);
                    var match = BankList.Items.GetItemAt(index);

                    BankList.SelectedItems.Remove(match);
                }
            }
            else if (sender == BankList)
            {
                foreach (var item in e.AddedItems)
                {
                    var index = BankList.Items.IndexOf(item);
                    var match = YNABList.Items.GetItemAt(index);

                    YNABList.SelectedItems.Add(match);
                }

                foreach (var item in e.RemovedItems)
                {
                    var index = BankList.Items.IndexOf(item);
                    var match = YNABList.Items.GetItemAt(index);

                    YNABList.SelectedItems.Remove(match);
                }
            }
        }
    }
}
