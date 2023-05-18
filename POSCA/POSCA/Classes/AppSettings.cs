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

       
        public static string APIUri = "http://localhost:7473/api/";


        #region folders Paths
        public const string TMPFolder = "Thumb";
        public const string TMPSupFolder = "Thumb/SupDocuments";
        #endregion

        //general info
        internal static string accuracy = "3";

        #region company info
        //default system info
        internal static string companyName;
        internal static string Address;

        internal static string Email;
        internal static string Mobile;
        internal static string Phone;
        internal static string Fax;
        internal static string logoImage;
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
