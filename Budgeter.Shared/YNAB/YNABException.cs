using Budgeter.Shared.YNAB.Models;
using System;

namespace Budgeter.Shared.YNAB
{
    public class YNABException : Exception
    {
        public YNABException(Error error) : base(error.Name + " - " + error.Detail) { }
    }
}
