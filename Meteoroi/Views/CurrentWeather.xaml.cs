using Meteoroi.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using WeatherService;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Meteoroi.Views
{
    public sealed partial class CurrentWeather : Page
    {
        CurrentForecastItem CurrentForecast = new CurrentForecastItem(null);

        public CurrentWeather()
        {
            this.InitializeComponent();
            UpdateForecastItems();
        }

        async Task UpdateForecastItems()
        {
            DarkSkyService weatherService = new DarkSkyService();
            var weatherData = await weatherService.GetWeatherData();
            UpdateCurrentForecast(weatherData);
        }

        void UpdateCurrentForecast(WeatherData weatherData)
        {
            var newItem = new CurrentForecastItem(weatherData.Currently);
            CurrentForecast.Location = "Seattle, WA";
            CurrentForecast.Temp = newItem.Temp;
            CurrentForecast.Icon = newItem.Icon;
            CurrentForecast.Summary = newItem.Summary;
            CurrentForecast.Time = newItem.Time;
        }

        private void ToggleIsCelcius_Click(object sender, RoutedEventArgs e)
        {
            CurrentForecast.IsCelcius = !CurrentForecast.IsCelcius;
            //CurrentForecast.Temp = CurrentForecast.Temp;
        }
    }
}
