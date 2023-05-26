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
        public string GetIconImagePath(string iconName)
        {
            try
            {
                string imageName = iconName + ".png";
                string dir = Directory.GetCurrentDirectory();
                string tmpPath = Path.Combine(dir, @"pic");
                tmpPath = Path.Combine(tmpPath, imageName);
                if (File.Exists(tmpPath))
                {

                    return tmpPath;
                }
                else
                {
                    return Path.Combine(Directory.GetCurrentDirectory(), @"Thumb\setting\emptylogo.png");
                }



                //string addpath = @"\Thumb\setting\" ;

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

        public string GetSupplyingOrderRdlcpath( )
        {
            string addpath;
            //rs.width = 224;//224 =5.7cm
            //rs.height = GetpageHeight(itemscount, 500);

            if (AppSettings.lang == "ar")
            {

                //order Ar
                addpath = @"\Reports\ar\Report1.rdlc";              
                
            }
            else 
            {
                addpath = @"\Reports\en\supplyingOrder.rdlc";
            }
          
            //
            string reppath = PathUp(addpath);
  
            return reppath;
        }

        public List<ReportParameter> fillSupplyingOrderReport(PurchaseInvoice invoice, LocalReport rep, List<ReportParameter> paramarr)
        {
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();

            paramarr.Add(new ReportParameter("OrderDate", HelpClass.DateToString(invoice.OrderDate)));
            paramarr.Add(new ReportParameter("invNumber", invoice.InvNumber == null ? "-" : invoice.InvNumber.ToString()));//paramarr[6]
            paramarr.Add(new ReportParameter("LocationName", invoice.LocationName == null ? "-" : invoice.LocationName.ToString()));
            paramarr.Add(new ReportParameter("OrderRecieveDate", HelpClass.DateToString(invoice.OrderRecieveDate)));
            paramarr.Add(new ReportParameter("SupplierNumber",AppSettings.resourcemanager.GetString("SupplierNumber")+": "+ invoice.supplier.SupId.ToString()));
            paramarr.Add(new ReportParameter("SupplierName", invoice.supplier.Name));

            paramarr.Add(new ReportParameter("Title", AppSettings.resourcemanager.GetString("ProcurementRequest")));
            paramarr.Add(new ReportParameter("trDate", AppSettings.resourcemanager.GetString("trDate")));
            paramarr.Add(new ReportParameter("trTheProcurementRequest", AppSettings.resourcemanager.GetString("TheProcurementRequest")));
            paramarr.Add(new ReportParameter("trToBranch", AppSettings.resourcemanager.GetString("trToBranch")));
            paramarr.Add(new ReportParameter("trDeliveryDate", AppSettings.resourcemanager.GetString("DeliveryDate")));
            paramarr.Add(new ReportParameter("trOrderDescription", AppSettings.resourcemanager.GetString("SupplymentOrderDescription")));
            paramarr.Add(new ReportParameter("trSupplierName", AppSettings.resourcemanager.GetString("SupplierName")+":"));
            paramarr.Add(new ReportParameter("trTotalSale", AppSettings.resourcemanager.GetString("trTotalSale")));
            paramarr.Add(new ReportParameter("trTotalCost", AppSettings.resourcemanager.GetString("trTotalPurchase")));
            paramarr.Add(new ReportParameter("trSeuenceAbbrevation", AppSettings.resourcemanager.GetString("SeuenceAbbrevation")));
            paramarr.Add(new ReportParameter("trItemCode", AppSettings.resourcemanager.GetString("ItemNumber")));
            paramarr.Add(new ReportParameter("trBarcode", AppSettings.resourcemanager.GetString("trBarcode")));
            paramarr.Add(new ReportParameter("trDescription", AppSettings.resourcemanager.GetString("trDescription")));
            paramarr.Add(new ReportParameter("trQTR", AppSettings.resourcemanager.GetString("trQTR")));
            paramarr.Add(new ReportParameter("trUnit", AppSettings.resourcemanager.GetString("trUnit")));
            paramarr.Add(new ReportParameter("trFactor", AppSettings.resourcemanager.GetString("Factor")));
            paramarr.Add(new ReportParameter("trBalance", AppSettings.resourcemanager.GetString("trBalance")));
            paramarr.Add(new ReportParameter("trPurchasePrice", AppSettings.resourcemanager.GetString("PurchasePrice")));
            paramarr.Add(new ReportParameter("trSalePrice", AppSettings.resourcemanager.GetString("SalePrice")));

            //paramarr.Add(new ReportParameter("trItemNum", AppSettings.resourcemanager.GetString("ItemNumber")));


            //dataSet
            rep.DataSources.Add(new ReportDataSource("DataSetPurchaseDetails", invoice.PurchaseDetails));

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
