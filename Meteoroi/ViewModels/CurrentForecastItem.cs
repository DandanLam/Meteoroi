using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherService;
using Windows.Services.Maps;

namespace Meteoroi.ViewModels
{
    public class CurrentForecastItem : ForecastItem
    {
        private MapLocation _Location;
        public MapLocation Location
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
            get
            {
                if (IsCelcius)
                    return Math.Round((_ApparentTemp - 32) * 5 / 9, 0);
                else
                    return Math.Round(_ApparentTemp, 0);
            }
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
                DewPoint = _DewPoint + .0000001;
            }
        }
        private bool _IsMetric;
        public bool IsMetric
        {
            get { return _IsMetric; }
            set
            {
                SetProperty(_IsMetric, value, () => _IsMetric = value);
                Pressure = _Pressure + .0000001;
                Visibility = _Visibility + .0000001;
                WindSpeed = _WindSpeed + .0000001;
                Gust = _Gust + .0000001;
                WindBearing = _WindBearing + .0000001;
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

        private double _WindSpeed;
        public double WindSpeed
        {
            get
            {
                if (IsMetric)
                    return Math.Round(_WindSpeed * 1.609344, 0);
                else
                    return Math.Round(_WindSpeed, 0);
            }
            set { SetProperty(_WindSpeed, value, () => _WindSpeed = value); }
        }
        private double _Gust;
        public double Gust
        {
            get
            {
                if (IsMetric)
                    return Math.Round(_Gust * 1.609344, 0);
                else
                    return Math.Round(_Gust, 0);
            }
            set { SetProperty(_Gust, value, () => _Gust = value); }
        }
        private double _WindBearing;
        public double WindBearing
        {
            get { return _WindBearing; }
            set { SetProperty(_WindBearing, value, () => _WindBearing = value); }
        }
        private DateTimeOffset _GustTime;
        public DateTimeOffset GustTime
        {
            get { return _GustTime; }
            set { SetProperty(_GustTime, value, () => _GustTime = value); }
        }

        private DateTimeOffset _UvIndexTime;
        public DateTimeOffset UvIndexTime
        {
            get { return _UvIndexTime; }
            set { SetProperty(_UvIndexTime, value, () => _UvIndexTime = value); }
        }

        private double _DewPoint;
        public double DewPoint
        {
            get
            {
                if (IsCelcius)
                    return Math.Round((_DewPoint - 32) * 5 / 9, 0);
                else
                    return Math.Round(_DewPoint, 0);
            }
            set { SetProperty(_DewPoint, value, () => _DewPoint = value); }
        }
        private double _Humidity;
        public double Humidity
        {
            get { return _Humidity; }
            set { SetProperty(_Humidity, value, () => _Humidity = value); }
        }
        private double _Pressure;
        public double Pressure
        {
            get
            {
                if (IsMetric)
                    return Math.Round(_Pressure * 29.92 / 1013.25, 2);
                else
                    return Math.Round(_Pressure, 2);
            }
            set { SetProperty(_Pressure, value, () => _Pressure = value); }
        }
        private double _CloudCover;
        public double CloudCover
        {
            get { return _CloudCover; }
            set { SetProperty(_CloudCover, value, () => _CloudCover = value); }
        }
        private double _UvIndex;
        public double UvIndex
        {
            get { return _UvIndex; }
            set { SetProperty(_UvIndex, value, () => _UvIndex = value); }
        }
        private double _Visibility;
        public double Visibility
        {
            get
            {
                if (IsMetric)
                    return Math.Round(_Visibility * 1.609344, 0);
                else
                    return Math.Round(_Visibility, 0);
            }
            set { SetProperty(_Visibility, value, () => _Visibility = value); }
        }
        private double _Ozone;
        public double Ozone
        {
            get { return _Ozone; }
            set { SetProperty(_Ozone, value, () => _Ozone = value); }
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
            //Wind = forecast.Data.Wind;
            WindSpeed = forecast.Data.Wind.Speed;
            Gust = forecast.Data.Wind.Gust;
            WindBearing = forecast.Data.Wind.Bearing;
            GustTime = forecast.Data.Wind.GustTime;
            DewPoint = forecast.Data.DewPoint;
            Humidity = forecast.Data.Humidity;
            Pressure = forecast.Data.Pressure;
            CloudCover = forecast.Data.CloudCover;
            UvIndex = forecast.Data.UvIndex;
            UvIndexTime = forecast.Data.UvIndexTime;
            Visibility = forecast.Data.Visibility;
            Ozone = forecast.Data.Ozone;
        }
    }
}
