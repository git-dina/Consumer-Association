using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace POSCA.Classes.ApiClasses
{
    public class Receipt
    {
        #region Attributes
        public long ReceiptId { get; set; }
        public string InvType { get; set; } 
        public string ReceiptStatus { get; set; } = "new";
        public bool IsRecieveAll { get; set; } = true;
        public string InvNumber { get; set; }
        public string ReceiptType { get; set; }
        public string CustomFreeType { get; set; }
        public Nullable<long> LocationId { get; set; }
        public Nullable<long> PurchaseId { get; set; }
        public Nullable<System.DateTime> ReceiptDate { get; set; } = DateTime.Now;
        public Nullable<long> SupId { get; set; }
        public string SupInvoiceNum { get; set; }
        public System.DateTime SupInvoiceDate { get; set; } = DateTime.Now;
        public Nullable<decimal> InvoiceAmount { get; set; }
        public Nullable<decimal> AmountDifference { get; set; } = 0;
        public string Notes { get; set; }
        public string SupplierNotes { get; set; }
        public string SupplierPurchaseNotes { get; set; }
        public decimal CoopDiscount { get; set; }
        public decimal DiscountValue { get; set; }
        public Nullable<decimal> FreePercentage { get; set; }
        public Nullable<decimal> FreeValue { get; set; }
        public Nullable<decimal> ConsumerDiscount { get; set; }
        public Nullable<decimal> CostNet { get; set; }
        public Nullable<decimal> TotalCost { get; set; }
        public Nullable<decimal> TotalPrice { get; set; }

        //////////////////// extra
       public decimal? NetInvoice { get; set; }
        public bool IsTransfer { get; set; }
        public Nullable<long> TransferBy { get; set; }
        public Nullable<System.DateTime> TransferDate { get; set; }
        public bool ISAccountTransfer { get; set; }
        public Nullable<System.DateTime> AccountTransferDate { get; set; }
        public Nullable<long> AccountEntryCode { get; set; }
        public Nullable<long> AccountEntryCodeCustody { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }

        public string LocationName { get; set; }
        public Supplier supplier { get; set; }
        public string PurchaseInvNumber { get; set; }

        public List<RecieptDetails> ReceiptDetails { get; set; }
        #endregion

        #region Methods
        public async Task<Receipt> SaveReceiptOrder(Receipt invoice)
        {
            var result = new Receipt();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Receipts/SaveReceiptOrder";

            var myContent = JsonConvert.SerializeObject(invoice);
            parameters.Add("itemObject", myContent);

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result = JsonConvert.DeserializeObject<Receipt>(c.Value);
                }
            }
            return result;
        } 
        
        public async Task<Receipt> PostingReceiptOrder(long receiptId,long userId)
        {
            var result = new Receipt();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Receipts/PostingReceiptOrder";

            parameters.Add("receiptId", receiptId.ToString());
            parameters.Add("userId", userId.ToString());

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result = JsonConvert.DeserializeObject<Receipt>(c.Value);
                }
            }
            return result;
        } 
        public async Task<Receipt> CanclePostingReceiptOrder(long receiptId,long userId)
        {
            var result = new Receipt();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Receipts/CanclePostingReceiptOrder";

            parameters.Add("receiptId", receiptId.ToString());
            parameters.Add("userId", userId.ToString());

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result = JsonConvert.DeserializeObject<Receipt>(c.Value);
                }
            }
            return result;
        }
         public async Task<Receipt> SaveReturnOrder(Receipt invoice)
        {
            var result = new Receipt();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Receipts/SaveReturnOrder";

            var myContent = JsonConvert.SerializeObject(invoice);
            parameters.Add("itemObject", myContent);

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result = JsonConvert.DeserializeObject<Receipt>(c.Value);
                }
            }
            return result;
        }

        public async Task<List<Receipt>> searchOrders(long locationId, string invNumber,string invType, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var result = new List<Receipt>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Receipts/SearchOrders";

            parameters.Add("locationId", locationId.ToString());
            parameters.Add("invNumber", invNumber);
            parameters.Add("invType", invType);
            parameters.Add("fromDate", fromDate.ToString());
            parameters.Add("toDate", toDate.ToString());

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<Receipt>(c.Value));
                }
            }
            return result;
        }
        #endregion
    }
    public class RecieptDetails
    {
        #region Attributes
        public long DetailsId { get; set; }
        public Nullable<long> ItemId { get; set; }
        public string ItemName { get; set; }
        public Nullable<long> ReceiptId { get; set; }
        public string ItemCode { get; set; }
        public string Barcode { get; set; }
        public string ItemNotes { get; set; }
        public Nullable<int> Factor { get; set; }
        public decimal MainCost { get; set; }
        public decimal Cost { get; set; }
        public decimal MainPrice { get; set; }
        public decimal Price { get; set; }
        public int? MaxQty { get; set; }
        public int? MinQty { get; set; }
        public int? FreeQty { get; set; }
        public Nullable<decimal> CoopDiscount { get; set; }
        public Nullable<decimal> ConsumerDiscount { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }
        #endregion

        //extra
    
        #region extra
        public int Sequence { get; set; }
        public decimal Balance { get; set; }
        public string ItemUnit { get; set; }
        #endregion

    }
}
