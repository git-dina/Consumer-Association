using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace POSCA.Classes.ApiClasses
{
    public class Unit
    {
        #region Atrributes
        public long UnitId { get; set; }
        public string Name { get; set; }
        public Nullable<long> MinUnitId { get; set; }
        public int UnitValue { get; set; }
        public string Notes { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }

        #region extra
        public int Factor { get; set; }

        #endregion
        #endregion

        #region Methods
        public async Task<List<Unit>> save(Unit unit)
        {
            var result = new List<Unit>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Unit/Save";

            var myContent = JsonConvert.SerializeObject(unit);
            parameters.Add("itemObject", myContent);

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<Unit>(c.Value));
                }
            }
            return result;
        }

        public async Task<List<Unit>> get(bool? isActive = null)
        {
            var result = new List<Unit>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Unit/Get";

            parameters.Add("isActive", isActive.ToString());

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<Unit>(c.Value));
                }
            }
            return result;
        }

        public async Task<List<Unit>> delete(long UnitId, long userId)
        {
            var result = new List<Unit>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", UnitId.ToString());
            parameters.Add("userId", userId.ToString());
            string method = "Unit/delete";

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<Unit>(c.Value));
                }
            }
            return result;
        }
        #endregion
    }
}
