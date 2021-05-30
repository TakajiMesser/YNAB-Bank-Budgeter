using System.Collections.Generic;
using System.Text;

namespace Budgeter.Shared.CSV
{
    public class CSVRow
    {
        public CSVRow() { }
        public CSVRow(string line)
        {
            var builder = new StringBuilder();
            var isQuote = false;

            foreach (var character in line)
            {
                switch (character)
                {
                    case '\"':
                        if (isQuote)
                        {
                            isQuote = false;
                        }
                        else
                        {
                            isQuote = true;
                        }
                        break;
                    case ',':
                        if (isQuote)
                        {
                            builder.Append(character);
                        }
                        else
                        {
                            Values.Add(builder.ToString());
                            builder.Clear();
                        }
                        break;
                    default:
                        builder.Append(character);
                        break;
                }
            }

            if (builder.Length > 0)
            {
                Values.Add(builder.ToString());
            }
        }

        public List<string> Values { get; } = new List<string>();

        public string ToLine() => string.Join(",", Values);
    }
}
