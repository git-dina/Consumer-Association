using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace POSCA.Classes.ApiClasses
{
    public class CustomerBankAccount
    {
        #region Attributes
        public long BankAccountId { get; set; }
        public Nullable<long> CustomerId { get; set; }
        public string OldIBAN { get; set; }
        public string NewIBAN { get; set; }
        public Nullable<int> OldBankId { get; set; }
        public Nullable<int> NewBankId { get; set; }
        public string Notes { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }
        //extra
        public string OldBankName { get; set; }
        public string NewBankName { get; set; }
        #endregion

        #region Methods
        public async Task<List<CustomerBankAccount>> save(CustomerBankAccount CustomerBank)
        {
            var result = new List<CustomerBankAccount>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "CustomerBankAccount/Save";

            var myContent = JsonConvert.SerializeObject(CustomerBank);
            parameters.Add("itemObject", myContent);

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<CustomerBankAccount>(c.Value));
                }
            }
            return result;
        }

        public async Task<List<CustomerBankAccount>> GetByCustomerId(long customerId)
        {
            var result = new List<CustomerBankAccount>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "CustomerBankAccount/GetByCustomerId";

            parameters.Add("customerId", customerId.ToString());

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<CustomerBankAccount>(c.Value));
                }
            }
            return result;
        }
        #endregion
    }
}
