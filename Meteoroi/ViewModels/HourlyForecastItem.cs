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
