using Avalonia.Controls;
using Library_of_Books.Model;
using Library_of_Books.ViewModels;
using System.Threading.Tasks;

namespace Library_of_Books.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }
        private void OnScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (DataContext is MainWindowViewModel viewModel)
            {
                var scrollViewer = sender as ScrollViewer;
                if (scrollViewer != null)
                {
                    double offset = scrollViewer.Offset.Y;
                    double maxOffset = scrollViewer.Extent.Height - scrollViewer.Viewport.Height;

                    if (offset >= maxOffset - 100)
                    {
                        viewModel.CheckForMoreBooksCommand.Execute(null);
                    }
                }
            }
        }

    }
}
