using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace POSCA.Classes.ApiClasses
{
    public class PhoneType
    {
        #region Attributes
        public int PhoneTypeId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }

        #endregion

        #region Methods
        public async Task<List<PhoneType>> save(PhoneType group)
        {
            var result = new List<PhoneType>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "PhoneType/Save";

            var myContent = JsonConvert.SerializeObject(group);
            parameters.Add("itemObject", myContent);

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<PhoneType>(c.Value));
                }
            }
            return result;
        }

        public async Task<List<PhoneType>> get(bool? isActive = null)
        {
            var result = new List<PhoneType>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "PhoneType/Get";

            parameters.Add("isActive", isActive.ToString());

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<PhoneType>(c.Value));
                }
            }
            return result;
        }

        public async Task<List<PhoneType>> delete(long phoneTypeId, long userId)
        {
            var result = new List<PhoneType>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", phoneTypeId.ToString());
            parameters.Add("userId", userId.ToString());
            string method = "PhoneType/delete";

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<PhoneType>(c.Value));
                }
            }
            return result;
        }
        #endregion
    }
}
