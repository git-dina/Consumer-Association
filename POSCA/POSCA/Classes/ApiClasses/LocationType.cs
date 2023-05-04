using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace POSCA.Classes.ApiClasses
{
    public class LocationType
    {
        #region attributes
        public int LocationTypeId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }
        #endregion

        #region Methods
        public async Task<List<LocationType>> save(LocationType group)
        {
            var result = new List<LocationType>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "LocationType/Save";

            var myContent = JsonConvert.SerializeObject(group);
            parameters.Add("itemObject", myContent);

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<LocationType>(c.Value));
                }
            }
            return result;
        }

        public async Task<List<LocationType>> get(bool? isActive = null)
        {
            var result = new List<LocationType>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "LocationType/Get";

            parameters.Add("isActive", isActive.ToString());

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<LocationType>(c.Value));
                }
            }
            return result;
        }

        public async Task<List<LocationType>> delete(long locationTypeId, long userId)
        {
            var result = new List<LocationType>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", locationTypeId.ToString());
            parameters.Add("userId", userId.ToString());
            string method = "LocationType/delete";

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<LocationType>(c.Value));
                }
            }
            return result;
        }
        #endregion
    }
}
