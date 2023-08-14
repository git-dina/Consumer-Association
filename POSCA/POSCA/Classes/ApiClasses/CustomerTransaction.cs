using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace POSCA.Classes.ApiClasses
{
    public class CustomerTransaction
    {
        #region Attributes
        public long TransactionId { get; set; }
        public Nullable<long> CustomerId { get; set; }
        public Nullable<long> BoxNumber { get; set; }
        public Nullable<System.DateTime> TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public Nullable<int> TransactionStocksCount { get; set; }
        public Nullable<int> StocksCount { get; set; }
        public decimal StocksPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string ApprovalNumber { get; set; }
        public Nullable<System.DateTime> MeetingDate { get; set; }
        public string CheckNumber { get; set; }
        public Nullable<System.DateTime> CheckDate { get; set; }
        public string Notes { get; set; }
        public Nullable<long> ToCustomerId { get; set; }
        public Nullable<long> ToBoxNumber { get; set; }
        public Nullable<int> BondNo { get; set; }
        public Nullable<System.DateTime> BondDate { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }

        //extra
        public int JoinDay { get; set; }
        public int JoinMonth{ get; set; }
        public int JoinYear{ get; set; }

        // customer
        public string CustomerName { get; set; }
        public Nullable<System.DateTime> JoinDate { get; set; }


        #endregion

        #region
        internal async Task<int> AddTransaction(CustomerTransaction transaction)
        {
            int result =0;
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "CustomerTransaction/AddTransaction";

            var myContent = JsonConvert.SerializeObject(transaction);
            parameters.Add("itemObject", myContent);

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                     result = JsonConvert.DeserializeObject<int>(c.Value);
                }
            }
            return result;
        }
        #endregion
    }
}
