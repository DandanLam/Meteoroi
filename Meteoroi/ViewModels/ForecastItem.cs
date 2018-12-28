using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherService;

namespace Meteoroi.ViewModels
{
    public abstract class ForecastItem : NotificationBase<Forecast>
    {
        private string _Summary;
        public string Summary
        {
            get { return _Summary; }
            set { SetProperty(_Summary, value, () => _Summary = value); }
        }
        private WeatherIcon _Icon;
        public WeatherIcon Icon
        {
            get { return _Icon; }
            set { SetProperty(_Icon, value, () => _Icon = value); }
        }
        private DateTimeOffset _Time;
        public DateTimeOffset Time
        {
            get { return _Time; }
            set { SetProperty(_Time, value, () => _Time = value); }
        }

        public double PrecipIntensity { get; set; }
        public double PrecipIntensityError { get; set; }
        public double PrecipProbability { get; set; }

        public ForecastItem(Forecast forecast)
        {
            if (forecast == null)
                return;
            Summary = forecast.Summary;
            Icon = GetWeatherIcon(forecast.Icon);
        }
        WeatherIcon GetWeatherIcon(string iconStr)
        {
            switch (iconStr)
            {
                case "clear-day":
                    return WeatherIcon.CLEAR_DAY;
                case "clear-night":
                    return WeatherIcon.CLEAR_NIGHT;
                case "cloudy":
                    return WeatherIcon.CLOUDY;
                case "partly-cloudy-day":
                    return WeatherIcon.PARTLY_CLOUDY_DAY;
                case "partly-cloudy-night":
                    return WeatherIcon.PARTLY_CLOUDY_NIGHT;
                case "fog":
                    return WeatherIcon.FOG;
                case "rain":
                    return WeatherIcon.RAIN;
                case "sleet":
                    return WeatherIcon.SLEET;
                case "snow":
                    return WeatherIcon.SNOW;
                case "wind":
                    return WeatherIcon.WIND;
                default:
                    throw new ArgumentException("Invalid icon string");
            }
        }
    }

    public enum WeatherIcon { CLEAR_DAY, CLEAR_NIGHT, CLOUDY, PARTLY_CLOUDY_DAY, PARTLY_CLOUDY_NIGHT, FOG, RAIN, SLEET, SNOW, WIND, }
}
