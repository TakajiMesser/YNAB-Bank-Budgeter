using System;

namespace Budgeter.Shared.YNAB
{
    public class YNABConfiguration : IConfiguration
    {
        public string PersonalAccessToken { get; set; }
        public string BudgetName { get; set; }
        public string AccountName { get; set; }
        public DateTime? SinceDate { get; set; }
    }
}
