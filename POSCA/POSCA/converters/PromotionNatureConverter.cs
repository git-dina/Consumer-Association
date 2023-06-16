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
     public class PromotionNatureConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                string s = value as string;
                switch (value)
                {
                    case "continual":
                        return AppSettings.resourcemanager.GetString("ContinualOffer");
                    case "temporary":
                        return AppSettings.resourcemanager.GetString("TemporaryOffer");
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
