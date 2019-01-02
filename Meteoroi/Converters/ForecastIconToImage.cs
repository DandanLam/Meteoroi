using Meteoroi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
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

    public class DailyForecastIconVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                return DailyForecastItem.ShowIcon ? Visibility.Visible : Visibility.Collapsed;
            }
            catch { return Visibility.Collapsed; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class DailyForecastLine1Visibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                return DailyForecastItem.Line1 == 5 ? Visibility.Collapsed : Visibility.Visible;
            }
            catch { return Visibility.Collapsed; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class DailyForecastTempVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                return DailyForecastItem.Line1 == 5 ? Visibility.Visible : Visibility.Collapsed;
            }
            catch { return Visibility.Collapsed; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class DailyForecastTempHigh : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                var forecast = value as DailyForecastItem;
                if (DailyForecastItem.RealTemp)
                    return string.Concat(Math.Round((double)forecast.Temp.High, 0), "°");
                else
                    return string.Concat(Math.Round((double)forecast.ApparentTemp.High, 0), "°");
            }
            catch { return ""; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    public class DailyForecastTempLow : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                var forecast = value as DailyForecastItem;
                if (DailyForecastItem.RealTemp)
                    return string.Concat(Math.Round((double)forecast.Temp.Low, 0), "°");
                else
                    return string.Concat(Math.Round((double)forecast.ApparentTemp.Low, 0), "°");
            }
            catch { return ""; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    public class DailyForecastLineAlignment : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                if (DailyForecastItem.Line1 == 0)
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

    public class Line1MaxLines : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                int lines = 3;
                if (!DailyForecastItem.ShowIcon)
                    lines += 3;
                if (DailyForecastItem.Line1 == 5)
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

    public class Line2MaxLines : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                int lines = 3;
                if (!DailyForecastItem.ShowIcon)
                    lines += 3;
                if (DailyForecastItem.Line1 != 5)
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

    public class Line1Text : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                var forecast = value as DailyForecastItem;
                switch (DailyForecastItem.Line1)
                {
                    case 0: return forecast.Summary;
                    case 1: return string.Concat("Humidity: ", Math.Round((double)forecast.Humidity * 100, 0), "%");
                    case 2: return string.Concat("Percipitation: ", Math.Round((double)forecast.Percipitation.Probability * 100, 0), "%");
                    case 3: return string.Concat("Cloud Cover: ", Math.Round((double)forecast.CloudCover * 100, 0), "%");
                    case 4: return string.Concat("UV Index: ", forecast.UvIndex);
                    //case 7: return string.Concat("Wind: ", forecast.Wind.Speed);
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
    public class Line2Text : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                var forecast = value as DailyForecastItem;
                switch (DailyForecastItem.Line2)
                {
                    case 0: return forecast.Summary;
                    case 1: return string.Concat("Humidity: ", Math.Round((double)forecast.Humidity * 100, 0), "%"); 
                    case 2: return string.Concat("Percipitation: ", Math.Round((double)forecast.Percipitation.Probability * 100, 0), "%"); 
                    case 3: return string.Concat("Cloud Cover: ", Math.Round((double)forecast.CloudCover * 100, 0), "%"); 
                    case 4: return string.Concat("UV Index: ", forecast.UvIndex); 
                    //case 6: return string.Concat("Wind: ", forecast.Wind.Speed); 
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

    public class NumberToZeroDecimals : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                return Math.Round((double)value, 0).ToString();
            }
            catch { return ""; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    public class NumberToTwoDecimals : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                return Math.Round((double)value, 2).ToString();
            }
            catch { return ""; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
