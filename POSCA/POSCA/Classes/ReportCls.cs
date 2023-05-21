using Microsoft.Reporting.WebForms;
using Microsoft.Reporting.WinForms;
using POSCA.Classes.ApiClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace POSCA.Classes
{
    public class reportSize
    {

        public string reppath { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public string printerName { get; set; }
        public string paperSize { get; set; }
    }
    class ReportCls
    {
        public string GetLogoImagePath()
        {
            try
            {
                string imageName = AppSettings.companylogoImage;
                string dir = Directory.GetCurrentDirectory();
                string tmpPath = Path.Combine(dir, @"Thumb\setting");
                tmpPath = Path.Combine(tmpPath, imageName);
                if (File.Exists(tmpPath))
                {

                    return tmpPath;
                }
                else
                {
                    return Path.Combine(Directory.GetCurrentDirectory(), @"Thumb\setting\emptylogo.png");
                }

            }
            catch
            {
                return Path.Combine(Directory.GetCurrentDirectory(), @"Thumb\setting\emptylogo.png");
            }
        }
        public int GetpageHeight(int itemcount, int repheight)
        {
            // int repheight = 457;
            int tableheight = 33 * itemcount;// 33 is cell height


            int totalheight = repheight + tableheight;
            return totalheight;

        }

        public reportSize GetSupplyingOrderRdlcpath(PurchaseInvoice invoice, int itemscount, string PaperSize)
        {
            string addpath;
            reportSize rs = new reportSize();
            rs.width = 224;//224 =5.7cm
            rs.height = GetpageHeight(itemscount, 500);

            if (AppSettings.lang == "ar")
            {

                //order Ar
                addpath = @"\Reports\ar\supplyingOrder.rdlc";              
                
            }
            else 
            {
                addpath = @"\Reports\en\supplyingOrder.rdlc";
            }
          
            //
            string reppath = PathUp(addpath);
            rs.reppath = reppath;
            rs.paperSize = PaperSize;
            return rs;
        }

        public List<ReportParameter> fillSupplyingOrderReport(PurchaseInvoice invoice, List<ReportParameter> paramarr)
        {

            paramarr.Add(new ReportParameter("invNumber", invoice.InvNumber == null ? "-" : invoice.InvNumber.ToString()));//paramarr[6]

            paramarr.Add(new ReportParameter("title", AppSettings.resourcemanager.GetString("ProcurementRequest")));
            paramarr.Add(new ReportParameter("trLocation", AppSettings.resourcemanager.GetString("Location")));
            paramarr.Add(new ReportParameter("trSupplierName", AppSettings.resourcemanager.GetString("SupplierName")));
            paramarr.Add(new ReportParameter("trDocumentDate",AppSettings.resourcemanager.GetString("DocumentDate")));
            paramarr.Add(new ReportParameter("trItemNum", AppSettings.resourcemanager.GetString("ItemNumber")));

            paramarr.Add(new ReportParameter("LocationName", invoice.LocationName == null ? "-" : invoice.LocationName.ToString()));
            paramarr.Add(new ReportParameter("SupplierName", invoice.SupplierName == null ? "-" : invoice.SupplierName.ToString()));
            paramarr.Add(new ReportParameter("DocumentDate", HelpClass.DateToString(invoice.OrderDate) == null ? "-" : HelpClass.DateToString(invoice.OrderDate)));
           
            return paramarr;
        }
        public string PathUp( string addtopath)
        {
            string path = Directory.GetCurrentDirectory();
            string newPath = path + addtopath;
            try
            {
                FileAttributes attr = File.GetAttributes(newPath);
                if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                { }
                else
                {
                    string finalDir = Path.GetDirectoryName(newPath);
                    if (!Directory.Exists(finalDir))
                        Directory.CreateDirectory(finalDir);
                    if (!File.Exists(newPath))
                        File.Create(newPath);
                }
            }
            catch { }
            return newPath;
        }
    }
}
