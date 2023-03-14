using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace POSCA.Classes.ApiClasses
{
    public class Country
    {
        #region Attributes
        public long CountryId { get; set; }
        public string CountryName { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }
        #endregion

        #region Methods
        public async Task<List<Country>> save(SupplierType group)
        {
            var result = new List<Country>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "LstCountry/Save";

            var myContent = JsonConvert.SerializeObject(group);
            parameters.Add("itemObject", myContent);

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<Country>(c.Value));
                }
            }
            return result;
        }

        public async Task<List<Country>> get(bool? isActive = null)
        {
            var result = new List<Country>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "LstCountry/Get";

            parameters.Add("isActive", isActive.ToString());

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<Country>(c.Value));
                }
            }
            return result;
        }

        public async Task<List<Country>> delete(long supGroupId, long userId)
        {
            var result = new List<Country>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", supGroupId.ToString());
            parameters.Add("userId", userId.ToString());
            string method = "LstCountry/delete";

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<Country>(c.Value));
                }
            }
            return result;
        }
        #endregion
    }
}
