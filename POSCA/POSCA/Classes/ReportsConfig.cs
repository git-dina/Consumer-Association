using Microsoft.Reporting.WebForms;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSCA.Classes
{
    class ReportsConfig
    {
        public static void setReportLanguage(List<ReportParameter> paramarr)
        {

            paramarr.Add(new ReportParameter("lang", AppSettings.lang));

        }

        public static void ExportToPDF(LocalReport report, String FullPath)
        {

            string deviceInfo = string.Format(
          CultureInfo.InvariantCulture,
          "<DeviceInfo>" +
              "<OutputFormat>PDF</OutputFormat>" +
          "</DeviceInfo>");
            

            byte[] Bytes = report.Render(format: "PDF", deviceInfo);

            try
            {
                using (FileStream stream = new FileStream(FullPath, FileMode.Create))
                {
                    try
                    {
                        stream.Write(Bytes, 0, Bytes.Length);
                        stream.Close();

                    }
                    catch
                    {

                    }
                    finally
                    {
                        stream.Close();
                    }
                }
            }
            catch { }

        }
        public static void InvoiceHeader(List<ReportParameter> paramarr)
        {

            ReportCls rep = new ReportCls();

            paramarr.Add(new ReportParameter("companyName", AppSettings.companyName == null? "-": AppSettings.companyName));
            paramarr.Add(new ReportParameter("Address", AppSettings.companyAddress == null? "-": AppSettings.companyAddress));
           
            paramarr.Add(new ReportParameter("trcomAddress", AppSettings.resourcemanager.GetString("trAddress")));
            paramarr.Add(new ReportParameter("trcomTel", AppSettings.resourcemanager.GetString("tel")));
            paramarr.Add(new ReportParameter("trcomFax", AppSettings.resourcemanager.GetString("fax")));
            paramarr.Add(new ReportParameter("trcomEmail", AppSettings.resourcemanager.GetString("email")));
            //
            paramarr.Add(new ReportParameter("Fax", AppSettings.companyFax == null? "-" : AppSettings.companyFax.Replace("--", "")));
            paramarr.Add(new ReportParameter("Tel", AppSettings.companyPhone == null? "-" : AppSettings.companyPhone.Replace("--", "")));

            paramarr.Add(new ReportParameter("Email", AppSettings.companyEmail == null? "-" : AppSettings.companyEmail));
            paramarr.Add(new ReportParameter("logoImage", "file:\\" + rep.GetLogoImagePath()));

            string iconname = AppSettings.companylogoImage;//temp value

            paramarr.Add(new ReportParameter("com_mobile", AppSettings.companyMobile == null ? "-": AppSettings.companyMobile.Replace("--", "")));
        }

       
    }
}
