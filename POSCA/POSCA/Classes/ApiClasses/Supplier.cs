//using Newtonsoft.Json;
//using Newtonsoft.Json.Converters;
//using POSCA.Classes;
//using POSCA.ApiClasses;
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
        public int DiscountPercentage { get; set; }
        public int FreePercentag { get; set; }
        public Nullable<int> BankId { get; set; }
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
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }

        public List<SupplierPhone> SupplierPhones { get; set; }
        public List<SupplierSector> SupplierSectors { get; set; }
        #endregion

        /*
        public async Task<List<Vendor>> Get(string type)
        {
            List<Vendor> items = new List<Vendor>();

            //  to pass parameters (optional)
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string myContent = type;
            parameters.Add("type", myContent);
            // 
            IEnumerable<Claim> claims = await APIResult.getList("Vendor/Get", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<Vendor>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }

        public async Task<List<Vendor>> GetAll()
        {
            List<Vendor> items = new List<Vendor>();

            IEnumerable<Claim> claims = await APIResult.getList("Vendor/GetAll");

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<Vendor>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }
        public async Task<List<Vendor>> GetVendorsActive(string type)
        {
            List<Vendor> items = new List<Vendor>();

            //  to pass parameters (optional)
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string myContent = type;
            parameters.Add("type", myContent);
            // 
            IEnumerable<Claim> claims = await APIResult.getList("Vendor/GetActive", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<Vendor>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }

        public async Task<List<Vendor>> GetActiveForAccount(string type , string payType)
        {
            List<Vendor> items = new List<Vendor>();

            //  to pass parameters (optional)
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("type"   , type.ToString());
            parameters.Add("payType", payType.ToString());


            IEnumerable<Claim> claims = await APIResult.getList("Vendor/GetActiveForAccount", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<Vendor>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }

        public async Task<Vendor> getVendorById(long vendorId)
        {
            Vendor vendor = new Vendor();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("vendorId", vendorId.ToString());
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Vendor/GetVendorByID", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    vendor = JsonConvert.DeserializeObject<Vendor>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                    break;
                }
            }
            return vendor;
        }
        /// </summary>
        /// <param name="vendor"></param>
        /// <returns></returns>
        /// 

        public async Task<long> save(Vendor vendor)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Vendor/Save";

            var myContent = JsonConvert.SerializeObject(vendor);
            parameters.Add("itemObject", myContent);
           return await APIResult.post(method, parameters);
        }
    
        /// ///////////////////////////////////////
        /// before
        /// //////////////////////////////////////

        public string EncodeNonAsciiCharacters(string value)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in value)
            {
                if (c > 127)
                {
                    // This character is too big for ASCII
                    string encodedValue = "\\u" + ((int)c).ToString("x4");
                    sb.Append(encodedValue);
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
        public string DecodeEncodedNonAsciiCharacters(string value)
        {
            return Regex.Replace(
                value,
                @"\\u(?<Value>[a-zA-Z0-9]{4})",
                m => {
                    return ((char)int.Parse(m.Groups["Value"].Value, NumberStyles.HexNumber)).ToString();
                });
        }
        // delete vendor
        public async Task<long> delete(long vendorId, long userId, Boolean final)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", vendorId.ToString());
            parameters.Add("userId", userId.ToString());
            parameters.Add("final", final.ToString());

            string method = "Vendor/Delete";
           return await APIResult.post(method, parameters);
        }

         
        //public async Task<Boolean> uploadImage(string imagePath, int vendorId)
        public async Task<string> uploadImage(string imagePath, string imageName, long vendorId)
        {
            if (imagePath != "")
            {
                //string imageName = vendorId.ToString();
                MultipartFormDataContent form = new MultipartFormDataContent();
                // get file extension
                var ext = imagePath.Substring(imagePath.LastIndexOf('.'));
                var extension = ext.ToLower();
                string fileName = imageName + extension;
                try
                {
                    // configure trmporery path
                    string dir = Directory.GetCurrentDirectory();
                    string tmpPath = Path.Combine(dir, Global.TMPVendorsFolder);
                    //create vendor folder
                    if (!Directory.Exists(tmpPath))
                        Directory.CreateDirectory(tmpPath);
                    string[] files = System.IO.Directory.GetFiles(tmpPath, imageName + ".*");
                    foreach (string f in files)
                    {
                        System.IO.File.Delete(f);
                    }

                    tmpPath = Path.Combine(tmpPath, imageName + extension);

                    if (imagePath != tmpPath) // edit mode
                    {
                        // resize image
                        ImageProcess imageP = new ImageProcess(150, imagePath);
                        imageP.ScaleImage(tmpPath);

                        // read image file
                        var stream = new FileStream(tmpPath, FileMode.Open, FileAccess.Read);

                        // create http client request
                        using (var client = new HttpClient())
                        {
                            client.BaseAddress = new Uri(Global.APIUri);
                            client.Timeout = System.TimeSpan.FromSeconds(3600);
                            string boundary = string.Format("----WebKitFormBoundary{0}", DateTime.Now.Ticks.ToString("x"));
                            HttpContent content = new StreamContent(stream);
                            content.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                            content.Headers.Add("client", "true");

                            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                            {
                                Name = imageName,
                                FileName = fileName
                            };
                            form.Add(content, "fileToUpload");

                            var response = await client.PostAsync(@"vendor/PostUserImage", form);
                        }
                        stream.Dispose();
                    }
                    // save image name in DB
                    Vendor vendor = new Vendor();
                    vendor.vendorId = vendorId;
                    vendor.image = fileName;
                    await updateImage(vendor);

                    return fileName;
                }
                catch
                { return ""; }
            }
            return "";
        }
        // update image field in DB
        public async Task<int> updateImage(Vendor vendor)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            var myContent = JsonConvert.SerializeObject(vendor);
            parameters.Add("itemObject", myContent);

            string method = "Vendor/UpdateImage";
           return await APIResult.post(method, parameters);
        }
      
       
        public async Task<byte[]> downloadImage(string imageName)
        {
            byte[] byteImg = null;
            if (imageName != "")
            {
                byteImg = await APIResult.getImage("Vendor/GetImage", imageName);

                string dir = Directory.GetCurrentDirectory();
                string tmpPath = Path.Combine(dir, Global.TMPVendorsFolder);
                if (!Directory.Exists(tmpPath))
                    Directory.CreateDirectory(tmpPath);
                tmpPath = Path.Combine(tmpPath, imageName);
                if (System.IO.File.Exists(tmpPath))
                {
                    System.IO.File.Delete(tmpPath);
                }
                if (byteImg != null)
                {
                    using (FileStream fs = new FileStream(tmpPath, FileMode.Create, FileAccess.ReadWrite))
                    {
                        fs.Write(byteImg, 0, byteImg.Length);
                    }
                }

            }

            return byteImg;

        }
        //public async Task<int> updateBalance(int vendorId, decimal balance)
        //{
        //    Dictionary<string, string> parameters = new Dictionary<string, string>();
        //    parameters.Add("vendorId", vendorId.ToString());
        //    parameters.Add("balance", balance.ToString());

        //    string method = "Vendor/UpdateBalance";
        //   return await APIResult.post(method, parameters);
        //}



        public async Task<string> generateCodeNumber(string type)
        {
            int sequence = await GetLastNumOfCode(type);
            sequence++;
            string strSeq = sequence.ToString();
            if (sequence <= 999999)
                strSeq = sequence.ToString().PadLeft(6, '0');
            string transNum = type + "-" + strSeq;
            return transNum;
        }
        public async Task<int> GetLastNumOfCode(string type)
        {
            int value = 0 ;
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("type", type);
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Vendor/GetLastNumOfCode", parameters);


            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    
                    value =int.Parse(JsonConvert.DeserializeObject<String>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                    break;
                }
            }
            return value;
        }

        public async Task<List<Vendor>> GetVendorsByMembershipId(long membershipId)
        {
            List<Vendor> items = new List<Vendor>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", membershipId.ToString());
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Vendor/GetVendorsByMembershipId", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<Vendor>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));

                }
            }
            return items;
        }
        */
    }

}

