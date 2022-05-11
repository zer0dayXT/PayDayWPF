using Microsoft.Extensions.DependencyInjection;
using PayDayWPF.Pages;
using PayDayWPF.Persistence;
using PayDayWPF.Persistence.Implementation;
using System.Windows;
using System.Windows.Controls;

namespace PayDayWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ServiceProvider ServiceProvider { get; set; }
        
        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnLoaded;

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IRepository, JsonRepository>();
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        public void ChangePage(Page page)
        {
            NavigationFrame.NavigationService.Navigate(page);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ChangePage(new MainMenuPage());
        }
    }
}
