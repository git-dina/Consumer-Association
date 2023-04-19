using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSCA.Classes.ApiClasses
{
    public class Item
    {
        #region Attributes
        public long ItemId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string EngName { get; set; }
        public string ItemStatus { get; set; }
        public string ItemReceiptType { get; set; }
        public string ItemType { get; set; }
        public string ItemTransactionType { get; set; }
        public Nullable<long> CategoryId { get; set; }
        public Nullable<int> BrandId { get; set; }
        public Nullable<long> SupId { get; set; }
        public Nullable<long> SupSectorId { get; set; }
        public Nullable<long> CountryId { get; set; }
        public Nullable<int> CommitteeNo { get; set; }
        public Nullable<long> UnitId { get; set; }
        public Nullable<int> Factor { get; set; }
        public decimal MainCost { get; set; }
        public decimal Cost { get; set; }
        public decimal ConsumerProfitPerc { get; set; }
        public decimal WholesaleProfitPerc { get; set; }
        public decimal ConsumerDiscPerc { get; set; }
        public decimal WholesaleDiscPerc { get; set; }
        public decimal FreePerc { get; set; }
        public decimal DiscPerc { get; set; }
        public decimal MainPrice { get; set; }
        public decimal Price { get; set; }
        public decimal WholesalePrice { get; set; }
        public Nullable<int> QtyMin { get; set; }
        public Nullable<int> QtyMax { get; set; }
        public Nullable<decimal> PackageWeight { get; set; }
        public Nullable<long> PackageUnit { get; set; }
        public bool IsSpecialOffer { get; set; }
        public bool IsWeight { get; set; }
        public bool IsContainExpiryDate { get; set; }
        public bool IsSellNotAllow { get; set; }
        public string Notes { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }
        #endregion
    }

    public class ItemGeneralization
    {

        public long Id { get; set; }
        public Nullable<long> ItemId { get; set; }
        public Nullable<int> GeneralizationYear { get; set; }
        public Nullable<int> GeneralizationNo { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }
    }
}
