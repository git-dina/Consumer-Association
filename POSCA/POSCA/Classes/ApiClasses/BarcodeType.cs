using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace POSCA.Classes.ApiClasses
{
    public class BarcodeType
    {
        #region Attributes
        public int TypeId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }
        #endregion

        #region Methods
        public async Task<List<BarcodeType>> save(BarcodeType type)
        {
            var result = new List<BarcodeType>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "BarcodeType/Save";

            var myContent = JsonConvert.SerializeObject(type);
            parameters.Add("itemObject", myContent);

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<BarcodeType>(c.Value));
                }
            }
            return result;
        }

        public async Task<List<BarcodeType>> get(bool? isActive = null)
        {
            var result = new List<BarcodeType>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "BarcodeType/Get";

            parameters.Add("isActive", isActive.ToString());

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<BarcodeType>(c.Value));
                }
            }
            return result;
        }

        public async Task<List<BarcodeType>> delete(long typeId, long userId)
        {
            var result = new List<BarcodeType>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", typeId.ToString());
            parameters.Add("userId", userId.ToString());
            string method = "BarcodeType/delete";

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<BarcodeType>(c.Value));
                }
            }
            return result;
        }
        #endregion
    }
}
