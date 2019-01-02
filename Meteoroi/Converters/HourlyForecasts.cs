using Meteoroi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Meteoroi.Converters
{
    public class HourlyForecastIconVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                return HourlyForecastItem.ShowIcon ? Visibility.Visible : Visibility.Collapsed;
            }
            catch { return Visibility.Collapsed; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class HourlyForecastTimeText : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                var timeString = ((HourlyForecastItem)value).Time.ToString(HourlyForecastItem.TimeForemat);
                return HourlyForecastItem.TimeForematToLower ? timeString.ToLower() : timeString;
            }
            catch { return Visibility.Collapsed; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class HourlyCurrentTemp : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                var forecast = value as HourlyForecastItem;
                if (HourlyForecastItem.RealTemp)
                    return string.Concat(Math.Round((double)forecast.Temp.Current, 0), "°");
                else
                    return string.Concat(Math.Round((double)forecast.ApparentTemp.Current, 0), "°");
            }
            catch { return Visibility.Collapsed; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class HourlyLine1MaxLines : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                int lines = 3;
                if (!HourlyForecastItem.ShowIcon)
                    lines += 3;
                if (HourlyForecastItem.Line1 == 5)
                    lines += 1;
                return lines;
            }
            catch { return 3; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class HourlyLine2MaxLines : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                int lines = 3;
                if (!HourlyForecastItem.ShowIcon)
                    lines += 3;
                if (HourlyForecastItem.Line1 != 5)
                    lines += 1;
                return lines;
            }
            catch { return 3; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class HourlyLine1Text : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                var forecast = value as HourlyForecastItem;
                switch (HourlyForecastItem.Line1)
                {
                    case 0: return forecast.Summary;
                    case 1: return string.Concat("Humidity: ", Math.Round((double)forecast.Humidity * 100, 0), "%");
                    case 2: return string.Concat("Percipitation: ", Math.Round((double)forecast.Percipitation.Probability * 100, 0), "%");
                    case 3: return string.Concat("UV Index: ", forecast.UvIndex);
                    case 4: return string.Concat("Wind: ", forecast.Wind.Speed, "mph");
                    default: return "";
                }
            }
            catch { return ""; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    public class HourlyLine2Text : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                var forecast = value as HourlyForecastItem;
                switch (HourlyForecastItem.Line2)
                {
                    case 0: return forecast.Summary;
                    case 1: return string.Concat("Humidity: ", Math.Round((double)forecast.Humidity * 100, 0), "%");
                    case 2: return string.Concat("Percipitation: ", Math.Round((double)forecast.Percipitation.Probability * 100, 0), "%");
                    case 3: return string.Concat("UV Index: ", forecast.UvIndex);
                    case 4: return string.Concat("Wind: ", forecast.Wind.Speed, "mph");
                    default: return "";
                }
            }
            catch { return ""; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    public class HourlyForecastLineAlignment : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                if (HourlyForecastItem.Line1 == 0)
                    return HorizontalAlignment.Stretch;
                else
                    return HorizontalAlignment.Center;
            }
            catch { return ""; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class HourlyForecastLine1Visibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                return HourlyForecastItem.Line1 == 5 ? Visibility.Collapsed : Visibility.Visible;
            }
            catch { return Visibility.Collapsed; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
