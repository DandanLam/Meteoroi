using StorageService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Storage;
using Windows.Web.Http;

namespace WeatherService
{
    public class DarkSkyService
    {
        private const string UriBase = "https://api.forecast.io/forecast/";
        public const string GEOLOCATION_STRING = "GeolocationString";

        public async Task<WeatherData>GetWeatherData()
        {
            var location = await GetLocationAsString();
            var rawJson = await GetRemoteWeatherData(location);
            return ParseWeatherData(rawJson);
            throw new NotImplementedException();
        }
        
        private async Task<string> GetLocationAsString()
        {
            if (Settings.IsKeyPresent(GEOLOCATION_STRING))
                return Settings.GetStringValue(GEOLOCATION_STRING);

            var locationData = await GetCurrentLocation();
            return locationData.Coordinate.Point.Position.Latitude + "," + locationData.Coordinate.Point.Position.Longitude;
        }

        public async Task<Geoposition> GetCurrentLocation()
        {
            try
            {
                var accessStatus = await Geolocator.RequestAccessAsync();
                var geolocator = new Geolocator { DesiredAccuracy = PositionAccuracy.Default };
                switch (accessStatus)
                {
                    case GeolocationAccessStatus.Allowed:
                        CancellationTokenSource _cts = new CancellationTokenSource();
                        CancellationToken token = _cts.Token;

                        return await geolocator.GetGeopositionAsync().AsTask(token);
                    case GeolocationAccessStatus.Denied:
                        geolocator.AllowFallbackToConsentlessPositions();
                        goto case GeolocationAccessStatus.Allowed;
                    //case GeolocationAccessStatus.Unspecified:
                    //    return null;
                    default:
                        return null;
                }
            }
            catch { throw new Exception(); }
        }

        private async Task<string> GetRemoteWeatherData(string locationString)
        {
            var http = new HttpClient();
            var resp = await http.GetAsync(new Uri(UriBase + Keys.DarkSkyApi + "/" + locationString));
            if (resp.StatusCode != HttpStatusCode.Ok)
                throw new Exception("HttpStatusCode != OK");

            return await resp.Content.ReadAsStringAsync();
        }

        private WeatherData ParseWeatherData(string rawJson)
        {
            return new WeatherData(rawJson);
        }
    }
}
