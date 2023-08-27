using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace POSCA.Classes.ApiClasses
{
    public class Activity
    {
        #region Attributes
        public long ActivityId { get; set; }
        public Nullable<int> TypeId { get; set; }
        public string Description { get; set; }
        public decimal BasicValue { get; set; }
        public decimal ValueAfterDiscount { get; set; }
        public decimal MaximumBenefit { get; set; }
        public int RegestrtionCount { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; } = DateTime.Now;
        public Nullable<System.DateTime> EndDate { get; set; } = DateTime.Now;
        public string Notes { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }

        //extra
        public string TypeName { get; set; }
        public int RemainCount { get; set; }

        #endregion

        #region Methods
        public async Task<List<Activity>> save(Activity activity)
        {
            var result = new List<Activity>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Activity/Save";

            var myContent = JsonConvert.SerializeObject(activity);
            parameters.Add("itemObject", myContent);

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<Activity>(c.Value));
                }
            }
            return result;
        }

        public async Task<List<Activity>> get(bool? isActive = null)
        {
            var result = new List<Activity>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Activity/Get";

            parameters.Add("isActive", isActive.ToString());

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<Activity>(c.Value));
                }
            }
            return result;
        }

        public async Task<List<Activity>> delete(long activityId, long userId)
        {
            var result = new List<Activity>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", activityId.ToString());
            parameters.Add("userId", userId.ToString());
            string method = "Activity/delete";

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<Activity>(c.Value));
                }
            }
            return result;

        }

        public async Task<String> getMaxActivityId()
        {
            var result = "";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Activity/GetMaxActivityId";

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
