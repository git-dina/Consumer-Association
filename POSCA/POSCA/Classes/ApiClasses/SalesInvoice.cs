using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace POSCA.Classes.ApiClasses
{
    public class SalesInvoice
    {
        #region Attributes
        public long InvoiceId { get; set; }
        public long LocationId { get; set; }
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
        public async Task<List<PaymentType>> GetPaymentTypes()
        {
            var result = new List<PaymentType>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "SalesInvoice/GetPaymentTypes";


            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<PaymentType>(c.Value));
                }
            }
            return result;
        }

        public async Task<String> GetLastInvNum()
        {
            var result = "";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "SalesInvoice/GetLastInvNum";

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result = c.Value;
                }
            }
            return result;
        }
        #endregion
    }

    public class SalesPayment
    {
        #region Attributes
        public long PaymentId { get; set; }
        public Nullable<long> InvoiceId { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public int PaymentTypeId { get; set; }

        public string ReceiptNum { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        #endregion
    }

    public class PaymentType
    {
        public int PaymentTypeId { get; set; }
        public string PaymentTypeName { get; set; }
        public bool IsCard { get; set; }
        public bool IsBlocked { get; set; }
    }
}
