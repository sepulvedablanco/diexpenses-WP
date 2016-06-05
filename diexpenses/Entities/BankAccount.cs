using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace diexpenses.Entities
{
    class BankAccount
    {
        public int Id { get; set; }
        public string Iban { get; set; }
        public string Entity { get; set; }
        public string Office { get; set; }
        public string ControlDigit { get; set; }
        public string AccountNumber { get; set; }
        public double Balance { get; set; }
        public string Description { get; set; }
        public string CompleteBankAccount { get; set; }
    }
}
