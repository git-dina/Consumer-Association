using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace POSCA.Classes.ApiClasses
{
    public class Category
    {
        #region Attributes
        public long CategoryId { get; set; }
        public Nullable<long> CategoryParentId { get; set; }
        public string Name { get; set; }
        public decimal ProfitPercentage { get; set; }
        public decimal WholesalePercentage { get; set; }
        public decimal FreePercentage { get; set; }
        public decimal DiscountPercentage { get; set; }
        public string Notes { get; set; }
        public bool CanContainItems { get; set; }
        public string Image { get; set; }

        public bool IsBlocked { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }
        #endregion

        #region Methods

        public async Task<List<Category>> get(bool? isActive = null)
        {
            var result = new List<Category>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "ItemCategory/Get";

            parameters.Add("isActive", isActive.ToString());

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<Category>(c.Value));
                }
            }
            return result;
        }

        public async Task<List<Category>> GetCategoriesTree(bool? isActive = null)
        {
            var result = new List<Category>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "ItemCategory/Get";

            parameters.Add("isActive", isActive.ToString());

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<Category>(c.Value));
                }
            }
            return result;
        }

        public async Task<List<Category>> save(Supplier group)
        {
            var result = new List<Category>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "ItemCategory/Save";

            var myContent = JsonConvert.SerializeObject(group);
            parameters.Add("itemObject", myContent);

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<Category>(c.Value));
                }
            }
            return result;
        }

        public async Task<List<Category>> delete(long supGroupId, long userId)
        {
            var result = new List<Category>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", supGroupId.ToString());
            parameters.Add("userId", userId.ToString());
            string method = "ItemCategory/delete";

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<Category>(c.Value));
                }
            }
            return result;
        }
        #endregion
    }
}
