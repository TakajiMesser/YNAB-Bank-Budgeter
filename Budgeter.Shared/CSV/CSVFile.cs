﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

        public void Load()
        {
            var hasHeaders = false;
            var lines = File.ReadLines(FilePath);

            foreach (var line in lines.Where(l => !string.IsNullOrWhiteSpace(l)))
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

        public IEnumerable<T> Deserialize<T>() where T : new()
        {
            foreach (var row in Rows)
            {
                var t = new T();

                foreach (var property in typeof(T).GetProperties())
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

                yield return t;
            }
        }
    }
}
