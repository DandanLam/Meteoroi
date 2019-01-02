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
        private static int _Line1 = 7;
        public static int Line1
        {
            get { return _Line1; }
            set { _Line1 = value; }
        }
        private static int _Line2 = 0;
        public static int Line2
        {
            get { return _Line2; }
            set { _Line2 = value; }
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
