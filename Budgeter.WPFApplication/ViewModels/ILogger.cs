namespace Budgeter.WPFApplication.ViewModels
{
    public interface ILogger
    {
        void WriteLine(string line = "");
        void Clear();
    }
}