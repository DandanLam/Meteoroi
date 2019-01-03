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

        public static string TimeForemat { get; set; }
        public static bool TimeForematToLower { get; set; }
        private static bool _RealTemp = true;
        public static bool RealTemp
        {
            get { return _RealTemp; }
            set { _RealTemp = value; }
        }
        private static bool _ShowIcon = true;
        public static bool ShowIcon
        {
            get { return _ShowIcon; }
            set { _ShowIcon = value; }
        }
        public static int Line1
        {
            get { return Settings.HourlyLine1; }
            set { Settings.HourlyLine1 = value; }
        }
        public static int Line2
        {
            get { return Settings.HourlyLine2; }
            set { Settings.HourlyLine2 = value; }
        }
        public static int Line3
        {
            get { return Settings.HourlyLine3; }
            set { Settings.HourlyLine3 = value; }
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
