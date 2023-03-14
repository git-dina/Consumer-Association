using Newtonsoft.Json;
using POSCA.Classes.ApiClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace POSCA.Classes
{
    public class SupplierDocType
    {
        #region Attributes
        public long TypeId { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }

        #region no database attributes
        public long DocumentsNumber { get; set; }

        #endregion
        #endregion

        #region Methods
        public async Task<List<SupplierDocType>> save(Country group)
        {
            var result = new List<SupplierDocType>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "SupplierDocType/Save";

            var myContent = JsonConvert.SerializeObject(group);
            parameters.Add("itemObject", myContent);

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<SupplierDocType>(c.Value));
                }
            }
            return result;
        }

        public async Task<List<SupplierDocType>> get(bool? isActive = null)
        {
            var result = new List<SupplierDocType>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "SupplierDocType/Get";

            parameters.Add("isActive", isActive.ToString());

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<SupplierDocType>(c.Value));
                }
            }
            return result;
        }

        public async Task<List<SupplierDocType>> delete(long typeId, long userId)
        {
            var result = new List<SupplierDocType>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", typeId.ToString());
            parameters.Add("userId", userId.ToString());
            string method = "SupplierDocType/delete";

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<SupplierDocType>(c.Value));
                }
            }
            return result;
        }
        #endregion
    }
}
