using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace POSCA.Classes.ApiClasses
{
    public class AssistantSupplier
    {
        public long AssistantSupId { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }

        internal async Task<List<AssistantSupplier>> get(bool? v = null)
        {
            var result = new List<AssistantSupplier>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "AssistantSup/Get";

            parameters.Add("isActive", v.ToString());

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<AssistantSupplier>(c.Value));
                }
            }
            return result;
        }
        internal async Task<List<AssistantSupplier>> search(string searchText)
        {
            var result = new List<AssistantSupplier>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "AssistantSup/Search";

            parameters.Add("searchText", searchText);

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<AssistantSupplier>(c.Value));
                }
            }
            return result;
        }

        internal async Task<AssistantSupplier> save(AssistantSupplier assistantSupplier)
        {
            var result = new AssistantSupplier();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "AssistantSup/Save";

            var myContent = JsonConvert.SerializeObject(assistantSupplier);
            parameters.Add("itemObject", myContent);

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result = JsonConvert.DeserializeObject<AssistantSupplier>(c.Value);
                }
            }
            return result;
        }

        internal async Task<long> delete(long assistantSupId, long userId)
        {
            var result = new List<AssistantSupplier>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", assistantSupId.ToString());
            parameters.Add("userId", userId.ToString());
            string method = "AssistantSup/delete";

            return await APIResult.post(method, parameters);
            
        }

        public async Task<String> getMaxSupplierId()
        {
            var result = "";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "AssistantSup/GetMaxSupplierId";

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
    }
}
