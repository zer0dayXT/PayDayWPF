using System.Windows;
using System.Windows.Controls;

namespace PayDayWPF
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Page
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void OnQuit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void OnAdd(object sender, RoutedEventArgs e)
        {

        }
    }
}
