using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
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

        public static void InvoiceHeader(List<ReportParameter> paramarr)
        {

            ReportCls rep = new ReportCls();
            // AppSettings.lang;
           // if (AppSettings.lang == "en")
            {
                paramarr.Add(new ReportParameter("companyName", AppSettings.companyName));
                paramarr.Add(new ReportParameter("Address", AppSettings.companyAddress));
            }
            //else if (AppSettings.lang == "ar")
            //{
            //    paramarr.Add(new ReportParameter("companyName", AppSettings.com_name_ar));
            //    paramarr.Add(new ReportParameter("Address", AppSettings.com_address_ar));
            //}
           
            paramarr.Add(new ReportParameter("trcomAddress", AppSettings.resourcemanagerreport.GetString("trAddress")));
            paramarr.Add(new ReportParameter("trcomTel", AppSettings.resourcemanagerreport.GetString("tel")));
            paramarr.Add(new ReportParameter("trcomFax", AppSettings.resourcemanagerreport.GetString("fax")));
            paramarr.Add(new ReportParameter("trcomEmail", AppSettings.resourcemanagerreport.GetString("email")));
            //
            paramarr.Add(new ReportParameter("Fax", AppSettings.companyFax.Replace("--", "")));
            paramarr.Add(new ReportParameter("Tel", AppSettings.companyPhone.Replace("--", "")));

            paramarr.Add(new ReportParameter("Email", AppSettings.companyEmail));
            paramarr.Add(new ReportParameter("logoImage", "file:\\" + rep.GetLogoImagePath()));
            //social
            string iconname = AppSettings.companylogoImage;//temp value
            //paramarr.Add(new ReportParameter("com_tel_icon", "file:\\" + rep.GetIconImagePath("phone")));
            //paramarr.Add(new ReportParameter("com_fax_icon", "file:\\" + rep.GetIconImagePath("fax")));
            //paramarr.Add(new ReportParameter("com_email_icon", "file:\\" + rep.GetIconImagePath("email")));

            paramarr.Add(new ReportParameter("com_mobile", AppSettings.companyMobile.Replace("--", "")));
           // paramarr.Add(new ReportParameter("com_mobile_icon", "file:\\" + rep.GetIconImagePath("mobile")));
        }

       
    }
}
