using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace POSCA.Classes.ApiClasses
{
    public class ActivityType
    {
        #region AAttributes
        public int Id { get; set; }
        public Nullable<int> ParentTypeId { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public bool AllContributors { get; set; }
        public bool IsFinal { get; set; }
        public bool OnlyFamilyCardHolder { get; set; }
        public bool IsBlocked { get; set; }
        public bool OnlyOneActivity { get; set; }
        public string AccountCode { get; set; }
        public string AccountNumber { get; set; }
        public string AccountName { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }


        #endregion

        #region Methods

        public async Task<List<ActivityType>> get(bool? isActive = null)
        {
            var result = new List<ActivityType>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "ActivityType/Get";

            parameters.Add("isActive", isActive.ToString());

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<ActivityType>(c.Value));
                }
            }
            return result;
        }

      

        public async Task<List<ActivityType>> save(ActivityType type)
        {
            var result = new List<ActivityType>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "ActivityType/Save";

            var myContent = JsonConvert.SerializeObject(type);
            parameters.Add("itemObject", myContent);

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<ActivityType>(c.Value));
                }
            }
            return result;
        }

        public async Task<List<ActivityType>> delete(long typeId, long userId)
        {
            var result = new List<ActivityType>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", typeId.ToString());
            parameters.Add("userId", userId.ToString());
            string method = "ActivityType/delete";

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<ActivityType>(c.Value));
                }
            }
            return result;
        }
        #endregion
    }
}
