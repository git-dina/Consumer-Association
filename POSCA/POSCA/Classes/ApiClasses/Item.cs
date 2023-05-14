using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        public string ItemStatus { get; set; } = "normal";
        public string ItemReceiptType { get; set; }
        public string ItemType { get; set; } = "general";
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
        public Nullable<DateTime> OfferEndDate { get; set; }
        public bool IsWeight { get; set; }
        public bool IsContainExpiryDate { get; set; }
        public bool IsSellNotAllow { get; set; }
        public string Notes { get; set; }
        public bool IsActive { get; set; }
        public Nullable<DateTime> CreateDate { get; set; }
        public Nullable<DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }

        #region extra attributes
        public string CategoryName { get; set; }

        public Supplier Supplier { get; set; }
        public List<ItemGeneralization> ItemGeneralizations { get; set; }
        public List<ItemUnit> ItemUnits { get; set; }
        public List<ItemAllowedTransaction> ItemAllowedTransactions { get; set; }
        public List<ItemLocation> ItemLocations { get; set; }
        public string ItemUnit { get; set; }
        #endregion
        #endregion

        #region Methods
        public async Task<List<Item>> get(bool? isActive = null)
        {
            var result = new List<Item>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Item/Get";

            parameters.Add("isActive", isActive.ToString());

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<Item>(c.Value));
                }
            }
            return result;
        }

        public async Task<List<Item>> save(Item group)
        {
            var result = new List<Item>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Item/Save";

            var myContent = JsonConvert.SerializeObject(group);
            parameters.Add("itemObject", myContent);

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<Item>(c.Value));
                }
            }
            return result;
        }
       

        public async Task<List<Item>> delete(long supGroupId, long userId)
        {
            var result = new List<Item>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", supGroupId.ToString());
            parameters.Add("userId", userId.ToString());
            string method = "Item/delete";

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<Item>(c.Value));
                }
            }
            return result;
        }
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

    public class ItemAllowedTransaction
    {
        public long Id { get; set; }
        public Nullable<long> ItemId { get; set; }
        public string Transaction { get; set; }
        public bool IsActive { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }

        //extra
        public bool IsAllowed { get; set; }
        public string TransactionText { get; set; }

    }
}
