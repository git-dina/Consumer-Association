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
    public class changeBoxTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                string s = value as string;
                switch (value)
                {
                    case "change":
                        return AppSettings.resourcemanager.GetString("ChangeBoxNumber");
                    case "exchange":
                        return AppSettings.resourcemanager.GetString("SwitchWithAnotherContributor");
                    case "emptying":
                        return AppSettings.resourcemanager.GetString("ClearBoxNumber");
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
