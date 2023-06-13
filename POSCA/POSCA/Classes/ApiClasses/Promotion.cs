using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace POSCA.Classes.ApiClasses
{
    public class Promotion
    {
        #region Attributes
        public long PromotionId { get; set; }
        public string RefId { get; set; }
        public Nullable<System.DateTime> PromotionDate { get; set; } = DateTime.Now;
        public Nullable<System.DateTime> PromotionStartDate { get; set; } = DateTime.Now;
        public Nullable<System.DateTime> PromotionEndDate { get; set; } = DateTime.Now;
        public bool IsStoped { get; set; }
        public Nullable<System.DateTime> StopedDate { get; set; }
        public Nullable<long> StopedBy { get; set; }
        public string PromotionCategory { get; set; }
        public string PromotionType { get; set; }
        public string PromotionNature { get; set; }
        public decimal PromotionPercentage { get; set; }
        public int? PromotionQuantity { get; set; }
        public string Notes { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }

        //extra 
        public List<PromotionDetails> PromotionDetails { get; set; }
        public List<PromotionLocations> PromotionLocations { get; set; }
        #endregion

        #region Methods
        public async Task<Promotion> SaveReceiptOrder(Receipt invoice)
        {
            var result = new Promotion();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Promotion/SavePromotion";

            var myContent = JsonConvert.SerializeObject(invoice);
            parameters.Add("itemObject", myContent);

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result = JsonConvert.DeserializeObject<Promotion>(c.Value);
                }
            }
            return result;
        }
        #endregion
    }

    public class PromotionDetails
    {
        #region Attributes
        public long DetailsId { get; set; }
        public Nullable<long> PromotionId { get; set; }
        public Nullable<long> ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public string Barcode { get; set; }
        public Nullable<int> UnitId { get; set; }
        public string UnitName { get; set; }
        public Nullable<int> Factor { get; set; }
        public decimal MainCost { get; set; }
        public decimal MainPrice { get; set; }
        public decimal PromotionPrice { get; set; }
        public decimal NetDeffirence { get; set; }
        public decimal Qty { get; set; }
        public decimal PromotionPercentage { get; set; }
        public bool IsItemStoped { get; set; }
        public Nullable<long> StoppedItemBy { get; set; }
        public Nullable<System.DateTime> StoppedItemDate { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }
        #endregion

        #region extra attributes
        public int Sequence { get; set; }
        public bool IsSelected { get; set; }

        #endregion
    }

    public class PromotionLocations
    {
        public long PromotionLocationId { get; set; }
        public Nullable<long> LocationId { get; set; }
        public Nullable<long> PromotionId { get; set; }
    }
}
