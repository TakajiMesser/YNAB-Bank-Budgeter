using System;
using System.Collections.Generic;

namespace Budgeter.Shared.YNAB
{
    public class YNABConfiguration : IConfiguration
    {
        public string PersonalAccessToken { get; set; }
        public string BudgetName { get; set; }
        public List<string> AccountNames { get; set; }
        public DateTime? SinceDate { get; set; }
    }
}
