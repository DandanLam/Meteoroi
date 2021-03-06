﻿using Meteoroi.ViewModels;
using Microsoft.Toolkit.Uwp.UI.Animations;
using NotificationService;
using StorageService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using WeatherService;
using Windows.ApplicationModel.Store;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
            SplashImage();
            RefreshInForeground();
        }
        async void SplashImage()
        {
            while(HourlyForecasts.Count == 0)
            {
                await Task.Delay(100);
            }
            await SplashImg.Fade(value: 0f, duration: 500, delay: 0).StartAsync();
            SplashImg.Visibility = Visibility.Collapsed;
            if (Settings.ShowTutorialOverlay)
            {
                ShowGrid(TutorialGrid);
                Settings.ShowTutorialOverlay = false;
            }
            ShowGrid(MainGrid);
        }
        async Task UpdateForecastItems(int minFreshness = 10)
        {
            if (weatherData == null || DateTime.Now.Subtract(new TimeSpan(0, minFreshness, 0)) < weatherData.Currently.Data.Time)
            {
                DarkSkyService weatherService = new DarkSkyService();
                weatherData = await weatherService.GetWeatherData(minFreshness);
                UpdateCurrentForecast(weatherData);
            }
            UpdateDailyForecast(weatherData);
            UpdateHourlyForecast(weatherData);
            LiveTiles.SetLiveTile(weatherData);
        }

        async void RefreshInForeground()
        {
            while (true)
            {
                await UpdateForecastItems();
                await Task.Delay(new TimeSpan(0, 1, 0));
            }
        }

        void UpdateCurrentForecast(WeatherData weatherData)
        {
            if (weatherData == null)
                return;
            var newItem = new CurrentForecastItem(weatherData.Currently);
            newItem.Location = weatherData.Location;
            CurrentForecast.CopyFromForecast(newItem);
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

            var clickedBtn = sender as Button;
            if (clickedBtn == null)
                return;
            if (CurrentDailyInView == 0)
                clickedBtn.Foreground = new SolidColorBrush(Colors.DarkGray);
            NextDayBtn.Foreground = new SolidColorBrush(Colors.White);
        }

        private void NextDay_Click(object sender, RoutedEventArgs e)
        {
            int visibleBlocks = (int)DailyGridView.ActualWidth / 133;
            DailyGridView.ScrollIntoView(DailyForecasts.Last());
            CurrentDailyInView = CalcNext(DailyForecasts.Count, CurrentDailyInView, visibleBlocks);
            DailyGridView.ScrollIntoView(DailyForecasts[CurrentDailyInView]);

            var clickedBtn = sender as Button;
            if (clickedBtn == null)
                return;
            if (CurrentDailyInView == DailyForecasts.Count - 1)
                clickedBtn.Foreground = new SolidColorBrush(Colors.DarkGray);
            PrevDayBtn.Foreground = new SolidColorBrush(Colors.White);
        }

        private void PrevHour_Click(object sender, RoutedEventArgs e)
        {
            int visibleBlocks = -(int)HourlyGridView.ActualWidth / 133;
            HourlyGridView.ScrollIntoView(HourlyForecasts.First());
            CurrentHourlyInView = CalcNext(HourlyForecasts.Count, CurrentHourlyInView, visibleBlocks);
            HourlyGridView.ScrollIntoView(HourlyForecasts[CurrentHourlyInView]);

            var clickedBtn = sender as Button;
            if (clickedBtn == null)
                return;
            if (CurrentDailyInView == 0)
                clickedBtn.Foreground = new SolidColorBrush(Colors.DarkGray);
            NextHourBtn.Foreground = new SolidColorBrush(Colors.White);
        }

        private void NextHour_Click(object sender, RoutedEventArgs e)
        {
            int visibleBlocks = (int)HourlyGridView.ActualWidth / 133;
            HourlyGridView.ScrollIntoView(HourlyForecasts.Last());
            CurrentHourlyInView = CalcNext(HourlyForecasts.Count, CurrentHourlyInView, visibleBlocks);
            HourlyGridView.ScrollIntoView(HourlyForecasts[CurrentHourlyInView]);

            var clickedBtn = sender as Button;
            if (clickedBtn == null)
                return;
            if (CurrentDailyInView == DailyForecasts.Count - 1)
                clickedBtn.Foreground = new SolidColorBrush(Colors.DarkGray);
            PrevHourBtn.Foreground = new SolidColorBrush(Colors.White);
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
                if (PageIsLoading || changedGridView == null || Math.Round(changedGridView.ActualWidth, 0) == Math.Round(DetailsStackPanel.ActualWidth, 0))
                    return;
                
                DetailsStackPanel.Width = changedGridView.ActualWidth;
                DailyHeaderGrid.Width = changedGridView.ActualWidth - 20;
                HourlyHeaderGrid.Width = changedGridView.ActualWidth - 20;
                CurrentHeaderGrid.Width = changedGridView.ActualWidth - 20;
                CurrentStackPanel.Width = changedGridView.ActualWidth + 90;

                if (changedGridView.ActualWidth >= 728)
                {
                    MicrosoftAdControl.Width = 728;
                    MicrosoftAdControl.Height = 90;
                }
                else if (changedGridView.ActualWidth >= 480)
                {
                    MicrosoftAdControl.Width = 480;
                    MicrosoftAdControl.Height = 80;
                }
                else if (changedGridView.ActualWidth >= 300)
                {
                    MicrosoftAdControl.Width = 300;
                    MicrosoftAdControl.Height = 50;
                }
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
                case "HourlyLine3Box":
                    HourlyForecastItem.Line3 = idx;
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

        private void DailyDateBox_Loaded(object sender, RoutedEventArgs e)
        {
            var changedComboBox = sender as ComboBox;
            if (changedComboBox == null)
                return;

            int idx = 0;
            switch (DailyForecastItem.DateForemat)
            {
                case "ddd d":
                    idx = 0;
                    break;
                case "d ddd":
                    idx = 1;
                    break;
                case "dddd d":
                    idx = 2;
                    break;
                case "d dddd":
                    idx = 3;
                    break;
                case "MMM d":
                    idx = 4;
                    break;
                case "d MMM":
                    idx = 5;
                    break;
                case "MMMM d":
                    idx = 6;
                    break;
                case "d MMMM":
                    idx = 7;
                    break;
            }

            changedComboBox.SelectedIndex = idx;
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

        private void DailyIconBox_Loaded(object sender, RoutedEventArgs e)
        {
            var changedComboBox = sender as ComboBox;
            if (changedComboBox == null)
                return;
            changedComboBox.SelectedIndex = DailyForecastItem.ShowIcon ? 0 : 1;
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

        private void WeeklyDescVisibility_Loaded(object sender, RoutedEventArgs e)
        {
            var changedComboBox = sender as ComboBox;
            if (changedComboBox == null || WeeklySummaryTextBlock == null)
                return;
            changedComboBox.SelectedIndex = Settings.DailyShowSummary ? 0 : 1;
        }

        private void WeeklyDescVisibility_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var changedComboBox = sender as ComboBox;
            if (changedComboBox == null || WeeklySummaryTextBlock == null)
                return;
            try
            {
                WeeklySummaryTextBlock.Visibility = changedComboBox.SelectedIndex == 0 ? Visibility.Visible : Visibility.Collapsed;
                Settings.DailyShowSummary = changedComboBox.SelectedIndex == 0 ? true : false;
            }
            catch { }
        }

        private void DailyTempType_Loaded(object sender, RoutedEventArgs e)
        {
            var changedComboBox = sender as ComboBox;
            if (changedComboBox == null || WeeklySummaryTextBlock == null)
                return;
            changedComboBox.SelectedIndex  = DailyForecastItem.RealTemp ? 0 : 1;
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

        private void HourlyDescVisibility_Loaded(object sender, RoutedEventArgs e)
        {
            var changedComboBox = sender as ComboBox;
            if (changedComboBox == null || WeeklySummaryTextBlock == null)
                return;

            changedComboBox.SelectedIndex = Settings.HourlyShowSummary ? 0 : 1;
        }

        private void HourlyDescVisibility_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var changedComboBox = sender as ComboBox;
            if (changedComboBox == null || WeeklySummaryTextBlock == null)
                return;
            try
            {
                HourlySummaryTextBlock.Visibility = changedComboBox.SelectedIndex == 0 ? Visibility.Visible : Visibility.Collapsed;
                Settings.HourlyShowSummary = changedComboBox.SelectedIndex == 0 ? true : false;
            }
            catch { }
        }

        private void HourlyTempType_Loaded(object sender, RoutedEventArgs e)
        {
            var changedComboBox = sender as ComboBox;
            if (changedComboBox == null || HourlySummaryTextBlock == null)
                return;

            changedComboBox.SelectedIndex = HourlyForecastItem.RealTemp ? 0 : 1;
        }

        private void HourlyTempType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var changedComboBox = sender as ComboBox;
            if (changedComboBox == null || HourlySummaryTextBlock == null)
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

        private void HourlyIconBox_Loaded(object sender, RoutedEventArgs e)
        {
            var changedComboBox = sender as ComboBox;
            if (changedComboBox == null)
                return;
            changedComboBox.SelectedIndex = HourlyForecastItem.ShowIcon ? 0 : 1;
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
                List<ComboBox> DailyComboBoxes = new List<ComboBox> { HourlyLine1Box, HourlyLine2Box, HourlyLine3Box };
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

        private void HourlyTimeBox_Loaded(object sender, RoutedEventArgs e)
        {
            var changedComboBox = sender as ComboBox;
            if (changedComboBox == null)
                return;

            int idx = 0;
            switch (HourlyForecastItem.TimeFormat)
            {
                case "H":
                    idx = 0;
                    break;
                case "HH":
                    idx = 1;
                    break;
                case "h tt":
                    idx = HourlyForecastItem.TimeFormatToLower ? 2 : 3;
                    break;
                case "h:00 tt":
                    idx = HourlyForecastItem.TimeFormatToLower ? 4 : 5;
                    break;
                case "H:00":
                    idx = 6;
                    break;
                case "HH:00":
                    idx = 7;
                    break;
            }

            changedComboBox.SelectedIndex = idx;
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
                        HourlyForecastItem.TimeFormat = "H";
                        break;
                    case 1:
                        HourlyForecastItem.TimeFormat = "HH";
                        break;
                    case 2:
                        HourlyForecastItem.TimeFormat = "h tt";
                        HourlyForecastItem.TimeFormatToLower = true;
                        break;
                    case 3:
                        HourlyForecastItem.TimeFormat = "h tt";
                        HourlyForecastItem.TimeFormatToLower = false;
                        break;
                    case 4:
                        HourlyForecastItem.TimeFormat = "h:00 tt";
                        HourlyForecastItem.TimeFormatToLower = true;
                        break;
                    case 5:
                        HourlyForecastItem.TimeFormat = "h:00 tt";
                        HourlyForecastItem.TimeFormatToLower = false;
                        break;
                    case 6:
                        HourlyForecastItem.TimeFormat = "H:00";
                        break;
                    case 7:
                        HourlyForecastItem.TimeFormat = "HH:00";
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
                    CurrentForecast.RegionFormat = Settings.LocationDisplayType.HIDDEN;
                    CurrentForecast.CountryFormat = Settings.LocationDisplayType.HIDDEN;
                    break;
                case 1:
                    CurrentForecast.RegionFormat = Settings.LocationDisplayType.ABBREV;
                    CurrentForecast.CountryFormat = Settings.LocationDisplayType.HIDDEN;
                    break;
                case 2:
                    CurrentForecast.RegionFormat = Settings.LocationDisplayType.ABBREV;
                    CurrentForecast.CountryFormat = Settings.LocationDisplayType.ABBREV;
                    break;
                case 3:
                    CurrentForecast.RegionFormat = Settings.LocationDisplayType.ABBREV;
                    CurrentForecast.CountryFormat = Settings.LocationDisplayType.FULL;
                    break;
                case 4:
                    CurrentForecast.RegionFormat = Settings.LocationDisplayType.FULL;
                    CurrentForecast.CountryFormat = Settings.LocationDisplayType.ABBREV;
                    break;
            }
            //CurrentForecast.LocationString = Guid.NewGuid().ToString();
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
            CurrentForecast.IsMainRealTemp = combobox.SelectedIndex == 0 ? true : false;
            Settings.ShowCurrentRealTemp = Settings.ShowCurrentRealTemp;
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

        private void HourlyLineBox_Loaded(object sender, RoutedEventArgs e)
        {
            var combobox = sender as ComboBox;
            if (combobox == null)
                return;
            switch (combobox.Name)
            {
                case "HourlyLine1Box":
                    combobox.SelectedIndex = Settings.HourlyLine1;
                    break;
                case "HourlyLine2Box":
                    combobox.SelectedIndex = Settings.HourlyLine2;
                    break;
                case "HourlyLine3Box":
                    combobox.SelectedIndex = Settings.HourlyLine3;
                    break;
            }
        }

        private void DailyLineBox_Loaded(object sender, RoutedEventArgs e)
        {
            var combobox = sender as ComboBox;
            if (combobox == null)
                return;
            switch (combobox.Name)
            {
                case "DailyLine1Box":
                    combobox.SelectedIndex = Settings.DailyLine1;
                    break;
                case "DailyLine2Box":
                    combobox.SelectedIndex = Settings.DailyLine2;
                    break;
            }
        }

        private void MedTileItemCountComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var combobox = sender as ComboBox;
            if (combobox == null)
                return;

            try { combobox.SelectedIndex = Settings.MedTileItemCount - 2;
                LiveTiles.SetLiveTile(weatherData);
            }
            catch { }
        }

        private void WideTileItemCountComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var combobox = sender as ComboBox;
            if (combobox == null)
                return;

            try { combobox.SelectedIndex = Settings.WideTileItemCount - 5;
                LiveTiles.SetLiveTile(weatherData);
            }
            catch { }
        }

        private void LiveTileHourIntervalComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var combobox = sender as ComboBox;
            if (combobox == null)
                return;

            try { combobox.SelectedIndex = Settings.TileInterval - 1;
                LiveTiles.SetLiveTile(weatherData);
            }
            catch { }
        }

        private void MedTileItemCountComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var combobox = sender as ComboBox;
            if (combobox == null || PageIsLoading)
                return;

            try {
                Settings.MedTileItemCount = int.Parse((combobox.SelectedItem as ComboBoxItem).Content as string);
                LiveTiles.SetLiveTile(weatherData);
            }
            catch { }
        }

        private void WideTileItemCountComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var combobox = sender as ComboBox;
            if (combobox == null || PageIsLoading)
                return;

            try { Settings.WideTileItemCount = int.Parse((combobox.SelectedItem as ComboBoxItem).Content as string);
                LiveTiles.SetLiveTile(weatherData);
            }
            catch { }
        }

        private void LiveTileHourIntervalComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var combobox = sender as ComboBox;
            if (combobox == null || PageIsLoading)
                return;

            try { Settings.TileInterval = int.Parse((combobox.SelectedItem as ComboBoxItem).Content as string);
                LiveTiles.SetLiveTile(weatherData);
            }
            catch { }
        }

        private void ShowHourlyLiveTile_Loaded(object sender, RoutedEventArgs e)
        {
            var checkbox = sender as CheckBox;
            if (checkbox == null)
                return;
            try { checkbox.IsChecked = Settings.ShowHourTile ? true : false; }
            catch { }
        }

        private void ShowHourlyLiveTile_Checked(object sender, RoutedEventArgs e)
        {
            var checkbox = sender as CheckBox;
            if (checkbox == null || PageIsLoading)
                return;
            try { Settings.ShowHourTile = (bool)checkbox.IsChecked; }
            catch { }
        }

        private void ShowDailyLiveTile_Loaded(object sender, RoutedEventArgs e)
        {
            var checkbox = sender as CheckBox;
            if (checkbox == null)
                return;
            try { checkbox.IsChecked = Settings.ShowDayTile ? true : false; }
            catch { }
        }

        private void ShowDailyLiveTile_Checked(object sender, RoutedEventArgs e)
        {
            var checkbox = sender as CheckBox;
            if (checkbox == null || PageIsLoading)
                return;
            try { Settings.ShowDayTile = (bool)checkbox.IsChecked; }
            catch { }
        }

        private void LiveTileEdit_Click(object sender, RoutedEventArgs e)
        {
            ShowGrid(LiveTileEditGrid);
        }

        private void LiveTileEditDone_Click(object sender, RoutedEventArgs e)
        {
            HideGrid(LiveTileEditGrid);
        }

        private void LiveTileTempComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var combobox = sender as ComboBox;
            if (combobox == null)
                return;

            try { combobox.SelectedIndex = Settings.LiveTileTempIsReal ? 0 : 1; }
            catch { }
        }

        private void LiveTileTempComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var combobox = sender as ComboBox;
            if (combobox == null || PageIsLoading)
                return;

            try { Settings.LiveTileTempIsReal = combobox.SelectedIndex == 0 ? true : false;
                LiveTiles.SetLiveTile(weatherData);
            }
            catch { }
        }

        private void TutorialGrid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try { HideGrid(TutorialGrid); }
            catch { }
        }

        private async void Upgrade_Click(object sender, RoutedEventArgs e)
        {
            var success = await StoreService.PurchaseAddOn(true);
            if (success)
                UpgradeButton.Visibility = Visibility.Collapsed;
        }

        private void Upgrade_Loaded(object sender, RoutedEventArgs e)
        {
            var clicked = sender as Button;
            if (clicked == null)
                return;

            if (StoreService.WasPurchaseMade())
                clicked.Visibility = Visibility.Collapsed;
        }

        private void Trial_Click(object sender, RoutedEventArgs e)
        {
            var clicked = sender as HyperlinkButton;
            if (clicked == null)
                return;

            if (StoreService.TrialAvailable())
                StoreService.ActivateTrial();

            ContentDialog alreadyPro = new ContentDialog()
            {
                Title = "Trial Started",
                Content = "Trial ends in 14 days.",
                PrimaryButtonText = "OK",
            };
            alreadyPro.ShowAsync();

            clicked.Visibility = Visibility.Collapsed;
        }

        private void Trial_Loaded(object sender, RoutedEventArgs e)
        {
            var clicked = sender as HyperlinkButton;
            if (clicked == null)
                return;

            if (StoreService.IsProUnlocked() || !StoreService.TrialAvailable())
                clicked.Visibility = Visibility.Collapsed;
        }

        private void TrialDaysRemaining_Loaded(object sender, RoutedEventArgs e)
        {
            var clicked = sender as TextBlock;
            if (clicked == null)
                return;

            clicked.Text = string.Concat("(ends in ", StoreService.PromoDaysRemaining(), " days)");
        }

        private void MicrosoftAd_Loaded(object sender, RoutedEventArgs e)
        {
            var adControl = sender as Microsoft.Advertising.WinRT.UI.AdControl;
            if (adControl == null)
                return;

            if ((!StoreService.IsProUnlocked() && !StoreService.TrialAvailable()) || 
                (StoreService.TrialAvailable() && StoreService.PromoDaysRemaining() < 13))
            {
                adControl.Visibility = Visibility.Visible;
            }
        }

        private void Locations_Click(object sender, RoutedEventArgs e)
        {
            

        }

        private async Task SaveProFeaturesDialog()
        {
            try
            {
                ContentDialog dialog = new ContentDialog
                {
                    Title = "Please Note",
                    Content = "Non-Pro users may only enable/disable notifications",
                    PrimaryButtonText = "Upgrade to Pro",
                };
                try { dialog.CloseButtonText = "Ok"; }
                catch { }
                if (StoreService.TrialAvailable())
                    dialog.SecondaryButtonText= "Try for Free";

                ContentDialogResult response = await dialog.ShowAsync();
                switch (response)
                {
                    case ContentDialogResult.Primary:
                        var success = await StoreService.PurchaseAddOn(StoreService.PromoDaysRemaining() > 0);
                        ContentDialog dialog1 = new ContentDialog()
                        {
                            PrimaryButtonText = "Ok"
                        };
                        dialog1.Title = success ? "Thanks for upgrading to Pro!" : "Oh no! Something went wrong. Please try again later.";
                        await dialog1.ShowAsync();
                        break;
                    case ContentDialogResult.Secondary:
                        StoreService.ActivateTrial();
                        dialog = new ContentDialog()
                        {
                            Title = "Thanks for trying Weather Balloon Pro!",
                            Content = "This trial will expire in 14 days",
                            PrimaryButtonText = "Ok"
                        };
                        await dialog.ShowAsync();
                        break;
                }
            }
            catch { }
        }

        private void NotificationsButton_Loaded(object sender, RoutedEventArgs e)
        {
            NotificationBtnText.Text = NotificationSettings.NotificationsEnabled ? "" : "";
        }

        private void NotificationsButton_Click(object sender, RoutedEventArgs e)
        {
            ShowGrid(NotificationsEditGrid);
        }

        private async void NotificationsEditDone_Click(object sender, RoutedEventArgs e)
        {
            await SaveProFeaturesDialog();
            HideGrid(NotificationsEditGrid);
        }


        private void NotifyIfCheckbox_Loaded(object sender, RoutedEventArgs e)
        {
            var checkbox = sender as CheckBox;
            if (checkbox == null)
                return;

            checkbox.IsChecked = NotificationSettings.NotificationsEnabled;
        }

        private void NotifyIfCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            var checkbox = sender as CheckBox;
            if (checkbox == null || PageIsLoading)
                return;

            try { NotificationSettings.NotificationsEnabled = (bool)checkbox.IsChecked; }
            catch { }
        }

        private void HighTempCheckbox_Loaded(object sender, RoutedEventArgs e)
        {
            var checkbox = sender as CheckBox;
            if (checkbox == null)
                return;

            checkbox.IsChecked = NotificationSettings.HighTempThresholdIsEnabled;
        }

        private void HighTempCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            var checkbox = sender as CheckBox;
            if (checkbox == null || PageIsLoading)
                return;

            try { NotificationSettings.HighTempThresholdIsEnabled = (bool)checkbox.IsChecked; }
            catch { }
        }

        private void HighTempTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            var textbox = sender as TextBox;
            if (textbox == null)
                return;

            textbox.Text = string.Concat(NotificationSettings.HighTempThreshold, "°");
        }

        private void HighTempTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textbox = sender as TextBox;
            if (textbox == null || PageIsLoading)
                return;

            NotificationSettings.HighTempThreshold = NotificationSettings.TryParseInt(NotificationSettings.HighTempThreshold, textbox.Text);
        }

        private void LowTempCheckbox_Loaded(object sender, RoutedEventArgs e)
        {
            var checkbox = sender as CheckBox;
            if (checkbox == null)
                return;

            checkbox.IsChecked = NotificationSettings.LowTempThresholdIsEnabled;
        }

        private void LowTempCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            var checkbox = sender as CheckBox;
            if (checkbox == null || PageIsLoading)
                return;

            try { NotificationSettings.LowTempThresholdIsEnabled = (bool)checkbox.IsChecked; }
            catch { }
        }

        private void LowTempTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            var textbox = sender as TextBox;
            if (textbox == null)
                return;

            textbox.Text = string.Concat(NotificationSettings.LowTempThreshold, "°");
        }

        private void LowTempTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textbox = sender as TextBox;
            if (textbox == null || PageIsLoading)
                return;

            NotificationSettings.LowTempThreshold = NotificationSettings.TryParseInt(NotificationSettings.LowTempThreshold, textbox.Text);
        }

        private void HighFeelsLikeTempCheckbox_Loaded(object sender, RoutedEventArgs e)
        {
            var checkbox = sender as CheckBox;
            if (checkbox == null)
                return;

            checkbox.IsChecked = NotificationSettings.HighFeelsLikeTempThresholdIsEnabled;
        }

        private void HighFeelsLikeTempCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            var checkbox = sender as CheckBox;
            if (checkbox == null || PageIsLoading)
                return;

            try { NotificationSettings.HighFeelsLikeTempThresholdIsEnabled = (bool)checkbox.IsChecked; }
            catch { }
        }

        private void HighFeelsLikeTempTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            var textbox = sender as TextBox;
            if (textbox == null)
                return;

            textbox.Text = string.Concat(NotificationSettings.HighFeelsLikeTempThreshold, "°");
        }

        private void HighFeelsLikeTempTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textbox = sender as TextBox;
            if (textbox == null || PageIsLoading)
                return;

            NotificationSettings.HighFeelsLikeTempThreshold = NotificationSettings.TryParseInt(NotificationSettings.HighFeelsLikeTempThreshold, textbox.Text);
        }

        private void LowFeelsLikeTempCheckbox_Loaded(object sender, RoutedEventArgs e)
        {
            var checkbox = sender as CheckBox;
            if (checkbox == null)
                return;

            checkbox.IsChecked = NotificationSettings.LowFeelsLikeTempThresholdIsEnabled;
        }

        private void LowFeelsLikeTempCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            var checkbox = sender as CheckBox;
            if (checkbox == null || PageIsLoading)
                return;

            try { NotificationSettings.LowFeelsLikeTempThresholdIsEnabled = (bool)checkbox.IsChecked; }
            catch { }
        }

        private void LowFeelsLikeTempTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            var textbox = sender as TextBox;
            if (textbox == null)
                return;

            textbox.Text = string.Concat(NotificationSettings.LowFeelsLikeTempThreshold, "°");
        }

        private void LowFeelsLikeTempTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textbox = sender as TextBox;
            if (textbox == null || PageIsLoading)
                return;

            NotificationSettings.LowFeelsLikeTempThreshold = NotificationSettings.TryParseInt(NotificationSettings.LowFeelsLikeTempThreshold, textbox.Text);
        }

        private void PercipProbCheckbox_Loaded(object sender, RoutedEventArgs e)
        {
            var checkbox = sender as CheckBox;
            if (checkbox == null)
                return;

            checkbox.IsChecked = NotificationSettings.PercipitationThresholdIsEnabled;
        }

        private void PercipProbCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            var checkbox = sender as CheckBox;
            if (checkbox == null || PageIsLoading)
                return;

            try { NotificationSettings.PercipitationThresholdIsEnabled = (bool)checkbox.IsChecked; }
            catch { }
        }

        private void PercipProbTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            var textbox = sender as TextBox;
            if (textbox == null)
                return;

            textbox.Text = string.Concat(NotificationSettings.PercipitationThreshold, "%");
        }

        private void PercipProbTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textbox = sender as TextBox;
            if (textbox == null || PageIsLoading)
                return;

            NotificationSettings.PercipitationThreshold = NotificationSettings.TryParseInt(NotificationSettings.PercipitationThreshold, textbox.Text);
        }

        private void UvIndexCheckbox_Loaded(object sender, RoutedEventArgs e)
        {
            var checkbox = sender as CheckBox;
            if (checkbox == null)
                return;

            checkbox.IsChecked = NotificationSettings.UvIndexThresholdIsEnabled;
        }

        private void UvIndexCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            var checkbox = sender as CheckBox;
            if (checkbox == null || PageIsLoading)
                return;

            try { NotificationSettings.UvIndexThresholdIsEnabled = (bool)checkbox.IsChecked; }
            catch { }
        }

        private void UvIndexTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            var textbox = sender as TextBox;
            if (textbox == null)
                return;

            textbox.Text = NotificationSettings.UvIndexThreshold.ToString();
        }

        private void UvIndexTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textbox = sender as TextBox;
            if (textbox == null || PageIsLoading)
                return;

            NotificationSettings.UvIndexThreshold = NotificationSettings.TryParseInt(NotificationSettings.UvIndexThreshold, textbox.Text);
        }

        private void ConditionTypeTriggerComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox == null)
                return;

            var comboIdx = 1;
            switch (NotificationSettings.ConditionTypeTrigger)
            {
                case NotificationSettings.WeatherType.CLEAR:
                    comboIdx = 0;
                    break;
                default:
                case NotificationSettings.WeatherType.RAIN:
                    comboIdx = 1;
                    break;
                case NotificationSettings.WeatherType.FOG:
                    comboIdx = 2;
                    break;
                case NotificationSettings.WeatherType.PARTLY_CLOUDY:
                    comboIdx = 3;
                    break;
                case NotificationSettings.WeatherType.CLOUDY:
                    comboIdx = 4;
                    break;
                case NotificationSettings.WeatherType.SNOW:
                    comboIdx = 5;
                    break;
                case NotificationSettings.WeatherType.SLEET:
                    comboIdx = 6;
                    break;
            }
            comboBox.SelectedIndex = comboIdx;
        }

        private void ConditionTypeTriggerComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox == null || PageIsLoading)
                return;

            var selectedType = NotificationSettings.WeatherType.RAIN;
            switch (comboBox.SelectedIndex)
            {
                case 0:
                    selectedType = NotificationSettings.WeatherType.CLEAR;
                    break;
                default:
                case 1:
                    selectedType = NotificationSettings.WeatherType.RAIN;
                    break;
                case 2:
                    selectedType = NotificationSettings.WeatherType.FOG;
                    break;
                case 3:
                    selectedType = NotificationSettings.WeatherType.PARTLY_CLOUDY;
                    break;
                case 4:
                    selectedType = NotificationSettings.WeatherType.CLOUDY;
                    break;
                case 5:
                    selectedType = NotificationSettings.WeatherType.SNOW;
                    break;
                case 6:
                    selectedType = NotificationSettings.WeatherType.SLEET;
                    break;
            }
            NotificationSettings.ConditionTypeTrigger = selectedType;
        }

    }
}