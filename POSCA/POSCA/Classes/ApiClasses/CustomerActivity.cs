using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace POSCA.Classes.ApiClasses
{
    public class CustomerActivity
    {
        #region Attributes
        public long RequestId { get; set; }
        public Nullable<long> CustomerId { get; set; }
        public Nullable<long> BoxNumber { get; set; }
        public Nullable<long> ActivityId { get; set; }
        public int Count { get; set; }
        public string Notes { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }

        //extra
        public string CustomerName { get; set; }
        public string CustomerStatus { get; set; }
        public string CivilNum { get; set; }
        public bool FamilyCardHolder { get; set; }

        //activity
        public string ActivityName { get; set; }
        public decimal BasicValue { get; set; }
        public decimal ValueAfterDiscount { get; set; }
        public int MaximumBenefit { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<System.DateTime> RegistrationDate { get; set; }

        #endregion

        #region Methods
        public async Task<List<CustomerActivity>> save(CustomerActivity activity)
        {
            var result = new List<CustomerActivity>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "CustomerActivity/Save";

            var myContent = JsonConvert.SerializeObject(activity);
            parameters.Add("itemObject", myContent);

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<CustomerActivity>(c.Value));
                }
            }
            return result;
        }

        public async Task<List<CustomerActivity>> get(bool? isActive = null)
        {
            var result = new List<CustomerActivity>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "CustomerActivity/Get";

            parameters.Add("isActive", isActive.ToString());

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<CustomerActivity>(c.Value));
                }
            }
            return result;
        }

        public async Task<List<CustomerActivity>> delete(long activityId, long userId)
        {
            var result = new List<CustomerActivity>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", activityId.ToString());
            parameters.Add("userId", userId.ToString());
            string method = "CustomerActivity/delete";

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<CustomerActivity>(c.Value));
                }
            }
            return result;

        }

        public async Task<String> getMaxRequestId()
        {
            var result = "";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "CustomerActivity/getMaxRequestId";

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
