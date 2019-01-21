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
        public const string IAP_KEY  = "WeatherBalloon_Pro_Sale";
        public const string IAP_KEY2 = "WeatherBalloon_Pro";
        public const string TRIAL_ENABLED_ON_KEY = "Trial_Enabled_On";
        public const string INSTALL_DATE_KEY= "INSTALL_DATE";

        public static bool IsProUnlocked()
        {
            if (WasPurchaseMade() || IsTrialActive())
                return true;
            else
                return false;
        }

        public static int PromoDaysRemaining()
        {
            DateTimeOffset installDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            if (IsKeyPresent(INSTALL_DATE_KEY))
            {
                installDate = installDate.AddDays((int)ApplicationData.Current.LocalSettings.Values[INSTALL_DATE_KEY]);
            }
            else
            {
                Settings.SetIntValue(INSTALL_DATE_KEY, (int)(DateTimeOffset.UtcNow - installDate).Days);
                installDate = DateTimeOffset.UtcNow;
            }

            var timeDelta = installDate.AddDays(14) - DateTimeOffset.UtcNow;
            return timeDelta.Days;
        }

        public static int TrialDaysRemaining()
        {
            var timeDelta = TrialActivatedOn().AddDays(14) - DateTimeOffset.UtcNow;
            return timeDelta.Days;
        }

        public static DateTimeOffset TrialActivatedOn()
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            if (IsKeyPresent(TRIAL_ENABLED_ON_KEY))
            {
                return epoch.AddDays((int)ApplicationData.Current.LocalSettings.Values[TRIAL_ENABLED_ON_KEY]);
            }
            else
                return epoch;
        }

        public static bool IsTrialActive()
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

        public static bool WasPurchaseMade()
        {
            if (IsKeyPresent(IAP_KEY))
                return true;

            var keyList = new List<string> { IAP_KEY, IAP_KEY2 };
            bool keyFound = false;
            LicenseInformation licenseInformation = licenseInformation = CurrentApp.LicenseInformation;
            //LicenseInformation licenseInformation = CurrentAppSimulator.LicenseInformation;
            foreach (var key in keyList)
            {
                if (licenseInformation.ProductLicenses[IAP_KEY].IsActive)
                {
                    keyFound = true;
                    break;
                }
            }

            if (keyFound)
            {
                Settings.SetBoolValue(IAP_KEY, true);
                return true;
            }
            else
                return false;
        }

        public static async Task<bool> PurchaseAddOn(bool isSaleItem)
        {
            if (IsKeyPresent(IAP_KEY))
                return true;

            LicenseInformation licenseInformation = licenseInformation = CurrentApp.LicenseInformation;
            //LicenseInformation licenseInformation = CurrentAppSimulator.LicenseInformation;
            if (licenseInformation != null)
            {
                var productId = isSaleItem ? IAP_KEY : IAP_KEY2;
                if (!licenseInformation.ProductLicenses[productId].IsActive)
                {
                    await CurrentApp.RequestProductPurchaseAsync(productId);
                    return licenseInformation.ProductLicenses[productId].IsActive ? true : false;

                }
            }
            return false;
        }

        private static bool IsKeyPresent(string key)
        {
            return ApplicationData.Current.LocalSettings.Values.ContainsKey(key);
        }

    }
}
