using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace StorageService
{
    public class Settings
    {
        private const string IS_METRIC_KEY = "IS_METRIC";
        private const string IS_CELCIUS_KEY = "IS_CELCIUS";
        private const string DATE_FORMAT_KEY = "DATE_FORMAT";
        private const string DAILY_ICON_KEY = "DAILY_ICON";
        private const string HOURLY_ICON_KEY = "HOURLY_ICON";
        private const string DAILY_LINE_1_KEY = "DAILY_LINE_1";
        private const string DAILY_LINE_2_KEY = "DAILY_LINE_2";
        private const string DAILY_SUMMARY_KEY = "DAILY_SUMMARY";
        private const string DAILY_SHOW_REAL_TEMP_KEY = "DAILY_SHOW_REAL_TEMP";
        private const string TIME_FORMAT_KEY = "TIME_FORMAT";
        private const string TIME_FORMAT_LOWERCASE_KEY = "TIME_FORMAT_LOWERCASE_";
        private const string HOURLY_LINE_1_KEY = "HOURLY_LINE_1";
        private const string HOURLY_LINE_2_KEY = "HOURLY_LINE_2";
        private const string HOURLY_SUMMARY_KEY = "HOURLY_SUMMARY";
        private const string HOURLY_SHOW_REAL_TEMP_KEY = "HOURLY_SHOW_REAL_TEMP";

        private const string SHOW_CURRENT_REGION_KEY = "SHOW_CURRENT_REGION";
        private const string SHOW_CURRENT_COUNTRY_KEY = "SHOW_CURRENT_COUNTRY";
        private const string SHOW_CURRENT_ICON_KEY = "SHOW_CURRENT_ICON";
        private const string SHOW_CURRENT_REAL_TEMP_KEY = "SHOW_CURRENT_REAL_TEMP";

        private const string SHOW_CURRENT_ALT_TEMP_KEY = "SHOW_CURRENT_ALT_TEMP";
        private const string SHOW_CURRENT_WINDSPEED_KEY = "SHOW_CURRENT_WINDSPEED";
        private const string SHOW_CURRENT_VISIBILITY_KEY = "SHOW_CURRENT_VISIBILITY";
        private const string SHOW_CURRENT_PRESSURE_KEY = "SHOW_CURRENT_PRESSURE";
        private const string SHOW_CURRENT_HUMIDITY_KEY = "SHOW_CURRENT_HUMIDITY";
        private const string SHOW_CURRENT_DEWPOINT_KEY = "SHOW_CURRENT_DEWPOINT";

        public static bool IsMetric
        {
            get
            {
                try
                {
                    if (IsKeyPresent(IS_METRIC_KEY) && GetBoolValue(IS_METRIC_KEY))
                        return true;
                    else
                        return false;
                }
                catch { return false; }
            }
            set
            {
                SetBoolValue(IS_METRIC_KEY, value);
            }
        }
        public static bool IsCelcius
        {
            get
            {
                if (IsKeyPresent(IS_CELCIUS_KEY) || !GetBoolValue(IS_CELCIUS_KEY))
                    return false;
                else
                    return true;
            }
            set
            {
                SetBoolValue(IS_CELCIUS_KEY, value);
            }
        }

        public enum LocationDisplayType { FULL, ABBREV, HIDDEN }
        public static LocationDisplayType ShowCurrentRegion
        {
            get
            {
                if (IsKeyPresent(SHOW_CURRENT_REGION_KEY))
                {
                    switch (GetIntValue(SHOW_CURRENT_REGION_KEY))
                    {
                        default:
                        case 0:
                            return LocationDisplayType.FULL;
                        //case 1:
                        //    return LocationDisplayType.ABBREV;
                        case 2:
                            return LocationDisplayType.HIDDEN;
                    }
                }
                return LocationDisplayType.HIDDEN;
            }
            set
            {
                switch (value)
                {
                    default:
                    case LocationDisplayType.FULL:
                        SetIntValue(SHOW_CURRENT_REGION_KEY, 0);
                        break;
                    //case LocationDisplayType.ABBREV:
                    //    SetIntValue(SHOW_CURRENT_REGION_KEY, 1);
                    //    break;
                    case LocationDisplayType.HIDDEN:
                        SetIntValue(SHOW_CURRENT_REGION_KEY, 2);
                        break;
                }
            }
        }
        public static LocationDisplayType ShowCurrentCountry
        {
            get
            {
                if (IsKeyPresent(SHOW_CURRENT_COUNTRY_KEY))
                {
                    switch (GetIntValue(SHOW_CURRENT_COUNTRY_KEY))
                    {
                        case 0:
                            return LocationDisplayType.FULL;
                        case 1:
                            return LocationDisplayType.ABBREV;
                        case 2:
                            return LocationDisplayType.HIDDEN;
                    }
                }
                return LocationDisplayType.HIDDEN;
            }
            set
            {
                switch (value)
                {
                    case LocationDisplayType.FULL:
                        SetIntValue(SHOW_CURRENT_COUNTRY_KEY, 0);
                        break;
                    case LocationDisplayType.ABBREV:
                        SetIntValue(SHOW_CURRENT_COUNTRY_KEY, 1);
                        break;
                    case LocationDisplayType.HIDDEN:
                        SetIntValue(SHOW_CURRENT_COUNTRY_KEY, 2);
                        break;
                }
            }
        }
        public static bool ShowCurrentIcon
        {
            get
            {
                if (IsKeyPresent(SHOW_CURRENT_ICON_KEY))
                    return GetBoolValue(SHOW_CURRENT_ICON_KEY);
                else
                    return true;
            }
            set
            {
                SetBoolValue(SHOW_CURRENT_ICON_KEY, value);
            }
        }
        public static bool ShowCurrentRealTemp
        {
            get
            {
               if (IsKeyPresent(SHOW_CURRENT_REAL_TEMP_KEY))
                    return GetBoolValue(SHOW_CURRENT_REAL_TEMP_KEY);
                else
                    return true;
            }
            set
            {
                SetBoolValue(SHOW_CURRENT_REAL_TEMP_KEY, value);
            }
        }

        public static bool ShowCurrentAltTemp
        {
            get
            {
                if (IsKeyPresent(SHOW_CURRENT_ALT_TEMP_KEY))
                    return GetBoolValue(SHOW_CURRENT_ALT_TEMP_KEY);
                else
                    return true;
            }
            set
            {
                SetBoolValue(SHOW_CURRENT_ALT_TEMP_KEY, value);
            }
        }
        public static bool ShowCurrentWindSpeed
        {
            get
            {
                if (IsKeyPresent(SHOW_CURRENT_WINDSPEED_KEY))
                    return GetBoolValue(SHOW_CURRENT_WINDSPEED_KEY);
                else
                    return true;
            }
            set
            {
                SetBoolValue(SHOW_CURRENT_WINDSPEED_KEY, value);
            }
        }
        public static bool ShowCurrentVisibility
        {
            get
            {
                if (IsKeyPresent(SHOW_CURRENT_VISIBILITY_KEY))
                    return GetBoolValue(SHOW_CURRENT_VISIBILITY_KEY);
                else
                    return true;
            }
            set
            {
                SetBoolValue(SHOW_CURRENT_VISIBILITY_KEY, value);
            }
        }
        public static bool ShowCurrentPressure
        {
            get
            {
                if (IsKeyPresent(SHOW_CURRENT_PRESSURE_KEY))
                    return GetBoolValue(SHOW_CURRENT_PRESSURE_KEY);
                else
                    return true;
            }
            set
            {
                SetBoolValue(SHOW_CURRENT_PRESSURE_KEY, value);
            }
        }
        public static bool ShowCurrentHumidity
        {
            get
            {
                if (IsKeyPresent(SHOW_CURRENT_HUMIDITY_KEY))
                    return GetBoolValue(SHOW_CURRENT_HUMIDITY_KEY);
                else
                    return true;
            }
            set
            {
                SetBoolValue(SHOW_CURRENT_HUMIDITY_KEY, value);
            }
        }
        public static bool ShowCurrentDewPoint
        {
            get
            {
                if (IsKeyPresent(SHOW_CURRENT_DEWPOINT_KEY))
                    return GetBoolValue(SHOW_CURRENT_DEWPOINT_KEY);
                else
                    return true;
            }
            set
            {
                SetBoolValue(SHOW_CURRENT_DEWPOINT_KEY, value);
            }
        }

        public static string DateFormat
        {
            get
            {
                if (IsKeyPresent(DATE_FORMAT_KEY))
                    return GetStringValue(DATE_FORMAT_KEY);
                else
                    return "ddd d";
            }
            set
            {
                SetStringValue(DATE_FORMAT_KEY, value);
            }
        }
        public static bool ShowDailyIcon
        {
            get
            {
                if (IsKeyPresent(DAILY_ICON_KEY))
                    return GetBoolValue(DAILY_ICON_KEY);
                else
                    return true;
            }
            set
            {
                SetBoolValue(DAILY_ICON_KEY, value);
            }
        }
        public static bool ShowHourlyIcon
        {
            get
            {
                if (IsKeyPresent(HOURLY_ICON_KEY))
                    return GetBoolValue(HOURLY_ICON_KEY);
                else
                    return true;
            }
            set
            {
                SetBoolValue(HOURLY_ICON_KEY, value);
            }
        }
        public static int DailyLine1
        {
            get
            {
                if (IsKeyPresent(DAILY_LINE_1_KEY))
                    return GetIntValue(DAILY_LINE_1_KEY);
                else
                    return 8;
            }
            set
            {
                SetIntValue(DAILY_LINE_1_KEY, value);
            }
        }
        public static int DailyLine2
        {
            get
            {
                if (IsKeyPresent(DAILY_LINE_2_KEY))
                    return GetIntValue(DAILY_LINE_2_KEY);
                else
                    return 0;
            }
            set
            {
                SetIntValue(DAILY_LINE_2_KEY, value);
            }
        }
        public static bool DailyShowSummary
        {
            get
            {
                if (IsKeyPresent(DAILY_SUMMARY_KEY))
                    return GetBoolValue(DAILY_SUMMARY_KEY);
                else
                    return true;
            }
            set
            {
                SetBoolValue(DAILY_SUMMARY_KEY, value);
            }
        }
        public static bool DailyShowRealTemp
        {
            get
            {
                if (IsKeyPresent(DAILY_SHOW_REAL_TEMP_KEY))
                    return GetBoolValue(DAILY_SHOW_REAL_TEMP_KEY);
                else
                    return true;
            }
            set
            {
                SetBoolValue(DAILY_SHOW_REAL_TEMP_KEY, value);
            }
        }

        public static string TimeFormat
        {
            get
            {
                if (IsKeyPresent(TIME_FORMAT_KEY))
                    return GetStringValue(TIME_FORMAT_KEY);
                else
                    return "h tt";
            }
            set
            {
                SetStringValue(TIME_FORMAT_KEY, value);
            }
        }
        public static bool TimeFormatIsLowercase
        {
            get
            {
                if (IsKeyPresent(TIME_FORMAT_LOWERCASE_KEY))
                    return GetBoolValue(TIME_FORMAT_LOWERCASE_KEY);
                else
                    return true;
            }
            set
            {
                SetBoolValue(TIME_FORMAT_LOWERCASE_KEY, value);
            }
        }
        public static int HourlyLine1
        {
            get
            {
                if (IsKeyPresent(HOURLY_LINE_1_KEY))
                    return GetIntValue(HOURLY_LINE_1_KEY);
                else
                    return 8;
            }
            set
            {
                SetIntValue(HOURLY_LINE_1_KEY, value);
            }
        }
        public static int HourlyLine2
        {
            get
            {
                if (IsKeyPresent(HOURLY_LINE_2_KEY))
                    return GetIntValue(HOURLY_LINE_2_KEY);
                else
                    return 0;
            }
            set
            {
                SetIntValue(HOURLY_LINE_2_KEY, value);
            }
        }
        public static bool HourlyShowSummary
        {
            get
            {
                if (IsKeyPresent(HOURLY_SUMMARY_KEY))
                    return GetBoolValue(HOURLY_SUMMARY_KEY);
                else
                    return true;
            }
            set
            {
                GetBoolValue(HOURLY_SUMMARY_KEY, value);
            }
        }
        public static bool HourlyShowRealTemp
        {
            get
            {
                if (IsKeyPresent(HOURLY_SHOW_REAL_TEMP_KEY))
                    return GetBoolValue(HOURLY_SHOW_REAL_TEMP_KEY);
                else
                    return true;
            }
            set
            {
                GetBoolValue(HOURLY_SHOW_REAL_TEMP_KEY, value);
            }
        }

        public static bool IsKeyPresent(string key)
        {
            return ApplicationData.Current.LocalSettings.Values.ContainsKey(key);
        }

        public static bool GetBoolValue(string key, bool defaultValue = false)
        {
            try
            {
                return (bool)ApplicationData.Current.LocalSettings.Values[key];
            }
            catch
            {
                throw new InvalidCastException();
            }
        }

        public static string GetStringValue(string key)
        {
            try
            {
                return ApplicationData.Current.LocalSettings.Values[key] as string;
            }
            catch
            {
                throw new Exception();
            }
        }

        public static int GetIntValue(string key)
        {
            try
            {
                return (int)ApplicationData.Current.LocalSettings.Values[key];
            }
            catch
            {
                throw new InvalidCastException();
            }
        }

        public static void RemoveValue(string key)
        {
            try
            {
                ApplicationData.Current.LocalSettings.Values.Remove(key);
            }
            catch
            {
                throw new Exception();
            }
        }

        public static void SetBoolValue(string key, bool defaultValue = false)
        {
            try
            {
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey(key))
                    ApplicationData.Current.LocalSettings.Values[key] = defaultValue;
                else
                    ApplicationData.Current.LocalSettings.Values.Add(key, defaultValue);
            }
            catch
            {
                throw new InvalidCastException();
            }
        }

        public static void SetStringValue(string key, string value)
        {
            try
            {
                if (!string.IsNullOrEmpty(value))
                {
                    if (ApplicationData.Current.LocalSettings.Values.ContainsKey(key))
                        ApplicationData.Current.LocalSettings.Values[key] = value;
                    else
                        ApplicationData.Current.LocalSettings.Values.Add(key, value);
                }
            }
            catch
            {
                throw new InvalidCastException();
            }
        }

        public static void SetIntValue(string key, int value)
        {
            try
            {
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey(key))
                    ApplicationData.Current.LocalSettings.Values[key] = value;
                else
                    ApplicationData.Current.LocalSettings.Values.Add(key, value);
            }
            catch (Exception e)
            {
                throw new InvalidCastException();
            }
        }
    }
}
