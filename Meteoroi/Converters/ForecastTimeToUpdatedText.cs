using Meteoroi.ViewModels;
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

    public class DailyForecastDateText : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                var forecast = value as DailyForecastItem;
                return forecast.Time.ToLocalTime().ToString(DailyForecastItem.DateForemat);
            }
            catch { return ""; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class ForecastTimeToShortDateText : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                return ((DateTimeOffset)value).ToLocalTime().ToString("ddd  d");
            }
            catch { return ""; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class ForecastTimeToHourText : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                return ((DateTimeOffset)value).ToLocalTime().ToString("h tt").ToLower();
            }
            catch { return ""; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class ForecastTimeText : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                return ((DateTimeOffset)value).ToLocalTime().ToString("h:mm tt").ToUpper();
            }
            catch { return ""; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
