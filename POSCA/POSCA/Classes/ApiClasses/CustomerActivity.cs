﻿using Newtonsoft.Json;
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
        public int? Count { get; set; }
        public string Notes { get; set; }
        public bool IsActive { get; set; } = true;
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
        public async Task<CustomerActivity> Save(CustomerActivity activity)
        {
            var result = new CustomerActivity();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "CustomerActivity/Save";

            var myContent = JsonConvert.SerializeObject(activity);
            parameters.Add("itemObject", myContent);

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result = JsonConvert.DeserializeObject<CustomerActivity>(c.Value);
                }
            }
            return result;
        }

        public async Task<List<CustomerActivity>> SearchActivities(string textSearch)
        {
            var result = new List<CustomerActivity>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "CustomerActivity/SearchActivities";

            parameters.Add("textSearch", textSearch.ToString());

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
         public async Task<List<CustomerActivity>> GetActivitiesReport(long boxNumberFrom , long boxNumberTo,
             long customerIdFrom, long customerIdTo, string customerName, long activityId,
             DateTime? activityStartDateFrom, DateTime? activityStartDateTo,
             DateTime? activityEndDateFrom, DateTime? activityEndDateTo,
             DateTime? joinDateFrom, DateTime? joinDateTo)
        {
            var result = new List<CustomerActivity>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "CustomerActivity/GetActivitiesReport";

            parameters.Add("boxNumberFrom", boxNumberFrom.ToString());
            parameters.Add("boxNumberTo", boxNumberTo.ToString());
            parameters.Add("customerIdFrom", customerIdFrom.ToString());
            parameters.Add("customerIdTo", customerIdTo.ToString());
            parameters.Add("customerName", customerName.ToString());
            parameters.Add("activityId", activityId.ToString());
            parameters.Add("activityStartDateFrom", activityStartDateFrom.ToString());
            parameters.Add("activityStartDateTo", activityStartDateTo.ToString());
            parameters.Add("activityEndDateFrom", activityEndDateFrom.ToString());
            parameters.Add("activityEndDateTo", activityEndDateTo.ToString());
            parameters.Add("joinDateFrom", joinDateFrom.ToString());
            parameters.Add("joinDateTo", joinDateTo.ToString());

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

        public async Task<int> delete(long requestId, long userId)
        {
            var result =0;
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", requestId.ToString());
            parameters.Add("userId", userId.ToString());
            string method = "CustomerActivity/delete";

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result = int.Parse(c.Value);
                }
            }
            return result;

        } 
        public async Task<int> GetUserUsedCount(long activityId, long userId)
        {
            var result =0;
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", activityId.ToString());
            parameters.Add("userId", userId.ToString());
            string method = "CustomerActivity/GetUserUsedCount";

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result = int.Parse(c.Value);
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
