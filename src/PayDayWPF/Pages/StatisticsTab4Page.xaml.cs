using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using PayDayWPF.ViewModels;

namespace PayDayWPF.Pages
{
    public partial class StatisticsTab4Page : Page
    {
        public StatisticsTab4Page()
        {
            InitializeComponent();
            DataContext = MainWindow.ServiceProvider.GetService<StatisticsViewModel>().StatisticsTab4ViewModel;
        }
    }
}
