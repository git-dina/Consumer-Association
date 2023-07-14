using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace POSCA.Classes.ApiClasses
{
    public class Job
    {
        #region Attributs
        public int JobId { get; set; }
        public string Name { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }
        #endregion

        #region Methods
        public async Task<List<Job>> save(Job job)
        {
            var result = new List<Job>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Job/Save";

            var myContent = JsonConvert.SerializeObject(job);
            parameters.Add("itemObject", myContent);

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<Job>(c.Value));
                }
            }
            return result;
        }

        public async Task<List<Job>> get(bool? isActive = null)
        {
            var result = new List<Job>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Job/Get";

            parameters.Add("isActive", isActive.ToString());

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<Job>(c.Value));
                }
            }
            return result;
        }

        public async Task<List<Job>> delete(long JobId, long userId)
        {
            var result = new List<Job>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", JobId.ToString());
            parameters.Add("userId", userId.ToString());
            string method = "Job/delete";

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<Job>(c.Value));
                }
            }
            return result;
        }

        public async Task<String> getMaxJobId()
        {
            var result = "";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Job/getMaxJobId";

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
