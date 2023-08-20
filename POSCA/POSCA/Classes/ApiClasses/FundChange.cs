using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSCA.Classes.ApiClasses
{
    public  class FundChange
    {
        #region Methods
        public long Id { get; set; }
        public Nullable<long> CustomerId { get; set; }
        public Nullable<long> OldFundNumber { get; set; }
        public Nullable<long> NewFundNumber { get; set; }
        public Nullable<long> SecondCustomerId { get; set; }
        public string ChangeType { get; set; }
        public string Reason { get; set; }
        public Nullable<System.DateTime> ChangeDate { get; set; }
        public Nullable<long> EmptyFundNumber { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }
        #endregion
    }
}
