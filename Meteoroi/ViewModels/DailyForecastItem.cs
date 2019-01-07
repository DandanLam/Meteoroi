using StorageService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherService;

namespace Meteoroi.ViewModels
{
    public class DailyForecastItem : ForecastItem
    {
        public DateTimeOffset SunriseTime { get; set; }
        public DateTimeOffset SunsetTime { get; set; }
        public double MoonPhase { get; set; }
        public double Humidity { get; set; }
        public double Ozone { get; set; }
        public double CloudCover { get; set; }
        public Percipitation Percipitation { get; set; }
        public double Pressure { get; set; }
        public Temperature Temp { get; set; }
        public Temperature ApparentTemp { get; set; }
        public double UvIndex { get; set; }
        public DateTimeOffset UvIndexTime { get; set; }
        public double Visibility { get; set; }
        public Wind Wind { get; set; }

        private static string _DateForemat = Settings.DateFormat;
        public static string DateForemat
        {
            get { return _DateForemat; }
            set
            {
                _DateForemat = value;
                Settings.DateFormat = value;
            }
        }
        private static bool _RealTemp = Settings.DailyShowRealTemp;
        public static bool RealTemp
        {
            get { return _RealTemp; }
            set { _RealTemp = value; 
                Settings.DailyShowRealTemp = value;
            }
        }
        private static bool _ShowIcon = Settings.ShowDailyIcon;
        public static bool ShowIcon
        {
            get { return _ShowIcon; }
            set { _ShowIcon = value;
                Settings.ShowDailyIcon = value;
            }
        }
        private static int _Line1 = Settings.DailyLine1;
        public static int Line1
        {
            get {
                if (StoreService.IsProUnlocked())
                    return Settings.DailyLine1;
                return _Line1; }
            set { _Line1 = value;
                Settings.DailyLine1 = value;
            }
        }
        private static int _Line2 = Settings.DailyLine2;
        public static int Line2
        {
            get {
                if (StoreService.IsProUnlocked())
                    return Settings.DailyLine2;
                return _Line2; }
            set {
                _Line2 = value;
                Settings.DailyLine2 = value;
            }
        }

        public DailyForecastItem(Data forecast) : base(forecast)
        {
            if (forecast == null)
                return;

            SunriseTime = forecast.SunriseTime;
            SunsetTime = forecast.SunsetTime;
            MoonPhase = forecast.MoonPhase;
            Humidity = forecast.Humidity;
            Ozone = forecast.Ozone;
            CloudCover = forecast.CloudCover;
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
