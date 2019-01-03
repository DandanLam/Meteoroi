using Meteoroi.ViewModels;
using StorageService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Meteoroi.Converters
{
    public class CurrentForecastAltTempVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                return !(bool)value ? Visibility.Visible : Visibility.Collapsed;
            }
            catch { return Visibility.Collapsed; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    public class CurrentForecastRegTempVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                return (bool)value ? Visibility.Visible : Visibility.Collapsed;
            }
            catch { return Visibility.Collapsed; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    public class CurrentForecastWindSpeedVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                var cf = value as CurrentForecastItem;
                if (cf == null)
                    return Visibility.Collapsed;
                return cf.ShowWindSpeed ? Visibility.Visible : Visibility.Collapsed;
            }
            catch { return Visibility.Collapsed; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    public class CurrentForecastVisibilityVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                var cf = value as CurrentForecastItem;
                if (cf == null)
                    return Visibility.Collapsed;
                return cf.ShowVisibility ? Visibility.Visible : Visibility.Collapsed;
            }
            catch { return Visibility.Collapsed; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    public class CurrentForecastBarometerVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                var cf = value as CurrentForecastItem;
                if (cf == null)
                    return Visibility.Collapsed;
                return cf.ShowPressure ? Visibility.Visible : Visibility.Collapsed;
            }
            catch { return Visibility.Collapsed; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    public class CurrentForecastHumidityVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                var cf = value as CurrentForecastItem;
                if (cf == null)
                    return Visibility.Collapsed;
                return cf.ShowHumidity ? Visibility.Visible : Visibility.Collapsed;
            }
            catch { return Visibility.Collapsed; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    public class CurrentForecastDewPointVisibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                var cf = value as CurrentForecastItem;
                if (cf == null)
                    return Visibility.Collapsed;
                return cf.ShowDewPoint ? Visibility.Visible : Visibility.Collapsed;
            }
            catch { return Visibility.Collapsed; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    public class CurrentForecastAltTempText : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                return string.Concat(Math.Round((double)value, 0), "°");
            }
            catch { return ""; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    public class CurrentForecastWindSpeedText : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                return string.Concat(Math.Round((double)value, 0), Settings.IsMetric ? "km/h" : "mph");
            }
            catch { return ""; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    public class CurrentForecastVisibilityText : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                return string.Concat("Visibility: ", Math.Round((double)value, 0), Settings.IsMetric ? "km" : "mi");
            }
            catch { return ""; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    public class CurrentForecastBarometerText : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                return string.Concat("Barometer: ", Math.Round((double)value, 0), Settings.IsMetric ? "in" : "mb");
            }
            catch { return ""; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    public class CurrentForecastHumidityText : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                return string.Concat("Humidity: ", Math.Round((double)value * 100, 0), "%");
            }
            catch { return ""; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    public class CurrentForecastDewPointText : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                return string.Concat("Dew Point: ", Math.Round((double)value, 0), "°");
            }
            catch { return ""; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
