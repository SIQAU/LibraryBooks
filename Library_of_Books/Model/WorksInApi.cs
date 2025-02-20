using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Library_of_Books.Model
{
    // Класс для десериализации ответа API
    public class OpenLibraryResponse
    {
        [JsonPropertyName("numFound")]
        public int TotalBooks { get; set; } // Общее количество книг в API

        [JsonPropertyName("docs")]
        public List<OpenLibraryBook> Docs { get; set; }
    }


    // Класс для десериализации отдельной книги из API
    public class OpenLibraryBook
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("author_name")]
        public List<string> AuthorName { get; set; }

        [JsonPropertyName("first_publish_year")]
        public int? FirstPublishYear { get; set; }

        [JsonPropertyName("cover_i")]
        public int? CoverId { get; set; }
    }

    public class WorksInApi
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private const string BaseUrl = "https://openlibrary.org/search.json";
        public async Task<(List<Book> Books, int TotalBooks)> GetBooksAsync(string query, int limit = 20, int offset = 0)
        {
            string url = $"{BaseUrl}?q={query}&limit={limit}&offset={offset}";
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Ошибка запроса: {response.StatusCode}");
            }

            string jsonResponse = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var result = JsonSerializer.Deserialize<OpenLibraryResponse>(jsonResponse, options);

            if (result?.Docs == null) return (new List<Book>(), 0);

            var books = result.Docs.Select(item => new Book
            {
                Title = item.Title,
                Author = (item.AuthorName != null && item.AuthorName.Count > 0) ? item.AuthorName[0] : "Неизвестный автор",
                Year = item.FirstPublishYear ?? 0,
                CoverImageUrl = item.CoverId != null
                    ? $"https://covers.openlibrary.org/b/id/{item.CoverId}-M.jpg"
                    : "https://via.placeholder.com/100x150"
            }).ToList();

            return (books, result.TotalBooks); // Теперь возвращаем общее количество книг
        }

    }

}