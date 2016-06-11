namespace diexpenses.Entities
{
    using SQLite.Net.Attributes;
    using System;

    [Table("Movements")]
    public class Movement
    {
        [Column("Id"), PrimaryKey()]
        public int Id { get; set; }

        [Column("Expense")]
        public bool Expense { get; set; }

        [Column("Concept")]
        public string Concept { get; set; }

        [Column("TransactionDate")]
        public DateTime TransactionDate { get; set; }

        [Column("Amount")]
        public Double Amount { get; set; }

        [Column("KindId")]
        public int KindId { get; set; }

        [Column("SubkindId")]
        public int SubkindId { get; set; }

        [Column("BankAccountId")]
        public int BankAccountId { get; set; }

        [Ignore]
        public Kind Kind { get; set; }

        [Ignore]
        public Subkind Subkind { get; set; }

        [Ignore]
        public BankAccount BankAccount { get; set; }

    }
}
