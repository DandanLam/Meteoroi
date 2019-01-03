using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherService;
using Windows.UI.Notifications;

namespace NotificationService
{
    public class LiveTiles
    {
        public static void ResetLiveTile()
        {
            try { TileUpdateManager.CreateTileUpdaterForApplication().Clear(); }
            catch { }
        }

        public static void SetLiveTile(WeatherData weatherData)
        {
            var tileContent = new TileContent()
            {
                Visual = new TileVisual()
                {
                    TileWide = GetWideTileBinding(weatherData),
                }
            };

            var tileNotification = new TileNotification(tileContent.GetXml());
            TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
        }

        private static TileBinding GetWideTileBinding(WeatherData weatherData)
        {
            return new TileBinding()
            {
                DisplayName = weatherData.Location.Address.Town,
                Branding = TileBranding.Name,
                Content = new TileBindingContentAdaptive()
                {
                    Children =
                    {
                        new AdaptiveGroup()
                        {
                            Children =
                            {
                                CreateSubgroup(weatherData.Daily.Data[0].Time.ToString("ddd"), weatherData.Daily.Data[0].Icon.ToUpper().Replace('-','_') + ".png", Math.Round(weatherData.Daily.Data[0].Temp.High, 0).ToString(), Math.Round(weatherData.Daily.Data[0].Temp.Low, 0).ToString()),
                                CreateSubgroup(weatherData.Daily.Data[1].Time.ToString("ddd"), weatherData.Daily.Data[1].Icon.ToUpper().Replace('-','_') + ".png", Math.Round(weatherData.Daily.Data[1].Temp.High, 0).ToString(), Math.Round(weatherData.Daily.Data[1].Temp.Low, 0).ToString()),
                                CreateSubgroup(weatherData.Daily.Data[2].Time.ToString("ddd"), weatherData.Daily.Data[2].Icon.ToUpper().Replace('-','_') + ".png", Math.Round(weatherData.Daily.Data[2].Temp.High, 0).ToString(), Math.Round(weatherData.Daily.Data[2].Temp.Low, 0).ToString()),
                                CreateSubgroup(weatherData.Daily.Data[3].Time.ToString("ddd"), weatherData.Daily.Data[3].Icon.ToUpper().Replace('-','_') + ".png", Math.Round(weatherData.Daily.Data[3].Temp.High, 0).ToString(), Math.Round(weatherData.Daily.Data[3].Temp.Low, 0).ToString()),
                                CreateSubgroup(weatherData.Daily.Data[4].Time.ToString("ddd"), weatherData.Daily.Data[4].Icon.ToUpper().Replace('-','_') + ".png", Math.Round(weatherData.Daily.Data[4].Temp.High, 0).ToString(), Math.Round(weatherData.Daily.Data[4].Temp.Low, 0).ToString()),
                                CreateSubgroup(weatherData.Daily.Data[5].Time.ToString("ddd"), weatherData.Daily.Data[5].Icon.ToUpper().Replace('-','_') + ".png", Math.Round(weatherData.Daily.Data[5].Temp.High, 0).ToString(), Math.Round(weatherData.Daily.Data[5].Temp.Low, 0).ToString()),
                                CreateSubgroup(weatherData.Daily.Data[6].Time.ToString("ddd"), weatherData.Daily.Data[6].Icon.ToUpper().Replace('-','_') + ".png", Math.Round(weatherData.Daily.Data[6].Temp.High, 0).ToString(), Math.Round(weatherData.Daily.Data[6].Temp.Low, 0).ToString()),
                            }
                        }
                    }
                }
            };
        }
        private static AdaptiveSubgroup CreateSubgroup(string datetime, string image, string highTemp, string lowTemp)
        {
            return new AdaptiveSubgroup()
            {
                HintWeight = 1,
                Children =
                {
                    new AdaptiveText()
                    {
                        Text = datetime.Substring(0,2),
                        HintAlign = AdaptiveTextAlign.Center
                    },
                    new AdaptiveImage()
                    {
                        Source = "Assets/WeatherIcons/" + image,
                        HintRemoveMargin = true,
                    },
                    new AdaptiveText()
                    {
                        Text = highTemp + "°",
                        HintAlign = AdaptiveTextAlign.Center,
                        HintStyle = AdaptiveTextStyle.Caption
                    },
                    new AdaptiveText()
                    {
                        Text = lowTemp + "°",
                        HintAlign = AdaptiveTextAlign.Center,
                        HintStyle = AdaptiveTextStyle.CaptionSubtle
                    }
                }
            };
        }

    }
}
