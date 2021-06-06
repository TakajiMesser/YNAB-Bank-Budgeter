using System.Collections.Generic;

namespace Budgeter.Shared.Banks
{
    public class BankConfiguration : IConfiguration
    {
        public Dictionary<string, string> CSVFilePathByBankName { get; set; } = new Dictionary<string, string>();
    }
}
