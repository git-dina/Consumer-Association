using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
//using System.Deployment.Application;
using System.Reflection;

namespace POSCA.Classes
{
    public class AppSettings
    {

        public static ResourceManager resourcemanager;
        public static ResourceManager resourcemanagerreport;
        public static ResourceManager resourcemanagerAr;
        public static ResourceManager resourcemanagerEn;

       
        public static string APIUri = "http://localhost:7473/api/";


        #region folders Paths
        public const string TMPFolder = "Thumb";
        public const string TMPSupFolder = "Thumb/SupDocuments";
        #endregion

        //general info
        internal static string accuracy = "3";
        public static string dateFormat = "ShortDatePattern";

        #region company info
        //default system info
        internal static string companyName;
        internal static string companyAddress;

        internal static string companyEmail;
        internal static string companyMobile;
        internal static string companyPhone;
        internal static string companyFax;
        internal static string companylogoImage;
        #endregion

        #region report settings
        public static string supplyingOrderPaperSize = "5.7cm";
        #endregion
        // app version
        //static public string CurrentVersion
        //{
        //    get
        //    {
        //        return ApplicationDeployment.IsNetworkDeployed
        //               ? ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString()
        //               : Assembly.GetExecutingAssembly().GetName().Version.ToString();
        //    }
        //}

        public static string lang = "ar";


    }
}
