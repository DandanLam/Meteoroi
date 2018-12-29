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
