using System;

namespace diexpenses.Entities
{
    class Movement
    {
        public int Id { get; set; }
        public bool Expense { get; set; }
        public string Concept { get; set; }
        public DateTime TransactionDate { get; set; }
        public Double Amount { get; set; }
        public Kind Kind { get; set; }
        public Subkind Subkind { get; set; }
        public BankAccount BankAccount { get; set; }

    }
}
