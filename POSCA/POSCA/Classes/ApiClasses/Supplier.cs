//using Newtonsoft.Json;
//using Newtonsoft.Json.Converters;
//using POSCA.Classes;
//using POSCA.ApiClasses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Windows;

namespace POSCA.Classes
{
    public class SupplierPhone
    {

        public int SupPhoneId { get; set; }
        public long SupId { get; set; }
        public int PhoneTypeID { get; set; }
        public string PhoneNumber { get; set; }
        public string PersonName { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }
    }
    public class SupplierSector
    {
        public long SupSectorId { get; set; }
        public Nullable<long> SupId { get; set; }
        public string SupSectorName { get; set; }
        public string Notes { get; set; }
        public decimal FreePercentageMarkets { get; set; }
        public Nullable<decimal> FreePercentageBranchs { get; set; }
        public Nullable<decimal> FreePercentageStores { get; set; }
        public decimal DiscountPercentageMarkets { get; set; }
        public Nullable<decimal> DiscountPercentageBranchs { get; set; }
        public Nullable<decimal> DiscountPercentageStores { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }

        public List<SupplierSectorSpecify> supplierSectorSpecifies { get; set; }
    }

    public class SupplierSectorSpecify
    {
        public long SupSectorSpecifyId { get; set; }
        public long SupId { get; set; }
        public long SupSectorId { get; set; }
        public long BranchId { get; set; }
        public decimal FreePercentage { get; set; }
        public decimal DiscountPercentage { get; set; }
        public string Notes { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }
    }

    public class SupplierDoc
    {
        #region Attributes
        public long DocumentId { get; set; }
        public Nullable<long> SupId { get; set; }
        public Nullable<long> TypeId { get; set; }
        public string DocName { get; set; }
        public string DocTitle { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }
        #endregion
    }
    public class Supplier
    {
        #region Attributes
        public long SupId { get; set; }
        public string SupRef { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Address { get; set; }
        public int SupplierTypeId { get; set; }
        public int SupplierGroupId { get; set; }
        public Nullable<long> AssistantSupId { get; set; }
        public Nullable<decimal> AssistantAccountNumber { get; set; }
        public string AssistantAccountName { get; set; }
        public Nullable<System.DateTime> AssistantStartDate { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal FreePercentag { get; set; }
        public Nullable<long> BankId { get; set; }
        public string BankAccount { get; set; }
        public Nullable<int> SupNODays { get; set; }
        public int AccountCode { get; set; }
        public string Email { get; set; }
        public string BOX { get; set; }
        public bool IsBlocked { get; set; }
        public string LicenseId { get; set; }
        public Nullable<System.DateTime> LicenseDate { get; set; }
        public string Notes { get; set; }
        public string PurchaseOrderNotes { get; set; }
        public string Image { get; set; }
        public bool IsAllowedPO { get; set; } = true;
        public bool IsAllowedReceipt { get; set; } = true;
        public bool IsAllowedDirectReturn { get; set; } = true;
        public bool IsAllowedReturnDiscount { get; set; } = true;
        public bool IsAllowCashingChecks { get; set; } = true;
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }


        public string SupplierGroup { get; set; }
        public string SupplierType { get; set; }

        public List<SupplierPhone> SupplierPhones { get; set; }
        public List<SupplierSector> SupplierSectors { get; set; }

        #endregion

        #region Methods
        public async Task<List<Supplier>> get(bool? isActive = null)
        {
            var result = new List<Supplier>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Supplier/Get";

            parameters.Add("isActive", isActive.ToString());

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<Supplier>(c.Value));
                }
            }
            return result;
        }

        public async Task<List<Supplier>> save(Supplier group)
        {
            var result = new List<Supplier>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Supplier/Save";

            var myContent = JsonConvert.SerializeObject(group);
            parameters.Add("itemObject", myContent);

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<Supplier>(c.Value));
                }
            }
            return result;
        }

        public async Task<List<Supplier>> delete(long supGroupId, long userId)
        {
            var result = new List<Supplier>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", supGroupId.ToString());
            parameters.Add("userId", userId.ToString());
            string method = "Supplier/delete";

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<Supplier>(c.Value));
                }
            }
            return result;
        }
        #endregion

    }

}

