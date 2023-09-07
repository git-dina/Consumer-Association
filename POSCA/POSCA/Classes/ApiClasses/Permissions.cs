using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace POSCA.Classes.ApiClasses
{
    public class Permissions
    {
        #region Attributes
        public int RolesPermissionId { get; set; }
        public Nullable<int> AppObjectId { get; set; }
        public Nullable<long> RoleId { get; set; }
        public Nullable<bool> ViewObject { get; set; }
        public Nullable<bool> EditObject { get; set; }
        public Nullable<bool> ApproveObject { get; set; }
        public string Notes { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }
        //extr 
        public string AppObject { get; set; }
        #endregion

        #region Methods
        public async Task<List<AppObject>> GetAppObjects()
        {
            var result = new List<AppObject>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Permissions/GetAppObject";


            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<AppObject>(c.Value));
                }
            }
            return result;
        }
        #endregion
    }

    public class Role
    {
        public long RoleID { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }


        public List<Permissions> Permissions { get; set; }
    }

    public class AppObject
    {
        public int AppObjectId { get; set; }
        public string Name { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
    }
}
