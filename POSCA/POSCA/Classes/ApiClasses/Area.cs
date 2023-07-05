using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace POSCA.Classes.ApiClasses
{
    public class Area
    {
        #region Attributes
        public int AreaId { get; set; }
        public string Name { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }

        //extra
        public List<Section> Sections { get; set; }
        #endregion

        #region Methods
        public async Task<List<Area>> save(Area area)
        {
            var result = new List<Area>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Area/Save";

            var myContent = JsonConvert.SerializeObject(area);
            parameters.Add("itemObject", myContent);

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<Area>(c.Value));
                }
            }
            return result;
        }


        public async Task<List<Area>> get(bool? isActive = null)
        {
            var result = new List<Area>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Area/Get";

            parameters.Add("isActive", isActive.ToString());

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<Area>(c.Value));
                }
            }
            return result;
        }

        public async Task<List<Area>> delete(int areaId, long userId)
        {
            var result = new List<Area>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", areaId.ToString());
            parameters.Add("userId", userId.ToString());
            string method = "Area/delete";

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<Area>(c.Value));
                }
            }
            return result;
        }
        #endregion
    }
}
