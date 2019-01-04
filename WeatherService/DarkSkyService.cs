using System;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Services.Maps;
using Windows.Storage;
using Windows.Web.Http;

namespace WeatherService
{
    public class DarkSkyService
    {
        private const string UriBase = "https://api.forecast.io/forecast/";
        private const string DARKSKY_FILENAME = "DarkSkyData.dat";

        public async Task<WeatherData>GetWeatherData(int freshness = 30)
        {
            var locationService = new GeoLocationService();
            var currentLocation = await locationService.GetCurrentMapLocation();

            var rawJson = await GetLocalWeatherData();
            if (freshness <= 1 || rawJson == null)
            {
                rawJson = await GetRemoteWeatherData(locationService.MapLocationToString(currentLocation));
                await SaveToFile(rawJson, DARKSKY_FILENAME);
                freshness = 1;
            }
            var parsedData = await ParseWeatherData(rawJson);
            if (parsedData.Location.Address.Town != currentLocation.Address.Town ||
                parsedData.Location.Address.Region != currentLocation.Address.Region ||
                parsedData.Currently.Data.Time.AddMinutes(freshness) <= DateTime.UtcNow)
            {
                parsedData = await GetWeatherData(-1);
            }
            return parsedData;
        }

        private async Task<string> GetRemoteWeatherData(string locationString)
        {
            var http = new HttpClient();
            var resp = await http.GetAsync(new Uri(UriBase + Keys.DarkSkyApi + "/" + locationString));
            if (resp.StatusCode != HttpStatusCode.Ok)
                throw new Exception("HttpStatusCode != OK");

            return await resp.Content.ReadAsStringAsync();
        }

        private async Task<string> GetLocalWeatherData()
        {
            var savedData = await GetFile(DARKSKY_FILENAME);
            if (savedData == null)
                return null;
            var rawJson = await FileIO.ReadTextAsync(savedData) as string;
            return rawJson;
        }

        private async Task<WeatherData> ParseWeatherData(string rawJson)
        {
            var weatherData = new WeatherData(rawJson);
            var savedLocation = new BasicGeoposition()
            {
                Latitude = weatherData.Latitude,
                Longitude = weatherData.Longitude
            };
            var locationService = new GeoLocationService();
            weatherData.Location = await locationService.GetAddressFromGeoLocation(savedLocation);
            return weatherData;
        }

        private async Task<StorageFile> SaveToFile(string jsonData, string filename)
        {
            var cacheFile = await ApplicationData.Current.LocalFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(cacheFile, jsonData);
            return cacheFile;
        }

        private async Task<StorageFile> GetFile(string filename)
        {
            try { return await ApplicationData.Current.LocalFolder.GetFileAsync(filename); }
            catch { return null; }
        }
    }
}
