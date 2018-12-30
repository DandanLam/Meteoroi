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
using Windows.UI.Core;
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
        WeatherData weatherData = null;
        CurrentForecastItem CurrentForecast = new CurrentForecastItem(null);
        CurrentForecastItem TodayForecast = new CurrentForecastItem(null);
        ObservableCollection<DailyForecastItem> DailyForecasts = new ObservableCollection<DailyForecastItem>();
        int CurrentDailyInView = 1;
        ObservableCollection<HourlyForecastItem> HourlyForecasts = new ObservableCollection<HourlyForecastItem>();
        int CurrentHourlyInView = 1;

        public CurrentWeather()
        {
            this.InitializeComponent();
            UpdateForecastItems();
        }

        async Task UpdateForecastItems(bool forceDownload = false)
        {
            if (forceDownload || weatherData == null)
            {
                DarkSkyService weatherService = new DarkSkyService();
                weatherData = await weatherService.GetWeatherData();
                UpdateCurrentForecast(weatherData);
            }
            UpdateDailyForecast(weatherData);
            UpdateHourlyForecast(weatherData);
        }

        void UpdateCurrentForecast(WeatherData weatherData)
        {
            var newItem = new CurrentForecastItem(weatherData.Currently);
            CurrentForecast.Location = weatherData.Location;
            CurrentForecast.Temp = newItem.Temp;
            CurrentForecast.Icon = newItem.Icon;
            CurrentForecast.Summary = newItem.Summary;
            CurrentForecast.Time = newItem.Time;
            CurrentForecast.Sunrise = newItem.Sunrise;
            CurrentForecast.Sunset = newItem.Sunset;

            CurrentForecast.Humidity = newItem.Humidity;
            CurrentForecast.Ozone = newItem.Ozone;
            CurrentForecast.Pressure = newItem.Pressure;
            CurrentForecast.UvIndex = newItem.UvIndex;
            CurrentForecast.UvIndexTime = newItem.UvIndexTime;
            CurrentForecast.Visibility = newItem.Visibility;
            CurrentForecast.Wind = newItem.Wind;
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

            var today = DailyForecasts.First();
            TodayForecast.Humidity = today.Humidity;
            TodayForecast.Ozone = today.Ozone;
            TodayForecast.Pressure = today.Pressure;
            TodayForecast.UvIndex = today.UvIndex;
            TodayForecast.UvIndexTime = today.UvIndexTime;
            TodayForecast.Visibility = today.Visibility;
            TodayForecast.Wind = today.Wind;
            TodayForecast.CloudCover = today.CloudCover;
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
            CoreWindow.GetForCurrentThread().Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                try
                {
                    UpdateForecastItems();
                }
                catch { }
            });
        }

        private void PrevDay_Click(object sender, RoutedEventArgs e)
        {
            int visibleBlocks = -(int)DailyGridView.ActualWidth / 133;
            CurrentDailyInView = CalcNext(DailyForecasts.Count, CurrentDailyInView, visibleBlocks);
            DailyGridView.ScrollIntoView(DailyForecasts[CurrentDailyInView]);
        }

        private void NextDay_Click(object sender, RoutedEventArgs e)
        {
            int visibleBlocks = (int)DailyGridView.ActualWidth / 133;
            DailyGridView.ScrollIntoView(DailyForecasts.Last());
            CurrentDailyInView = CalcNext(DailyForecasts.Count, CurrentDailyInView, visibleBlocks);
            DailyGridView.ScrollIntoView(DailyForecasts[CurrentDailyInView]);
        }

        private void PrevHour_Click(object sender, RoutedEventArgs e)
        {
            int visibleBlocks = -(int)HourlyGridView.ActualWidth / 133;
            CurrentHourlyInView = CalcNext(HourlyForecasts.Count, CurrentHourlyInView, visibleBlocks);
            HourlyGridView.ScrollIntoView(HourlyForecasts[CurrentHourlyInView]);
        }

        private void NextHour_Click(object sender, RoutedEventArgs e)
        {
            int visibleBlocks = (int)HourlyGridView.ActualWidth / 133;
            CurrentHourlyInView = CalcNext(HourlyForecasts.Count, CurrentHourlyInView, visibleBlocks);
            HourlyGridView.ScrollIntoView(HourlyForecasts.Last());
            HourlyGridView.ScrollIntoView(HourlyForecasts[CurrentHourlyInView]);
        }

        private int CalcNext(int CollectionTotal, int currentVisible, int adjustCurrentVisible )
        {
            int visibleBlocks = (int)HourlyGridView.ActualWidth / 133;
            currentVisible += adjustCurrentVisible;
            if (adjustCurrentVisible > 0) //next
            {
                if (currentVisible > CollectionTotal)
                {
                    currentVisible = CollectionTotal - visibleBlocks - 1;
                }
            }
            else //prev
            {
                if (currentVisible < 0)
                {
                    currentVisible = 0;
                }
            }
            return currentVisible;
        }

        private void WideGridView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                var changedGridView = sender as GridView;
                if (changedGridView == null || Math.Round(changedGridView.ActualWidth, 0) == Math.Round(DetailsStackPanel.ActualWidth, 0))
                    return;
                
                DetailsStackPanel.Width = changedGridView.ActualWidth;
            }
            catch { }
        }
    }
}
