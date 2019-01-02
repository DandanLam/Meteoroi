using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherService;
using Windows.UI.Xaml.Data;

namespace Meteoroi.Converters
{
    public class WindSpeedUnit : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                return (bool)value ? " km/h" : "mph";
            }
            catch { return ""; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    
}
