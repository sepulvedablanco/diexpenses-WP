namespace diexpenses.Entities
{
    using Base;
    using SQLite.Net.Attributes;
    using SQLiteNetExtensions.Attributes;
    using System;

    [Table("Movements")]
    public class Movement : EntityBase
    {
        private bool expense;
        private string concept;
        private DateTime transactionDate;
        private double amount;
        private int kindId;
        private int subkindId;
        private int bankAccountId;

        private Kind kind;
        private Subkind subkind;
        private BankAccount bankAccount;

        public Movement()
        {
            Expense = true;
            TransactionDate = DateTime.Today;
        }

        [Column("Expense")]
        public bool Expense
        {
            get
            {
                return expense;
            }
            set
            {
                expense = value;
                RaisePropertyChanged();
            }
        }

        [Column("Concept")]
        public string Concept
        {
            get
            {
                return concept;
            }
            set
            {
                concept = value;
                RaisePropertyChanged();
            }
        }

        [Column("TransactionDate")]
        public DateTime TransactionDate
        {
            get
            {
                return transactionDate;
            }
            set
            {
                transactionDate = value;
                RaisePropertyChanged();
            }
        }

        [Column("Amount")]
        public double Amount
        {
            get
            {
                return amount;
            }
            set
            {
                amount = value;
                RaisePropertyChanged();
            }
        }

        [ForeignKey(typeof(Kind))]
        public int KindId
        {
            get
            {
                return kindId;
            }
            set
            {
                kindId = value;
                RaisePropertyChanged();
            }
        }

        [ForeignKey(typeof(Subkind))]
        public int SubkindId
        {
            get
            {
                return subkindId;
            }
            set
            {
                subkindId = value;
                RaisePropertyChanged();
            }
        }

        [ForeignKey(typeof(BankAccount))]
        public int BankAccountId
        {
            get
            {
                return bankAccountId;
            }
            set
            {
                bankAccountId = value;
                RaisePropertyChanged();
            }
        }

        [Ignore()]
        public Kind Kind
        {
            get
            {
                return kind;
            }
            set
            {
                kind = value;
                if (value != null)
                {
                    kindId = value.Id.GetValueOrDefault(-1);
                }
                RaisePropertyChanged();
            }
        }

        [Ignore()]
        public Subkind Subkind
        {
            get
            {
                return subkind;
            }
            set
            {
                subkind = value;
                if (value != null)
                {
                    subkindId = value.Id.GetValueOrDefault(-1);
                }
                RaisePropertyChanged();
            }
        }

        [Ignore()]
        public BankAccount BankAccount
        {
            get
            {
                return bankAccount;
            }
            set
            {
                bankAccount = value;
                if (value != null)
                {
                    bankAccountId = value.Id.GetValueOrDefault(-1);
                }
                RaisePropertyChanged();
            }
        }
        public override string ToString()
        {
            return base.ToString() + ": " + "Id=" + Id + ", ApiId=" + ApiId + ", Expense=" + Expense + ", Concept=" + Concept + ", TransactionDate=" + TransactionDate + ", Amount=" + Amount + ", KindId=" + KindId
                 + ", Kind=" + Kind.ToString() + ", SubkindId=" + SubkindId + ", Subkind=" + Subkind.ToString() + ", BankAccountId=" + BankAccountId + ", BankAccount=" + BankAccount.ToString();
        }
    }
}
