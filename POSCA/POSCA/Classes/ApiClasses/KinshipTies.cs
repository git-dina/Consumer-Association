using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace POSCA.Classes.ApiClasses
{
    public class KinshipTies
    {
        #region Attributes
        public int KinshipId { get; set; }
        public string Name { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }

        #endregion

        #region Methods
        public async Task<List<KinshipTies>> save(KinshipTies KinshipTies)
        {
            var result = new List<KinshipTies>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "KinshipTies/Save";

            var myContent = JsonConvert.SerializeObject(KinshipTies);
            parameters.Add("itemObject", myContent);

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<KinshipTies>(c.Value));
                }
            }
            return result;
        }

        public async Task<List<KinshipTies>> get(bool? isActive = null)
        {
            var result = new List<KinshipTies>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "KinshipTies/Get";

            parameters.Add("isActive", isActive.ToString());

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<KinshipTies>(c.Value));
                }
            }
            return result;
        }

        public async Task<List<KinshipTies>> delete(long KinshipTiesId, long userId)
        {
            var result = new List<KinshipTies>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", KinshipTiesId.ToString());
            parameters.Add("userId", userId.ToString());
            string method = "KinshipTies/delete";

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<KinshipTies>(c.Value));
                }
            }
            return result;
        }

        public async Task<String> GetMaxKinshipId()
        {
            var result = "";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "KinshipTies/GetMaxKinshipId";

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
