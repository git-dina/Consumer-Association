using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace POSCA.Classes.ApiClasses
{
    public class SupplierGroup
    {
        #region Attributes
        public int SupplierGroupId { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }

        public bool HasSuppliers { get; set; }


        #endregion

        #region Methods
        public async Task<List<SupplierGroup>> save(SupplierGroup group)
        {
            var result = new List<SupplierGroup>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "SupplierGroup/Save";

            var myContent = JsonConvert.SerializeObject(group);
            parameters.Add("itemObject", myContent);

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add( JsonConvert.DeserializeObject<SupplierGroup>(c.Value));
                }
            }
            return result;
        }

        public async Task<List<SupplierGroup>> get(bool? isActive = null)
        {
            var result = new List<SupplierGroup>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "SupplierGroup/Get";

            parameters.Add("isActive", isActive.ToString());

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<SupplierGroup>(c.Value));
                }
            }
            return result;
        }

        public async Task<List<SupplierGroup>> delete(long supGroupId, long userId)
        {
            var result = new List<SupplierGroup>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", supGroupId.ToString());
            parameters.Add("userId", userId.ToString());
            string method = "SupplierGroup/delete";

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add( JsonConvert.DeserializeObject<SupplierGroup>(c.Value));
                }
            }
            return result;
        }
        #endregion
    }
}
