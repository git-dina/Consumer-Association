using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSCA.Classes.ApiClasses
{
    public class CustomerBankAccount
    {
        public long BankAccountId { get; set; }
        public Nullable<long> CustomerId { get; set; }
        public string OldIBAN { get; set; }
        public string NewIBAN { get; set; }
        public Nullable<int> OldBankId { get; set; }
        public Nullable<int> NewBankId { get; set; }
        public string Notes { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }
        //extra
        public string OldBankName { get; set; }
        public string NewBankName { get; set; }
    }
}
