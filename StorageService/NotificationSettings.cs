using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageService
{
    public class NotificationSettings : Settings
    {
        private const string NOTIFICATIONS_ENABLED_KEY = "NOTIFICATIONS_ENABLED";
        private const string NOTIFY_IF_ALL_CONDITIONS_MET_KEY = "NOTIFY_IF_ALL_CONDITIONS_MET";
        private const string NOTIFY_HIGH_TEMP_THRESHOLD_ENABLED_KEY = "NOTIFY_HIGH_TEMP_THRESHOLD_ENABLED";
        private const string NOTIFY_HIGH_TEMP_THRESHOLD_KEY = "NOTIFY_HIGH_TEMP_THRESHOLD";
        private const string NOTIFY_LOW_TEMP_THRESHOLD_ENABLED_KEY = "NOTIFY_LOW_TEMP_THRESHOLD_ENABLED";
        private const string NOTIFY_LOW_TEMP_THRESHOLD_KEY = "NOTIFY_LOW_TEMP_THRESHOLD";
        private const string NOTIFY_HIGH_FEELSLIKE_TEMP_THRESHOLD_ENABLED_KEY = "NOTIFY_HIGH_FEELSLIKE_TEMP_THRESHOLD_ENABLED";
        private const string NOTIFY_HIGH_FEELSLIKE_TEMP_THRESHOLD_KEY = "NOTIFY_HIGH_FEELSLIKE_TEMP_THRESHOLD";
        private const string NOTIFY_LOW_FEELSLIKE_TEMP_THRESHOLD_ENABLED_KEY = "NOTIFY_LOW_FEELSLIKE_TEMP_THRESHOLD_ENABLED";
        private const string NOTIFY_LOW_FEELSLIKE_TEMP_THRESHOLD_KEY = "NOTIFY_LOW_FEELSLIKE_TEMP_THRESHOLD";
        private const string NOTIFY_PERCIP_THRESHOLD_ENABLED_KEY = "NOTIFY_PERCIP_THRESHOLD_ENABLED";
        private const string NOTIFY_PERCIP_THRESHOLD_KEY = "NOTIFY_PERCIP_THRESHOLD";
        private const string NOTIFY_UX_IDX_THRESHOLD_ENABLED_KEY = "NOTIFY_UX_IDX_THRESHOLD_ENABLED";
        private const string NOTIFY_UX_IDX_THRESHOLD_KEY = "NOTIFY_UX_IDX_THRESHOLD";
        private const string NOTIFY_CONDITION_TYPE_ENABLED_KEY = "NOTIFY_CONDITION_TYPE_ENABLED";
        private const string NOTIFY_CONDITION_TYPE_KEY = "NOTIFY_CONDITION_TYPE";

        public enum NotificationType { ALL, ANY, NONE }
        public enum WeatherType { CLEAR, RAIN, FOG, PARTLY_CLOUDY, CLOUDY, SNOW, SLEET }

        public static bool NotificationsEnabled
        {
            get
            {
                return GetBoolValue(NOTIFICATIONS_ENABLED_KEY, true);
            }
            set
            {
                SetBoolValue(NOTIFICATIONS_ENABLED_KEY, value);
            }
        }
        public static NotificationType NotificationSelected
        {
            get
            {
                if (!NotificationsEnabled)
                    return NotificationType.NONE;
                return GetBoolValue(NOTIFY_IF_ALL_CONDITIONS_MET_KEY, false) ? NotificationType.ALL : NotificationType.ANY;
            }
            set
            {
                switch (value)
                {
                    case NotificationType.ALL:
                        SetBoolValue(NOTIFY_IF_ALL_CONDITIONS_MET_KEY, true);
                        break;
                    case NotificationType.ANY:
                        SetBoolValue(NOTIFY_IF_ALL_CONDITIONS_MET_KEY, false);
                        break;
                    case NotificationType.NONE:
                        NotificationsEnabled = false;
                        break;
                }
                
            }
        }
        public static bool HighTempThresholdIsEnabled
        {
            get
            {
                return GetBoolValue(NOTIFY_HIGH_TEMP_THRESHOLD_ENABLED_KEY, true);
            }
            set
            {
                SetBoolValue(NOTIFY_HIGH_TEMP_THRESHOLD_ENABLED_KEY, true);
            }
        }
        public static int HighTempThreshold
        {
            get
            {
                return GetIntValue(NOTIFY_HIGH_TEMP_THRESHOLD_KEY, 90);
            }
            set
            {
                SetIntValue(NOTIFY_HIGH_TEMP_THRESHOLD_KEY, value);
            }
        }
        public static int TryParseInt(int oldValue, string newValue)
        {
            var temp = oldValue;
            int.TryParse(newValue, out temp);
            return temp;
        }
        public static bool LowTempThresholdIsEnabled
        {
            get
            {
                return GetBoolValue(NOTIFY_LOW_TEMP_THRESHOLD_ENABLED_KEY, true);
            }
            set
            {
                SetBoolValue(NOTIFY_LOW_TEMP_THRESHOLD_ENABLED_KEY, true);
            }
        }
        public static int LowTempThreshold
        {
            get
            {
                return GetIntValue(NOTIFY_LOW_TEMP_THRESHOLD_KEY, 45);
            }
            set
            {
                SetIntValue(NOTIFY_LOW_TEMP_THRESHOLD_KEY, value);
            }
        }
        public static bool HighFeelsLikeTempThresholdIsEnabled
        {
            get
            {
                return IsKeyPresent(NOTIFY_HIGH_FEELSLIKE_TEMP_THRESHOLD_KEY);
            }
            set
            {
                if (value)
                    SetBoolValue(NOTIFY_HIGH_FEELSLIKE_TEMP_THRESHOLD_KEY, true);
                else
                    RemoveValue(NOTIFY_HIGH_FEELSLIKE_TEMP_THRESHOLD_KEY);
            }
        }
        public static int HighFeelsLikeTempThreshold
        {
            get
            {
                return GetIntValue(NOTIFY_HIGH_FEELSLIKE_TEMP_THRESHOLD_KEY, 90);
            }
            set
            {
                SetIntValue(NOTIFY_HIGH_FEELSLIKE_TEMP_THRESHOLD_KEY, value);
            }
        }
        public static bool LowFeelsLikeTempThresholdIsEnabled
        {
            get
            {
                return IsKeyPresent(NOTIFY_HIGH_FEELSLIKE_TEMP_THRESHOLD_ENABLED_KEY);
            }
            set
            {
                if (value)
                    SetBoolValue(NOTIFY_HIGH_FEELSLIKE_TEMP_THRESHOLD_ENABLED_KEY, true);
                else
                    RemoveValue(NOTIFY_HIGH_FEELSLIKE_TEMP_THRESHOLD_ENABLED_KEY);
            }
        }
        public static int LowFeelsLikeTempThreshold
        {
            get
            {
                return GetIntValue(NOTIFY_LOW_FEELSLIKE_TEMP_THRESHOLD_KEY, 45);
            }
            set
            {
                SetIntValue(NOTIFY_LOW_FEELSLIKE_TEMP_THRESHOLD_KEY, value);
            }
        }
        public static bool PercipitationThresholdIsEnabled
        {
            get
            {
                return IsKeyPresent(NOTIFY_PERCIP_THRESHOLD_ENABLED_KEY);
            }
            set
            {
                if (value)
                    SetBoolValue(NOTIFY_PERCIP_THRESHOLD_ENABLED_KEY, true);
                else
                    RemoveValue(NOTIFY_PERCIP_THRESHOLD_ENABLED_KEY);
            }
        }
        public static int PercipitationThreshold
        {
            get
            {
                return GetIntValue(NOTIFY_PERCIP_THRESHOLD_KEY, 50);
            }
            set
            {
                SetIntValue(NOTIFY_PERCIP_THRESHOLD_KEY, value);
            }
        }
        public static bool UvIndexThresholdIsEnabled
        {
            get
            {
                return IsKeyPresent(NOTIFY_UX_IDX_THRESHOLD_ENABLED_KEY);
            }
            set
            {
                if (value)
                    SetBoolValue(NOTIFY_UX_IDX_THRESHOLD_ENABLED_KEY, true);
                else
                    RemoveValue(NOTIFY_UX_IDX_THRESHOLD_ENABLED_KEY);
            }
        }
        public static int UvIndexThreshold
        {
            get
            {
                return GetIntValue(NOTIFY_UX_IDX_THRESHOLD_KEY, 4);
            }
            set
            {
                SetIntValue(NOTIFY_UX_IDX_THRESHOLD_KEY, value);
            }
        }
        public static bool ConditionTypeTriggerIsEnabled
        {
            get
            {
                return IsKeyPresent(NOTIFY_CONDITION_TYPE_ENABLED_KEY);
            }
            set
            {
                if (value)
                    SetBoolValue(NOTIFY_CONDITION_TYPE_ENABLED_KEY, true);
                else
                    RemoveValue(NOTIFY_CONDITION_TYPE_ENABLED_KEY);
            }
        }
        public static WeatherType ConditionTypeTrigger
        {
            get
            {
                switch (GetIntValue(NOTIFY_CONDITION_TYPE_KEY, 1))
                {
                    case 0:
                        return WeatherType.CLEAR;
                    default:
                    case 1:
                        return WeatherType.RAIN;
                    case 2:
                        return WeatherType.FOG;
                    case 3:
                        return WeatherType.PARTLY_CLOUDY;
                    case 4:
                        return WeatherType.CLOUDY;
                    case 5:
                        return WeatherType.SNOW;
                    case 6:
                        return WeatherType.SLEET;
                }
            }
            set
            {
                var weatherType = 1;
                switch (value)
                {
                    case WeatherType.CLEAR:
                        weatherType = 0;
                        break;
                    case WeatherType.RAIN:
                        weatherType = 1;
                        break;
                    case WeatherType.FOG:
                        weatherType = 2;
                        break;
                    case WeatherType.PARTLY_CLOUDY:
                        weatherType = 3;
                        break;
                    case WeatherType.CLOUDY:
                        weatherType = 4;
                        break;
                    case WeatherType.SNOW:
                        weatherType = 5;
                        break;
                    case WeatherType.SLEET:
                        weatherType = 6;
                        break;
                }
                SetIntValue(NOTIFY_CONDITION_TYPE_KEY, weatherType);
            }
        }
    }
}
