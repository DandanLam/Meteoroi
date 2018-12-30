using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Services.Maps;
using Windows.UI.Xaml.Data;

namespace Meteoroi.Converters
{
    public class LocationToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                var location = value as MapLocation;
                if (location == null)
                    return "Unknown Location";
                return string.Concat(location.Address.Town, ", ", location.Address.Region, ", ", location.Address.CountryCode);
            }
            catch { return null; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
