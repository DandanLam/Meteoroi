﻿using Meteoroi.ViewModels;
using Microsoft.Toolkit.Uwp.UI.Animations;
using StorageService;
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
        bool PageIsLoading = true;
        const int TutorialFontSize = 10;
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
            if (weatherData == null)
                return;
            var newItem = new CurrentForecastItem(weatherData.Currently) { IsMetric = false, IsCelcius = false };
            CurrentForecast.Location = weatherData.Location;
            CurrentForecast.Temp = newItem.Temp;
            CurrentForecast.ApparentTemp = newItem.ApparentTemp;
            CurrentForecast.Icon = newItem.Icon;
            CurrentForecast.Summary = newItem.Summary;
            CurrentForecast.Time = newItem.Time;
            CurrentForecast.Sunrise = newItem.Sunrise;
            CurrentForecast.Sunset = newItem.Sunset;

            CurrentForecast.Humidity = newItem.Humidity;
            CurrentForecast.Ozone = newItem.Ozone;
            CurrentForecast.DewPoint = newItem.DewPoint;
            CurrentForecast.Pressure = newItem.Pressure;
            CurrentForecast.UvIndex = newItem.UvIndex;
            CurrentForecast.UvIndexTime = newItem.UvIndexTime;
            CurrentForecast.Visibility = newItem.Visibility;
            CurrentForecast.WindSpeed = newItem.WindSpeed;
            CurrentForecast.Gust = newItem.Gust;
            CurrentForecast.WindBearing = newItem.WindBearing;
            CurrentForecast.GustTime = newItem.GustTime;

        }

        void UpdateDailyForecast(WeatherData weatherData)
        {
            if (weatherData == null)
                return;
            DailyForecasts.Clear();
            foreach (var data in weatherData.Daily.Data)
            {
                data.Temp.IsCelcius = CurrentForecast.IsCelcius;
                data.ApparnetTemp.IsCelcius = CurrentForecast.IsCelcius;
                var dfi = new DailyForecastItem(data);
                dfi.Wind.IsMetric = CurrentForecast.IsMetric;
                DailyForecasts.Add(dfi);
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
            TodayForecast.CloudCover = today.CloudCover;
            TodayForecast.WindSpeed = today.Wind.Speed;
            TodayForecast.Gust = today.Wind.Gust;
            TodayForecast.WindBearing = today.Wind.Bearing;
            TodayForecast.GustTime = today.Wind.GustTime;
            TodayForecast.PercipProbability = today.Percipitation.Probability;
        }

        void UpdateHourlyForecast(WeatherData weatherData)
        {
            if (weatherData == null)
                return;
            HourlyForecasts.Clear();
            foreach (var data in weatherData.Hourly.Data)
            {
                data.Temp.IsCelcius = CurrentForecast.IsCelcius;
                data.ApparnetTemp.IsCelcius = CurrentForecast.IsCelcius;
                var hfi = new HourlyForecastItem(data);
                hfi.Wind.IsMetric = CurrentForecast.IsMetric;
                HourlyForecasts.Add(hfi);
            }
            HourlyGridView.ScrollIntoView(HourlyForecasts.Last());
            HourlyGridView.ScrollIntoView(HourlyForecasts[CurrentHourlyInView]);
            CurrentForecast.ThisDaySummary = weatherData.Hourly.Summary;
        }

        private void ToggleIsCelcius_Click(object sender, RoutedEventArgs e)
        {
            CurrentForecast.IsCelcius = !CurrentForecast.IsCelcius;
            CurrentForecast.IsMetric = !CurrentForecast.IsMetric;
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
            DailyGridView.ScrollIntoView(DailyForecasts.First());
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
            HourlyGridView.ScrollIntoView(HourlyForecasts.First());
            CurrentHourlyInView = CalcNext(HourlyForecasts.Count, CurrentHourlyInView, visibleBlocks);
            HourlyGridView.ScrollIntoView(HourlyForecasts[CurrentHourlyInView]);
        }

        private void NextHour_Click(object sender, RoutedEventArgs e)
        {
            int visibleBlocks = (int)HourlyGridView.ActualWidth / 133;
            HourlyGridView.ScrollIntoView(HourlyForecasts.Last());
            CurrentHourlyInView = CalcNext(HourlyForecasts.Count, CurrentHourlyInView, visibleBlocks);
            HourlyGridView.ScrollIntoView(HourlyForecasts[CurrentHourlyInView]);
        }

        private int CalcNext(int CollectionTotal, int currentVisible, int adjustCurrentVisible )
        {
            int visibleBlocks = (int)HourlyGridView.ActualWidth / 133;
            currentVisible += adjustCurrentVisible;
            if (adjustCurrentVisible > 0) //next
            {
                if (currentVisible >= CollectionTotal)
                {
                    currentVisible = CollectionTotal - 1;
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
                DailyHeaderGrid.Width = changedGridView.ActualWidth - 20;
                HourlyHeaderGrid.Width = changedGridView.ActualWidth - 20;
                CurrentHeaderGrid.Width = changedGridView.ActualWidth - 20;
                CurrentStackPanel.Width = changedGridView.ActualWidth + 90;
            }
            catch { }
        }

        private void DailyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var changedComboBox = sender as ComboBox;
            if (changedComboBox == null)
                return;
            try
            {
                List<ComboBox> DailyComboBoxes = new List<ComboBox> { DailyLine1Box, DailyLine2Box };
                var oldItem = e.RemovedItems.First() as ComboBoxItem;
                //check for conflict
                foreach (var combobox in DailyComboBoxes)
                {
                    var selectedOne = combobox.SelectedItem as ComboBoxItem;
                    var selectedTwo = changedComboBox.SelectedItem as ComboBoxItem;
                    if (combobox != changedComboBox && 
                        selectedOne != null &&
                        selectedTwo != null &&
                        selectedOne.Content as string != "Hide" &&
                        selectedTwo.Content as string != "Hide" &&
                        selectedOne.Content as string != "Temperature" &&
                        combobox.SelectedIndex == changedComboBox.SelectedIndex)
                    {
                        for (int i = 0; i < DailyLine2Box.Items.Count; i++)
                        {
                            if (changedComboBox.Items[i] == oldItem)
                            {
                                combobox.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                }
                UpdateForecastItemLine(changedComboBox.Name, changedComboBox.SelectedIndex);
                UpdateDailyForecast(weatherData);
            }
            catch { }
        }

        private void UpdateForecastItemLine(string comboBoxItemName, int idx)
        {
            switch (comboBoxItemName)
            {
                case "DailyLine1Box":
                    DailyForecastItem.Line1 = idx;
                    break;
                case "DailyLine2Box":
                    DailyForecastItem.Line2 = idx;
                    break;
                case "HourlyLine1Box":
                    HourlyForecastItem.Line1 = idx;
                    break;
                case "HourlyLine2Box":
                    HourlyForecastItem.Line2 = idx;
                    break;
            }
        }

        private async void ShowGrid(Grid grid)
        {
            if (grid == null)
                return;
            grid.Opacity = 0.1f;
            grid.Visibility = Visibility.Visible;
            await grid.Fade(value: 1f, duration: 500, delay: 0).StartAsync();
        }

        private async void HideGrid(Grid grid)
        {
            if (grid == null)
                return;
            await grid.Fade(value: 0f, duration: 500, delay: 0).StartAsync();
            grid.Visibility = Visibility.Collapsed;
        }

        private void DailyEdit_Click(object sender, RoutedEventArgs e)
        {
            ShowGrid(DailyEditGrid);
        }

        private void DailyEditDone_Click(object sender, RoutedEventArgs e)
        {
            HideGrid(DailyEditGrid);
        }

        private void DailyDateBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var changedComboBox = sender as ComboBox;
            if (changedComboBox == null)
                return;
            switch (changedComboBox.SelectedIndex)
            {
                case 0:
                    DailyForecastItem.DateForemat = "ddd d";
                    break;
                case 1:
                    DailyForecastItem.DateForemat = "d ddd";
                    break;
                case 2:
                    DailyForecastItem.DateForemat = "dddd d";
                    break;
                case 3:
                    DailyForecastItem.DateForemat = "d dddd";
                    break;
                case 4:
                    DailyForecastItem.DateForemat = "MMM d";
                    break;
                case 5:
                    DailyForecastItem.DateForemat = "d MMM";
                    break;
                case 6:
                    DailyForecastItem.DateForemat = "MMMM d";
                    break;
                case 7:
                    DailyForecastItem.DateForemat = "d MMMM";
                    break;
            }
            UpdateDailyForecast(weatherData);
        }

        private void DailyIconBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var changedComboBox = sender as ComboBox;
            if (changedComboBox == null)
                return;
            DailyForecastItem.ShowIcon = changedComboBox.SelectedIndex == 0 ? true : false;
            UpdateDailyForecast(weatherData);
        }

        private void WeeklyDescVisibility_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var changedComboBox = sender as ComboBox;
            if (changedComboBox == null || WeeklySummaryTextBlock == null)
                return;
            try
            {
                WeeklySummaryTextBlock.Visibility = changedComboBox.SelectedIndex == 0 ? Visibility.Visible : Visibility.Collapsed;
            }
            catch { }
        }

        private void DailyTempType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var changedComboBox = sender as ComboBox;
            if (changedComboBox == null || WeeklySummaryTextBlock == null)
                return;
            try
            {
                DailyForecastItem.RealTemp = changedComboBox.SelectedIndex == 0 ? true : false;
                UpdateDailyForecast(weatherData);
            }
            catch { }
        }

        private void HourlyDescVisibility_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var changedComboBox = sender as ComboBox;
            if (changedComboBox == null || WeeklySummaryTextBlock == null)
                return;
            try
            {
                HourlySummaryTextBlock.Visibility = changedComboBox.SelectedIndex == 0 ? Visibility.Visible : Visibility.Collapsed;
            }
            catch { }
        }

        private void HourlyTempType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var changedComboBox = sender as ComboBox;
            if (changedComboBox == null || WeeklySummaryTextBlock == null)
                return;
            try
            {
                HourlyForecastItem.RealTemp = changedComboBox.SelectedIndex == 0 ? true : false;
                UpdateHourlyForecast(weatherData);
            }
            catch { }
        }

        private void HourlyEditDone_Click(object sender, RoutedEventArgs e)
        {
            HideGrid(HourlyEditGrid);
        }

        private void HourlyEdit_Click(object sender, RoutedEventArgs e)
        {
            ShowGrid(HourlyEditGrid);
        }

        private void HourlyIconBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var changedComboBox = sender as ComboBox;
            if (changedComboBox == null)
                return;
            HourlyForecastItem.ShowIcon = changedComboBox.SelectedIndex == 0 ? true : false;
            UpdateHourlyForecast(weatherData);
        }

        private void HourlyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var changedComboBox = sender as ComboBox;
            if (changedComboBox == null)
                return;
            try
            {
                List<ComboBox> DailyComboBoxes = new List<ComboBox> { HourlyLine1Box, HourlyLine2Box };
                var oldItem = e.RemovedItems.First() as ComboBoxItem;
                //check for conflict
                foreach (var combobox in DailyComboBoxes)
                {
                    var selectedOne = combobox.SelectedItem as ComboBoxItem;
                    var selectedTwo = changedComboBox.SelectedItem as ComboBoxItem;
                    if (combobox != changedComboBox && 
                        selectedOne != null &&
                        selectedTwo != null &&
                        selectedOne.Content as string != "Hide" &&
                        selectedTwo.Content as string != "Hide" &&
                        selectedOne.Content as string != "Temperature" &&
                        combobox.SelectedIndex == changedComboBox.SelectedIndex)
                    {
                        for (int i = 0; i < HourlyLine2Box.Items.Count; i++)
                        {
                            if (changedComboBox.Items[i] == oldItem)
                            {
                                combobox.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                }
                UpdateForecastItemLine(changedComboBox.Name, changedComboBox.SelectedIndex);
                UpdateHourlyForecast(weatherData);
            }
            catch { }
        }

        private void HourlyTimeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var changedComboBox = sender as ComboBox;
            if (changedComboBox == null)
                return;
            try
            {
                switch (changedComboBox.SelectedIndex)
                {
                    case 0:
                        HourlyForecastItem.TimeForemat = "H";
                        break;
                    case 1:
                        HourlyForecastItem.TimeForemat = "HH";
                        break;
                    case 2:
                        HourlyForecastItem.TimeForemat = "h tt";
                        HourlyForecastItem.TimeForematToLower = true;
                        break;
                    case 3:
                        HourlyForecastItem.TimeForemat = "h tt";
                        HourlyForecastItem.TimeForematToLower = false;
                        break;
                    case 4:
                        HourlyForecastItem.TimeForemat = "h:00 tt";
                        HourlyForecastItem.TimeForematToLower = true;
                        break;
                    case 5:
                        HourlyForecastItem.TimeForemat = "h:00 tt";
                        HourlyForecastItem.TimeForematToLower = false;
                        break;
                    case 6:
                        HourlyForecastItem.TimeForemat = "H:00";
                        break;
                    case 7:
                        HourlyForecastItem.TimeForemat = "HH:00";
                        break;
                }
                UpdateHourlyForecast(weatherData);
            }
            catch { }
        }

        private void CurrentEditDone_Click(object sender, RoutedEventArgs e)
        {
            HideGrid(CurrentEditGrid);
        }

        private void CurrentEdit_Click(object sender, RoutedEventArgs e)
        {
            ShowGrid(CurrentEditGrid);
        }

        private void CurrnetCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var cb = sender as CheckBox;
            if (cb == null)
                return;
            try
            {
                switch(cb.Content as string)
                {
                    case "Alternate Temp":
                        CurrentForecast.ShowTempAlt = (bool)cb.IsChecked;
                        Settings.ShowCurrentAltTemp = (bool)cb.IsChecked;
                        break;
                    case "Wind Speed":
                        CurrentForecast.ShowWindSpeed = (bool)cb.IsChecked;
                        Settings.ShowCurrentWindSpeed = (bool)cb.IsChecked;
                        break;
                    case "Visibility":
                        CurrentForecast.ShowVisibility = (bool)cb.IsChecked;
                        Settings.ShowCurrentVisibility = (bool)cb.IsChecked;
                        break;
                    case "Barometer":
                        CurrentForecast.ShowPressure = (bool)cb.IsChecked;
                        Settings.ShowCurrentPressure = (bool)cb.IsChecked;
                        break;
                    case "Humidity":
                        CurrentForecast.ShowHumidity = (bool)cb.IsChecked;
                        Settings.ShowCurrentHumidity = (bool)cb.IsChecked;
                        break;
                    case "Dew Point":
                        CurrentForecast.ShowDewPoint = (bool)cb.IsChecked;
                        Settings.ShowCurrentDewPoint = (bool)cb.IsChecked;
                        break;
                }
            }
            catch { }
        }

        private void LocationFormatComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PageIsLoading)
                return;
            switch ((sender as ComboBox).SelectedIndex)
            {
                case 0:
                    Settings.ShowCurrentRegion = Settings.LocationDisplayType.HIDDEN;
                    Settings.ShowCurrentCountry = Settings.LocationDisplayType.HIDDEN;
                    break;
                case 1:
                    Settings.ShowCurrentRegion = Settings.LocationDisplayType.ABBREV;
                    Settings.ShowCurrentCountry = Settings.LocationDisplayType.HIDDEN;
                    break;
                case 2:
                    Settings.ShowCurrentRegion = Settings.LocationDisplayType.ABBREV;
                    Settings.ShowCurrentCountry = Settings.LocationDisplayType.ABBREV;
                    break;
                case 3:
                    Settings.ShowCurrentRegion = Settings.LocationDisplayType.ABBREV;
                    Settings.ShowCurrentCountry = Settings.LocationDisplayType.FULL;
                    break;
                case 4:
                    Settings.ShowCurrentRegion = Settings.LocationDisplayType.FULL;
                    Settings.ShowCurrentCountry = Settings.LocationDisplayType.ABBREV;
                    break;
            }
            CurrentForecast.LocationString = Guid.NewGuid().ToString();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            PageIsLoading = false;
        }

        private void LocationFormatComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var combobox = sender as ComboBox;
            if (combobox == null)
                return;
            if (Settings.ShowCurrentRegion == Settings.LocationDisplayType.HIDDEN &&
                Settings.ShowCurrentCountry == Settings.LocationDisplayType.HIDDEN)
                combobox.SelectedIndex = 0;
            else if (Settings.ShowCurrentRegion == Settings.LocationDisplayType.FULL &&
                Settings.ShowCurrentCountry == Settings.LocationDisplayType.HIDDEN)
                combobox.SelectedIndex = 1;
            else if (Settings.ShowCurrentRegion == Settings.LocationDisplayType.FULL &&
                Settings.ShowCurrentCountry == Settings.LocationDisplayType.ABBREV)
                combobox.SelectedIndex = 2;
            else if (Settings.ShowCurrentRegion == Settings.LocationDisplayType.FULL &&
                Settings.ShowCurrentCountry == Settings.LocationDisplayType.FULL)
                combobox.SelectedIndex = 3;
        }

        private void CurrentIconComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var combobox = sender as ComboBox;
            if (combobox == null)
                return;
            Settings.ShowCurrentIcon = combobox.SelectedIndex == 0 ? true : false;
        }

        private void CurrentIconComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var combobox = sender as ComboBox;
            if (combobox == null)
                return;
            combobox.SelectedIndex = Settings.ShowCurrentIcon ? 0 : 1;
        }

        private void CurrentTempIsRealComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var combobox = sender as ComboBox;
            if (combobox == null)
                return;
            Settings.ShowCurrentRealTemp = combobox.SelectedIndex == 0 ? true : false;
            CurrentForecast.IsMainRealTemp = Settings.ShowCurrentRealTemp;
        }

        private void CurrentTempIsRealComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var combobox = sender as ComboBox;
            if (combobox == null)
                return;
            combobox.SelectedIndex = Settings.ShowCurrentRealTemp ? 0 : 1;
        }

        private void AltTempCheckbox_Loaded(object sender, RoutedEventArgs e)
        {
            var checkbox = sender as CheckBox;
            if (checkbox == null)
                return;
            checkbox.IsChecked = Settings.ShowCurrentAltTemp ? true : false;
        }

        private void WindSpeedCheckbox_Loaded(object sender, RoutedEventArgs e)
        {
            var checkbox = sender as CheckBox;
            if (checkbox == null)
                return;
            checkbox.IsChecked = Settings.ShowCurrentWindSpeed ? true : false;
        }

        private void VisibilityCheckbox_Loaded(object sender, RoutedEventArgs e)
        {
            var checkbox = sender as CheckBox;
            if (checkbox == null)
                return;
            checkbox.IsChecked = Settings.ShowCurrentVisibility ? true : false;
        }

        private void BarometerCheckbox_Loaded(object sender, RoutedEventArgs e)
        {
            var checkbox = sender as CheckBox;
            if (checkbox == null)
                return;
            checkbox.IsChecked = Settings.ShowCurrentPressure ? true : false;
        }

        private void HumidityCheckbox_Loaded(object sender, RoutedEventArgs e)
        {
            var checkbox = sender as CheckBox;
            if (checkbox == null)
                return;
            checkbox.IsChecked = Settings.ShowCurrentHumidity ? true : false;
        }

        private void DewPointCheckbox_Loaded(object sender, RoutedEventArgs e)
        {
            var checkbox = sender as CheckBox;
            if (checkbox == null)
                return;
            checkbox.IsChecked = Settings.ShowCurrentDewPoint ? true : false;
        }
    }
}
