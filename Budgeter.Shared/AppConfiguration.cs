using Budgeter.Shared.Rules;

namespace Budgeter.Shared
{
    public class AppConfiguration
    {
        public string PTCUFilePath { get; set; }
        public string YNABCredentials { get; set; }
        public RuleSet RuleSet { get; set; }
        public string OutputFilePath { get; set; }
    }
}
