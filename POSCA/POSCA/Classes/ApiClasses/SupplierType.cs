using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace POSCA.Classes.ApiClasses
{
    public class SupplierType
    {
        #region Attributes
        public int SupplierTypeId { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }
        #endregion

        #region Methods
        public async Task<List<SupplierType>> save(SupplierType group)
        {
            var result = new List<SupplierType>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "SupplierType/Save";

            var myContent = JsonConvert.SerializeObject(group);
            parameters.Add("itemObject", myContent);

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<SupplierType>(c.Value));
                }
            }
            return result;
        }

        public async Task<List<SupplierType>> get(bool? isActive = null)
        {
            var result = new List<SupplierType>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "SupplierType/Get";

            parameters.Add("isActive", isActive.ToString());

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<SupplierType>(c.Value));
                }
            }
            return result;
        }

        public async Task<List<SupplierType>> delete(long supGroupId, long userId)
        {
            var result = new List<SupplierType>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", supGroupId.ToString());
            parameters.Add("userId", userId.ToString());
            string method = "SupplierType/delete";

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<SupplierType>(c.Value));
                }
            }
            return result;
        }
        #endregion
    }
}
