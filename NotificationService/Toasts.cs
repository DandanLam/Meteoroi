using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;

namespace NotificationService
{
    public class Toasts
    {
        public static void CreateNotification(string title, string body, string imgPath)
        {
            try
            {
                ToastBindingGeneric bindingGeneric = new ToastBindingGeneric()
                {
                    Children =
                            {
                                new AdaptiveText()
                                {
                                    Text = body
                                }
                            }
                };

                if (imgPath != null)
                    bindingGeneric.AppLogoOverride = new ToastGenericAppLogo()
                    {
                        Source = imgPath
                    };

                ToastContent content = new ToastContent()
                {
                    //Launch = "app-defined-string",
                    //Header = new ToastHeader
                    //(
                    //    CalculateSHA1(title),
                    //    title,
                    //    "args"
                    //),
                    Visual = new ToastVisual()
                    {
                        BindingGeneric = bindingGeneric
                    }
                };
                var toast = new ToastNotification(content.GetXml())
                {
                    Tag = CalculateSHA1(body + title),
                    RemoteId = CalculateSHA1(body + title, false)
                };
                var history = ToastNotificationManager.History.GetHistory();
                
                ToastNotificationManager.CreateToastNotifier().Show(toast);
            }
            catch (Exception e)
            { }
        }

        private static string CalculateSHA1(string data, bool truncate = true)
        {
            SHA1 sha1 = SHA1.Create();

            //convert the input text to array of bytes
            byte[] hashData = sha1.ComputeHash(Encoding.UTF8.GetBytes(data));

            //create new instance of StringBuilder to save hashed data
            StringBuilder returnValue = new StringBuilder();

            //loop for each byte and add it to StringBuilder
            for (int i = 0; i < hashData.Length; i++)
                returnValue.Append(hashData[i].ToString());

            // return hexadecimal string
            if (truncate)
            {
                try { return returnValue.ToString().Substring(0, 16); }
                catch { return ""; }
            }
            else
            {
                try { return returnValue.ToString(); }
                catch { return ""; }
            }
        }

        public static bool CancelScheduledToastNotification(string tag)
        {
            if (!string.IsNullOrEmpty(tag))
                foreach (var toast in ToastNotificationManager.CreateToastNotifier().GetScheduledToastNotifications())
                {
                    if (toast.Tag == tag)
                    {
                        ToastNotificationManager.CreateToastNotifier().RemoveFromSchedule(toast);
                        return true;
                    }
                }
            return false;
        }

        public static void ClearNotifications()
        {
            try { ToastNotificationManager.History.Clear(); }
            catch { }
        }
    }
}
