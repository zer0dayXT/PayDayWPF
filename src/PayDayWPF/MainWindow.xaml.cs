using System.Windows;

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
        }

        private void OnQuit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
