using System.Collections.ObjectModel;

namespace Budgeter.WPFApplication.ViewModels
{
    public class LogViewViewModel : ViewModel, ILogger
    {
        public ObservableCollection<string> Lines { get; set; } = new ObservableCollection<string>();

        public void WriteLine(string line = "") => Lines.Add(line);
        public void Clear() => Lines.Clear();
    }
}