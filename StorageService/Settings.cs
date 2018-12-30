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
        public static bool IsKeyPresent(string key)
        {
            return ApplicationData.Current.LocalSettings.Values.ContainsKey(key);
        }

        public static bool GetBoolValue(string key, bool defaultValue = false)
        {
            try
            {
                return ApplicationData.Current.LocalSettings.Values.ContainsKey(key) ? (bool)ApplicationData.Current.LocalSettings.Values[key] : defaultValue;
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
                return ApplicationData.Current.LocalSettings.Values.ContainsKey(key) ? ApplicationData.Current.LocalSettings.Values[key] as string : null;
            }
            catch
            {
                throw new Exception();
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
    }
}
