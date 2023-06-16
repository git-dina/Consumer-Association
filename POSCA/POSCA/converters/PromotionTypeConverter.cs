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
     public class PromotionTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {

                string s = value as string;
                switch (value)
                {
                    case "quantity":
                        return AppSettings.resourcemanager.GetString("QuantityOffer");
                    case "percentage":
                        return AppSettings.resourcemanager.GetString("PercentageOffer");
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
