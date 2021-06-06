using System;

namespace Budgeter.Shared.CSV
{
    [AttributeUsage(AttributeTargets.Property)]
    public class CSVHeaderAttribute : Attribute
    {
        public string Header { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class CSVIgnoreAttribute : Attribute { }
}
