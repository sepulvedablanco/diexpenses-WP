namespace diexpenses.Entities
{
    using SQLite.Net.Attributes;

    [Table("BankAccounts")]
    public class BankAccount
    {
        [Column("Id"), PrimaryKey()]
        public int? Id { get; set; }

        [Column("Iban")]
        public string Iban { get; set; }

        [Column("Entity")]
        public string Entity { get; set; }

        [Column("Office")]
        public string Office { get; set; }

        [Column("ControlDigit")]
        public string ControlDigit { get; set; }

        [Column("AccountNumber")]
        public string AccountNumber { get; set; }

        [Column("Balance")]
        public double Balance { get; set; }

        [Column("Description")]
        public string Description { get; set; }

        [Column("CompleteBankAccount")]
        public string CompleteBankAccount { get; set; }
    }
}
