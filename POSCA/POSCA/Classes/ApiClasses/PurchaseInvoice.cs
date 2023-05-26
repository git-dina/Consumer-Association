using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace POSCA.Classes.ApiClasses
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
        public string SupplierNotes { get; set; }
        public string SupplierPurchaseNotes { get; set; }
        public Nullable<decimal> TotalCost { get; set; }
        public Nullable<decimal> TotalPrice { get; set; }
        public Nullable<decimal> CoopDiscount { get; set; }
        public Nullable<decimal> DiscountValue { get; set; }
        public Nullable<decimal> FreePercentage { get; set; }
        public Nullable<decimal> FreeValue { get; set; }
        public Nullable<decimal> ConsumerDiscount { get; set; }
        public Nullable<decimal> CostNet { get; set; }
        public string InvType { get; set; }
        public string InvStatus { get; set; }
        public bool IsApproved { get; set; }

        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }

        //extra
        public string LocationName { get; set; }
        public string SupplierName { get; set; }
        public IEnumerable<PurchaseInvDetails> PurchaseDetails { get; set; }
        public Supplier supplier { get; set; }
        #endregion

        #region Methods

        public async Task<List<PurchaseInvoice>> get(string invType)
        {
            var result = new List<PurchaseInvoice>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Purchase/Get";

            parameters.Add("invType", invType);

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<PurchaseInvoice>(c.Value));
                }
            }
            return result;
        }
        public async Task<List<PurchaseInvoice>> searchOrders(long locationId, string invNumber,string invType, bool? isApproved)
        {
            var result = new List<PurchaseInvoice>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Purchase/SearchOrders";

            parameters.Add("locationId", locationId.ToString());
            parameters.Add("invNumber", invNumber);
            parameters.Add("invType", invType);
            parameters.Add("isApproved", isApproved.ToString());

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<PurchaseInvoice>(c.Value));
                }
            }
            return result;
        }

        public async Task<PurchaseInvoice> SaveSupplyingOrder(PurchaseInvoice invoice)
        {
            var result = new PurchaseInvoice();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Purchase/SaveSupplyingOrder";

            var myContent = JsonConvert.SerializeObject(invoice);
            parameters.Add("itemObject", myContent);

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result = JsonConvert.DeserializeObject<PurchaseInvoice>(c.Value);
                }
            }
            return result;
        } 
        
        public async Task<long> approveSupplyingOrder(long purchaseId,long userId)
        {
            var result = new PurchaseInvoice();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Purchase/ApproveSupplyingOrder";

            parameters.Add("purchaseId", purchaseId.ToString());
            parameters.Add("userId", userId.ToString());

           return  await APIResult.post(method, parameters);
            
        } 
        
        public async Task<long> deletePurchaseInv(long purchaseId,long userId)
        {
            var result = new PurchaseInvoice();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Purchase/DeletePurchaseInv";

            parameters.Add("purchaseId", purchaseId.ToString());
            parameters.Add("userId", userId.ToString());

           return  await APIResult.post(method, parameters);
            
        }
        #endregion
    }
}
