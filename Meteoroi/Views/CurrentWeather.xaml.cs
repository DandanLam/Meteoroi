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
        ObservableCollection<DailyForecastItem> DailyForecasts = new ObservableCollection<DailyForecastItem>();
        int CurrentDailyInView = 1;
        ObservableCollection<HourlyForecastItem> HourlyForecasts = new ObservableCollection<HourlyForecastItem>();
        int CurrentHourlyInView = 1;

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
            UpdateDailyForecast(weatherData);
            UpdateHourlyForecast(weatherData);
        }

        void UpdateCurrentForecast(WeatherData weatherData)
        {
            var newItem = new CurrentForecastItem(weatherData.Currently);
            CurrentForecast.Location = "Seattle, WA";
            CurrentForecast.Temp = newItem.Temp;
            CurrentForecast.Icon = newItem.Icon;
            CurrentForecast.Summary = newItem.Summary;
            CurrentForecast.Time = newItem.Time;
            CurrentForecast.Sunrise = newItem.Sunrise;
            CurrentForecast.Sunset = newItem.Sunset;

            CurrentForecast.ThisHourSummary = weatherData.Minutely.Summary;
        }

        void UpdateDailyForecast(WeatherData weatherData)
        {
            DailyForecasts.Clear();
            foreach (var data in weatherData.Daily.Data)
            {
                data.Temp.IsCelcius = CurrentForecast.IsCelcius;
                data.ApparnetTemp.IsCelcius = CurrentForecast.IsCelcius;
                DailyForecasts.Add(new DailyForecastItem(data));
            }
            DailyGridView.ScrollIntoView(DailyForecasts.Last());
            DailyGridView.ScrollIntoView(DailyForecasts[CurrentDailyInView]);
            CurrentForecast.ThisWeekSummary = weatherData.Daily.Summary;
        }

        void UpdateHourlyForecast(WeatherData weatherData)
        {
            HourlyForecasts.Clear();
            foreach (var data in weatherData.Hourly.Data)
            {
                data.Temp.IsCelcius = CurrentForecast.IsCelcius;
                data.ApparnetTemp.IsCelcius = CurrentForecast.IsCelcius;
                HourlyForecasts.Add(new HourlyForecastItem(data));
            }
            HourlyGridView.ScrollIntoView(HourlyForecasts.Last());
            HourlyGridView.ScrollIntoView(HourlyForecasts[CurrentHourlyInView]);
            CurrentForecast.ThisDaySummary = weatherData.Hourly.Summary;
        }

        private void ToggleIsCelcius_Click(object sender, RoutedEventArgs e)
        {
            CurrentForecast.IsCelcius = !CurrentForecast.IsCelcius;
        }

        private void PrevDay_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentDailyInView > 0)
            {
                DailyGridView.ScrollIntoView(DailyForecasts[--CurrentDailyInView]);
            }
        }

        private void NextDay_Click(object sender, RoutedEventArgs e)
        {
            var visibleBlocks = DailyGridView.ActualWidth / 135;
            if (CurrentDailyInView < DailyForecasts.Count - visibleBlocks)
            {
                DailyGridView.ScrollIntoView(DailyForecasts.Last());
                DailyGridView.ScrollIntoView(DailyForecasts[++CurrentDailyInView]);
            }
        }

        private void PrevHour_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentHourlyInView > 0)
            {
                HourlyGridView.ScrollIntoView(HourlyForecasts[--CurrentHourlyInView]);
            }
        }

        private void NextHour_Click(object sender, RoutedEventArgs e)
        {
            var visibleBlocks = HourlyGridView.ActualWidth / 135;
            if (CurrentHourlyInView < HourlyForecasts.Count - visibleBlocks)
            {
                HourlyGridView.ScrollIntoView(HourlyForecasts.Last());
                HourlyGridView.ScrollIntoView(HourlyForecasts[++CurrentHourlyInView]);
            }
        }

    }
}
