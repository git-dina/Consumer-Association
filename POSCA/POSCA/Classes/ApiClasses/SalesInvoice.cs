using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSCA.Classes.ApiClasses
{
    public class SalesInvoice
    {
        #region Attributes
        public long InvoiceId { get; set; }
        public string InvNumber { get; set; }
        public Nullable<decimal> TotalNet { get; set; }
        public Nullable<long> CustomerId { get; set; }
        public Nullable<decimal> CashReturn { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }

        //extra
        public List<SalesInvoiceDetails> SalesDetails { get; set; }
        public Customer Customer { get; set; }
        #endregion

        #region Methods
        #endregion
    }
}
