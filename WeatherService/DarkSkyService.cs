using System;
using System.Threading.Tasks;
using Windows.Services.Maps;
using Windows.Web.Http;

namespace WeatherService
{
    public class DarkSkyService
    {
        private const string UriBase = "https://api.forecast.io/forecast/";

        public async Task<WeatherData>GetWeatherData()
        {
            var locationService = new GeoLocationService();
            var location = await locationService.GetCurrentMapLocation();
            var rawJson = await GetRemoteWeatherData(locationService.MapLocationToString(location));
            return ParseWeatherData(location, rawJson);
        }
        
        private async Task<string> GetRemoteWeatherData(string locationString)
        {
            var http = new HttpClient();
            var resp = await http.GetAsync(new Uri(UriBase + Keys.DarkSkyApi + "/" + locationString));
            if (resp.StatusCode != HttpStatusCode.Ok)
                throw new Exception("HttpStatusCode != OK");

            return await resp.Content.ReadAsStringAsync();
        }

        private WeatherData ParseWeatherData(MapLocation location, string rawJson)
        {
            return new WeatherData(location, rawJson);
        }
    }
}
