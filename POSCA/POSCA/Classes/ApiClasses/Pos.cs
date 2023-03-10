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
   public class Pos
    {
        public long posId { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public decimal balance { get; set; }
        public long branchId { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<long> createUserId { get; set; }
        public Nullable<long> updateUserId { get; set; }
        public byte isActive { get; set; }
        public string notes { get; set; }
        public decimal balanceAll { get; set; }
        public string branchName { get; set; }
        public string branchCode { get; set; }
        public Boolean canDelete { get; set; }

        public string boxState { get; set; }
        public byte isAdminClose { get; set; }
        /*
        public async Task<List<Pos>> Get()
        {
            List<Pos> items = new List<Pos>();
            IEnumerable<Claim> claims = await APIResult.getList("Pos/get");
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<Pos>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }
        public async Task<Pos> EditBalance(long posId, decimal balance)
        {
            Pos item = new Pos();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Pos/EditBalance";

            parameters.Add("posId", posId.ToString());
            parameters.Add("balance", balance.ToString());

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    item = JsonConvert.DeserializeObject<Pos>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                    break;
                }
            }
            return item;
        }
        public async Task<List<Pos>> GetUnactivated(long branchId)
        {
            List<Pos> items = new List<Pos>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("branchId", branchId.ToString());
            IEnumerable<Claim> claims = await APIResult.getList("Pos/GetUnactivated", parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<Pos>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }
        public async Task<Pos> getById(long itemId)
        {
            Pos item = new Pos();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", itemId.ToString());
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Pos/GetPosByID", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    item = JsonConvert.DeserializeObject<Pos>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                    break;
                }
            }
            return item;
        }
        public async Task<long> save(Pos item)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Pos/Save";
            var myContent = JsonConvert.SerializeObject(item);
            parameters.Add("itemObject", myContent);
           return await APIResult.post(method, parameters);
        }
        public async Task<long> delete(long itemId, long userId, Boolean final)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", itemId.ToString());
            parameters.Add("userId", userId.ToString());
            parameters.Add("final", final.ToString());
            string method = "Pos/Delete";
           return await APIResult.post(method, parameters);
        }

        public async Task<decimal> updateBoxState(long posId, string state, int isAdminClose, long userId, CashTransfer cashTransfer)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Pos/updateBoxState";
            parameters.Add("posId", posId.ToString());
            parameters.Add("state", state);
            parameters.Add("isAdminClose", isAdminClose.ToString());
            parameters.Add("userId", userId.ToString());
            var myContent = JsonConvert.SerializeObject(cashTransfer);
            parameters.Add("cashTransfer", myContent);
            return await APIResult.PostReturnDecimal(method, parameters);
        }

        public async Task<List<Pos>> GetByUserId(long userId)
        {
            List<Pos> items = new List<Pos>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", userId.ToString());

            IEnumerable<Claim> claims = await APIResult.getList("Pos/GetByUserId", parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<Pos>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }
        */
    }
}
