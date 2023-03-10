//using Newtonsoft.Json;
//using Newtonsoft.Json.Converters;
//using POSCA.ApiClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace POSCA.Classes
{

    public class Branch
    {
        public long branchId { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string mobile { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<long> createUserId { get; set; }
        public Nullable<long> updateUserId { get; set; }
        public string notes { get; set; }
        public Nullable<long> parentId { get; set; }
        public byte isActive { get; set; }
        public string type { get; set; }
        public Boolean canDelete { get; set; }
        public Nullable<decimal> balance { get; set; }
        // for report
        public int count { get; set; }

        public string invType { get; set; }
        public Nullable<decimal> total { get; set; }
        public Nullable<decimal> totalNet { get; set; }
        public Nullable<decimal> paid { get; set; }
        public Nullable<decimal> deserved { get; set; }


        public Nullable<decimal> discountValue { get; set; }
        public string discountType { get; set; }
        public Nullable<decimal> tax { get; set; }

        public string posName { get; set; }
        public string posCode { get; set; }
        public Nullable<long> invoiceId { get; set; }
        /*
        public async Task<List<Branch>> GetAll()
        {
            List<Branch> items = new List<Branch>();
            IEnumerable<Claim> claims = await APIResult.getList("Branches/GetAll");
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<Branch>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }
        public async Task<List<Branch>> Get(string type)
        {
            List<Branch> items = new List<Branch>();

            //  to pass parameters (optional)
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string myContent = type;
            parameters.Add("type", myContent);
            // 
            IEnumerable<Claim> claims = await APIResult.getList("Branches/Get", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<Branch>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }
        public async Task<Branch> getBranchById(long itemId)
        {
            Branch item = new Branch();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", itemId.ToString());
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Branches/GetBranchByID", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    item = JsonConvert.DeserializeObject<Branch>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                    break;
                }
            }
            return item;
        }
        public async Task<List<Branch>> GetBranchesActive(string type)
        {
            List<Branch> items = new List<Branch>();

            //  to pass parameters (optional)
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string myContent = type;
            parameters.Add("type", myContent);
            // 
            IEnumerable<Claim> claims = await APIResult.getList("Branches/GetActive", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<Branch>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }
       
        public async Task<List<Branch>> GetAllWithoutMain(string type)
        {
            List<Branch> items = new List<Branch>();

            //  to pass parameters (optional)
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string myContent = type;
            parameters.Add("type", myContent);
            // 
            IEnumerable<Claim> claims = await APIResult.getList("Branches/GetAllWithoutMain", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<Branch>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }
        
        public async Task<List<Branch>> BranchesByBranchandUser(long mainBranchId, long userId)
        {
            List<Branch> items = new List<Branch>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("mainBranchId", mainBranchId.ToString());
            parameters.Add("userId", userId.ToString());
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Branches/BranchesByBranchandUser", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<Branch>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }
        public async Task<long> save(Branch item)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Branches/Save";

            var myContent = JsonConvert.SerializeObject(item);
            parameters.Add("itemObject", myContent);
           return await APIResult.post(method, parameters);
        }
        public async Task<long> delete(long branchId, long userId, Boolean final)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", branchId.ToString());
            parameters.Add("userId", userId.ToString());
            parameters.Add("final", final.ToString());

            string method = "Branches/Delete";
           return await APIResult.post(method, parameters);
        }
        */
    }
}

