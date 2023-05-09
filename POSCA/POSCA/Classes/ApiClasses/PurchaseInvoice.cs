using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSCA.Classes
{
    public class PurchaseInvoice
    {
        #region Attributes
        public long PurchaseId { get; set; }
        public string InvNumber { get; set; }
        public Nullable<long> LocationId { get; set; }
        public Nullable<long> SupId { get; set; }
        public Nullable<System.DateTime> OrderDate { get; set; }
        public Nullable<System.DateTime> OrderRecieveDate { get; set; }
        public string Notes { get; set; }
        public Nullable<decimal> TotalCost { get; set; }
        public Nullable<decimal> TotalPrice { get; set; }
        public Nullable<decimal> EnterpriseDiscount { get; set; }
        public Nullable<decimal> CostAfterDiscount { get; set; }
        public Nullable<decimal> FreePercentage { get; set; }
        public Nullable<decimal> ConsumerDiscount { get; set; }
        public Nullable<decimal> CostNet { get; set; }
        public string InvType { get; set; }
        public string InvStatus { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }
        #endregion

        #region Methods
        #endregion
    }
}
