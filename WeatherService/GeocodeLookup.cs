using StorageService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Services.Maps;
using Windows.System;
using Windows.UI.Xaml.Controls;

namespace WeatherService
{
    public class GeoLocationService
    {
        public const string GEOLOCATION_STRING = "GeolocationString";

        public string MapLocationToString(MapLocation location)
        {
            return string.Concat(location.Point.Position.Latitude, ",", location.Point.Position.Longitude);
        }

        public async Task<MapLocation> GetCurrentMapLocation()
        {
            var locationFromSettings = TryGetBasicGeopositionFromSettings();
            if (locationFromSettings.Longitude == 0 && locationFromSettings.Latitude == 0)
            {
                var geoposition = await TryGetCurrentGeoposition();
                locationFromSettings = new BasicGeoposition()
                {
                    Latitude = geoposition.Coordinate.Point.Position.Latitude,
                    Longitude = geoposition.Coordinate.Point.Position.Longitude
                };
            }

            if (locationFromSettings.Longitude != 0 && locationFromSettings.Latitude != 0)
            {
                return await GetAddressFromGeoLocation(locationFromSettings);
            }
            return null;
        }

        private BasicGeoposition TryGetBasicGeopositionFromSettings()
        {
            if (Settings.IsKeyPresent(GEOLOCATION_STRING))
            {
                var stringValue = Settings.GetStringValue(GEOLOCATION_STRING);
                if (!string.IsNullOrEmpty(stringValue))
                {
                    stringValue.Split(',', StringSplitOptions.RemoveEmptyEntries);
                    return new BasicGeoposition()
                    {
                        Latitude = stringValue[0],
                        Longitude = stringValue[1],
                    };
                }
                Settings.RemoveValue(GEOLOCATION_STRING);
            }
            return new BasicGeoposition();
        }

        private async Task<Geoposition> TryGetCurrentGeoposition()
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
                        throw new Exception("Unable to get location");
                }
            }
            catch (Exception e)
            {
                ContentDialog errorDialog = new ContentDialog()
                {
                    Title = e.Message,
                    SecondaryButtonText = "Dismiss",
                    Content = "Please ensure that Windows Location Services are enabled and you've grated access to Weather Notify for either your precise or general location.",
                    PrimaryButtonText = "Windows Location Settings",
                };
                errorDialog.PrimaryButtonClick += LaunchLocationSettings;

                return null;
            }
        }

        private async void LaunchLocationSettings(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            try { await Launcher.LaunchUriAsync(new Uri("ms-settings:privacy-location")); } catch { }
        }

        public async Task<MapLocation> GetAddressFromGeoLocation(BasicGeoposition geoposition)
        {
            MapService.ServiceToken = Keys.BingMapsApi;
            Geopoint pointToReverseGeocode = new Geopoint(geoposition);
            var result = await MapLocationFinder.FindLocationsAtAsync(pointToReverseGeocode);
            if (result.Status == MapLocationFinderStatus.Success)
                return result.Locations[0]; //may return multiple results
            else
                return null;
        }
    }
}
