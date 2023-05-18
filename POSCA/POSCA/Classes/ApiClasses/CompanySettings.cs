using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace POSCA.Classes.ApiClasses
{
    public class CompanySettings
    {
        #region Attributs
        public long SettingId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Notes { get; set; }
        #endregion

        #region Methods
        public async Task<List<CompanySettings>> Get()
        {

            List<CompanySettings> list = new List<CompanySettings>();

            IEnumerable<Claim> claims = await APIResult.getList("CompanySettings/Get");

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<CompanySettings>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;

        }
        #endregion
    }
}
