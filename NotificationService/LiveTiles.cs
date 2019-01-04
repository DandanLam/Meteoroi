using Microsoft.Toolkit.Uwp.Notifications;
using StorageService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherService;
using Windows.Services.Maps;
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
            TileUpdateManager.CreateTileUpdaterForApplication().Clear();
            TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(true);
            var tileContent = new List<TileContent>();

            var MedTiles = new List<TileBinding>();
            if (Settings.ShowHourTile && Settings.ShowDayTile)
            {
                MedTiles.Add(GetMedTileBindingForHours(weatherData));
                MedTiles.Add(GetMedTileBindingForDays(weatherData));
            }
            else if (Settings.ShowHourTile)
            {
                MedTiles.Add(GetMedTileBindingForHours(weatherData));
                //MedTiles.Add(GetMedTileBindingForHours(weatherData));
            }
            else
            {
                MedTiles.Add(GetMedTileBindingForDays(weatherData));
                //MedTiles.Add(GetMedTileBindingForDays(weatherData));
            }

            var WideTiles = new List<TileBinding>();
            if (Settings.ShowHourTile && Settings.ShowDayTile)
            {
                WideTiles.Add(GetWideTileBindingForHours(weatherData));
                WideTiles.Add(GetWideTileBindingForDays(weatherData));
            }
            else if (Settings.ShowHourTile)
            {
                WideTiles.Add(GetWideTileBindingForHours(weatherData));
                //WideTiles.Add(GetWideTileBindingForHours(weatherData));
            }
            else
            {
                WideTiles.Add(GetWideTileBindingForDays(weatherData));
                //WideTiles.Add(GetWideTileBindingForDays(weatherData));
            }

            tileContent.Add(new TileContent()
            {
                Visual = new TileVisual()
                {
                    TileMedium = MedTiles[0],
                    TileWide   = WideTiles[0],
                }
            });
            tileContent.Add(new TileContent()
            {
                Visual = new TileVisual()
                {
                    TileMedium = MedTiles[1],
                    TileWide   = WideTiles[1],
                }
            });
            foreach (var tile in tileContent)
            {
                var tileNotification = new TileNotification(tile.GetXml());
                TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
            }
        }

        private static string GetLocationString(MapLocation location)
        {
            if (location == null)
                return "";
            var sb = new StringBuilder();
            sb.Append(location.Address.Town);
            if (Settings.ShowCurrentRegion != Settings.LocationDisplayType.HIDDEN)
            {
                sb.Append(", ");
                sb.Append(location.Address.Region);
            }
            if (Settings.ShowCurrentCountry == Settings.LocationDisplayType.FULL)
            {
                sb.Append(", ");
                sb.Append(location.Address.Country);
            }
            else if (Settings.ShowCurrentCountry == Settings.LocationDisplayType.ABBREV)
            {
                sb.Append(", ");
                sb.Append(location.Address.CountryCode);
            }
            return sb.ToString();
        }

        private static TileBinding GetMedTileBindingForDays(WeatherData weatherData)
        {
            var ag = new AdaptiveGroup();

            for (int i = 0; i < Settings.MedTileItemCount; i++)
            {
                ag.Children.Add(CreateDaySubgroup(weatherData.Daily.Data[i]));
            }
            return new TileBinding()
            {
                DisplayName = GetLocationString(weatherData.Location),
                Branding = TileBranding.None,
                Content = new TileBindingContentAdaptive()
                {
                    Children = { ag }
                }
            };
        }
        private static TileBinding GetMedTileBindingForHours(WeatherData weatherData)
        {
            var ag = new AdaptiveGroup();

            for (int i = 0; i < Settings.MedTileItemCount * Settings.TileInterval; i += Settings.TileInterval)
            {
                ag.Children.Add(CreateHourSubgroup(weatherData.Hourly.Data[i]));
            }
            return new TileBinding()
            {
                DisplayName = GetLocationString(weatherData.Location),
                Branding = TileBranding.None,
                Content = new TileBindingContentAdaptive()
                {
                    Children = { ag }
                }
            };
        }
        private static TileBinding GetWideTileBindingForDays(WeatherData weatherData)
        {
            var ag = new AdaptiveGroup();

            for (int i = 0; i < Settings.WideTileItemCount; i++)
            {
                ag.Children.Add(CreateDaySubgroup(weatherData.Daily.Data[i]));
            }
            return new TileBinding()
            {
                DisplayName = GetLocationString(weatherData.Location),
                Branding = TileBranding.None,
                Content = new TileBindingContentAdaptive()
                {
                    Children = { ag }
                }
            };
        }
        private static TileBinding GetWideTileBindingForHours(WeatherData weatherData)
        {
            var ag = new AdaptiveGroup();

            for (int i = 0; i < Settings.WideTileItemCount * Settings.TileInterval; i += Settings.TileInterval)
            {
                ag.Children.Add(CreateHourSubgroup(weatherData.Hourly.Data[i]));
            }
            return new TileBinding()
            {
                DisplayName = GetLocationString(weatherData.Location),
                Branding = TileBranding.None,
                Content = new TileBindingContentAdaptive()
                {
                    Children = { ag }
                }
            };
        }
        private static AdaptiveSubgroup CreateDaySubgroup(Data data)
        {
            return CreateSubgroup(
                data.Time.ToLocalTime().ToString("ddd").Substring(0, 2),
                data.Icon.ToUpper().Replace('-', '_') + ".png",
                Math.Round(Settings.LiveTileTempIsReal ? data.Temp.High : data.ApparnetTemp.High, 0).ToString() + "°",
                Math.Round(Settings.LiveTileTempIsReal ? data.Temp.Low : data.ApparnetTemp.Low, 0).ToString() + "°");
        }
        private static AdaptiveSubgroup CreateHourSubgroup(Data data)
        {
            return CreateSubgroup(
                data.Time.ToLocalTime().ToString("ht").ToLower(),
                data.Icon.ToUpper().Replace('-', '_') + ".png",
                Math.Round(Settings.LiveTileTempIsReal ? data.Temp.Current : data.ApparnetTemp.Current, 0).ToString() + "°",
                "", false);
        }
        private static AdaptiveSubgroup CreateSubgroup(string datetime, string image, string line1, string line2, bool removeMargin = true)
        {
            return new AdaptiveSubgroup()
            {
                HintWeight = 1,
                Children =
                {
                    new AdaptiveText()
                    {
                        Text = datetime,
                        HintAlign = AdaptiveTextAlign.Center
                    },
                    new AdaptiveImage()
                    {
                        Source = "Assets/WeatherIcons/" + image,
                        HintRemoveMargin = removeMargin,
                    },
                    new AdaptiveText()
                    {
                        Text = line1,
                        HintAlign = AdaptiveTextAlign.Center,
                        HintStyle = AdaptiveTextStyle.Caption
                    },
                    new AdaptiveText()
                    {
                        Text = line2,
                        HintAlign = AdaptiveTextAlign.Center,
                        HintStyle = AdaptiveTextStyle.CaptionSubtle
                    }
                }
            };
        }
    }
}
