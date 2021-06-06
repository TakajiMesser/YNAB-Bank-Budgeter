using Budgeter.Shared.Banks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Budgeter.Shared.CSV
{
    public class CSVFile
    {
        public CSVFile(string filePath) => FilePath = filePath;

        public string FilePath { get; }
        public List<string> Headers { get; } = new List<string>();
        public List<CSVRow> Rows { get; } = new List<CSVRow>();

        public int IndexOf(string header)
        {
            var index = Headers.IndexOf(header);
            if (index < 0) throw new ArgumentOutOfRangeException("");

            return index;
        }

        public async Task Load()
        {
            using (var reader = File.OpenText(FilePath))
            {
                var hasHeaders = false;
                string line;

                while ((line = await reader.ReadLineAsync()) != null)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        if (hasHeaders)
                        {
                            Rows.Add(new CSVRow(line));
                        }
                        else
                        {
                            foreach (var value in line.Split(','))
                            {
                                Headers.Add(value);
                            }

                            hasHeaders = true;
                        }
                    }
                }
            }
        }

        public void Save()
        {
            var lines = new List<string>
            {
                string.Join(",", Headers)
            };

            foreach (var row in Rows)
            {
                lines.Add(row.ToLine());
            }

            File.WriteAllLines(FilePath, lines);
        }

        public void DeserializeInto(BankTransaction transaction, int rowIndex)
        {
            var row = Rows[rowIndex];

            foreach (var property in transaction.GetType().GetProperties())
            {
                if (property.GetCustomAttributes(typeof(CSVIgnoreAttribute), true) == null)
                {
                    var headerName = property.GetCustomAttributes(typeof(CSVHeaderAttribute), true).FirstOrDefault() is CSVHeaderAttribute headerAttribute
                        ? headerAttribute.Header
                        : property.Name;

                    var index = IndexOf(headerName);

                    if (index >= 0)
                    {
                        var value = row.Values[index];

                        if (property.PropertyType == typeof(DateTime))
                        {
                            property.SetValue(transaction, DateTime.Parse(value));
                        }
                        else if (property.PropertyType == typeof(float))
                        {
                            property.SetValue(transaction, !string.IsNullOrEmpty(value) ? float.Parse(value) : 0.0f);
                        }
                        else if (property.PropertyType == typeof(int))
                        {
                            property.SetValue(transaction, !string.IsNullOrEmpty(value) ? int.Parse(value) : 0);
                        }
                        else if (property.PropertyType == typeof(string))
                        {
                            property.SetValue(transaction, value);
                        }
                    }
                }
            }
        }

        public IEnumerable<T> Deserialize<T>() where T : new()
        {
            foreach (var row in Rows)
            {
                var t = new T();

                foreach (var property in typeof(T).GetProperties())
                {
                    if (property.GetCustomAttributes(typeof(CSVIgnoreAttribute), true) == null)
                    {
                        var headerName = property.GetCustomAttributes(typeof(CSVHeaderAttribute), true).FirstOrDefault() is CSVHeaderAttribute headerAttribute
                            ? headerAttribute.Header
                            : property.Name;

                        var index = IndexOf(headerName);

                        if (index >= 0)
                        {
                            var value = row.Values[index];

                            if (property.PropertyType == typeof(DateTime))
                            {
                                property.SetValue(t, DateTime.Parse(value));
                            }
                            else if (property.PropertyType == typeof(float))
                            {
                                property.SetValue(t, !string.IsNullOrEmpty(value) ? float.Parse(value) : 0.0f);
                            }
                            else if (property.PropertyType == typeof(int))
                            {
                                property.SetValue(t, !string.IsNullOrEmpty(value) ? int.Parse(value) : 0);
                            }
                            else if (property.PropertyType == typeof(string))
                            {
                                property.SetValue(t, value);
                            }
                        }
                    }
                }

                yield return t;
            }
        }
    }
}
