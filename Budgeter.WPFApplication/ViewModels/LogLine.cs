using System;
using System.Windows.Media;

namespace Budgeter.WPFApplication.ViewModels
{
    public enum LogType
    {
        Message,
        Warning,
        Error,
        Empty
    }

    public struct LogLine
    {
        public LogLine(LogType type, string value)
        {
            LogType = type;
            Value = value;
        }

        public LogType LogType { get; set; }
        public string Value { get; set; }

        public Brush Color => LogType switch
        {
            LogType.Message => Brushes.White,
            LogType.Warning => Brushes.Yellow,
            LogType.Error => Brushes.Red,
            LogType.Empty => Brushes.Black,
            _ => throw new NotImplementedException("Could not handle log type " + LogType)
        };
    }
}