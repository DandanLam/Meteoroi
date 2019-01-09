using NotificationService;
using StorageService;
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
                var weatherData = (DateTime.Now.Hour > 17 && DateTime.Now.Hour < 5) ? await weatherService.GetWeatherData(119) : await weatherService.GetWeatherData(59);
                LiveTiles.SetLiveTile(weatherData);

                if (StoreService.IsTrialActive() && StoreService.TrialDaysRemaining() <= 1 && DateTime.Now.Hour == 8)
                    Toasts.CreateNotification("Pro Trial Expires Soon!", "Upgrade if you enjoyed the benefits so far", "ms-appx:///Assets/Logo.png");
                if (StoreService.PromoDaysRemaining() <= 1 && DateTime.Now.Hour == 8)
                    Toasts.CreateNotification("New User Promo Expires Soon!", "Upgrade today to get 20%!", "ms-appx:///Assets/Logo.png");
            }
            catch { }
            _deferral.Complete();
        }

        private void OnCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {

        }
    }

    public sealed class DeferredStart : IBackgroundTask
    {
        BackgroundTaskDeferral _deferral;

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            _deferral = taskInstance.GetDeferral();
            try
            {
                RegisterBackgroundTask("BackgroundTasks.DeferredStart", "DeferredStart", new TimeTrigger(60, false), new SystemCondition(SystemConditionType.InternetAvailable));
            }
            catch { }
            _deferral.Complete();
        }

        private void OnCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {

        }

        private async Task RegisterBackgroundTask(String taskEntryPoint, String name, IBackgroundTrigger trigger = null, IBackgroundCondition condition = null)
        {
            try
            {
                foreach (var cur in BackgroundTaskRegistration.AllTasks)
                {
                    if (cur.Value.Name == name)
                    {
                        cur.Value.Unregister(true);
                    }
                }
                var allowed = await BackgroundExecutionManager.RequestAccessAsync();
                switch (allowed)
                {
                    case BackgroundAccessStatus.AlwaysAllowed: break;
                    case BackgroundAccessStatus.AllowedSubjectToSystemPolicy: break;

                    case BackgroundAccessStatus.DeniedBySystemPolicy: break;
                    case BackgroundAccessStatus.DeniedByUser: break;

                    case BackgroundAccessStatus.Unspecified: break;
                }

                BackgroundTaskBuilder taskBuilder = new BackgroundTaskBuilder
                {
                    Name = name,
                    CancelOnConditionLoss = false,
                    TaskEntryPoint = taskEntryPoint,
                };
                if (trigger == null)
                    taskBuilder.SetTrigger(new TimeTrigger(60, false));
                else
                    taskBuilder.SetTrigger(trigger);
                if (condition != null)
                    taskBuilder.AddCondition(condition);
                var reg = taskBuilder.Register();
            }
            catch { }

        }
    }
}