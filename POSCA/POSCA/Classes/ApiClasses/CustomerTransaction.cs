using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSCA.Classes.ApiClasses
{
    public class CustomerTransaction
    {
        #region Attributes
        public long TransactionId { get; set; }
        public Nullable<long> CustomerId { get; set; }
        public Nullable<long> BoxNumber { get; set; }
        public Nullable<System.DateTime> TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public Nullable<int> SharesCount { get; set; }
        public decimal SharePrice { get; set; }
        public string ApprovalNumber { get; set; }
        public Nullable<System.DateTime> MeetingDate { get; set; }
        public Nullable<System.DateTime> CheckDate { get; set; }
        public string Notes { get; set; }
        public Nullable<long> ToCustomerId { get; set; }
        public Nullable<long> ToBoxNumber { get; set; }
        public Nullable<int> BondNo { get; set; }
        public Nullable<System.DateTime> BondDate { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }

        //extra
        public int JoinDay { get; set; }
        public int JoinMonth{ get; set; }
        public int JoinYear{ get; set; }

        #endregion
    }
}
