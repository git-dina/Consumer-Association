using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace POSCA.Classes.ApiClasses
{
    public  class FundChange
    {
        #region Attributes
        public long Id { get; set; }
        public Nullable<long> CustomerId { get; set; }
        public Nullable<long> OldFundNumber { get; set; }
        public Nullable<long> NewFundNumber { get; set; }
        public Nullable<long> SecondCustomerId { get; set; }
        public string ChangeType { get; set; }
        public string Reason { get; set; }
        public Nullable<System.DateTime> ChangeDate { get; set; }
        public Nullable<long> EmptyFundNumber { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }

        // customer
        public string CustomerName { get; set; }
        public string CustomerStatus { get; set; }
        public string CivilNum { get; set; }
        public string MobileNumber { get; set; }
        public Nullable<System.DateTime> JoinDate { get; set; }

        public string SecondCustomerName { get; set; }
        public string SecondCustomerStatus { get; set; }
        public string SecondCivilNum { get; set; }
        public string SecondMobileNumber { get; set; }
        public Nullable<System.DateTime> SecondJoinDate { get; set; }

        #endregion

        #region Methods
        public async Task<FundChange> save(FundChange fundChange)
        {
            var result = new FundChange();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "FundChange/Save";

            var myContent = JsonConvert.SerializeObject(fundChange);
            parameters.Add("itemObject", myContent);

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result = JsonConvert.DeserializeObject<FundChange>(c.Value);
                }
            }
            return result;
        }
        #endregion
    }
}
