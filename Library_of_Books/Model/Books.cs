using Avalonia.Media.Imaging;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Http;
using System.Threading.Tasks;

namespace Library_of_Books.Model
{
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Title { get; set; } = "Без названия";
        public string Author { get; set; } = "Неизвестный автор";
        public int Year { get; set; }
        public string CoverImageUrl { get; set; }
        public Bitmap? CoverImage { get; set; }  
    }
}