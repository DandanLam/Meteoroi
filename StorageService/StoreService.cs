using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Store;
using Windows.Storage;

namespace StorageService
{
    public class StoreService
    {
        public const string IAP_KEY = "WeatherBalloon_Sale";
        public const string TRIAL_ENABLED_ON_KEY = "Trial_Enabled_On";

        public static bool IsProUnlocked()
        {
            if (WasPurchaseMade() || IsTrialActive())
                return true;
            else
                return false;
        }

        private static bool IsTrialActive()
        {
            if (IsKeyPresent(TRIAL_ENABLED_ON_KEY))
            {
                var trialEnabledDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddDays((int)ApplicationData.Current.LocalSettings.Values[TRIAL_ENABLED_ON_KEY]);
                if (trialEnabledDate.AddDays(15) >= new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day))
                {
                    return true;
                }

            }
            return false;
        }

        public static bool TrialAvailable()
        {
            if (!IsKeyPresent(TRIAL_ENABLED_ON_KEY))
                return true;
            else
                return false;
        }

        public static void ActivateTrial()
        {
            if (TrialAvailable())
            {
                var timespanSinceEpoch = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                Settings.SetIntValue(TRIAL_ENABLED_ON_KEY, (int)timespanSinceEpoch.TotalDays);
            }
        }

        private static bool WasPurchaseMade()
        {
            if (IsKeyPresent(IAP_KEY))
                return true;

            //LicenseInformation licenseInformation = licenseInformation = CurrentApp.LicenseInformation;
            LicenseInformation licenseInformation = CurrentAppSimulator.LicenseInformation;
            if (licenseInformation.ProductLicenses[IAP_KEY].IsActive || licenseInformation.ProductLicenses["WeatherBalloon_Full"].IsActive)
            {
                Settings.SetBoolValue(IAP_KEY, true);
                return true;
            }
            else
                return false;
        }

        private static bool IsKeyPresent(string key)
        {
            return ApplicationData.Current.LocalSettings.Values.ContainsKey(key);
        }

    }
}
