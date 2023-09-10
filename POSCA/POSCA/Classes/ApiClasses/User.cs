//using Newtonsoft.Json;
//using Newtonsoft.Json.Converters;
//using POSCA.ApiClasses;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace POSCA.Classes.ApiClasses
{
    public class User
    {
        #region Attributes
        public long UserId { get; set; }
        public long? RoleId { get; set; }
        public string LoginName { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }

        //extra
        public Role userRole { get; set; }
        public string RoleNameEn { get; set; }
        public string RoleNameAr { get; set; }
        #endregion

        #region Methods
        public async Task<User> LoginUser(string loginName, string password)
        {
            User user = new User();

            //########### to pass parameters (optional)
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("loginName", loginName);
            parameters.Add("password", password);

            IEnumerable<Claim> claims = await APIResult.getList("User/LoginUser", parameters);
            //#################

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    user = JsonConvert.DeserializeObject<User>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                    break;
                }
            }
            return user;
        }

        public async Task<List<User>> Get(bool? isActive = null)
        {
            var result = new List<User>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "User/Get";

            parameters.Add("isActive", isActive.ToString());

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<User>(c.Value));
                }
            }
            return result;
        }

        public async Task<List<User>> Save(User user)
        {
            var result = new List<User>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "User/Save";

            var myContent = JsonConvert.SerializeObject(user);
            parameters.Add("itemObject", myContent);

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<User>(c.Value));
                }
            }
            return result;
        }
        public async Task<List<User>> Delete(long deletedUserId, long userId)
        {
            var result = new List<User>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", deletedUserId.ToString());
            parameters.Add("userId", userId.ToString());
            string method = "User/Delete";

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<User>(c.Value));
                }
            }
            return result;
        }
        #endregion
    }
}
