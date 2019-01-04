using NotificationService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherService;
using Windows.ApplicationModel.Background;

namespace BackgroundTasks
{
    public sealed class DataRefresher : IBackgroundTask
    {
        BackgroundTaskDeferral _deferral;

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            _deferral = taskInstance.GetDeferral();
            try
            {
                DarkSkyService weatherService = new DarkSkyService();
                var weatherData = (DateTime.Now.Hour > 17 && DateTime.Now.Hour < 5) ? await weatherService.GetWeatherData(120) : await weatherService.GetWeatherData(60);
                LiveTiles.SetLiveTile(weatherData);
            }
            catch { }
            _deferral.Complete();
        }

        private void OnCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {

        }
    }
}