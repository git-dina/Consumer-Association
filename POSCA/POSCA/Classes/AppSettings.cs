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

       
        public static string APIUri = "http://145.239.195.166:44370/api/";

        #region folders Paths
        public const string TMPFolder = "Thumb";
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

        public static string lang;


    }
}
