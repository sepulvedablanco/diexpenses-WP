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

        public override string ToString()
        {
            return base.ToString() + ": " + "Id=" + Id + ", ApiId=" + ApiId + ", Expense=" + Expense + ", Concept=" + Concept + ", TransactionDate=" + TransactionDate
                 + ", Amount=" + Amount + ", KindId=" + KindId + ", SubkindId=" + SubkindId + ", BankAccountId=" + BankAccountId;
        }
    }
}
