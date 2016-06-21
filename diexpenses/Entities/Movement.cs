﻿namespace diexpenses.Entities
{
    using Base;
    using SQLite.Net.Attributes;
    using SQLiteNetExtensions.Attributes;
    using System;
    using Windows.Devices.Geolocation;

    [Table("Movements")]
    public class Movement : EntityBase
    {
        private bool expense;
        private string concept;
        private DateTime transactionDate;
        private double amount;
        private Geopoint location;
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

        [Column("Latitude")]
        public double Latitude
        {
            get {
                return Location?.Position.Latitude ?? 0;
            }
            set
            {
                this.location = new Geopoint(new BasicGeoposition() { Latitude = value, Longitude = Longitude });
            }
        }

        [Column("Longitude")]
        public double Longitude
        {
            get {
                return Location?.Position.Longitude ?? 0;
            }
            set
            {
                this.location = new Geopoint(new BasicGeoposition() { Latitude = Latitude, Longitude = value });
            }
        }

        [ForeignKey(typeof(Kind))]
        public int KindId
        {
            get
            {
                return Kind?.Id ?? -1;
            }
        }

        [ForeignKey(typeof(Subkind))]
        public int SubkindId
        {
            get
            {
                return Subkind?.Id ?? -1;
            }
        }

        [ForeignKey(typeof(BankAccount))]
        public int BankAccountId
        {
            get
            {
                return BankAccount.Id ?? -1;
            }
        }

        [Ignore()]
        public Geopoint Location
        {
            get
            {
                return this.location;
            }
            set
            {
                this.location = value;
                RaisePropertyChanged();
            }
        }

        [Ignore()]
        public Kind Kind
        {
            get
            {
                return this.kind;
            }
            set
            {
                this.kind = value;
                RaisePropertyChanged();
            }
        }

        [Ignore()]
        public Subkind Subkind
        {
            get
            {
                return this.subkind;
            }
            set
            {
                this.subkind = value;
                RaisePropertyChanged();
            }
        }

        [Ignore()]
        public BankAccount BankAccount
        {
            get
            {
                return this.bankAccount;
            }
            set
            {
                this.bankAccount = value;
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
