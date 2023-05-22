using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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
    public class CompanySettings
    {
        #region Attributs
        public long SettingId { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Notes { get; set; }
        #endregion

        #region Methods
        public async Task<List<CompanySettings>> Get()
        {

            List<CompanySettings> list = new List<CompanySettings>();

            IEnumerable<Claim> claims = await APIResult.getList("CompanySettings/Get");

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<CompanySettings>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;

        }

        public async Task<decimal> SaveList(List<CompanySettings> obj)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "companySettings/SaveList";

            var myContent = JsonConvert.SerializeObject(obj);
            parameters.Add("Object", myContent);
            return await APIResult.post(method, parameters);


        }

        public async Task<string> uploadImage(string imagePath, string imageName, long valId)
        {
            if (imagePath != "")
            {
                MultipartFormDataContent form = new MultipartFormDataContent();
                // get file extension
                var ext = imagePath.Substring(imagePath.LastIndexOf('.'));
                var extension = ext.ToLower();
                string fileName = imageName + extension;
                try
                {
                    // configure trmporery path
                    string dir = Directory.GetCurrentDirectory();
                    string tmpPath = Path.Combine(dir, AppSettings.TMPSettingFolder);

                    string[] files = System.IO.Directory.GetFiles(tmpPath, imageName + ".*");
                    foreach (string f in files)
                    {
                        System.IO.File.Delete(f);
                    }

                    tmpPath = Path.Combine(tmpPath, imageName + extension);
                    if (imagePath != tmpPath) // edit mode
                    {
                        // resize image
                        //ImageProcess imageP = new ImageProcess(150, imagePath);
                        //imageP.ScaleImage(tmpPath);

                        // read image file
                        var stream = new FileStream(tmpPath, FileMode.Open, FileAccess.Read);

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
                                Name = imageName,
                                FileName = fileName
                            };
                            form.Add(content, "fileToUpload");

                            var response = await client.PostAsync(@"companySettings/PostImage", form);
                        }
                        stream.Dispose();
                    }
                    // save image name in DB
                    CompanySettings setValues = new CompanySettings();
                    setValues.SettingId = valId;
                    setValues.Value = fileName;
                    await updateImage(setValues);
                    return fileName;
                }
                catch
                { return ""; }
            }
            return "";
        }

        public async Task<decimal> updateImage(CompanySettings setValues)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "companySettings/UpdateImage";

            var myContent = JsonConvert.SerializeObject(setValues);
            parameters.Add("Object", myContent);
            return await APIResult.post(method, parameters);

        }
            #endregion
        }
    }
