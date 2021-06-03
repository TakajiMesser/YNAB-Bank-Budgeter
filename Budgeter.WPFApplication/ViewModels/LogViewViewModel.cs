using System;
using System.Collections.ObjectModel;

namespace Budgeter.WPFApplication.ViewModels
{
    public class LogViewViewModel : ViewModel, ILogger
    {
        public ObservableCollection<LogLine> Lines { get; set; } = new();

        public void Message(string message) => Lines.Add(new LogLine(LogType.Message, message));
        public void Warning(string warning) => Lines.Add(new LogLine(LogType.Warning, warning));
        public void Error(string error) => Lines.Add(new LogLine(LogType.Error, error));
        public void Error(Exception ex) => Lines.Add(new LogLine(LogType.Error, ex.ToString()));

        public void LineBreak() => Lines.Add(new LogLine(LogType.Empty, string.Empty));

        public void Clear() => Lines.Clear();
    }
}