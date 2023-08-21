using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace POSCA.Classes.ApiClasses
{
    public class FamilyCard
    {
        #region Attributes
        public long FamilyCardId { get; set; }
        public Nullable<long> CustomerId { get; set; }
        public Nullable<System.DateTime> ReleaseDate { get; set; } = DateTime.Now;
        public Nullable<bool> IsStopped { get; set; }
        public string Notes { get; set; }
        public Nullable<bool> IsActive { get; set; } = true;
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }

        //extra
        public Nullable<long> BoxNumber { get; set; }
        public string CustomerName { get; set; }
        public string CustomerStatus { get; set; }
        public string CivilNum { get; set; }

        public string AutomatedNumber { get; set; }

        public List<Escort> Escorts { get; set; }
        #endregion

        #region Methods
        public async Task<FamilyCard> save(Customer customer)
        {
            var result = new FamilyCard();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Customer/SaveFamilyCard";

            var myContent = JsonConvert.SerializeObject(customer);
            parameters.Add("itemObject", myContent);

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result = JsonConvert.DeserializeObject<FamilyCard>(c.Value);
                }
            }
            return result;
        }
        #endregion
    }

    public class Escort
    {
        public long EscortId { get; set; }
        public Nullable<long> FamilyCardId { get; set; }
        public Nullable<long> CustomerId { get; set; }
        public string CivilNum { get; set; }
        public string EscortName { get; set; }
        public Nullable<int> KinshipId { get; set; }
        public Nullable<System.DateTime> AddedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }

        //extra
        public Nullable<long> BoxNumber { get; set; }
        public Nullable<bool> IsCustomer { get; set; } = false;
        public string KinshipName { get; set; }
    }
}
