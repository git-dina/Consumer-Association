using POSCA.Classes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace POSCA.converters
{
     public class receiptTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {

                string s = value as string;
                switch (value)
                {
                    case "purchaseOrders":
                        return AppSettings.resourcemanager.GetString("PurchaseOrders");
                   case "direct":
                        return AppSettings.resourcemanager.GetString("Direct");
                   case "vegetable":
                        return AppSettings.resourcemanager.GetString("Vegetable");
                   case "service":
                        return AppSettings.resourcemanager.GetString("Service");
                   case "free":
                        return AppSettings.resourcemanager.GetString("trFree");
                   case "freeVegetables":
                        return AppSettings.resourcemanager.GetString("FreeVegetables");
                   case "customFree":
                        return AppSettings.resourcemanager.GetString("CustomFree");
                    default:
                        return "";
                }
            }
            catch
            {
                return "";
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
