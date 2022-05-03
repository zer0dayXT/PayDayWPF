using System.Windows;
using System.Windows.Controls;

namespace PayDayWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        public void ChangePage(MainMenu page)
        {
            NavigationFrame.NavigationService.Navigate(page);
        }

        public void ChangePage(AddMenu page)
        {
            NavigationFrame.NavigationService.Navigate(page);
        }

        public void ChangePage(MainMenu page)
        {
            NavigationFrame.NavigationService.Navigate(page);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ChangePage(new MainMenu());
        }
    }
}
