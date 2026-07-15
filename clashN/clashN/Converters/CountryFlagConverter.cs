using ClashN.Tool;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace ClashN.Converters
{
    public class CountryFlagConverter : IValueConverter
    {
        private static readonly Dictionary<string, BitmapImage?> Cache = new(StringComparer.OrdinalIgnoreCase);

        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var countryCode = value as string;
            if (string.IsNullOrEmpty(countryCode))
            {
                return null;
            }

            lock (Cache)
            {
                if (Cache.TryGetValue(countryCode, out var cachedImage))
                {
                    return cachedImage;
                }

                try
                {
                    var image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.UriSource = new Uri($"pack://application:,,,/Resources/Flags/{CountryFlagHelper.GetAssetName(countryCode)}");
                    image.EndInit();
                    image.Freeze();
                    Cache[countryCode] = image;
                    return image;
                }
                catch
                {
                    Cache[countryCode] = null;
                    return null;
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
