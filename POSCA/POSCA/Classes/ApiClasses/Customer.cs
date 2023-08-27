using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace POSCA.Classes.ApiClasses
{
    public class Customer
    {
        #region Attributes
        public long CustomerId { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
        public string InvoiceName { get; set; }
        public string Gender { get; set; }
        //public string FundNumber { get; set; }
        public string CivilNum { get; set; }
       // public string FamilyCard { get; set; }
        public string MaritalStatus { get; set; }
        public Nullable<int> JobId { get; set; }
        public Nullable<System.DateTime> DOB { get; set; } = DateTime.Now;
        public Nullable<long> BoxNumber { get; set; }
        public string CustomerStatus { get; set; } = "continouse";
        public string MemberNature { get; set; }
        public Nullable<int> SessionNumber { get; set; }
        public Nullable<System.DateTime> JoinDate { get; set; } = DateTime.Now;
        public Nullable<int> ReceiptVoucherNumber { get; set; }
        public Nullable<System.DateTime> ReceiptVoucherDate { get; set; } = DateTime.Now;
        public int JoiningSharesCount { get; set; } = 5;
        public int SharesCount { get; set; }
        public bool CalculateEarnings { get; set; } = true;
        //public bool IsBlocked { get; set; }
        public string IBAN { get; set; }
        public Nullable<int> BankId { get; set; }
        public bool PrintNameOnInv { get; set; } = true;
        public bool RegisteredInMinistry { get; set; }
        public bool StopOnPOS { get; set; }
        public bool DataCompleted { get; set; }
        public string Notes { get; set; }
        public bool IsArchived { get; set; }
        public bool IsActive { get; set; } = true;
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }

        public CustomerAddress customerAddress { get; set; }

        public List<CustomerDocument> customerDocuments { get; set; }

        //extra 
        public int AllStocksCount { get; set; }
        public bool FamilyCardHolder { get; set; }
        public decimal CurrentPurchses { get; set; }
        public bool CanArchive { get; set; }

        public int JoinDay { get; set; }
        public int JoinMonth { get; set; }
        public int JoinYear { get; set; }


        #endregion

        #region Methods
        internal async Task<List<Customer>> get(bool v)
        {
            var result = new List<Customer>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Customer/Get";

            parameters.Add("isActive", v.ToString());

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<Customer>(c.Value));
                }
            }
            return result;
        }
        public async Task<List<Customer>> SearchCustomers(string textSearch)
        {
            var result = new List<Customer>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Customer/SearchCustomers";

            parameters.Add("textSearch", textSearch.ToString());

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<Customer>(c.Value));
                }
            }
            return result;
        }
        public async Task<List<FamilyCard>> SearchFamilyCardsCustomers(string textSearch)
        {
            var result = new List<FamilyCard>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Customer/SearchFamilyCardsCustomers";

            parameters.Add("textSearch", textSearch.ToString());

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result.Add(JsonConvert.DeserializeObject<FamilyCard>(c.Value));
                }
            }
            return result;
        }
        public async Task<String> GetMaxFundNum()
        {
            var result = "";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Customer/GetMaxFundNum";

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result = c.Value;
                }
            }
            return result;
        } 
        public async Task<String> GetMaxCustomerId()
        {
            var result = "";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Customer/GetMaxCustomerId";

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result = c.Value;
                }
            }
            return result;
        }
        internal async Task<bool> CheckBoxNumber(long fundNumber,long customerId)
        {
            bool result = false;
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Customer/CheckBoxNumber";

            parameters.Add("fundNumber", fundNumber.ToString());
            parameters.Add("customerId", customerId.ToString());

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result=bool.Parse( c.Value);
                }
            }
            return result;
        }
        internal async Task<Customer> GetById(long customerId)
        {
            var result = new Customer();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Customer/GetById";

            parameters.Add("customerId", customerId.ToString());

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result=JsonConvert.DeserializeObject<Customer>(c.Value);
                }
            }
            return result;
        }
        internal async Task<FamilyCard> GetFamilyCardById(long customerId)
        {
            var result = new FamilyCard();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Customer/GetFamilyCardById";

            parameters.Add("customerId", customerId.ToString());

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result=JsonConvert.DeserializeObject<FamilyCard>(c.Value);
                }
            }
            return result;
        }

        internal async Task<Customer> GetByBoxNumber(long boxNumber)
        {
            var result = new Customer();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Customer/GetByBoxNumber";

            parameters.Add("boxNumber", boxNumber.ToString());

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result=JsonConvert.DeserializeObject<Customer>(c.Value);
                }
            }
            return result;
        }





        public async Task<Customer> save(Customer customer)
        {
            var result = new Customer();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Customer/Save";

            var myContent = JsonConvert.SerializeObject(customer);
            parameters.Add("itemObject", myContent);

            IEnumerable<Claim> claims = await APIResult.getList(method, parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    result = JsonConvert.DeserializeObject<Customer>(c.Value);
                }
            }
            return result;
        }

        public async Task<string> uploadDocument(string documentPath, string documentName)
        {
            if (documentPath != "")
            {
                //string imageName = userId.ToString();
                MultipartFormDataContent form = new MultipartFormDataContent();
                // get file extension
                var ext = documentPath.Substring(documentPath.LastIndexOf('.'));
                var extension = ext.ToLower();
                string fileName = documentName + extension;
                try
                {
                    // configure trmporery path
                    string dir = Directory.GetCurrentDirectory();
                    string tmpPath = Path.Combine(dir, AppSettings.TMPSupFolder);

                    string[] files = System.IO.Directory.GetFiles(tmpPath, documentName + ".*");
                    foreach (string f in files)
                    {
                        System.IO.File.Delete(f);
                    }

                    tmpPath = Path.Combine(tmpPath, documentName + extension);
                    if (documentPath != tmpPath) // edit mode
                    {

                        // read document file
                        var stream = new FileStream(documentPath, FileMode.Open, FileAccess.Read);

                        // create http client request
                        using (var client = new HttpClient())
                        {
                            client.BaseAddress = new Uri(AppSettings.APIUri);
                            client.Timeout = System.TimeSpan.FromSeconds(3600);
                            string boundary = string.Format("----WebKitFormBoundary{0}", DateTime.Now.Ticks.ToString("x"));
                            HttpContent content = new StreamContent(stream);
                            content.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                            content.Headers.Add("client", "true");

                            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                            {
                                Name = documentName,
                                FileName = fileName
                            };
                            form.Add(content, "fileToUpload");

                            var response = await client.PostAsync(@"Customer/PostDocument", form);
                        }
                        stream.Dispose();
                    }
                    return fileName;
                }
                catch
                { return ""; }
            }
            return "";
        }

        public async Task<byte[]> downloadDocument(string documentName, string destinationDoc)
        {
            byte[] byteImg = null;
            if (documentName != "")
            {
                byteImg = await APIResult.getDocument("Customer/downloadDocument", documentName);

                if (byteImg != null)
                {
                    using (FileStream fs = new FileStream(destinationDoc, FileMode.Create, FileAccess.ReadWrite))
                    {
                        fs.Write(byteImg, 0, byteImg.Length);
                    }
                }

            }
            return byteImg;
        }
        #endregion
    }

    public class CustomerAddress
    {
        public long CustomerId { get; set; }
        public string AutomtedNumber { get; set; }
        public Nullable<int> AreaId { get; set; }
        public Nullable<int> SectionId { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public Nullable<int> Floor { get; set; }
        public string AvenueNumber { get; set; }
        public string Plot { get; set; }
        public string MailBox { get; set; }
        public string PostalCode { get; set; }
        public string Employer { get; set; }
        public string Guardian { get; set; }
        public string HomePhone { get; set; }
        public string WorkPhone { get; set; }
        public string MobileNumber { get; set; }
        public string MobileNumber2 { get; set; }
        public string WorkAddress { get; set; }
        public Nullable<int> KinshipId { get; set; }
    }

    public class CustomerDocument
    {
        public long DocumentId { get; set; }
        public Nullable<long> CustomerId { get; set; }
        public string DocName { get; set; }
        public string DocTitle { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<long> CreateUserId { get; set; }
        public Nullable<long> UpdateUserId { get; set; }

        //extra
        public bool IsEdited { get; set; }
        public string DocPath { get; set; }

    }
}
