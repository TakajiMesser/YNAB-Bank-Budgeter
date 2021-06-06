using Budgeter.Shared;
using Budgeter.WPFApplication.Views;
using Microsoft.Win32;
using System;
using System.IO;

namespace Budgeter.WPFApplication.ViewModels
{
    public class MainMenuViewModel : ViewModel
    {
        private RelayCommand _openSettingsCommand;

        public MainWindow MainWindow { get; set; }
        public ILogger Logger { get; set; }

        public RelayCommand OpenSettingsCommand => _openSettingsCommand ??= new RelayCommand(
            p =>
            {
                var filePath = OpenDialog(".json", "JSON Files|*.json");

                if (LoadSettings(filePath))
                {
                    MainWindow.ViewModel.Configuration = Configuration.Instance;
                    Logger.Message("Settings loaded.");
                }
                else
                {
                    MainWindow.ViewModel.Configuration = null;
                    Logger.Warning("Settings failed to load.");
                }
            },
            p => true
        );

        private static bool LoadSettings(string filePath)
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                Configuration.Instance.Load(filePath);
                return true;
            }

            return false;
        }

        private static string OpenDialog(string defaultExt, string filter, string initialDirectory = null)
        {
            var dialog = new OpenFileDialog()
            {
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = defaultExt,
                Filter = filter
            };

            if (initialDirectory != null)
            {
                dialog.InitialDirectory = NormalizedPath(initialDirectory);
            }

            return dialog.ShowDialog() == true
                ? dialog.FileName
                : null;
        }

        private static string NormalizedPath(string path) => Path.GetFullPath(new Uri(path).LocalPath)
            .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
            .ToUpperInvariant();
    }
}