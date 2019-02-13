# Weather Balloon for Windows 10

Weather Balloon is a Universal Windows Platform (UWP) app that integrates with the DarkSky API for forecast data and is [published to the Microsoft Store](https://www.microsoft.com/store/apps/9NZPK6B93791). 

### Requirements to run locally

* [Visual Studio 2017](https://visualstudio.microsoft.com/downloads/?utm_medium=microsoft&utm_source=docs.microsoft.com&utm_campaign=button+cta&utm_content=download+vs2017): You will need to install the Universal Windows Platform Development Suite + Windows 10 SDK 14393 + Windows 10 SDK 17763

* [WindowsCommunityToolkit](https://github.com/windows-toolkit/WindowsCommunityToolkit): Packages can also be installed via NuGet (Microsoft.Toolkit.Uwp).

* [Enable Windows Developer Mode](https://docs.microsoft.com/en-us/windows/uwp/get-started/enable-your-device-for-development)

The app's responsibilities are grouped into several categories.
<!--
## App architecture

* [WeatherService](#WeatherService)

* [StorageService](#storageservice)

* [NotificationService](#notificationservice)

* [BackgroundTasks](#backgroundtasks)
-->
## WeatherService

For handling forecast related tasks (i.e. retrieving data from APIs and parsing data)

* [DarkSkyService](/WeatherService/WeatherData.cs): Gets forecast information from [DarkSky.net](https://darksky.net) API.

* [WeatherData](/WeatherService/WeatherData.cs): Object created from parsed JSON.

* [GeoLocationService](/WeatherService/GeocodeLookup.cs): Get human readable information details from GPS coordinates.

## StorageService

For caching data/settings and interacting with Microsoft Store APIs

* [StoreService](/StorageService/Settings.cs): Gets In-App Purchase(IAP) status from Microsoft Store and manages trial status.

* [NotificationSettings](/StorageService/NotificationSettings.cs): Get/set all notification related settings (e.g. notify user when temp is above 100F).

* [Settings](/StorageService/Settings.cs): Get/set all other app settings (e.g. user prefers Metric system).

## NotificationService

For sending updates to user based on user settings and forecast data.

* [Toasts](/NotificationService/Toasts.cs): Pushes Windows Toast notifications to users.

* [LiveTiles](/NotificationService/LiveTiles.cs): Updates Windows Live Tile.

## BackgroundTasks

For checking weather data and updating the user regardless of app life-cycle status.

* [DataRefresher](/BackgroundTasks/DataRefresher.cs): Gets weather data in background when app is not running in foreground.
