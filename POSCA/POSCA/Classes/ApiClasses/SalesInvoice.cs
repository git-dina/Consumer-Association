using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

        private string _InvNumber;
        public string InvNumber { get; set; }
        public Nullable<decimal> TotalNet { get; set; }
        public Nullable<long> CustomerId { get; set; }
        public decimal CashReturn { get; set; }
        public string InvStatus { get; set; } = "draft"; //draft - paid
        public bool IsActive { get; set; } = true;
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; } = MainWindow.userLogin.UserId;
        public Nullable<long> UpdateUserId { get; set; } = MainWindow.userLogin.UserId;


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) { PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); }
        //extra
        public List<SalesInvoiceDetails> SalesDetails { get; set; }
        public List<SalesPayment> Payments { get; set; }
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

        public async Task<SalesInvoice> Save(SalesInvoice invoice)
        {
            var result = new SalesInvoice();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "SalesInvoice/Save";

            var myContent = JsonConvert.SerializeObject(invoice);
            parameters.Add("itemObject", myContent);
            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result = JsonConvert.DeserializeObject<SalesInvoice>(c.Value);
                }
            }
            return result;
        }
        
        public async Task<int> DeleteInvoice(long invoiceId, long userId)
        {
            var result =0;
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", invoiceId.ToString());
            parameters.Add("userId", userId.ToString());
            string method = "SalesInvoice/Delete";

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result = JsonConvert.DeserializeObject<int>(c.Value);
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
        public decimal Amount { get; set; }
        public int PaymentTypeId { get; set; }

        public string ReceiptNum { get; set; }
        public Nullable<long> CreateUserId { get; set; } = MainWindow.userLogin.UserId;
        public Nullable<long> UpdateUserId { get; set; } = MainWindow.userLogin.UserId;
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
