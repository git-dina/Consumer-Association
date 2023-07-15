using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace POSCA.Classes.ApiClasses
{
    public class CustomerBank
    {
        #region Attributes
        public int BankId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Symbol { get; set; }
        public string Code { get; set; }
        public string Notes { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }
        #endregion

        #region Methods
        public async Task<List<CustomerBank>> save(CustomerBank CustomerBank)
        {
            var result = new List<CustomerBank>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "CustomerBank/Save";

            var myContent = JsonConvert.SerializeObject(CustomerBank);
            parameters.Add("itemObject", myContent);

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<CustomerBank>(c.Value));
                }
            }
            return result;
        }

        public async Task<List<CustomerBank>> get(bool? isActive = null)
        {
            var result = new List<CustomerBank>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "CustomerBank/Get";

            parameters.Add("isActive", isActive.ToString());

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<CustomerBank>(c.Value));
                }
            }
            return result;
        }

        public async Task<List<CustomerBank>> delete(long CustomerBankId, long userId)
        {
            var result = new List<CustomerBank>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", CustomerBankId.ToString());
            parameters.Add("userId", userId.ToString());
            string method = "CustomerBank/delete";

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<CustomerBank>(c.Value));
                }
            }
            return result;
        }

        public async Task<String> getMaxBankId()
        {
            var result = "";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "CustomerBank/getMaxBankId";

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
}
