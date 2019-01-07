using StorageService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherService;

namespace Meteoroi.ViewModels
{
    public class HourlyForecastItem : ForecastItem
    {
        public double Humidity { get; set; }
        public double Ozone { get; set; }
        public Percipitation Percipitation { get; set; }
        public double Pressure { get; set; }
        public Temperature Temp { get; set; }
        public Temperature ApparentTemp { get; set; }
        public double UvIndex { get; set; }
        public DateTimeOffset UvIndexTime { get; set; }
        public double Visibility { get; set; }
        public Wind Wind { get; set; }

        private static string _TimeFormat = Settings.TimeFormat;
        public static string TimeFormat
        {
            get { return _TimeFormat; }
            set { _TimeFormat = value;
                Settings.TimeFormat = value;
            }
        }
        public static bool TimeFormatToLower { get; set; }
        private static bool _RealTemp = Settings.HourlyShowRealTemp;
        public static bool RealTemp
        {
            get { return _RealTemp; }
            set
            {
                _RealTemp = value;
                Settings.HourlyShowRealTemp = value;
            }
        }
        private static bool _ShowIcon = Settings.ShowHourlyIcon;
        public static bool ShowIcon
        {
            get { return _ShowIcon; }
            set { _ShowIcon = value;
                Settings.ShowHourlyIcon = value;
            }
        }

        private static int _Line1 = Settings.HourlyLine1;
        public static int Line1
        {
            get { return _Line1; }
            set { _Line1 = value;
                Settings.HourlyLine1 = value;
            }
        }
        private static int _Line2 = Settings.HourlyLine2;
        public static int Line2
        {
            get { return _Line2; }
            set { _Line2 = value;
                Settings.HourlyLine2 = value;
            }
        }
        private static int _Line3 = Settings.HourlyLine3;
        public static int Line3
        {
            get { return _Line3; }
            set { _Line3 = value;
                Settings.HourlyLine3 = value;
            }
        }

        public HourlyForecastItem(Data forecast) : base(forecast)
        {
            if (forecast == null)
                return;

            Humidity = forecast.Humidity;
            Ozone = forecast.Ozone;
            Percipitation = forecast.Percipitation;
            Pressure = forecast.Pressure;
            Temp = forecast.Temp;
            ApparentTemp = forecast.ApparnetTemp;
            UvIndex = forecast.UvIndex;
            UvIndexTime = forecast.UvIndexTime;
            Visibility = forecast.Visibility;
            Wind = forecast.Wind;
        }
    }
}
