using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Meteoroi.Converters
{
    public class ForecastTimeToUpdatedText : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                var ts = ((DateTimeOffset)value).ToLocalTime().ToString("t");
                return string.Concat("Updated as of ", ((DateTimeOffset)value).ToLocalTime().ToString("t"));
            }
            catch { return ""; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
