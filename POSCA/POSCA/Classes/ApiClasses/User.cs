//using Newtonsoft.Json;
//using Newtonsoft.Json.Converters;
//using POSCA.ApiClasses;

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
        public long RoleId { get; set; }
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
        #endregion
    }
}
