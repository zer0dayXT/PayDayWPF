using System.Windows;
using System.Windows.Controls;

namespace PayDayWPF.Pages
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenuPage : Page
    {
        public MainMenuPage()
        {
            InitializeComponent();
        }

        private void OnQuit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void OnAdd(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).ChangePage(new AddPackagePage());
        }
    }
}
