using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Library_of_Books.Converters
{
    public class StringToImageConverter : IValueConverter
    {
        private static readonly HttpClient _httpClient = new(); // Оставляем HttpClient на всё время работы

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string url && !string.IsNullOrWhiteSpace(url))
            {
                return LoadImageAsync(url);
            }
            return null;
        }

        private async Task<Bitmap> LoadImageAsync(string url)
        {
            try
            {
                var stream = await _httpClient.GetStreamAsync(url);
                return new Bitmap(stream);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки изображения: {ex.Message}");
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
