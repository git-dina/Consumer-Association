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
    public class RoleNameConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {

                switch (AppSettings.lang)
                {
                    case "ar":
                        return values[0];
                    case "en":
                        return values[1];

                    default:
                        return "";
                }
            }
            catch
            {
                return "";
            }
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {

            string[] values = null;
            if (value != null)
                return values = value.ToString().Split(' ');
            return values;
        }
    }
}
