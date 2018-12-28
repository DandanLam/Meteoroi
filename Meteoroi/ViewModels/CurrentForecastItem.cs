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

        public CurrentForecastItem(MomentForecast forecast) : base(forecast)
        {
            if (forecast == null)
                return;

            Time = forecast.Data.Time;
            Temp = forecast.Data.Temp.Current;
            ApparentTemp = forecast.Data.ApparnetTemp.Current;

            PrecipIntensity = forecast.Data.Percipitation.Intensity;
            PrecipIntensityError = forecast.Data.Percipitation.IntensityError;
            PrecipProbability = forecast.Data.Percipitation.Probability;

        }
    }
}
