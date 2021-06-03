using System;

namespace Budgeter.WPFApplication.ViewModels
{
    public interface ILogger
    {
        void Message(string message);
        void Warning(string warning);
        void Error(string error);
        void Error(Exception ex);

        void LineBreak();

        void Clear();
    }
}