using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Library_of_Books.Model;
using Library_of_Books.Helpers;

namespace Library_of_Books.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly WorksInApi _api;

        [Reactive]
        public ObservableCollection<Book> Books { get; set; } = new();

        [Reactive]
        public string SearchQuery { get; set; } = "Космос";

        [Reactive]
        public bool IsLoading { get; set; } = false;

        [Reactive]
        public int LoadedBooks { get; set; } = 0;

        [Reactive]
        public int TotalBooks { get; set; } = 0;

        public ReactiveCommand<Unit, Unit> LoadMoreCommand { get; }
        public ICommand CheckForMoreBooksCommand { get; }

        public MainWindowViewModel()
        {
            _api = new WorksInApi();

            LoadMoreCommand = ReactiveCommand.CreateFromTask(() => LoadBooksAsync(true));

            CheckForMoreBooksCommand = ReactiveCommand.CreateFromTask(CheckForMoreBooks);

            LoadMoreCommand.ThrownExceptions.Subscribe(ex =>
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка загрузки: {ex.Message}");
            });
        }

        private async Task LoadBooksAsync(bool isNewSearch = false)
        {
            if (IsLoading) return;

            try
            {
                IsLoading = true;

                if (isNewSearch)
                {
                    Books.Clear();
                    LoadedBooks = 0;
                    TotalBooks = 0;
                }

                int limit = 20, offset = Books.Count;
                var result = await _api.GetBooksAsync(SearchQuery, limit, offset);

                if (offset == 0)
                {
                    TotalBooks = result.TotalBooks;
                }

                foreach (var book in result.Books)
                {
                    book.CoverImage = await ImageHelper.LoadFromWeb(book.CoverImageUrl);
                    Books.Add(book);
                    LoadedBooks++;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка в LoadBooksAsync: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }



        private async Task CheckForMoreBooks()
        {
            if (!IsLoading)
            {
                await LoadBooksAsync();
            }
        }
    }
}
