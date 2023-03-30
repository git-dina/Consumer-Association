using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace POSCA.Classes.ApiClasses
{
    public class Brand
    {
        #region Attributes
        public int BrandId { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }
        #endregion

        #region Methods
        public async Task<List<Brand>> save(Brand brand)
        {
            var result = new List<Brand>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Brand/Save";

            var myContent = JsonConvert.SerializeObject(brand);
            parameters.Add("itemObject", myContent);

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<Brand>(c.Value));
                }
            }
            return result;
        }

        public async Task<List<Brand>> get(bool? isActive = null)
        {
            var result = new List<Brand>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Brand/Get";

            parameters.Add("isActive", isActive.ToString());

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<Brand>(c.Value));
                }
            }
            return result;
        }

        public async Task<List<Brand>> delete(long brandId, long userId)
        {
            var result = new List<Brand>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", brandId.ToString());
            parameters.Add("userId", userId.ToString());
            string method = "Brand/delete";

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<Brand>(c.Value));
                }
            }
            return result;
        }
        #endregion
    }
}
