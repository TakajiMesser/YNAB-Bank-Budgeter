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

        public RelayCommand OpenSettingsCommand
        {
            get
            {
                return _openSettingsCommand ?? (_openSettingsCommand = new RelayCommand(
                    p =>
                    {
                        var fileName = OpenDialog(".json", "JSON Files|*.json");
                        if (fileName != null)
                        {
                            var configuration = new Configuration(fileName);
                            configuration.Load();

                            MainWindow.ViewModel.Configuration = configuration;
                        }
                    },
                    p => true
                ));
            }
        }

        private string OpenDialog(string defaultExt, string filter, string initialDirectory = null)
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

        private string NormalizedPath(string path) => Path.GetFullPath(new Uri(path).LocalPath)
            .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
            .ToUpperInvariant();
    }
}