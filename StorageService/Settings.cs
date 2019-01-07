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
        private const string DAILY_LINE_3_KEY = "DAILY_LINE_3";
        private const string DAILY_SUMMARY_KEY = "DAILY_SUMMARY";
        private const string DAILY_SHOW_REAL_TEMP_KEY = "DAILY_SHOW_REAL_TEMP";
        private const string TIME_FORMAT_KEY = "TIME_FORMAT";
        private const string TIME_FORMAT_LOWERCASE_KEY = "TIME_FORMAT_LOWERCASE_";
        private const string HOURLY_LINE_1_KEY = "HOURLY_LINE_1";
        private const string HOURLY_LINE_2_KEY = "HOURLY_LINE_2";
        private const string HOURLY_LINE_3_KEY = "HOURLY_LINE_3";
        private const string HOURLY_SUMMARY_KEY = "HOURLY_SUMMARY";
        private const string HOURLY_SHOW_REAL_TEMP_KEY = "HOURLY_SHOW_REAL_TEMP";
        private const string MY_GEOLOCATION_KEY = "MY_GEOLOCATION";

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

        private const string LIVETILE_SHOW_HOUR_KEY      = "LIVETILE_MED_SHOWHOUR";
        private const string LIVETILE_MED_ITEM_COUNT_KEY    = "LIVETILE_MED_ITEM_COUNT";
        private const string LIVETILE_SHOW_DAY_KEY       = "LIVETILE_WIDE_SHOWDAY";
        private const string LIVETILE_WIDE_ITEM_COUNT_KEY    = "LIVETILE_WIDE_ITEM_COUNT";
        private const string LIVETILE_HOUR_INTERVAL_KEY      = "LIVETILE_HOUR_INTERVAL";
        private const string LIVETILE_TEMP_IS_REAL_KEY = "LIVETILE_TEMP_IS_REAL";

        public static bool IsMetric
        {
            get
            {
                return GetBoolValue(IS_METRIC_KEY, false);
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
                return GetBoolValue(IS_CELCIUS_KEY, false);
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
                switch (GetIntValue(SHOW_CURRENT_REGION_KEY, 0))
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
                switch (GetIntValue(SHOW_CURRENT_COUNTRY_KEY, 2))
                {
                    case 0:
                        return LocationDisplayType.FULL;
                    case 1:
                        return LocationDisplayType.ABBREV;
                    default:
                    case 2:
                        return LocationDisplayType.HIDDEN;
                }
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
                return GetBoolValue(SHOW_CURRENT_ICON_KEY, true);
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
                return GetBoolValue(SHOW_CURRENT_REAL_TEMP_KEY, true);
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
                return GetBoolValue(SHOW_CURRENT_ALT_TEMP_KEY, true);
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
                return GetBoolValue(SHOW_CURRENT_WINDSPEED_KEY, true);
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
                return GetBoolValue(SHOW_CURRENT_VISIBILITY_KEY, true);
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
                return GetBoolValue(SHOW_CURRENT_PRESSURE_KEY, true);
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
                return GetBoolValue(SHOW_CURRENT_HUMIDITY_KEY, true);
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
                return GetBoolValue(SHOW_CURRENT_DEWPOINT_KEY, true);
            }
            set
            {
                SetBoolValue(SHOW_CURRENT_DEWPOINT_KEY, value);
            }
        }

        public static string MyGeolocationString
        {
            get
            {
                return GetStringValue(MY_GEOLOCATION_KEY, null);
            }
            set
            {
                SetStringValue(MY_GEOLOCATION_KEY, value);
            }
        }

        public static string DateFormat
        {
            get
            {
                return GetStringValue(DATE_FORMAT_KEY, "ddd d");
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
                return GetBoolValue(DAILY_ICON_KEY, true);
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
                return GetBoolValue(HOURLY_ICON_KEY, true);
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
                return GetIntValue(DAILY_LINE_1_KEY, 8);
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
                return GetIntValue(DAILY_LINE_2_KEY, 0);
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
                return GetBoolValue(DAILY_SUMMARY_KEY, true);
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
                return GetBoolValue(DAILY_SHOW_REAL_TEMP_KEY, true);
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
                return GetStringValue(TIME_FORMAT_KEY, "h tt");
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
                return GetBoolValue(TIME_FORMAT_LOWERCASE_KEY, true);
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
                return GetIntValue(HOURLY_LINE_1_KEY, 7);
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
                return GetIntValue(HOURLY_LINE_2_KEY, 4);
            }
            set
            {
                SetIntValue(HOURLY_LINE_2_KEY, value);
            }
        }
        public static int HourlyLine3
        {
            get
            {
                return GetIntValue(HOURLY_LINE_3_KEY, 0);
            }
            set
            {
                SetIntValue(HOURLY_LINE_3_KEY, value);
            }
        }
        public static bool HourlyShowSummary
        {
            get
            {
                return GetBoolValue(HOURLY_SUMMARY_KEY, true);
            }
            set
            {
                SetBoolValue(HOURLY_SUMMARY_KEY, value);
            }
        }
        public static bool HourlyShowRealTemp
        {
            get
            {
                return GetBoolValue(HOURLY_SHOW_REAL_TEMP_KEY, true);
            }
            set
            {
                SetBoolValue(HOURLY_SHOW_REAL_TEMP_KEY, value);
            }
        }

        public static bool LiveTileTempIsReal
        {
            get
            {
                return GetBoolValue(LIVETILE_TEMP_IS_REAL_KEY, true);
            }
            set
            {
                SetBoolValue(LIVETILE_TEMP_IS_REAL_KEY, value);
            }
        }
        
        public static int MedTileItemCount
        {
            get
            {
                return GetIntValue(LIVETILE_MED_ITEM_COUNT_KEY, 2);
            }
            set
            {
                SetIntValue(LIVETILE_MED_ITEM_COUNT_KEY, value);
            }
        }
        public static bool ShowHourTile
        {
            get
            {
                return GetBoolValue(LIVETILE_SHOW_HOUR_KEY, true);
            }
            set
            {
                SetBoolValue(LIVETILE_SHOW_HOUR_KEY, value);
            }
        }
        public static bool ShowDayTile
        {
            get
            {
                return GetBoolValue(LIVETILE_SHOW_DAY_KEY, true);
            }
            set
            {
                SetBoolValue(LIVETILE_SHOW_DAY_KEY, value);
            }
        }
        public static int WideTileItemCount
        {
            get
            {
                return GetIntValue(LIVETILE_WIDE_ITEM_COUNT_KEY, 5);
            }
            set
            {
                SetIntValue(LIVETILE_WIDE_ITEM_COUNT_KEY, value);
            }
        }
        public static int TileInterval
        {
            get
            {
                return GetIntValue(LIVETILE_HOUR_INTERVAL_KEY, 1);
            }
            set
            {
                SetIntValue(LIVETILE_HOUR_INTERVAL_KEY, value);
            }
        }

        public static bool IsKeyPresent(string key)
        {
            return ApplicationData.Current.LocalSettings.Values.ContainsKey(key);
        }
        private static bool RemovedForNoPro(string key)
        {
            if (!StoreService.IsProUnlocked())
            {
                RemoveValue(key);
                return true;
            }
            return false;
        }
        public static bool GetBoolValue(string key, bool defaultValue)
        {
            try
            {
                if (IsKeyPresent(key) && !RemovedForNoPro(key))
                {
                    return (bool)ApplicationData.Current.LocalSettings.Values[key];
                }
                return defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }

        public static string GetStringValue(string key, string defaultValue)
        {
            try
            {
                if (IsKeyPresent(key) && !RemovedForNoPro(key))
                {
                    return ApplicationData.Current.LocalSettings.Values[key] as string;
                }
                return defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }

        public static int GetIntValue(string key, int defaultValue)
        {
            try
            {
                if (IsKeyPresent(key) && !RemovedForNoPro(key))
                {
                    return (int)ApplicationData.Current.LocalSettings.Values[key];
                }
                return defaultValue;
            }
            catch
            {
                return defaultValue;
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
