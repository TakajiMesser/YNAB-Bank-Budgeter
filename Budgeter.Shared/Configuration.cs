using Budgeter.Shared.Banks;
using Budgeter.Shared.Rules;
using Budgeter.Shared.YNAB;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;

namespace Budgeter.Shared
{
    public class Configuration : IConfiguration
    {
        private static Lazy<Configuration> _instance = new(() => new Configuration());

        public static Configuration Instance => _instance.Value;

        private Configuration() => JSONSettings = new JsonSerializerSettings()
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy { ProcessDictionaryKeys = true }
            },
            Error = (s, args) =>
            {
                Error?.Invoke(this, new UnhandledExceptionEventArgs(args.ErrorContext.Error, false));
                args.ErrorContext.Handled = true;
            }
        };

        public YNABConfiguration YNABConfiguration { get; set; }
        public List<BankConfiguration> BankConfigurations { get; set; } = new List<BankConfiguration>();
        public RuleSet RuleSet { get; set; }
        public string OutputFilePath { get; set; }

        public JsonSerializerSettings JSONSettings { get; }

        public event EventHandler<UnhandledExceptionEventArgs> Error;

        public void Load(string filePath)
        {
            YNABConfiguration = null;
            BankConfigurations.Clear();
            RuleSet = null;
            OutputFilePath = null;

            var jsonText = File.ReadAllText(filePath);
            JsonConvert.PopulateObject(jsonText, this, JSONSettings);
        }

        public void Save(string filePath)
        {
            var jsonText = JsonConvert.SerializeObject(this);
            File.WriteAllText(filePath, jsonText);
        }
    }
}
