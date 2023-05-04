using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace POSCA.Classes.ApiClasses
{
    public class Location
    {
        #region Attributes
        public long LocationId { get; set; }
        public Nullable<int> LocationTypeId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Notes { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsActive { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        #endregion

        #region Methods
        public async Task<List<Location>> save(Location group)
        {
            var result = new List<Location>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Location/Save";

            var myContent = JsonConvert.SerializeObject(group);
            parameters.Add("itemObject", myContent);

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<Location>(c.Value));
                }
            }
            return result;
        }

        public async Task<List<Location>> get(bool? isActive = null)
        {
            var result = new List<Location>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Location/Get";

            parameters.Add("isActive", isActive.ToString());

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<Location>(c.Value));
                }
            }
            return result;
        }

        public async Task<List<Location>> delete(long locationId, long userId)
        {
            var result = new List<Location>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", locationId.ToString());
            parameters.Add("userId", userId.ToString());
            string method = "Location/delete";

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<Location>(c.Value));
                }
            }
            return result;
        }
        #endregion
    }
}
