using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSCA.Classes.ApiClasses
{
    public class PurchaseInvDetails
    {

        #region Attributes
        public long DetailsId { get; set; }
        public Nullable<long> ItemId { get; set; }
        public Nullable<long> PurchaseId { get; set; }
        public string ItemCode { get; set; }
        public string Barcode { get; set; }
        //
        public string ItemName { get; set; }
        public string ItemUnit { get; set; }
        //
        public string ItemNotes { get; set; }
        public Nullable<int> Factor { get; set; }
        public decimal MainCost { get; set; }
        public decimal Cost { get; set; }
        public decimal MainPrice { get; set; }
        public decimal Price { get; set; }
        public decimal Balance { get; set; }
        public int MinQty { get; set; }
        public int MaxQty { get; set; }
        public int FreeQty { get; set; } = 0;
        public Nullable<decimal> CoopDiscount { get; set; }
        public Nullable<decimal> ConsumerDiscount { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }
        #endregion
    }
}
