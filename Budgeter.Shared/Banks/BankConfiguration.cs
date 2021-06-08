namespace Budgeter.Shared.Banks
{
    public class BankConfiguration : IConfiguration
    {
        public string BankName { get; set; }
        public string CSVFilePath { get; set; }
        public string AccountName { get; set; }
    }
}
