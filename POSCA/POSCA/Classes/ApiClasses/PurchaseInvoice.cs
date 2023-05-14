using Newtonsoft.Json;
using POSCA.Classes.ApiClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        public string SupplierNotes { get; set; }
        public string SupplierPurchaseNotes { get; set; }
        public Nullable<decimal> TotalCost { get; set; }
        public Nullable<decimal> TotalPrice { get; set; }
        public Nullable<decimal> CoopDiscount { get; set; }
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

        //extra
        public string LocationName { get; set; }
        public List<PurchaseInvDetails> PurchaseDetails { get; set; }
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
        #endregion
    }
}
