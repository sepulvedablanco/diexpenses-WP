namespace common.Entities
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

        public BankAccount()
        {
            this.iban = "ES";
        }

        public BankAccount(int id)
        {
            this.Id = id;
        }

        [Column("Iban"), MaxLength(4)]
        public string Iban
        {
            get
            {
                return iban;
            }
            set
            {
                iban = value;
                UpdateCompleteBankAccount();
                RaisePropertyChanged();
            }
        }

        [Column("Entity"), MaxLength(4)]
        public string Entity
        {
            get
            {
                return entity;
            }
            set
            {
                entity = value;
                UpdateCompleteBankAccount();
                RaisePropertyChanged();
            }
        }

        [Column("Office"), MaxLength(4)]
        public string Office
        {
            get
            {
                return office;
            }
            set
            {
                office = value;
                UpdateCompleteBankAccount();
                RaisePropertyChanged();
            }
        }

        [Column("ControlDigit"), MaxLength(2)]
        public string ControlDigit
        {
            get
            {
                return controlDigit;
            }
            set
            {
                controlDigit = value;
                UpdateCompleteBankAccount();
                RaisePropertyChanged();
            }
        }

        [Column("AccountNumber"), MaxLength(10)]
        public string AccountNumber
        {
            get
            {
                return accountNumber;
            }
            set
            {
                accountNumber = value;
                UpdateCompleteBankAccount();
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

        [Column("CompleteBankAccount"), MaxLength(24)]
        public string CompleteBankAccount
        {
            get
            {
                return completeBankAccount;
            }
            set
            {
                completeBankAccount = value;
            }
        }

        private void UpdateCompleteBankAccount()
        {
            CompleteBankAccount = Iban + Entity + Office + ControlDigit + AccountNumber;
        }

        public override string ToString()
        {
            return base.ToString() + ": " + "Id=" + Id + ", ApiId=" + ApiId + ", Iban=" + Iban + ", Entity=" + Entity + ", Office=" + Office + ", ControlDigit=" + ControlDigit
                 + ", AccountNumber=" + AccountNumber + ", Balance=" + Balance + ", Description=" + Description + ", CompleteBankAccount=" + CompleteBankAccount;
        }
    }
}
