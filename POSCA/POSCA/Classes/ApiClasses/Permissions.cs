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
        public string NameAr { get; set; }
        public string NameEn { get; set; }
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

        public async Task<List<Role>> save(Role role)
        {
            var result = new List<Role>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Permissions/Save";

            var myContent = JsonConvert.SerializeObject(role);
            parameters.Add("itemObject", myContent);

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<Role>(c.Value));
                }
            }
            return result;
        }
        public async Task<List<Role>> GetRoles(bool? isActive = null)
        {
            var result = new List<Role>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Permissions/GetRoles";

            parameters.Add("isActive", isActive.ToString());

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<Role>(c.Value));
                }
            }
            return result;
        }

        public async Task<List<Role>> DeleteRole(long roleId, long userId)
        {
            var result = new List<Role>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", roleId.ToString());
            parameters.Add("userId", userId.ToString());
            string method = "Permissions/DeleteRole";

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<Role>(c.Value));
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

        //extra
        public bool ViewObject { get; set; }
        public bool EditObject { get; set; }
        public bool ApproveObject { get; set; }
    }
}
