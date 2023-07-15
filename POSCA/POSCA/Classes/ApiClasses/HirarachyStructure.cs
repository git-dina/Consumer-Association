using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace POSCA.Classes.ApiClasses
{
    public class HirarachyStructure
    {
        #region Attributes
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Notes { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }
        #endregion

        #region Methods
        public async Task<List<HirarachyStructure>> save(HirarachyStructure HirarachyStructure)
        {
            var result = new List<HirarachyStructure>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "HirarachyStructure/Save";

            var myContent = JsonConvert.SerializeObject(HirarachyStructure);
            parameters.Add("itemObject", myContent);

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<HirarachyStructure>(c.Value));
                }
            }
            return result;
        }

        public async Task<List<HirarachyStructure>> get(bool? isActive = null)
        {
            var result = new List<HirarachyStructure>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "HirarachyStructure/Get";

            parameters.Add("isActive", isActive.ToString());

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<HirarachyStructure>(c.Value));
                }
            }
            return result;
        }

        public async Task<List<HirarachyStructure>> delete(long HirarachyStructureId, long userId)
        {
            var result = new List<HirarachyStructure>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", HirarachyStructureId.ToString());
            parameters.Add("userId", userId.ToString());
            string method = "HirarachyStructure/delete";

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<HirarachyStructure>(c.Value));
                }
            }
            return result;
        }

        public async Task<String> getMaxhirarachyId()
        {
            var result = "";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "HirarachyStructure/getMaxhirarachyId";

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
        #endregion
    }
}
