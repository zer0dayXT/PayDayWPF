using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using PayDayWPF.ViewModels;

namespace PayDayWPF.Pages
{
    public partial class MainMenuPage : Page
    {
        public MainMenuPage()
        {
            InitializeComponent();
            DataContext = MainWindow.ServiceProvider.GetService<MainMenuViewModel>();
        }
    }
}
