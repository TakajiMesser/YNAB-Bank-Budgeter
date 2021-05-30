using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Budgeter.Shared.Rules
{
    public class RuleSet
    {
        [JsonProperty(ItemConverterType = typeof(RuleConverter))]
        public List<IRule> Rules { get; set; } = new List<IRule>();
    }

    public class RuleConverter : JsonConverter<IRule>
    {
        public override bool CanWrite => false;
        public override bool CanRead => true;

        public override void WriteJson(JsonWriter writer, IRule value, JsonSerializer serializer) => throw new NotImplementedException("Not implemented yet");

        public override IRule ReadJson(JsonReader reader, Type objectType, IRule existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);
            var rule = CreateRule(jsonObject["Value"].Value<string>());

            serializer.Populate(jsonObject.CreateReader(), rule);
            return rule;
        }

        private static IRule CreateRule(string value)
        {
            switch (value)
            {
                case "Name":
                    return new NameRule();
                case "Amount":
                    return new AmountRule();
                case "date":
                    return new DateRule();
            }

            throw new ArgumentException("Could not handle rule value " + value);
        }
    }
}
