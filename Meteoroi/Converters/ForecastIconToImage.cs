using Meteoroi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace Meteoroi.Converters
{
    public class ForecastIconToImage : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                var sb = new StringBuilder();
                sb.Append("ms-appx:///Assets/WeatherIcons/");
                sb.Append(((WeatherIcon)value).ToString());
                sb.Append(".png");
                return new BitmapImage(new Uri(sb.ToString()));
            }
            catch { return null; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
