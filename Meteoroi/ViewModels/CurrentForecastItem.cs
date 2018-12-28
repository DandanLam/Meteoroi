using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherService;

namespace Meteoroi.ViewModels
{
    public class CurrentForecastItem : ForecastItem
    {
        private string _Location;
        public string Location
        {
            get { return _Location; }
            set { SetProperty(_Location, value, () => _Location = value); }
        }
        private double _Temp;
        public double Temp
        {
            get
            {
                if (IsCelcius)
                    return Math.Round((_Temp - 32) * 5 / 9, 0);
                else
                    return Math.Round(_Temp, 0);
            }
            set { SetProperty(_Temp, value, () => _Temp = value); }
        }
        private double _ApparentTemp;
        public double ApparentTemp
        {
            get { return Math.Round(_ApparentTemp, 0); }
            set { SetProperty(_ApparentTemp, value, () => _ApparentTemp = value); }
        }
        private bool _IsCelcius;
        public bool IsCelcius
        {
            get { return _IsCelcius; }
            set {
                SetProperty(_IsCelcius, value, () => _IsCelcius = value);
                Temp = _Temp + .0000001;
                ApparentTemp = _ApparentTemp + .0000001;
            }
        }
        private string _ThisDaySummary;
        public string ThisDaySummary
        {
            get { return _ThisDaySummary; }
            set { SetProperty(_ThisDaySummary, value, () => _ThisDaySummary = value); }
        }
        private string _ThisHourSummary;
        public string ThisHourSummary
        {
            get { return _ThisHourSummary; }
            set { SetProperty(_ThisHourSummary, value, () => _ThisHourSummary = value); }
        }
        private string _ThisWeekSummary;
        public string ThisWeekSummary
        {
            get { return _ThisWeekSummary; }
            set { SetProperty(_ThisWeekSummary, value, () => _ThisWeekSummary = value); }
        }

        private DateTimeOffset _Sunrise;
        public DateTimeOffset Sunrise
        {
            get { return _Sunrise; }
            set { SetProperty(_Sunrise, value, () => _Sunrise = value); }
        }
        private DateTimeOffset _Sunset;
        public DateTimeOffset Sunset
        {
            get { return _Sunset; }
            set { SetProperty(_Sunset, value, () => _Sunset = value); }
        }

        public CurrentForecastItem(MomentForecast forecast) : base(forecast)
        {
            if (forecast == null)
                return;

            Time = forecast.Data.Time;
            Temp = forecast.Data.Temp.Current;
            ApparentTemp = forecast.Data.ApparnetTemp.Current;
            Sunrise = forecast.Data.SunriseTime;
            Sunset = forecast.Data.SunsetTime;
        }
    }
}
