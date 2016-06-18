namespace diexpenses.Entities
{
    using Base;
    using SQLite.Net.Attributes;

    [Table("BankAccounts")]
    public class BankAccount : EntityBase
    {
        private string iban;
        private string entity;
        private string office;
        private string controlDigit;
        private string accountNumber;
        private double balance;
        private string description;
        private string completeBankAccount;

        [Column("Iban")]
        public string Iban
        {
            get
            {
                return iban;
            }
            set
            {
                iban = value;
                RaisePropertyChanged();
            }
        }

        [Column("Entity")]
        public string Entity
        {
            get
            {
                return entity;
            }
            set
            {
                entity = value;
                RaisePropertyChanged();
            }
        }

        [Column("Office")]
        public string Office
        {
            get
            {
                return office;
            }
            set
            {
                office = value;
                RaisePropertyChanged();
            }
        }

        [Column("ControlDigit")]
        public string ControlDigit
        {
            get
            {
                return controlDigit;
            }
            set
            {
                controlDigit = value;
                RaisePropertyChanged();
            }
        }

        [Column("AccountNumber")]
        public string AccountNumber
        {
            get
            {
                return accountNumber;
            }
            set
            {
                accountNumber = value;
                RaisePropertyChanged();
            }
        }

        [Column("Balance")]
        public double Balance
        {
            get
            {
                return balance;
            }
            set
            {
                balance = value;
                RaisePropertyChanged();
            }
        }

        [Column("Description")]
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
                RaisePropertyChanged();
            }
        }

        [Column("CompleteBankAccount")]
        public string CompleteBankAccount
        {
            get
            {
                return completeBankAccount;
            }
            set
            {
                completeBankAccount = value;
                RaisePropertyChanged();
            }
        }

        public override string ToString()
        {
            return base.ToString() + ": " + "Id=" + Id + ", ApiId=" + ApiId + ", Iban=" + Iban + ", Entity=" + Entity + ", Office=" + Office + ", ControlDigit=" + ControlDigit
                 + ", AccountNumber=" + AccountNumber + ", Balance=" + Balance + ", Description=" + Description + ", CompleteBankAccount=" + CompleteBankAccount;
        }
    }
}
