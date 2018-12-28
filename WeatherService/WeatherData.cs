using System;
using System.Collections.Generic;
using Windows.Data.Json;

namespace WeatherService
{
    public class WeatherData
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Timezone { get; set; }
        public TimeSpan Offset {get; set;}
        public MomentForecast Currently { get; set; }
        public PeriodForecast Minutely { get; set; }
        public PeriodForecast Hourly { get; set; }
        public PeriodForecast Daily { get; set; }
        //TODO: Add Alerts

        public WeatherData(string rawJson)
        {
            if (string.IsNullOrEmpty(rawJson))
                return;
            var parsedJson = JsonObject.Parse(rawJson);
            Latitude = Forecast.GetDoubleValueFromJson(parsedJson, "latitude");
            Longitude = Forecast.GetDoubleValueFromJson(parsedJson, "longitude");
            Timezone = Forecast.GetStringValueFromJson(parsedJson, "timezone");
            Offset = new TimeSpan((int)Forecast.GetDoubleValueFromJson(parsedJson, "offset"), 0, 0);

            Currently = new MomentForecast(parsedJson.GetNamedObject("currently"), Offset);

            var minutelyJson = parsedJson.GetNamedObject("minutely");
            Minutely = new PeriodForecast(minutelyJson, Offset);

            var hourlyJson = parsedJson.GetNamedObject("hourly");
            Hourly = new PeriodForecast(hourlyJson, Offset);

            var dailyJson = parsedJson.GetNamedObject("daily");
            Daily = new PeriodForecast(dailyJson, Offset);
        }
    }

    public class MomentForecast : Forecast
    {
        public Data Data { get; set; }
        public MomentForecast()
        { }
        public MomentForecast(JsonObject parsedJson, TimeSpan offset) : base(parsedJson)
        {
            Data = new Data(parsedJson, offset);
        }
    }

    public class PeriodForecast : Forecast
    {
        public List<Data> Data { get; set; }
        public PeriodForecast(JsonObject parsedJson, TimeSpan offset) : base(parsedJson)
        {
            var dataJson = parsedJson.GetNamedArray("data");
            Data = new List<Data>();
            foreach (var dataObj in dataJson)
            {
                Data.Add(new Data(dataObj.GetObject(), offset));
            }
        }
    }

    public class Forecast
    {
        internal const string SUMMARY = "summary";
        internal const string ICON = "icon";

        public string Summary { get; set; }
        public string Icon { get; set; }

        public Forecast() { }

        public Forecast(JsonObject parsedJson)
        {
            Summary = GetStringValueFromJson(parsedJson, SUMMARY);
            Icon = GetStringValueFromJson(parsedJson, ICON);
        }

        public static string GetStringValueFromJson(JsonObject parsedJson, string key)
        {
            return parsedJson.ContainsKey(key) ? parsedJson.GetNamedString(key) : null;
        }
        public static double GetDoubleValueFromJson(JsonObject parsedJson, string key)
        {
            //TODO: Not great
            return parsedJson.ContainsKey(key) ? parsedJson.GetNamedNumber(key) : 0;
        }
        public static DateTimeOffset GetDateTime(double secSinceEpoch)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return new DateTimeOffset(epoch.AddSeconds(secSinceEpoch));
        }

        
    }

    public class Data
    {
        private const string TIME = "time";
        private const string SUNRISE = "sunriseTime";
        private const string SUNSET = "sunsetTime";
        private const string MOON_PHASE = "moonPhase";

        public DateTimeOffset Time { get; set; }
        public string Icon { get; set; }
        public string Summary { get; set; }
        public DateTimeOffset SunriseTime { get; set; }
        public DateTimeOffset SunsetTime { get; set; }
        public double MoonPhase { get; set; }
        public Percipitation Percipitation { get; set; }
        public Temperature Temp { get; set; }
        public Temperature ApparnetTemp { get; set; }
        public double DewPoint { get; set; }
        public double Humidity { get; set; }
        public double Pressure { get; set; }
        public Wind Wind { get; set; }
        public double CloudCover { get; set; }
        public double UvIndex { get; set; }
        public DateTimeOffset UvIndexTime { get; set; }
        public double Visibility { get; set; }
        public double Ozone { get; set; }

        public Data(JsonObject parsedJson, TimeSpan offset)
        {
            Time =        Forecast.GetDateTime(Forecast.GetDoubleValueFromJson(parsedJson, TIME));
            Icon =        Forecast.GetStringValueFromJson(parsedJson, Forecast.ICON);
            Summary =     Forecast.GetStringValueFromJson(parsedJson, Forecast.SUMMARY);
            SunriseTime = Forecast.GetDateTime(Forecast.GetDoubleValueFromJson(parsedJson, SUNRISE));
            SunsetTime =  Forecast.GetDateTime(Forecast.GetDoubleValueFromJson(parsedJson, SUNSET));
            MoonPhase =   Forecast.GetDoubleValueFromJson(parsedJson, MOON_PHASE);

            Percipitation = new Percipitation(parsedJson);
            Temp = new Temperature(parsedJson, false);
            ApparnetTemp = new Temperature(parsedJson, true);

            DewPoint =    Forecast.GetDoubleValueFromJson(parsedJson, "dewPoint");
            Humidity =    Forecast.GetDoubleValueFromJson(parsedJson, "humidity");
            Pressure =    Forecast.GetDoubleValueFromJson(parsedJson, "pressure");
            Wind =        new Wind(parsedJson);
            CloudCover =  Forecast.GetDoubleValueFromJson(parsedJson, "cloudCover");
            UvIndex =     Forecast.GetDoubleValueFromJson(parsedJson, "uvIndex");
            UvIndexTime = Forecast.GetDateTime(Forecast.GetDoubleValueFromJson(parsedJson, "uvIndexTime"));
            Visibility =  Forecast.GetDoubleValueFromJson(parsedJson, "visibility");
            Ozone =       Forecast.GetDoubleValueFromJson(parsedJson, "ozone");
        }
    }

    public class Percipitation
    {
        private const string INTENSITY = "precipIntensity";
        private const string INTENSITY_ERROR = "precipIntensityError";
        private const string MAX = "precipIntensityMax";
        private const string MAX_TIME = "precipIntensityMaxTime";
        private const string PROBABILITY = "precipProbability";
        private const string TYPE = "precipType";

        public double Intensity { get; set; }
        public double IntensityError { get; set; }
        public double Max { get; set; }
        public DateTimeOffset MaxTime { get; set; }
        public double Probability { get; set; }
        public string Type { get; set; }

        public Percipitation(JsonObject parsedJson)
        {
            Intensity = Forecast.GetDoubleValueFromJson(parsedJson, INTENSITY);
            IntensityError = Forecast.GetDoubleValueFromJson(parsedJson, INTENSITY_ERROR);
            Max = Forecast.GetDoubleValueFromJson(parsedJson, MAX);
            MaxTime = Forecast.GetDateTime(Forecast.GetDoubleValueFromJson(parsedJson, MAX_TIME));
            Probability = Forecast.GetDoubleValueFromJson(parsedJson, PROBABILITY);
            Type = Forecast.GetStringValueFromJson(parsedJson, TYPE);
        }
    }

    public class Temperature
    {
        private const string HIGH = "temperatureHigh";
        private const string HIGH_TIME = "temperatureHighTime";
        private const string LOW = "temperatureLow";
        private const string LOW_TIME = "temperatureLowTime";
        private const string APPARENT_HIGH= "apparentTemperatureHigh";
        private const string APPARENT_HIGH_TIME = "apparentTemperatureHighTime";
        private const string APPARENT_LOW = "apparentTemperatureLow";
        private const string APPARENT_LOW_TIME = "apparentTemperatureLowTime";

        public bool IsCelcius { get; set; }
        private double _Current;
        public double Current { get { return IsCelcius ? GetCelcius(_Current) : _Current ; } set { _Current = value; } }
        private double _High;
        public double High { get { return IsCelcius ? GetCelcius(_High) : _High; } set { _High = value; } }
        public DateTimeOffset HighTime { get; set; }
        private double _Low;
        public double Low { get { return IsCelcius ? GetCelcius(_Low) : _Low; } set { _Low = value; } }
        public DateTimeOffset LowTime { get; set; }

        private double GetCelcius(double n)
        {
            return (n - 32) * 5 / 9;
        }

        public Temperature(JsonObject parsedJson, bool isApparent = false)
        {
            if (isApparent)
            {
                High = Forecast.GetDoubleValueFromJson(parsedJson, APPARENT_HIGH);
                HighTime = Forecast.GetDateTime(Forecast.GetDoubleValueFromJson(parsedJson, APPARENT_HIGH_TIME));
                Low = Forecast.GetDoubleValueFromJson(parsedJson, APPARENT_LOW);
                LowTime = Forecast.GetDateTime(Forecast.GetDoubleValueFromJson(parsedJson, APPARENT_LOW_TIME));
                Current = Forecast.GetDoubleValueFromJson(parsedJson, "apparentTemperature");
            }
            else
            {
                High = Forecast.GetDoubleValueFromJson(parsedJson, HIGH);
                HighTime = Forecast.GetDateTime(Forecast.GetDoubleValueFromJson(parsedJson, HIGH_TIME));
                Low = Forecast.GetDoubleValueFromJson(parsedJson, LOW);
                LowTime = Forecast.GetDateTime(Forecast.GetDoubleValueFromJson(parsedJson, LOW_TIME));
                Current = Forecast.GetDoubleValueFromJson(parsedJson, "temperature");
            }
        }
    }

    public class Wind
    {
        public double Speed { get; set; }
        public double Gust { get; set; }
        public DateTimeOffset GustTime { get; set; }
        public double Bearing { get; set; }

        public Wind(JsonObject parsedJson)
        {
            Speed = Forecast.GetDoubleValueFromJson(parsedJson, "windSpeed");
            Gust = Forecast.GetDoubleValueFromJson(parsedJson, "windGust");
            GustTime = Forecast.GetDateTime(Forecast.GetDoubleValueFromJson(parsedJson, "windGustTime"));
            Bearing = Forecast.GetDoubleValueFromJson(parsedJson, "windBearing");
        }
    }
}