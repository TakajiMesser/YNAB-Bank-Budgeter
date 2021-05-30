namespace Budgeter.Shared.YNAB.Models
{
    public class Account : Model
    {
        public string Name { get; set; }
        public string Type { get; set; } // checking
        public bool OnBudget { get; set; }
        public bool Closed { get; set; }
        public int Balance { get; set; }
        public int ClearedBalance { get; set; }
        public int UnclearedBalance { get; set; }
        public string TransferPayeeID { get; set; }
        public bool DirectImportLinked { get; set; }
        public bool DirectImportInError { get; set; }
        public bool Deleted { get; set; }
    }
}
