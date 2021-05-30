using Budgeter.Shared.PTCU;
using Budgeter.Shared.Rules;
using Budgeter.Shared.YNAB;
using Newtonsoft.Json;
using System.IO;

namespace Budgeter.Shared
{
    public class Configuration : IConfiguration
    {
        public Configuration(string filePath) => FilePath = filePath;

        public string FilePath { get; }

        public YNABConfiguration YNABConfiguration { get; set; }
        public PTCUConfiguration PTCUConfiguration { get; set; }
        public RuleSet RuleSet { get; set; }
        public string OutputFilePath { get; set; }

        public void Load()
        {
            var jsonText = File.ReadAllText(FilePath);
            JsonConvert.PopulateObject(jsonText, this);
        }

        public void Save()
        {
            var jsonText = JsonConvert.SerializeObject(this);
            File.WriteAllText(FilePath, jsonText);
        }
    }
}
