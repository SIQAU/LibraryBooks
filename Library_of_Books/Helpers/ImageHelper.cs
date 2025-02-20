using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;

namespace Library_of_Books.Helpers
{
    public static class ImageHelper
    {
        private static readonly HttpClient _httpClient = new();

        public static async Task<Bitmap?> LoadFromWeb(string url)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var data = await response.Content.ReadAsByteArrayAsync();
                return new Bitmap(new MemoryStream(data));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки изображения {url}: {ex.Message}");
                return null;
            }
        }
    }
}


